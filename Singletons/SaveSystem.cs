using Godot;
using Godot.Collections;

public static class SaveSystem
{
    // ==========================================
    // ============ LEVEL SAVING ================
    // ==========================================
    const string lvlSavePath = "user://save.dat";

    public static void SaveLevel(string lvlName) {
        var f = new File();
        f.Open(lvlSavePath, File.ModeFlags.Write);
        f.StoreString(lvlName);
        f.Close();
    }

    public static string LoadLevel() {
        var f = new File();
        f.Open(lvlSavePath, File.ModeFlags.Read);
        var result = f.GetAsText();
        f.Close();
        return result;
    }

    public static bool HasLevel() {
        var d = new Directory();
        if (!d.FileExists(lvlSavePath)) return false;
        return ResourceLoader.Exists(LoadLevel());
    }

    // ==========================================
    // ============ SETTING SAVING ==============
    // ==========================================
    const string settingSavePath = "user://settings.dat";
    public static Dictionary Settings { get; set; } = new Dictionary();

    public static void SaveSettings() {
        var f = new File();
        f.Open(settingSavePath, File.ModeFlags.Write);
        f.StoreVar(Settings);
        f.Close();
    }

    public static void LoadSettings() {
        Settings.Add("music_vol", 1f);
        Settings.Add("sound_vol", 1f);
        Settings.Add("fullscreen", false);

        var f = new File();
        var err = f.Open(settingSavePath, File.ModeFlags.Read);
        if (err != Error.Ok) return;

        Settings = f.GetVar() as Dictionary;
        f.Close();
    }
}
