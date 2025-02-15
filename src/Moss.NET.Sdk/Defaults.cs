using System.Runtime.InteropServices;
using Extism;
using Moss.NET.Sdk.FFI;

namespace Moss.NET.Sdk;

public static class Defaults
{
    [DllImport(Functions.DLL, EntryPoint = "moss_defaults_set_color")]
    private static extern void SetDefaultColor(ulong keyPtr, ulong colorPtr);
    [DllImport(Functions.DLL, EntryPoint = "moss_defaults_get_text_color")]
    private static extern ulong GetDefaultTextColor(ulong keyPtr); // -> TextColors
    [DllImport(Functions.DLL, EntryPoint = "moss_defaults_set_text_color")]
    private static extern void SetDefaultTextColor(ulong keyPtr, ulong textColorPtr);

    [DllImport(Functions.DLL, EntryPoint = "moss_defaults_get")]
    private static extern ulong GetDefaultValue(ulong keyPtr); //-> ConfigGet

    [DllImport(Functions.DLL, EntryPoint = "_moss_defaults_set")]
    private static extern void SetDefaultValue(ulong configSetPtr);

    public static void SetDefaultColor(string key, Color color)
    {
        var keyPtr = Pdk.Allocate(key).Offset;

        SetDefaultColor(keyPtr, Utils.Serialize(color, JsonContext.Default.Color));
    }

    public static void SetDefaultTextColor(string key, Color foreground, Color background)
    {
        var keyPtr = Pdk.Allocate(key).Offset;
        var textColor = new TextColor(foreground, background);
        var textColorPtr = Utils.Serialize(textColor, JsonContext.Default.TextColor);

        SetDefaultTextColor(keyPtr, textColorPtr);
    }

    public static void SetDefaultValue(string key, object value)
    {
        var valuePtr = Utils.Serialize(new ConfigSet(key, value), JsonContext.Default.ConfigSet);

        SetDefaultValue(valuePtr);
    }

    public static object GetDefaultValue(string key)
    {
        var keyPtr = Pdk.Allocate(key).Offset;
        var valuePtr = GetDefaultValue(keyPtr);

        return Utils.Deserialize(valuePtr, JsonContext.Default.ConfigGet).value;
    }

    public static TextColor GetDefaultTextColor(string key)
    {
        var keyPtr = Pdk.Allocate(key).Offset;
        var textColorPtr = GetDefaultTextColor(keyPtr);

        return Utils.Deserialize(textColorPtr, JsonContext.Default.TextColor);
    }

    public static Color GetDefaultColor(string key)
    {
        var keyPtr = Pdk.Allocate(key).Offset;
        var colorPtr = GetDefaultValue(keyPtr);

        return Utils.Deserialize(colorPtr, JsonContext.Default.Color);
    }
}