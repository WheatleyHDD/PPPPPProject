using Godot;

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

    // ==========================================
    // ============ SETTING SAVING ==============
    // ==========================================
    const string settingSavePath = "user://settings.dat";
    public static SettingsData Settings
    { 
        get { return Settings; }
        private set { Settings = value; }
    }

    public static void SaveSettings(string lvlName) {
        var f = new File();
        f.Open(settingSavePath, File.ModeFlags.Write);
        f.StoreVar(Settings);
        f.Close();
    }

    public static void LoadSettings() {
        var f = new File();
        f.Open(settingSavePath, File.ModeFlags.Read);
        Settings = f.GetVar() as SettingsData;
        f.Close();
    }
}

public class SettingsData {
    public float musicVolume = 1;
    public float soundVolume = 1;
    public bool fullscreen = false;

    public void SetMusicVolume(float value) => musicVolume = value;
    public void SetSoundVolume(float value) => soundVolume = value;
    public void SetFullscreen(bool value) => fullscreen = value;
}
