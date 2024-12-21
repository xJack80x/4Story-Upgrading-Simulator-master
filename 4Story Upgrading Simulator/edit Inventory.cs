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
    public partial class edit_Inventory : Form
    {
        private Dictionary<string, int> currentInventory = new();

        public edit_Inventory()
        {
            InitializeComponent();
            InitializeControls();
            LoadCurrentValuesFromForm1();
        }

        private void InitializeControls()
        {
            // For each TextBox, add these event handlers
            textBox1.KeyPress += ValidateNumberInput;
            textBox1.TextChanged += ValidateRange;
            textBox2.KeyPress += ValidateNumberInput;
            textBox2.TextChanged += ValidateRange;
            textBox3.KeyPress += ValidateNumberInput;
            textBox3.TextChanged += ValidateRange;
            textBox4.KeyPress += ValidateNumberInput;
            textBox4.TextChanged += ValidateRange;
            textBox5.KeyPress += ValidateNumberInput;
            textBox5.TextChanged += ValidateRange;
            textBox6.KeyPress += ValidateNumberInput;
            textBox6.TextChanged += ValidateRange;
            textBox7.KeyPress += ValidateNumberInput;
            textBox7.TextChanged += ValidateRange;
            // Repeat for other TextBoxes
        }

        private void LoadCurrentValuesFromForm1()
        {
            // Get current values from Form1
            Dictionary<string, int> currentValues = Form1.GetCurrentInventoryValues();

            // Update textboxes with current values   
            textBox1.Text = currentValues["tinture"].ToString();
            textBox2.Text = currentValues["pozionix100"].ToString();
            textBox3.Text = currentValues["pozionix150"].ToString();
            textBox4.Text = currentValues["pozionix200"].ToString();
            textBox5.Text = currentValues["pozionix300"].ToString();
            textBox6.Text = currentValues["pergameneMaestro"].ToString();
            textBox7.Text = currentValues["riflessioni"].ToString();
        }

        private void UpdateTextBoxes()
        {
            textBox1.Text = currentInventory["tinture"].ToString();
            textBox2.Text = currentInventory["pozionix100"].ToString();
            textBox3.Text = currentInventory["pozionix150"].ToString();
            textBox4.Text = currentInventory["pozionix200"].ToString();
            textBox5.Text = currentInventory["pozionix300"].ToString();
            textBox6.Text = currentInventory["pergameneMaestro"].ToString();
            textBox7.Text = currentInventory["riflessioni"].ToString();
        }


        private void edit_Inventory_Load(object sender, EventArgs e)
        {

        }
        private void ValidateNumberInput(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ValidateRange(object? sender, EventArgs e)
        {
            if (sender is TextBox txt)
            {
                if (txt.Text.Length > 0)
                {
                    if (int.TryParse(txt.Text, out int value))
                    {
                        if (value > 9999)
                            txt.Text = "9999";
                        else if (value < 0)
                            txt.Text = "0";
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> newInventory = new Dictionary<string, int>
        {
            
            {"tinture", int.Parse(textBox1.Text)},
            {"pozionix100", int.Parse(textBox2.Text)},
            {"pozionix150", int.Parse(textBox3.Text)},
            {"pozionix200", int.Parse(textBox4.Text)},
            {"pozionix300", int.Parse(textBox5.Text)},
            {"pergameneMaestro", int.Parse(textBox6.Text)},
            {"riflessioni", int.Parse(textBox7.Text)}
        };
            InventoryManager.SaveInventory(newInventory);
            Form1.UpdateInventoryFromEdit(newInventory);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentInventory = InventoryManager.GetDefaultInventory();
            UpdateTextBoxes();
        }
    }
}
