using System.IO;
using UnityEditor;
using UnityEngine;

// Inspector buttons for the "Ninja School ..." Hierarchy objects.
[CustomEditor(typeof(NsoServerControl))]
class NsoServerControlEditor : Editor
{
    /// <summary>Draw the custom Inspector and dispatch its editor controls.</summary>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        using (new EditorGUI.DisabledScope(Application.isPlaying))
            DrawPropertiesExcluding(serializedObject, "status", "address", "dataFolder");
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("status"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("address"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dataFolder"));
        }

        serializedObject.ApplyModifiedProperties();
        string folder;
        try
        {
            folder = ((NsoServerControl)target).ConfiguredDataFolder;
        }
        catch (System.Exception ex)
        {
            EditorGUILayout.HelpBox(ex.Message, MessageType.Error);
            return;
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Open save-data folder"))
        {
            Directory.CreateDirectory(folder);
            EditorUtility.RevealInFinder(folder);
        }

        using (new EditorGUI.DisabledScope(Application.isPlaying))
        {
            if (GUILayout.Button("Delete save data (new game state)") && EditorUtility.DisplayDialog("Delete save data", "Delete all accounts and characters in\n" + folder + "\n\nThis cannot be undone.", "Delete", "Cancel"))
            {
                if (Directory.Exists(folder))
                    Directory.Delete(folder, true);
                Debug.Log("[Ninja School] Deleted " + folder);
            }
        }

        if (Application.isPlaying)
            EditorGUILayout.HelpBox("Stop Play mode before deleting save data.", MessageType.Info);
    }
}

static class NsoControlMenu
{
    // Adds (or re-adds) the control objects to the open scene.
    /// <summary>Create nso control menu.</summary>
    [MenuItem("Tools/Ninja School/Create Control Objects")]
    static void Create()
    {
        Ensure<NsoGameControl>("Ninja School Game Control");
        // Gameplay input stays on Main Camera's NsoInputSystemBridge.
        Ensure<NsoServerControl>("Ninja School Server Control");
    }

    /// <summary>Perform ensure.</summary>
    /// <param name="name">Name.</param>
    /// <remarks>Locals: go: Unity scene object.</remarks>
    static void Ensure<T>(string name)
        where T : Component
    {
        if (UnityEngine.Object.FindFirstObjectByType<T>() != null)
            return;
        GameObject go = new GameObject(name);
        go.AddComponent<T>();
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
    }
}
