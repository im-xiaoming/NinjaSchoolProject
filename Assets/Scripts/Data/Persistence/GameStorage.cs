using System.IO;
using UnityEngine;

/// <summary>Keep existing saves and runtime configuration accessible after the product rename.</summary>
public static class GameStorage
{
    public static string PersistentRoot
    {
        get
        {
            return ResolvePersistentRoot(Application.persistentDataPath);
        }
    }

    /// <summary>Prefer current storage; reuse the previous product directory when it contains existing game data.</summary>
    public static string ResolvePersistentRoot(string currentRoot)
    {
        if (Directory.Exists(Path.Combine(currentRoot, "NsoData")))
            return currentRoot;
        string parent = Path.GetDirectoryName(currentRoot);
        if (string.IsNullOrEmpty(parent))
            return currentRoot;
        // Offline compatibility: keep old on-disk identifiers so renaming the game never moves or deletes saves.
        string previousRoot = Path.Combine(parent, "My project");
        if (Directory.Exists(Path.Combine(previousRoot, "NsoData")))
            return previousRoot;
        if (Directory.Exists(Path.Combine(currentRoot, "NsoConfig")))
            return currentRoot;
        if (Directory.Exists(Path.Combine(previousRoot, "NsoConfig")))
            return previousRoot;
        return currentRoot;
    }
}
