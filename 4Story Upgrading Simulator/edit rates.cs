using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _4Story_Upgrading_Simulator
{
    public partial class edit_rates : Form
    {
        private readonly int serverType;
        private static Dictionary<int, float[]> savedRates = new Dictionary<int, float[]>();

        public edit_rates(int serverType)
        {
            InitializeComponent();

            // Attach handlers to all textboxes
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.KeyPress += TextBox_KeyPress;
                    textBox.Leave += TextBox_Leave;
                }
            }


            this.serverType = serverType;
            // Set the label text based on server type
            LoadRatesFromFile();  // Load any saved rates
            
            label25.Text = Form1.getServerTitle(serverType);

            LoadRates();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoadRates()
        {
            float[] rates;
            if (savedRates.ContainsKey(serverType))
            {
                rates = savedRates[serverType];
            }
            else
            {
                rates = GetDefaultRates();
            }
            textBox1.Text = rates[0].ToString();
            textBox2.Text = rates[1].ToString();
            textBox3.Text = rates[2].ToString();
            textBox4.Text = rates[3].ToString();
            textBox5.Text = rates[4].ToString();
            textBox6.Text = rates[5].ToString();
            textBox7.Text = rates[6].ToString();
            textBox8.Text = rates[7].ToString();
            textBox9.Text = rates[8].ToString();
            textBox10.Text = rates[9].ToString();
            textBox11.Text = rates[10].ToString();
            textBox12.Text = rates[11].ToString();
            textBox13.Text = rates[12].ToString();
            textBox14.Text = rates[13].ToString();
            textBox15.Text = rates[14].ToString();
            textBox16.Text = rates[15].ToString();
            textBox17.Text = rates[16].ToString();
            textBox18.Text = rates[17].ToString();
            textBox19.Text = rates[18].ToString();
            textBox20.Text = rates[19].ToString();
            textBox21.Text = rates[20].ToString();
            textBox22.Text = rates[21].ToString();
            textBox23.Text = rates[22].ToString();
            textBox24.Text = rates[23].ToString();
        }

        private float[] GetDefaultRates()
        {
            switch (serverType)
            {
                case 0: return GetClassicRates();
                case 1: return GetAncientRates();
                case 2: return GetOfficialRates();
                case 3: return GetCustomRates();
                default: return new float[25];
            }
        }

        private void button1_Click(object sender, EventArgs e)  // Save button
        {
            // Save the rates to your variables
            float[] newRates = new float[24];
            newRates[0] = float.Parse(textBox1.Text);
            newRates[1] = float.Parse(textBox2.Text);
            newRates[2] = float.Parse(textBox3.Text);
            newRates[3] = float.Parse(textBox4.Text);
            newRates[4] = float.Parse(textBox5.Text);
            newRates[5] = float.Parse(textBox6.Text);
            newRates[6] = float.Parse(textBox7.Text);
            newRates[7] = float.Parse(textBox8.Text);
            newRates[8] = float.Parse(textBox9.Text);
            newRates[9] = float.Parse(textBox10.Text);
            newRates[10] = float.Parse(textBox11.Text);
            newRates[11] = float.Parse(textBox12.Text);
            newRates[12] = float.Parse(textBox13.Text);
            newRates[13] = float.Parse(textBox14.Text);
            newRates[14] = float.Parse(textBox15.Text);
            newRates[15] = float.Parse(textBox16.Text);
            newRates[16] = float.Parse(textBox17.Text);
            newRates[17] = float.Parse(textBox18.Text);
            newRates[18] = float.Parse(textBox19.Text);
            newRates[19] = float.Parse(textBox20.Text);
            newRates[20] = float.Parse(textBox21.Text);
            newRates[21] = float.Parse(textBox22.Text);
            newRates[22] = float.Parse(textBox23.Text);
            newRates[23] = float.Parse(textBox24.Text);

            // Update the savedRates dictionary
            if (savedRates.ContainsKey(serverType))
            {
                savedRates[serverType] = newRates;
            }
            else
            {
                savedRates.Add(serverType, newRates);
            }


            SaveRatesToFile();  // Save to file after updating savedRates

            Form1.UpdateSuccessChance();
            //DialogResult = DialogResult.OK;
        }

        // Method to get the saved rates from outside the form
        public static float[] GetSavedRates(int serverType)
        {
            return savedRates.ContainsKey(serverType)
                ? savedRates[serverType]
                : Array.Empty<float>();
        }


        public float[] GetCustomRates()
        {
            return new float[24] {
        100, 100, 100, 100, 100,
        100, 100, 100, 100, 100,
        60, 20, 35, 13,
        12, 5, 7, 3.50f, 1,
        0.50f, 0.80f, 0.50f,2,0
    };
        }

        // Add methods to return default rates for each server type
        public float[] GetOfficialRates()
        {
            return new float[24] {
        100, 100, 100, 75, 100,
        45, 90, 30, 75, 15,
        45, 8, 30, 8, 23, 
        5, 15, 5, 12, 3,
        9, 3, 8, 0
    };
        }

        public float[] GetAncientRates()
        {
            return new float[24] {
        100, 100, 100, 100, 100,
        100, 100, 91, 80, 50,
        65, 15, 35, 8, 10,
        5, 3, 3, 3, 1,
        2, 1, 3, 0
    };
        }

        public float[] GetClassicRates()
        {
            return new float[24] {
        100, 100, 100, 100, 100,
        100, 100, 100, 100, 100,
        60, 20, 35, 13,
        12, 5, 7, 3.50f, 1,
        0.50f, 0.80f, 0.50f,2,0
    };
        }
        private void button2_Click(object sender, EventArgs e)  // Default button
        {
            float[] defaultRates = GetDefaultRates();

            // Remove any saved rates for this server type
            if (savedRates.ContainsKey(serverType))
            {
                savedRates.Remove(serverType);
            }

            // Load the default values into textboxes
            textBox1.Text = defaultRates[0].ToString();
            textBox2.Text = defaultRates[1].ToString();
            textBox3.Text = defaultRates[2].ToString();
            textBox4.Text = defaultRates[3].ToString();
            textBox5.Text = defaultRates[4].ToString();
            textBox6.Text = defaultRates[5].ToString();
            textBox7.Text = defaultRates[6].ToString();
            textBox8.Text = defaultRates[7].ToString();
            textBox9.Text = defaultRates[8].ToString();
            textBox10.Text = defaultRates[9].ToString();
            textBox11.Text = defaultRates[10].ToString();
            textBox12.Text = defaultRates[11].ToString();
            textBox13.Text = defaultRates[12].ToString();
            textBox14.Text = defaultRates[13].ToString();
            textBox15.Text = defaultRates[14].ToString();
            textBox16.Text = defaultRates[15].ToString();
            textBox17.Text = defaultRates[16].ToString();
            textBox18.Text = defaultRates[17].ToString();
            textBox19.Text = defaultRates[18].ToString();
            textBox20.Text = defaultRates[19].ToString();
            textBox21.Text = defaultRates[20].ToString();
            textBox22.Text = defaultRates[21].ToString();
            textBox23.Text = defaultRates[22].ToString();
            textBox24.Text = defaultRates[23].ToString();
        }

        // Add this method to handle text validation for all textboxes
        private void TextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Accetta sia punto che virgola come separatore decimale
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
                return;
            }

            if (sender is TextBox textBox)
            {
                // Controlla se c'è già un separatore decimale (punto o virgola)
                if ((e.KeyChar == '.' || e.KeyChar == ',') &&
                    (textBox.Text.Contains('.') || textBox.Text.Contains(',')))
                {
                    e.Handled = true;
                    return;
                }
            }
        }


        private void TextBox_Leave(object? sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string normalizedText = textBox.Text.Replace(',', '.');
                if (float.TryParse(normalizedText,
                    System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out float value))
                {
                    value = Math.Max(0, Math.Min(100, value));
                    textBox.Text = value.ToString("F2",
                        System.Globalization.CultureInfo.CurrentCulture);
                }
                else
                {
                    textBox.Text = "0,00";
                }
            }
        }


        private void edit_rates_Load(object sender, EventArgs e)
        {

        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {

        }

        private static string GetSaveFilePath(int serverType)
        {

            string ratesFolder = Path.Combine(Directory.GetCurrentDirectory(), "rates");

            // Create the rates directory if it doesn't exist
            if (!Directory.Exists(ratesFolder))
            {
                Directory.CreateDirectory(ratesFolder);
            }

            string fileName = $"{Form1.getServerTitle(serverType)}_rates.txt";
            return Path.Combine(ratesFolder, fileName);
        }

        private static void SaveRatesToFile()
        {
            foreach (var kvp in savedRates)
            {
                string filePath = GetSaveFilePath(kvp.Key);
                File.WriteAllLines(filePath, kvp.Value.Select(r => r.ToString()).ToArray());
            }
        }

        private static void LoadRatesFromFile()
        {
            for (int i = 0; i < 4; i++)
            {
                string filePath = GetSaveFilePath(i);
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    float[] rates = lines.Select(l => float.Parse(l)).ToArray();
                    savedRates[i] = rates;
                }
            }
        }
    }
}

