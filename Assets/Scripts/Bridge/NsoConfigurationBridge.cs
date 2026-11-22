using System;
using System.IO;
using System.Reflection;
using NsoServer.Configuration;
using UnityEngine;

/// <summary>Unity lifecycle and legacy Inspector migration only. Effective settings belong to the pure C# model.</summary>
public static class NsoConfigurationBridge
{
    public static string ConfigPath
    {
        get
        {
            return Path.Combine(GameStorage.PersistentRoot, "NsoConfig", "runtime-config.json");
        }
    }

    /// <summary>Perform initialize.</summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        RuntimeConfiguration.Load(ConfigPath);
        if (RuntimeConfiguration.LastError.Length > 0)
            Debug.LogWarning("[NsoConfig] " + RuntimeConfiguration.LastError);
    }

    /// <summary>Register nso configuration uration bridge.</summary>
    /// <param name="section">Section.</param>
    /// <param name="component">Component.</param>
    public static void Register(string section, object component)
    {
        if (component == null)
            return;
        foreach (var field in component.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            if (Attribute.IsDefined(field, typeof(SerializeField)))
                RuntimeConfiguration.Register(section, field.Name, field.GetValue(component));
    }

    /// <summary>Export nso configuration uration bridge.</summary>
    public static void Export()
    {
        // Works outside Play too; read the actual scene/prefab overrides, never write scene assets.
        var server = UnityEngine.Object.FindFirstObjectByType<NsoServerControl>();
        var game = UnityEngine.Object.FindFirstObjectByType<NsoGameControl>();
        var layout = UnityEngine.Object.FindFirstObjectByType<NsoUiLayout>();
        if (!Application.isPlaying)
        {
            Register("server", server);
            Register("game", game);
            Register("ui", layout);
        }

        RuntimeConfiguration.Export(ConfigPath);
        Debug.Log("[NsoConfig] Exported " + ConfigPath);
    }
}
