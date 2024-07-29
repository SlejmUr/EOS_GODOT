using Godot;

namespace EOS_GODOT;

public class INI
{
    public static string Read(string section , string key)
    {
        ConfigFile configFile = new();
        var result = configFile.Load("eos_settings.ini");
        if (result != Error.Ok)
            return string.Empty;
        return (string)configFile.GetValue(section, key);
    }

    public static void Write(string section, string key, string value)
    {
        ConfigFile configFile = new();
        var result = configFile.Load("eos_settings.ini");
        if (result != Error.Ok)
            return;
        configFile.SetValue(section, key, value);
        configFile.Save("eos_settings.ini");
    }
}
