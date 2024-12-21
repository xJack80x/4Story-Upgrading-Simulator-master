using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Story_Upgrading_Simulator
{
    public static class InventoryManager
    {
        private const string INVENTORY_FOLDER = "inventory";
        private const string INVENTORY_FILE = "inventory_settings.txt";

        public static void SaveInventory(Dictionary<string, int> inventory)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), INVENTORY_FOLDER);
            Directory.CreateDirectory(folderPath);
            string filePath = Path.Combine(folderPath, INVENTORY_FILE);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var item in inventory)
                {
                    writer.WriteLine($"{item.Key}={item.Value}");
                }
            }
        }

        public static Dictionary<string, int> LoadInventory()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), INVENTORY_FOLDER, INVENTORY_FILE);
            Dictionary<string, int> inventory = GetDefaultInventory();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                inventory.Clear();

                foreach (string line in lines)
                {
                    string[] parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        inventory[parts[0]] = int.Parse(parts[1]);
                    }
                }
            }

            return inventory;
        }

        public static Dictionary<string, int> GetDefaultInventory()
        {
            return new Dictionary<string, int>
        {
            {"pergameneMaestro", 120},
            {"tinture", 50},
            {"pozionix100", 120},
            {"pozionix150", 3},
            {"pozionix200", 2},
            {"pozionix300", 1},
            {"riflessioni", 100}
        };
        }
    }


}
