using System;

namespace _4Story_Upgrading_Simulator
{
    public partial class Form1 : Form
    {
        // Constants
        private const int UPGRADE_MAX = 24;
        private const int POZIONE_FELICITA_X100 = 100;
        private const int POZIONE_FELICITA_X150 = 150;
        private const int POZIONE_FELICITA_X200 = 200;
        private const int POZIONE_FELICITA_X300 = 300;
        private const int PERGAMENA_MAESTRO_VAL = 3;
        private const int PERGAMENA_RIFLESSIONE_VAL = 1;

        // Item state
        private int upgrade = 0;
        private int balestra = 0;
        private int verga = 0;
        private int version = 0;

        // Inventory counts
        private int pergameneMaestro = 120;
        private int tinture = 50;
        private int pozionix100 = 120;
        private int pozionix150 = 3;
        private int pozionix200 = 2;
        private int pozionix300 = 1;
        private int riflessioni = 100;

        // Usage tracking
        private int pozioni100Utilizzate = 0;
        private int pozionix150Utilizzate = 0;
        private int pozionix200Utilizzate = 0;
        private int pozionix300Utilizzate = 0;
        private int pergameneMaestroUtilizzate = 0;
        private int tintureUtilizzate = 0;
        private int riflessioniUtilizzate = 0;
        private int soldi = 0;

        // State flags
        private bool tintura = false;
        private bool ready = true;
        private bool result = false;
        private int pozioneUsata = 1;
        private int pergamenaUsata = 0;


        private static Form1? instance;

        System.Media.SoundPlayer cash = new System.Media.SoundPlayer(Directory.GetCurrentDirectory() + @"\sounds\cash.wav");
        System.Media.SoundPlayer loading = new System.Media.SoundPlayer(Directory.GetCurrentDirectory() + @"\sounds\loading.wav");
        System.Media.SoundPlayer up = new System.Media.SoundPlayer(Directory.GetCurrentDirectory() + @"\sounds\up.wav");
        System.Media.SoundPlayer fail = new System.Media.SoundPlayer(Directory.GetCurrentDirectory() + @"\sounds\fail.wav");
        System.Media.SoundPlayer select = new System.Media.SoundPlayer(Directory.GetCurrentDirectory() + @"\sounds\select.wav");

        public Form1()
        {
            InitializeComponent();
            LoadInventory();
            instance = this;
            // Load saved rates at startup
            LoadSavedRatesForAllServers();

            label15.Text = upgrade.ToString();
            label10.Text = tinture.ToString();
            label11.Text = pozionix100.ToString();
            label15.Text = pergameneMaestro.ToString();
            label12.Text = pozionix150.ToString();
            label13.Text = pozionix200.ToString();
            label21.Text = pozionix300.ToString();
            label14.Text = riflessioni.ToString();
            label7.Text = "Money spent: " + soldi + " euros"; label7.Update();
            label20.Text = "Server: " + getServerTitle(version);

            for (int i = 0; i <= 6; i++)
                aggiornaContatori(i);

        }

        private void LoadInventory()
        {
            var inventory = InventoryManager.LoadInventory();
            pergameneMaestro = inventory["pergameneMaestro"];
            tinture = inventory["tinture"];
            pozionix100 = inventory["pozionix100"];
            pozionix150 = inventory["pozionix150"];
            pozionix200 = inventory["pozionix200"];
            pozionix300 = inventory["pozionix300"];
            riflessioni = inventory["riflessioni"];

            UpdateInventoryLabels();
        }

        private void UpdateInventoryLabels()
        {
            label15.Text = pergameneMaestro.ToString();
            label10.Text = tinture.ToString();
            label11.Text = pozionix100.ToString();
            label12.Text = pozionix150.ToString();
            label13.Text = pozionix200.ToString();
            label21.Text = pozionix300.ToString();
            label14.Text = riflessioni.ToString();
        }

        // After any inventory change (using items, buying manastones, etc.)
        private void UpdateInventoryAfterChange()
        {
            Dictionary<string, int> currentInventory = new Dictionary<string, int>
    {
        {"pergameneMaestro", pergameneMaestro},
        {"tinture", tinture},
        {"pozionix100", pozionix100},
        {"pozionix150", pozionix150},
        {"pozionix200", pozionix200},
        {"pozionix300", pozionix300},
        {"riflessioni", riflessioni}
    };

            // Update labels
            UpdateInventoryLabels();

            // Save to file
            InventoryManager.SaveInventory(currentInventory);

        }
        
        public static Dictionary<string, int> GetCurrentInventoryValues()
        {
            return new Dictionary<string, int>
    {
        {"pergameneMaestro", instance?.pergameneMaestro ?? 0},
        {"tinture", instance?.tinture ?? 0},
        {"pozionix100", instance?.pozionix100 ?? 0},
        {"pozionix150", instance?.pozionix150 ?? 0},
        {"pozionix200", instance?.pozionix200 ?? 0},
        {"pozionix300", instance?.pozionix300 ?? 0},
        {"riflessioni", instance?.riflessioni ?? 0}
    };
        }
        public static string getServerTitle(int serverType)
        {
            string temp_title;

            switch (serverType)
            {
                case 0:
                    temp_title = "4Classic";
                    break;
                case 1:
                    temp_title = "4Ancient";
                    break;
                case 2:
                    temp_title = "Official";
                    break;
                case 3:
                    temp_title = "Custom";
                    break;
                default:
                    temp_title = "error";
                    break;

            }
            return temp_title;
        }
        private void LoadSavedRatesForAllServers()
        {
            string ratesFolder = Path.Combine(Directory.GetCurrentDirectory(), "rates");

            // Create the rates directory if it doesn't exist
            if (!Directory.Exists(ratesFolder))
            {
                Directory.CreateDirectory(ratesFolder);
            }

            // Load rates for each server type (0=Classic, 1=Ancient, 2=Official, 3=Custom)
            for (int serverType = 0; serverType < 4; serverType++)
            {
                string filePath = Path.Combine(ratesFolder, $"{getServerTitle(serverType)}_rates.txt");
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    float[] rates = lines.Select(l => float.Parse(l)).ToArray();

                    // Store the rates in the static Dictionary in edit_rates
                    if (rates.Length > 0)
                    {
                        edit_rates.GetSavedRates(serverType);
                    }
                }
            }
        }

        private string upgrading(int pozione, int pergamena, int up, int bRand, float bProb)
        {


            string risultato = "";

            if (pergamena == PERGAMENA_RIFLESSIONE_VAL && upgrade > 0)
            {
                upgrade--;
                if (pictureBox19.Visible == true)
                    balestra = upgrade;
                else
                    verga = upgrade;

                pictureBox12.Visible = true;
                pictureBox13.Visible = true;

                if (riflessioni == 0)
                    pictureBox20.Visible = false;

                label9.Visible = true;
                label9.Text = show_upgrade(0, upgrade, up, 0);
                label9.Update();



            }
            else if (pergamena == PERGAMENA_MAESTRO_VAL && upgrade < 24)
            {

                if (bRand < bProb)
                {
                    Random rand2 = new Random();

                    up = pergamena == 3 ? (rand2.Next(0, 99) % 3) + 1 : 1;
                    upgrade += up;

                    if (upgrade > UPGRADE_MAX)
                        upgrade = UPGRADE_MAX;

                    if (pictureBox19.Visible == true)
                        balestra = upgrade;
                    else
                        verga = upgrade;


                    pictureBox12.Visible = true;
                    pictureBox13.Visible = true;
                    label9.Visible = true;

                    label9.Text = show_upgrade(1, upgrade, up, 0);
                    label9.Update();
                }
                else
                {
                    if (tintura == true)
                    {
                        int bLevelGuard = 11;
                        int bDownProb = 0;
                        int bDownGrade = 0;
                        Random rand3 = new Random();

                        if (upgrade <= bLevelGuard)
                            bDownProb = 0;
                        else
                            bDownProb = rand3.Next(0, 99) % 100;

                        if (bDownProb < 20)
                            bDownGrade = 0;
                        else if (bDownProb < 55)
                            bDownGrade = 1;
                        else
                            bDownGrade = 2;

                        if (upgrade > bLevelGuard && upgrade - bDownGrade < bLevelGuard)
                            bDownGrade = upgrade - bLevelGuard;

                        upgrade = upgrade - bDownGrade;

                        if (pictureBox19.Visible == true)
                            balestra = upgrade;
                        else
                            verga = upgrade;

                        tintura = false;
                        pictureBox21.Visible = false;
                        label9.Text = upgrade.ToString();
                        label9.Update();


                        pictureBox16.Visible = true;
                        pictureBox15.Visible = true;
                        label9.Text = show_upgrade(2, upgrade, up, bDownGrade);
                        label9.Update();
                    }
                    else
                    {
                        upgrade = 0;
                        if (pictureBox19.Visible == true)
                            balestra = upgrade;
                        else
                            verga = upgrade;


                        tintura = false;
                        pictureBox16.Visible = true;
                        pictureBox15.Visible = true;

                        label9.Text = show_upgrade(3, upgrade, up, 0);
                        label9.Update();
                    }

                }


                if (pozioneUsata == POZIONE_FELICITA_X100)
                    pictureBox22.Visible = false;
                else if (pozioneUsata == POZIONE_FELICITA_X150)
                    pictureBox23.Visible = false;
                else if (pozioneUsata == POZIONE_FELICITA_X200)
                    pictureBox24.Visible = false;
                else if (pozioneUsata == POZIONE_FELICITA_X300)
                    pictureBox29.Visible = false;


                pozioneUsata = 1;

                if (pergameneMaestro == 0)
                    pictureBox6.Visible = false;

                label16.Text = show_success_chance();
                label16.Update();


            }


            label18.Text = "+" + verga.ToString();
            label19.Text = "+" + balestra.ToString();
            return risultato;
        }

        public float check_prob_custom(int val)
        {
            if (val == 0) return 100.00f;
            float[] savedRates = edit_rates.GetSavedRates(3);
            if (savedRates != null && (val - 1) < savedRates.Length)
            {
                return savedRates[val - 1];
            }
            return new edit_rates(3).GetClassicRates()[val - 1];
        }

        public float check_prob_official(int val)
        {
            if (val == 0) return 100.00f;
            float[] savedRates = edit_rates.GetSavedRates(2);
            if (savedRates != null && (val - 1) < savedRates.Length)
            {
                return savedRates[val - 1];
            }
            return new edit_rates(2).GetClassicRates()[val - 1];
        }

        public float check_prob_ancient(int val)
        {
            if (val == 0) return 100.00f;
            float[] savedRates = edit_rates.GetSavedRates(1);
            if (savedRates != null && (val - 1) < savedRates.Length)
            {
                return savedRates[val - 1];
            }
            return new edit_rates(1).GetClassicRates()[val - 1];
        }

        public float check_prob(int val)
        {
            if (val == 0) return 100.00f;
            float[] savedRates = edit_rates.GetSavedRates(0);
            if (savedRates != null && (val - 1) < savedRates.Length)
            {
                return savedRates[val - 1];
            }
            return new edit_rates(0).GetClassicRates()[val - 1];
        }

        public float CalcProb(float bBaseProb, int pozione)
        {
            // If upgrade is 0, always return 100%
            if (upgrade == 0)
                return 100.00f;

            float wAddProb = 0;
            if (bBaseProb >= 100 || pozione == 1)
                wAddProb = bBaseProb;
            else
                // This is the key change - multiply by (1 + pozione/100) instead of just pozione/100
                wAddProb = bBaseProb * (1 + (float)pozione / 100);

            if (wAddProb > 100)
                wAddProb = 100;

            return (float)Math.Round(wAddProb, 2);
        }

        private string show_numbers(int bRand, float bProb)
        {
            label17.Visible = true;
            return "Random number: " + bRand + "  Chance: " + bProb + "%";
        }

        private string show_success_chance()
        {
            label16.Visible = true;

            // If upgrade is 0, return 100% success rate
            if (upgrade == 0)
            {
                return "Next success chance: 100.00%";
            }

            // Show current level's success rate instead of next level
            switch (version)
            {
                case 0: return "Next success chance: " + CalcProb(check_prob(upgrade), pozioneUsata).ToString("F2") + "%";
                case 1: return "Next Success chance: " + CalcProb(check_prob_ancient(upgrade), pozioneUsata).ToString("F2") + "%";
                case 2: return "Next Success chance: " + CalcProb(check_prob_official(upgrade), pozioneUsata).ToString("F2") + "%";
                case 3: return "Next Success chance: " + CalcProb(check_prob_custom(upgrade), pozioneUsata).ToString("F2") + "%";
                default: return "";
            }

        }

        public static void UpdateSuccessChance()
        {
            if (instance != null)
            {
                instance.label16.Text = instance.show_success_chance();
            }
        }

        public static void UpdateInventoryFromEdit(Dictionary<string, int> inventory)
        {
            if (instance != null)
            {
                instance.pergameneMaestro = inventory["pergameneMaestro"];
                instance.tinture = inventory["tinture"];
                instance.pozionix100 = inventory["pozionix100"];
                instance.pozionix150 = inventory["pozionix150"];
                instance.pozionix200 = inventory["pozionix200"];
                instance.pozionix300 = inventory["pozionix300"];
                instance.riflessioni = inventory["riflessioni"];
                // Update labels
                instance.UpdateInventoryLabels();
            }
        }


        private string show_upgrade(int result, int upgrade, int up, int bDownGrade)
        {
            label9.Visible = true;

            if (pictureBox19.Visible == true)
                label8.Text = "Dragon Hunter's Crossbow +" + balestra;
            else
                label8.Text = "Sepira Rod +" + verga;

            //label8.Visible = true;

            int oldlevel = upgrade + 1;
            int down = upgrade + bDownGrade;
            switch (result)
            {
                case 0: return "[4S] An Item has been succesfully deleveled from level +" + oldlevel + " to +" + upgrade;
                case 1: return "[4S] An Item has been succesfully improved to level +" + upgrade + ". Increased by " + up;
                case 2: return "[4S] An Item failed from level +" + down + ". Decreased by " + bDownGrade;
                case 3: return "[4S] An Item has been broken because you didn't use a tinture";
                default: return "";
            }
        }

        private void aggiornaContatori(int val)
        {
            switch (val)
            {
                case 0: label2.Text = "Serendipity Potion used: " + pozioni100Utilizzate; label2.Update(); break;
                case 1: label1.Text = "Tinture used: " + tintureUtilizzate; label1.Update(); break;
                case 2: label5.Text = "Reflections used: " + riflessioniUtilizzate; label5.Update(); break;
                case 3: label6.Text = "Master formula used: " + pergameneMaestroUtilizzate; label6.Update(); break;
                case 4: label3.Text = "Serendipity Potion 150% used: " + pozionix150Utilizzate; label3.Update(); break;
                case 5: label4.Text = "Serendipity Potion 200% used: " + pozionix200Utilizzate; label4.Update(); break;
                case 6: label22.Text = "Serendipity Potion 300% used: " + pozionix300Utilizzate; label22.Update(); break;
            }

        }
        private void playSounds(int value)
        {
            try
            {
                switch (value)
                {
                    case 0:
                        if (File.Exists(Directory.GetCurrentDirectory() + @"\sounds\select.wav"))
                            select.Play();
                        break;
                    case 1:
                        if (File.Exists(Directory.GetCurrentDirectory() + @"\sounds\loading.wav"))
                            loading.Play();
                        break;
                    case 2:
                        if (File.Exists(Directory.GetCurrentDirectory() + @"\sounds\up.wav"))
                            up.Play();
                        break;
                    case 3:
                        if (File.Exists(Directory.GetCurrentDirectory() + @"\sounds\fail.wav"))
                            fail.Play();
                        break;
                }

            }
            catch (FileNotFoundException)
            {

            }
        }
        private async Task CountDown()
        {
            int timeToComplete = 100; //Time in seconds
            playSounds(1);
            progressBar1.Minimum = 0;
            progressBar1.Maximum = timeToComplete;
            progressBar1.Value = 0;

            for (int i = 1; i <= timeToComplete; i++)
            {
                progressBar1.Value = i;
                progressBar1.Value = i - 1;
                await Task.Delay(1); // Replace Thread.Sleep with Task.Delay
            }
            progressBar1.Value = 100;

            if (result)
            {
                playSounds(2);
            }
            else
            {
                playSounds(3);
            }
        }



        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            pictureBox12.Visible = false;
            pictureBox13.Visible = false;
            ready = true;
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            pictureBox15.Visible = false;
            pictureBox16.Visible = false;
            ready = true;
        }

        private async void pictureBox14_Click(object sender, EventArgs e)
        {
            if ((pictureBox17.Visible == true || pictureBox19.Visible == true) && (pergamenaUsata > 0 && ready == true && pictureBox12.Visible == false && pictureBox16.Visible == false))
            {

                bool ok = false;
                Random rand = new Random();
                int number = rand.Next(0, 99); //returns random number between 0-99
                int bRand = number % 100;
                float bProb = 0;

                if (pergamenaUsata == PERGAMENA_RIFLESSIONE_VAL)
                {

                    if (upgrade > 0)
                    {
                        playSounds(0);

                        switch (version)
                        {
                            case 0: bProb = CalcProb(check_prob(upgrade), pozioneUsata); break;
                            case 1: bProb = CalcProb(check_prob_ancient(upgrade), pozioneUsata); break;
                            case 2: bProb = CalcProb(check_prob_official(upgrade), pozioneUsata); break;
                            case 3: bProb = CalcProb(check_prob_custom(upgrade), pozioneUsata); break;
                            default: break;
                        }

                        //label17.Text = show_numbers(bRand, bProb);

                        if (bRand < bProb)
                            result = true;
                        else
                            result = false;

                        result = true;
                        ready = false;
                        progressBar1.Enabled = true;
                        progressBar1.Visible = true;
                        await CountDown();

                        riflessioniUtilizzate++;
                        riflessioni--;
                        ok = true;
                    }
                }
                else
                {
                    playSounds(0);
                    switch (version)
                    {
                        case 0: bProb = CalcProb(check_prob(upgrade), pozioneUsata); break;
                        case 1: bProb = CalcProb(check_prob_ancient(upgrade), pozioneUsata); break;
                        case 2: bProb = CalcProb(check_prob_official(upgrade), pozioneUsata); break;
                        case 3: bProb = CalcProb(check_prob_custom(upgrade), pozioneUsata); break;
                        default: break;
                    }
                    label17.Text = show_numbers(bRand, bProb);

                    if (bRand < bProb)
                        result = true;
                    else
                        result = false;

                    if (pozioneUsata == POZIONE_FELICITA_X100)
                    {
                        pozionix100--;
                        pozioni100Utilizzate++;
                        aggiornaContatori(0);
                    }
                    else if (pozioneUsata == POZIONE_FELICITA_X150)
                    {
                        pozionix150--;
                        pozionix150Utilizzate++;
                        aggiornaContatori(4);
                    }
                    else if (pozioneUsata == POZIONE_FELICITA_X200)
                    {
                        pozionix200--;
                        pozionix200Utilizzate++;
                        aggiornaContatori(5);
                    }
                    else if (pozioneUsata == POZIONE_FELICITA_X300)
                    {
                        pozionix300--;
                        pozionix300Utilizzate++;
                        aggiornaContatori(6);
                    }

                    ready = false;
                    progressBar1.Enabled = true;
                    progressBar1.Visible = true;
                    await CountDown();
                    pergameneMaestroUtilizzate++;
                    pergameneMaestro--;
                    UpdateInventoryAfterChange();
                    ok = true;
                }

                if (ok == true)
                {

                    for (int i = 0; i < 6; i++)
                        aggiornaContatori(i);

                    progressBar1.Value = 0;
                    upgrading(pozioneUsata, pergamenaUsata, upgrade, bRand, bProb).ToString();
                    //modifica
                    UpdateInventoryAfterChange();
                    progressBar1.Enabled = false;
                    progressBar1.Visible = false;

                    if (pergameneMaestro == 0 || riflessioni == 0)
                        pergamenaUsata = 0;
                }
            }
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                if (File.Exists(Directory.GetCurrentDirectory() + @"\sounds\cash.wav"))
                    cash.Play();
            }
            catch (FileNotFoundException)
            {

            }


            System.Threading.Thread.Sleep(600);
            pergameneMaestro += 120;
            tinture += 50;
            pozionix100 += 120;
            pozionix150 += 3;
            pozionix200 += 2;
            pozionix300 += 1;
            riflessioni += 100;
            soldi += 50;
            UpdateInventoryAfterChange();
            label7.Text = "Money spent: " + soldi + " euros"; label7.Update();

            for (int i = 0; i < 6; i++)
                aggiornaContatori(i);

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox22.Visible == false)
            {
                if (pozionix100 > 0)
                {
                    //select.Play();
                    pozioneUsata = POZIONE_FELICITA_X100;
                    label11.Text = pozionix100.ToString();
                    pictureBox22.Visible = true;
                    pictureBox23.Visible = false;
                    pictureBox24.Visible = false;
                    pictureBox29.Visible = false;
                    if (pictureBox20.Visible == false)
                    {
                        label16.Visible = true;
                        label16.Text = show_success_chance();
                        label16.Update();
                    }
                }
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox21.Visible == false)
            {
                if (tinture > 0)
                {
                    //select.Play();
                    tinture--;
                    tintureUtilizzate++;
                    aggiornaContatori(1);
                    label10.Text = tinture.ToString();
                    tintura = true;
                    pictureBox21.Visible = true;
                }
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox21.Visible == false)
            {
                if (tinture > 0)
                {
                    //select.Play();
                    tinture--;
                    tintureUtilizzate++;
                    aggiornaContatori(1);
                    label10.Text = tinture.ToString();
                    tintura = true;
                    pictureBox21.Visible = true;
                }
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox22.Visible == false)
            {
                if (pozionix100 > 0)
                {
                    //select.Play();
                    pozioneUsata = POZIONE_FELICITA_X100;
                    label11.Text = pozionix100.ToString();
                    pictureBox23.Visible = false;
                    pictureBox24.Visible = false;
                    pictureBox29.Visible = false;
                    pictureBox22.Visible = true;
                    if (pictureBox20.Visible == false)
                    {
                        label16.Visible = true;
                        label16.Text = show_success_chance();
                        label16.Update();
                    }
                }
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox23.Visible == false)
            {
                if (pozionix150 > 0)
                {
                    //select.Play();
                    pozioneUsata = POZIONE_FELICITA_X150;
                    label12.Text = pozionix150.ToString();
                    pictureBox22.Visible = false;
                    pictureBox24.Visible = false;
                    pictureBox29.Visible = false;
                    pictureBox23.Visible = true;
                    if (pictureBox20.Visible == false)
                    {
                        label16.Visible = true;
                        label16.Text = show_success_chance();
                        label16.Update();
                    }
                }
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox23.Visible == false)
            {
                if (pozionix150 > 0)
                {
                    //select.Play();
                    pozioneUsata = POZIONE_FELICITA_X150;
                    label12.Text = pozionix150.ToString();
                    pictureBox22.Visible = false;
                    pictureBox24.Visible = false;
                    pictureBox29.Visible = false;
                    pictureBox23.Visible = true;
                    if (pictureBox20.Visible == false)
                    {
                        label16.Visible = true;
                        label16.Text = show_success_chance();
                        label16.Update();
                    }
                }
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox24.Visible == false)
            {
                if (pozionix200 > 0)
                {
                    //select.Play();
                    pozioneUsata = POZIONE_FELICITA_X200;
                    label13.Text = pozionix200.ToString();
                    pictureBox22.Visible = false;
                    pictureBox23.Visible = false;
                    pictureBox29.Visible = false;
                    pictureBox24.Visible = true;
                    if (pictureBox20.Visible == false)
                    {
                        label16.Visible = true;
                        label16.Text = show_success_chance();
                        label16.Update();
                    }
                }
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox24.Visible == false)
            {
                if (pozionix200 > 0)
                {
                    // select.Play();
                    pozioneUsata = POZIONE_FELICITA_X200;
                    label13.Text = pozionix200.ToString();
                    pictureBox22.Visible = false;
                    pictureBox23.Visible = false;
                    pictureBox29.Visible = false;
                    pictureBox24.Visible = true;
                    if (pictureBox20.Visible == false)
                    {
                        label16.Visible = true;
                        label16.Text = show_success_chance();
                        label16.Update();
                    }
                }
            }
        }

        private void label15_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox18.Visible == false)
            {
                if (pergameneMaestro > 0)
                {
                    //select.Play();
                    pictureBox18.Visible = true;
                    pictureBox20.Visible = false;
                    label17.Visible = false;
                    pergamenaUsata = PERGAMENA_MAESTRO_VAL;
                    label16.Text = show_success_chance();
                }
            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox18.Visible == false)
            {
                if (pergameneMaestro > 0)
                {
                    //select.Play();
                    pictureBox18.Visible = true;
                    pictureBox20.Visible = false;
                    label17.Visible = false;
                    pergamenaUsata = PERGAMENA_MAESTRO_VAL;
                    label16.Visible = true;
                    label16.Text = show_success_chance();
                }
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox20.Visible == false)
            {
                if (riflessioni > 0)
                {
                    //select.Play();
                    pictureBox18.Visible = false;
                    pictureBox20.Visible = true;
                    label17.Visible = false;
                    label16.Visible = false;
                    pergamenaUsata = PERGAMENA_RIFLESSIONE_VAL;
                }
            }
        }

        private void label14_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox20.Visible == false)
            {
                if (riflessioni > 0)
                {
                    pictureBox18.Visible = false;
                    pictureBox20.Visible = true;
                    label17.Visible = false;
                    label16.Visible = false;
                    pergamenaUsata = PERGAMENA_RIFLESSIONE_VAL;
                }
            }
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox19.Visible == false)
            {
                pictureBox19.Visible = true;
                pictureBox17.Visible = false;
                upgrade = balestra;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox17.Visible == false)
            {
                pictureBox17.Visible = true;
                pictureBox19.Visible = false;
                upgrade = verga;
            }
        }

        private void label19_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox19.Visible == false)
            {
                pictureBox19.Visible = true;
                pictureBox17.Visible = false;
                upgrade = balestra;
            }
        }

        private void label18_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox17.Visible == false)
            {
                pictureBox17.Visible = true;
                pictureBox19.Visible = false;
                upgrade = verga;
            }
        }

        private void change_server(int val)
        {
            switch (val)
            {
                case 0: MessageBox.Show("4Classic Upgrading rates has been added"); break;
                case 1: MessageBox.Show("4Ancient Upgrading rates has been added"); break;
                case 2: MessageBox.Show("4Story Official Upgrading rates has been added"); break;
                default: break;
            }


        }

        private void resetInfo(int xversion)
        {
            playSounds(0);
            upgrade = 0;
            balestra = 0;
            verga = 0;
            tintura = false;
            pergameneMaestro = 120;
            tinture = 50;
            pozionix100 = 120;
            pozionix150 = 3;
            pozionix200 = 2;
            pozionix300 = 1;
            riflessioni = 100;
            pozioneUsata = 1;
            pergamenaUsata = 0;
            ready = true;
            pozioni100Utilizzate = 0;
            pozionix150Utilizzate = 0;
            pozionix200Utilizzate = 0;
            pozionix300Utilizzate = 0;
            pergameneMaestroUtilizzate = 0;
            tintureUtilizzate = 0;
            riflessioniUtilizzate = 0;
            balestra = 0;
            verga = 0;
            soldi = 0;
            result = false;
            version = xversion;
            pictureBox12.Visible = false;
            pictureBox13.Visible = false;
            pictureBox15.Visible = false;
            pictureBox16.Visible = false;
            pictureBox17.Visible = false;
            pictureBox18.Visible = false;
            pictureBox19.Visible = false;
            pictureBox20.Visible = false;
            pictureBox21.Visible = false;
            pictureBox22.Visible = false;
            pictureBox23.Visible = false;
            pictureBox24.Visible = false;
            pictureBox29.Visible = false;
            label9.Visible = false;
            UpdateInventoryAfterChange();
            label7.Text = "Money spent: 0 euros";
            label16.Visible = false;
            label16.Update();
            label17.Visible = false;
            label17.Update();
            label18.Text = "+0";
            label19.Text = "+0";

            for (int i = 0; i < 6; i++)
                aggiornaContatori(i);

            label20.Text = "Server: " + getServerTitle(xversion);
        }


        private void pictureBox25_Click(object sender, EventArgs e)
        {
            resetInfo(0);
        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {
            resetInfo(1);
        }

        private void pictureBox27_Click(object sender, EventArgs e)
        {
            resetInfo(2);
        }

        private void custom_server_Click(object sender, EventArgs e)
        {
            resetInfo(3);
        }

        private void edit4storyOfficialRatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var editForm = new edit_rates(2))
            {
                editForm.ShowDialog();
            }
        }

        private void edit4ancientRatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var editForm = new edit_rates(1))
            {
                editForm.ShowDialog();
            }
        }

        private void edit4classicRatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var editForm = new edit_rates(0))
            {
                editForm.ShowDialog();
            }

        }


        private void editCustomServerRatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var editForm = new edit_rates(3))
            {
                editForm.ShowDialog();
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void pictureBox28_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox23.Visible == false)
            {
                if (pozionix300 > 0)
                {
                    pozioneUsata = POZIONE_FELICITA_X300;
                    label21.Text = pozionix300.ToString();
                    pictureBox22.Visible = false;
                    pictureBox23.Visible = false;
                    pictureBox24.Visible = false;
                    pictureBox29.Visible = true;
                    if (pictureBox20.Visible == false)
                    {
                        label16.Visible = true;
                        label16.Text = show_success_chance();
                        label16.Update();
                    }
                }
            }
        }

        private void label21_Click(object sender, EventArgs e)
        {
            if (pictureBox12.Visible == false && pictureBox16.Visible == false && pictureBox24.Visible == false)
            {
                if (pozionix300 > 0)
                {
                    pozioneUsata = POZIONE_FELICITA_X300;
                    label21.Text = pozionix300.ToString();
                    pictureBox22.Visible = false;
                    pictureBox23.Visible = false;
                    pictureBox24.Visible = false;
                    pictureBox29.Visible = true;
                    if (pictureBox20.Visible == false)
                    {
                        label16.Visible = true;
                        label16.Text = show_success_chance();
                        label16.Update();
                    }
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void editInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var editForm = new edit_Inventory())
            {
                editForm.ShowDialog();
            }
        }
    }
}