

using System;
using System.IO;

public class Paths
{
    public static readonly string ROOT_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GOGames", "PerfectDriver");
    public static readonly string PROFILE_PATH = Path.Combine(ROOT_PATH, "Profiles");
    public static readonly string KEYBINDS_PATH = Path.Combine(ROOT_PATH, "Keybinds");

}
