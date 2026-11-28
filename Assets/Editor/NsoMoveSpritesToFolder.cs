using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

// Chuyển mọi ảnh .png trong Assets/Resources/res sang Assets/Sprites/Resources/res (giữ nguyên thư mục con).
// - Dùng AssetDatabase.MoveAsset nên GUID giữ nguyên: material/prefab đang trỏ tới ảnh không bị mất liên kết.
// - Đích vẫn nằm trong một thư mục tên "Resources" nên Resources.Load("res/x2/...") chạy y như cũ.
// - File .bytes, âm thanh, font, material... ở lại Assets/Resources/res.
// Chạy xong, kiểm tra Console rồi có thể xóa file script này.
public static class NsoMoveSpritesToFolder
{
    private const string Src = "Assets/Resources/res";
    private const string Dst = "Assets/Sprites/Resources/res";

    private static List<string> FindPngs()
    {
        return Directory.GetFiles(Src, "*.png", SearchOption.AllDirectories)
            .Select(p => p.Replace('\\', '/'))
            .OrderBy(p => p)
            .ToList();
    }

    [MenuItem("NSO/Sprites/Chuyển ảnh res vào thư mục Sprites")]
    private static void MoveFromMenu()
    {
        int count = FindPngs().Count;
        if (count == 0)
        {
            EditorUtility.DisplayDialog("NSO", "Không còn ảnh .png nào trong " + Src + ".", "OK");
            return;
        }

        if (EditorUtility.DisplayDialog("NSO", "Chuyển " + count + " ảnh .png\ntừ " + Src + "\nsang " + Dst + " ?", "Chuyển", "Hủy"))
            Run();
    }

    // Làm việc thật, không hỏi gì (gọi được từ menu hoặc từ script). Trả về dòng tóm tắt.
    public static string Run()
    {
        List<string> files = FindPngs();
        if (files.Count == 0)
            return "[NSO] Không còn ảnh .png nào trong " + Src + ".";

        // Không ghi đè: nếu đích đã có file trùng tên thì dừng hẳn để bạn tự xem.
        List<string> clash = files.Where(p => File.Exists(Dst + p.Substring(Src.Length))).ToList();
        if (clash.Count > 0)
        {
            string stop = "[NSO] Đích đã có " + clash.Count + " file trùng, không chuyển gì cả. Ví dụ: " + clash[0];
            Debug.LogError(stop);
            return stop;
        }

        // 1. Tạo trước mọi thư mục đích (ngoài StartAssetEditing để MoveAsset thấy được thư mục mới).
        foreach (string dir in files.Select(p => Path.GetDirectoryName(Dst + p.Substring(Src.Length)).Replace('\\', '/')).Distinct())
            EnsureFolder(dir);

        // 2. Chuyển file. StartAssetEditing gom mọi thay đổi lại, Unity chỉ refresh một lần ở StopAssetEditing.
        List<string> errors = new List<string>();
        AssetDatabase.StartAssetEditing();
        try
        {
            for (int i = 0; i < files.Count; i++)
            {
                if (i % 100 == 0)
                    EditorUtility.DisplayProgressBar("NSO", files[i], (float)i / files.Count);
                string to = Dst + files[i].Substring(Src.Length);
                string err = AssetDatabase.MoveAsset(files[i], to); // "" = thành công
                if (!string.IsNullOrEmpty(err))
                    errors.Add(files[i] + ": " + err);
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
        }

        // 3. Xóa các thư mục nguồn đã rỗng (sâu nhất trước, không đụng tới Src).
        int removed = 0;
        foreach (string dir in Directory.GetDirectories(Src, "*", SearchOption.AllDirectories)
                     .Select(d => d.Replace('\\', '/'))
                     .OrderByDescending(d => d.Length))
        {
            if (Directory.GetFileSystemEntries(dir).All(e => e.EndsWith(".meta") && !File.Exists(e.Substring(0, e.Length - 5)) && !Directory.Exists(e.Substring(0, e.Length - 5))))
            {
                if (AssetDatabase.DeleteAsset(dir))
                    removed++;
            }
        }

        AssetDatabase.Refresh();

        // 4. Kiểm tra: không còn png ở nguồn, và Resources.Load vẫn tìm thấy ảnh ở chỗ mới.
        int left = Directory.GetFiles(Src, "*.png", SearchOption.AllDirectories).Length;
        int notFound = 0;
        for (int i = 0; i < files.Count; i += 50)
        {
            string resPath = files[i].Substring("Assets/Resources/".Length);
            resPath = resPath.Substring(0, resPath.Length - ".png".Length);
            if (Resources.Load<Texture2D>(resPath) == null)
            {
                notFound++;
                Debug.LogError("[NSO] Resources.Load không thấy: " + resPath);
            }
        }

        foreach (string e in errors.Take(20))
            Debug.LogError("[NSO] " + e);
        string msg = "[NSO] Đã chuyển " + (files.Count - errors.Count) + "/" + files.Count + " ảnh sang " + Dst
            + ", xóa " + removed + " thư mục rỗng. Còn " + left + " png ở nguồn. Resources.Load thử mẫu: "
            + (notFound == 0 ? "OK" : notFound + " lỗi") + ".";
        if (errors.Count == 0 && left == 0 && notFound == 0)
            Debug.Log(msg);
        else
            Debug.LogWarning(msg);
        return msg;
    }

    // Tạo thư mục theo từng cấp bằng AssetDatabase (để Unity tạo .meta cho thư mục).
    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
            return;
        string parent = Path.GetDirectoryName(path).Replace('\\', '/');
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, Path.GetFileName(path));
    }
}
