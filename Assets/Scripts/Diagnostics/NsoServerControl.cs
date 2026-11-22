using System.IO;
using UnityEngine;
using NsoServer.Configuration;

// Hierarchy object "Ninja School Server Control": shows the embedded server's state and where the save data lives.
public class NsoServerControl : MonoBehaviour
{
    [Header("Server paths — apply on next Play")]
    [Tooltip("Relative folder under GameStorage.PersistentRoot. Changing it does not migrate or delete saves.")]
    [SerializeField]
    private string _saveSubfolder = "NsoData";
    [Tooltip("Save the account and characters (bag, quests, progress) in the SQLite database nso.db. Off: the former users.tsv / players.tsv files. Old TSV saves are imported into a new database once.")]
    [SerializeField]
    private bool _useSqlite = true;
    public static bool UseSqlite
    {
        get
        {
            return RuntimeConfiguration.Value("server", "_useSqlite", Active == null || Active._useSqlite);
        }
    }

    [Tooltip("Relative folder under StreamingAssets containing tables.txt and Data.")]
    [SerializeField]
    private string _staticSubfolder = "NsoStatic";
    [Header("Player log (debug) — apply on next Play")]
    [Tooltip("Write every player action (client message), server reply, server trace line and periodic state snapshot to a text file. Editor: <project>/Logs/PlayerLog; build: <persistentDataPath>/Logs/PlayerLog.")]
    [SerializeField]
    private bool _playerLogEnabled = true;
    [Tooltip("Write the log into the SQLite database Logs/PlayerLog/player_log.db (one session per Play; table entries).")]
    [SerializeField]
    private bool _playerLogToDatabase = true;
    [Tooltip("Also write the classic text file player_<time>.log.")]
    [SerializeField]
    private bool _playerLogToFile = false;
    [Tooltip("Also log every server-to-client message (much larger files).")]
    [SerializeField]
    private bool _playerLogOutgoing = true;
    [Tooltip("Seconds between two full state snapshots of the character (0 = none).")]
    [SerializeField, Range(0, 60)]
    private int _playerLogSnapshotSeconds = 5;
    [Tooltip("Payload bytes written in hex per message.")]
    [SerializeField, Range(0, 512)]
    private int _playerLogPayloadBytes = 48;
    [Tooltip("Number of log sessions kept in the database / text files kept (one per Play session); older ones are deleted.")]
    [SerializeField, Range(1, 200)]
    private int _playerLogKeepFiles = 20;
    [Header("Ground drops — shared by every character, next Play")]
    [SerializeField, Range(0, 600)]
    private int _groundLootOwnerSeconds = 30;
    [SerializeField, Range(1, 3600)]
    private int _groundLootLifetimeSeconds = 60;
    /// <summary>Apply ground loot.</summary>
    public static void ApplyGroundLoot()
    {
        NsoServer.World.GroundLootPolicy.Configure(RuntimeConfiguration.Value("server", "_groundLootOwnerSeconds", Active != null ? Active._groundLootOwnerSeconds : 30), RuntimeConfiguration.Value("server", "_groundLootLifetimeSeconds", Active != null ? Active._groundLootLifetimeSeconds : 60));
    }

    /// <summary>Copies the player log settings into the server (ServerBootstrap, before the log opens).</summary>
    public static void ApplyPlayerLog()
    {
        NsoServer.Net.PlayerLog.Enabled = RuntimeConfiguration.Value("server", "_playerLogEnabled", Active != null ? Active._playerLogEnabled : true);
        NsoServer.Net.PlayerLog.LogOutgoing = RuntimeConfiguration.Value("server", "_playerLogOutgoing", Active != null ? Active._playerLogOutgoing : true);
        NsoServer.Net.PlayerLog.SnapshotSeconds = RuntimeConfiguration.Value("server", "_playerLogSnapshotSeconds", Active != null ? Active._playerLogSnapshotSeconds : 5);
        NsoServer.Net.PlayerLog.MaxPayloadBytes = RuntimeConfiguration.Value("server", "_playerLogPayloadBytes", Active != null ? Active._playerLogPayloadBytes : 48);
        NsoServer.Net.PlayerLog.KeepFiles = RuntimeConfiguration.Value("server", "_playerLogKeepFiles", Active != null ? Active._playerLogKeepFiles : 20);
        NsoServer.Net.PlayerLog.ToDatabase = RuntimeConfiguration.Value("server", "_playerLogToDatabase", Active != null ? Active._playerLogToDatabase : true);
        NsoServer.Net.PlayerLog.ToFile = RuntimeConfiguration.Value("server", "_playerLogToFile", Active != null ? Active._playerLogToFile : false);
    }

    public static string PlayerLogFolder
    {
        get
        {
            string root = Application.isEditor ? Path.Combine(Application.dataPath, "..") : GameStorage.PersistentRoot;
            return Path.GetFullPath(Path.Combine(root, "Logs", "PlayerLog"));
        }
    }

    public static NsoServerControl Active { get; private set; }

    public static string StaticFolder
    {
        get
        {
            return Resolve(Application.streamingAssetsPath, RuntimeConfiguration.Value("server", "_staticSubfolder", Active != null ? Active._staticSubfolder : "NsoStatic"));
        }
    }

    [Header("Runtime status")]
    public string status = "Not running";
    public string address = "";
    public string dataFolder = "";
    public string playerLogFile = "";
    /// <summary>Compute resolve.</summary>
    /// <param name="root">Root.</param>
    /// <param name="relative">Relative.</param>
    private static string Resolve(string root, string relative)
    {
        if (string.IsNullOrWhiteSpace(relative) || Path.IsPathRooted(relative))
            throw new System.ArgumentException("Server folder must be a non-empty relative path.");
        string basePath = Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
        string candidate = Path.GetFullPath(Path.Combine(basePath, relative));
        if (!candidate.StartsWith(basePath, System.StringComparison.OrdinalIgnoreCase))
            throw new System.ArgumentException("Server folder must stay inside its data root.");
        return candidate;
    }

    /// <summary>Initialize this component before Unity invokes its first Start callback.</summary>
    private void Awake()
    {
        if (Active != null && Active != this)
        {
            Debug.LogError("Only one Ninja School Server Control may own configuration.", this);
            enabled = false;
            return;
        }

        Active = this;
        NsoConfigurationBridge.Register("server", this);
        try
        {
            dataFolder = DataFolder;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message, this);
        }
    }

    /// <summary>Export runtime configuration uration.</summary>
    [ContextMenu("Export runtime configuration (never overwrite)")]
    private void ExportRuntimeConfiguration()
    {
        NsoConfigurationBridge.Export();
    }

    /// <summary>Release resources owned by this component when Unity destroys it.</summary>
    private void OnDestroy()
    {
        if (Active == this)
            Active = null;
    }

    /// <summary>Refresh the read-only runtime status shown in the Inspector.</summary>
    private void Update()
    {
        NsoServer.GameServer server = ServerBootstrap.Server;
        status = server != null ? "Running" : "Not running";
        address = server != null ? "in-process (no network)" : "";
        playerLogFile = NsoServer.Net.PlayerLog.DatabasePath != null ? NsoServer.Net.PlayerLog.DatabasePath + " (session " + NsoServer.Net.PlayerLog.SessionId + ")" + (NsoServer.Net.PlayerLog.FilePath != null ? " + " + NsoServer.Net.PlayerLog.FilePath : "") : NsoServer.Net.PlayerLog.FilePath ?? "";
    }

    public string ConfiguredDataFolder
    {
        get
        {
            return Resolve(GameStorage.PersistentRoot, RuntimeConfiguration.Value("server", "_saveSubfolder", _saveSubfolder));
        }
    }

    public static string DataFolder
    {
        get
        {
            if (ServerBootstrap.Server != null)
                return ServerBootstrap.Server.Config.DataDirectory;
            return Resolve(GameStorage.PersistentRoot, RuntimeConfiguration.Value("server", "_saveSubfolder", Active != null ? Active._saveSubfolder : "NsoData"));
        }
    }
}
