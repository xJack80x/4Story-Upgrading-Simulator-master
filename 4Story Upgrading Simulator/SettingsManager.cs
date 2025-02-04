using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _4Story_Upgrading_Simulator
{
    public static class SettingsManager
    {
        public static readonly string SettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "settings", "settings.txt");

        public static void SaveSettings(Settings settings)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));

            var lines = new[]
            {
                settings.AutoPotions.ToString(),
                settings.BestPotion.ToString(),
                settings.NoVolume.ToString(),
                settings.SiVolume.ToString(),
                settings.TrackBarValue.ToString(),
                settings.SoundVolume.ToString()
            };

            File.WriteAllLines(SettingsPath, lines);
        }

        public static Settings LoadSettings()
        {
            if (!File.Exists(SettingsPath))
            {
                return new Settings
                {
                    AutoPotions = false,
                    BestPotion = false,
                    NoVolume = false,
                    SiVolume = true,
                    TrackBarValue = 100,
                    SoundVolume = 1.0f
                };
            }

            var lines = File.ReadAllLines(SettingsPath);
            return new Settings
            {
                AutoPotions = bool.Parse(lines[0]),
                BestPotion = bool.Parse(lines[1]),
                NoVolume = bool.Parse(lines[2]),
                SiVolume = bool.Parse(lines[3]),
                TrackBarValue = int.Parse(lines[4]),
                SoundVolume = float.Parse(lines[5])
            };
        }
    }
}
