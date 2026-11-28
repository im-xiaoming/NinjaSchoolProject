using NsoServer;
using UnityEngine;

// Starts the embedded server when the game starts. The client talks to it in memory (GameServer.Connect), no network.
public static class ServerBootstrap
{
    private static GameServer server;
    public static GameServer Server
    {
        get
        {
            return server;
        }
    }

    /// <summary>Initialize server bootstrap.</summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        if (server != null)
            return;
        // Register cleanup before startup so a partial initialization is also cleaned up.
        Application.quitting -= Shutdown;
        Application.quitting += Shutdown;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= Shutdown;
        UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += Shutdown;
#endif
        // tiles for server-made map pictures (treasure maps); the Editor reads the client's Resources files
        // (ảnh .png đã chuyển sang Assets/Sprites/Resources/res, xem Assets/Editor/NsoMoveSpritesToFolder.cs)
        NsoServer.Net.TilePicture.TileRoot = System.IO.Path.Combine(Application.dataPath, "Sprites", "Resources", "res");
        try
        {
            server = new GameServer(new ServerConfig { DataDirectory = NsoServerControl.DataFolder, UseSqlite = NsoServerControl.UseSqlite });
            // only problems reach the Console; the per-command trace is too noisy
            server.Logger = text =>
            {
                if (text.IndexOf("error", System.StringComparison.OrdinalIgnoreCase) >= 0)
                    Debug.LogWarning("[NsoServer] " + text);
            };
            string staticDir = NsoServerControl.StaticFolder;
            server.Static = new NsoServer.Net.FileStaticData(staticDir);
            var tables = NsoServer.Data.TableFile.Load(System.IO.Path.Combine(staticDir, "tables.txt"));
            server.World = new NsoServer.World.GameWorld(tables, System.IO.Path.Combine(staticDir, "Data"));
            NsoServerControl.ApplyPlayerLog();
            NsoServer.Net.PlayerLog.Open(NsoServerControl.PlayerLogFolder); // offline debug log of the player's actions/state
            NsoServerControl.ApplyGroundLoot(); // applied before the tick starts
            server.Start();
        }
        catch (System.Exception ex)
        {
            Shutdown();
            Debug.LogError("[NsoServer] Embedded server could not start: " + ex.Message);
        }
    }

#if UNITY_EDITOR
    /// <summary>Handle play mode state changed.</summary>
    /// <param name="state">State.</param>
    private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
    {
        if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            Shutdown();
    }

#endif
    /// <summary>Perform shutdown.</summary>
    private static void Shutdown()
    {
        Application.quitting -= Shutdown;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= Shutdown;
#endif
        if (server == null)
            return;
        // Mark the client disconnected before the server closes its stream.
        // Otherwise the collector treats normal game shutdown as an unexpected disconnect.
        Session_ME.isCancel = false;
        Session_ME.huyLock();
        Session_ME.gI().close();
        GameServer stoppingServer = server;
        server = null;
        stoppingServer.Stop();
        NsoServer.Net.PlayerLog.Close();
    }
}
