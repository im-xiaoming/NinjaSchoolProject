using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace NsoServer.Net
{
    using Math = System.Math; // the client has its own global Math class

    /// <summary>
    /// The log database (offline debugging aid, separate from the save database nso.db): Logs/PlayerLog/player_log.db.
    /// Tables:
    ///   sessions(id, started_at, started_ms)               one row per Play session
    ///   entries(id, session_id, ts_ms, time, kind, text)   kind: IN, OUT, LOG, EVT, STATE (see PlayerLog)
    /// Lines are buffered by PlayerLog and inserted in one transaction per Flush (server tick, every 500 ms), so the
    /// game threads never wait for the disk. Not thread-safe: PlayerLog serializes the calls.
    /// Example: SELECT time, kind, text FROM entries WHERE session_id = (SELECT MAX(id) FROM sessions) AND kind = 'EVT';
    /// </summary>
    public sealed class PlayerLogDb : IDisposable
    {
        public struct Line
        {
            // Ms: Milliseconds.
            public long Ms;
            public string Time, Kind, Text;
        }

        private static readonly object InitLock = new object ();
        private static bool initialized;
        private readonly SqliteConnection conn;
        public long SessionId { get; private set; }
        public string FilePath { get; private set; }

        /// <summary>Initialize player log db state and its supplied dependencies.</summary>
        /// <param name="file">File.</param>
        /// <param name="keepSessions">Keep sessions.</param>
        /// <remarks>Locals: c: Current character/entry.</remarks>
        public PlayerLogDb(string file, int keepSessions)
        {
            lock (InitLock)
            {
                if (!initialized)
                {
                    SQLitePCL.Batteries_V2.Init();
                    initialized = true;
                }
            }

            FilePath = file;
            conn = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = file, Mode = SqliteOpenMode.ReadWriteCreate, Pooling = false }.ToString());
            conn.Open();
            Exec("PRAGMA journal_mode=WAL; PRAGMA synchronous=NORMAL;");
            Exec(@"CREATE TABLE IF NOT EXISTS sessions (id INTEGER PRIMARY KEY, started_at TEXT NOT NULL, started_ms INTEGER NOT NULL);
CREATE TABLE IF NOT EXISTS entries (id INTEGER PRIMARY KEY, session_id INTEGER NOT NULL, ts_ms INTEGER NOT NULL, time TEXT NOT NULL,
  kind TEXT NOT NULL, text TEXT NOT NULL);
CREATE INDEX IF NOT EXISTS entries_session_kind ON entries(session_id, kind);");
            // forget the oldest sessions (PlayerLog.KeepFiles applies to sessions here)
            using (var c = conn.CreateCommand())
            {
                c.CommandText = @"DELETE FROM entries WHERE session_id NOT IN (SELECT id FROM sessions ORDER BY id DESC LIMIT $keep);
DELETE FROM sessions WHERE id NOT IN (SELECT id FROM sessions ORDER BY id DESC LIMIT $keep);";
                c.Parameters.AddWithValue("$keep", Math.Max(0, keepSessions - 1));
                c.ExecuteNonQuery();
            }

            using (var c = conn.CreateCommand())
            {
                c.CommandText = "INSERT INTO sessions (started_at, started_ms) VALUES ($at, $ms); SELECT last_insert_rowid();";
                c.Parameters.AddWithValue("$at", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("$ms", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                SessionId = Convert.ToInt64(c.ExecuteScalar());
            }
        }

        /// <summary>Perform exec through the authoritative player rules.</summary>
        /// <param name="sql">Sql.</param>
        /// <remarks>Locals: c: Current character/entry.</remarks>
        private void Exec(string sql)
        {
            using (var c = conn.CreateCommand())
            {
                c.CommandText = sql;
                c.ExecuteNonQuery();
            }
        }

        /// <summary>Inserts buffered lines in one transaction.</summary>
        /// <param name="lines">Lines.</param>
        /// <remarks>Locals: tx: Tx; c: Current character/entry; s: Source text or serialized input; ms: Duration/timestamp in milliseconds; t: Current entry or time sample; k: Key or nested iteration index; x: Horizontal coordinate in the caller&#x27;s coordinate space; l: Length or current list.</remarks>
        public void Insert(List<Line> lines)
        {
            if (lines.Count == 0)
                return;
            using (var tx = conn.BeginTransaction())
            using (var c = conn.CreateCommand())
            {
                c.Transaction = tx;
                c.CommandText = "INSERT INTO entries (session_id, ts_ms, time, kind, text) VALUES ($s, $ms, $t, $k, $x)";
                var s = c.Parameters.Add("$s", SqliteType.Integer);
                var ms = c.Parameters.Add("$ms", SqliteType.Integer);
                var t = c.Parameters.Add("$t", SqliteType.Text);
                var k = c.Parameters.Add("$k", SqliteType.Text);
                var x = c.Parameters.Add("$x", SqliteType.Text);
                s.Value = SessionId;
                foreach (var l in lines)
                {
                    ms.Value = l.Ms;
                    t.Value = l.Time;
                    k.Value = l.Kind;
                    x.Value = l.Text ?? "";
                    c.ExecuteNonQuery();
                }

                tx.Commit();
            }
        }

        /// <summary>Release resources owned by this instance.</summary>
        public void Dispose()
        {
            try
            {
                Exec("PRAGMA wal_checkpoint(TRUNCATE);");
            }
            catch (Exception)
            {
            }

            conn.Dispose();
        }
    }
}
