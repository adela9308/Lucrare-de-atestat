using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PARADISUL_ANIMAL
{
   
    public partial class Form1 : Form
    {
        void adaugare_lista(List<string> animale, string de_adaugat)
        {
            int ok = 1;
            foreach (string s in animale)
                if (de_adaugat.IndexOf(s) != -1) ok = 0;
            if (ok == 1) animale.Add(de_adaugat);
        }
        void afisare_lista(List<string> animale)
        {
            foreach (string s in animale)
                listBox5.Items.Add(s);
               
        }

        int steluta = 0;
        int random_nr;

        public Form1()
        {
            //full screen
            InitializeComponent();
            TopMost = true;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;     
           
         }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.STELUTE' table. You can move, or remove it, as needed.
            this.sTELUTETableAdapter.Fill(this.database1DataSet.STELUTE);
            // TODO: This line of code loads data into the 'database1DataSet.SPECII' table. You can move, or remove it, as needed.
            this.sPECIITableAdapter.Fill(this.database1DataSet.SPECII);
            // TODO: This line of code loads data into the 'database1DataSet.SUBSPECII' table. You can move, or remove it, as needed.
            this.sUBSPECIITableAdapter.Fill(this.database1DataSet.SUBSPECII);
            // TODO: This line of code loads data into the 'database1DataSet.CATEGORII' table. You can move, or remove it, as needed.
            this.cATEGORIITableAdapter.Fill(this.database1DataSet.CATEGORII);
            this.cATEGORIITableAdapter.FillByorder_categorie(this.database1DataSet.CATEGORII);
            DataTable dt = database1DataSet.CATEGORII;
            for (int i = 0; i < dt.Rows.Count; i++)
                comboBox1.Items.Add(dt.Rows[i]["Categorie"]);
            // TODO: This line of code loads data into the 'database1DataSet.OPINII' table. You can move, or remove it, as needed.
            this.oPINIITableAdapter.Fill(this.database1DataSet.OPINII);
            

            //animalul zilei
            Random rnd = new Random();
            random_nr = rnd.Next(8, 52);
            this.sUBSPECIITableAdapter.Fill(this.database1DataSet.SUBSPECII);
            DataTable dt1 = this.database1DataSet.SUBSPECII;
            {
                Bitmap image = new Bitmap("E:\\PARADISUL ANIMAL\\imagini\\" + dt1.Rows[random_nr]["Poza"].ToString());
                pictureBox17.Image = (Image)image;
                label17.Text = dt1.Rows[random_nr]["Subspecie"].ToString();
            }

            //Timer
            label30.Visible = false;
            timer1.Start();
            timer1.Interval = 500;
            timer1.Tick += new EventHandler(timer1_Tick);
          
            

            //Statistica stelute-chart
            chart1.Visible = true;
            int maxim = 0;
            int one = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("1").ToString());
            int two = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("2").ToString());
            if (one > two) maxim = one;
            else maxim = two;
            int three = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("3").ToString());
            if (maxim < three) maxim = three;
            int four = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("4").ToString());
            if (maxim < four) maxim = four;
            int five = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("5").ToString());
            if (maxim < five) maxim = five;

            this.chart1.Series["1 STEA"].Points.AddXY(2, one);
            this.chart1.Series["2 STELE"].Points.AddXY(2.5, two);
            this.chart1.Series["3 STELE"].Points.AddXY(3, three);
            this.chart1.Series["4 STELE"].Points.AddXY(3.5, four);
            this.chart1.Series["5 STELE"].Points.AddXY(4, five);

            chart1.Series["1 STEA"]["PixelPointWidth"] = "170";
            chart1.Series["2 STELE"]["PixelPointWidth"] = "170";
            chart1.Series["3 STELE"]["PixelPointWidth"] = "170";
            chart1.Series["4 STELE"]["PixelPointWidth"] = "170";
            chart1.Series["5 STELE"]["PixelPointWidth"] = "170";
 
        }


        private void iNFORMATIIUTILEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void pREZENTAREToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void pAGINAPRINCIPALAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
  
        private void oPINIAVIZITATORILORToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            this.oPINIITableAdapter.Fill(this.database1DataSet.OPINII);
            DataTable dt = this.database1DataSet.OPINII;
            listBox1.Items.Add("PĂRERI ANTERIOARE:");
            for (int i = 0; i < dt.Rows.Count; i++)
                listBox1.Items.Add(dt.Rows[i]["Nume"] + ":" + dt.Rows[i]["Opinie"]);
            tabControl1.SelectedIndex = 4;

        }

        private void button1_Click(object sender, EventArgs e)//ADAUGA OPINIE
        {
            string litere = "abcdefghijklmnopqrstuvxyzw";
            int nume = 0, opinie = 0;
            for (int i = 0; i < litere.Length; i++)
            {
                if (textBox1.Text.IndexOf(litere[i]) != -1) nume++;
                if (textBox2.Text.IndexOf(litere[i]) != -1) opinie++;
                
            }

            if (nume < 2 || opinie < 2)
            { MessageBox.Show("Nu ai introdus datele corespuzatoare"); textBox1.Text = ""; textBox2.Text = ""; }
            else
            {

                this.oPINIITableAdapter.Fill(this.database1DataSet.OPINII);
                DataTable dt = this.database1DataSet.OPINII;

                int gasit = 0;//verific daca mai exista acest nume
                for (int i = 0; i < dt.Rows.Count; i++)
                    if (dt.Rows[i]["Nume"].ToString() == textBox1.Text.ToString()) gasit = 1;

                if (textBox1.Text == "") MessageBox.Show("Nu uita să îți introduci numele!");
                else if (gasit != 0) MessageBox.Show("Deja a mai fost adaugata o opinie pe acest nume.");
                else if (textBox2.Text == "") MessageBox.Show("Nu ne-ai spus părerea ta!");
                else
                {
                    MessageBox.Show("Mulțumim pentru opinie!");
                    this.oPINIITableAdapter.InsertQuery_opinie(textBox2.Text.ToString(), textBox1.Text.ToString());
                    listBox1.Items.Add(textBox1.Text.ToString() + ": " + textBox2.Text.ToString());
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)//vizualizeaza opinia celorlalti
        {
            listBox1.Items.Clear();
            this.oPINIITableAdapter.Fill(this.database1DataSet.OPINII);
            DataTable dt = this.database1DataSet.OPINII;
            listBox1.Items.Add("PĂRERI ANTERIOARE:");
            for (int i = 0; i < dt.Rows.Count; i++)
                listBox1.Items.Add(dt.Rows[i]["Nume"]+":" + dt.Rows[i]["Opinie"]);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)//1 steluta
        {

            if (steluta == 0 && radioButton1.Checked == true)
            {
                steluta = 1;
                DateTime x = DateTime.Now;
                if (radioButton1.Checked == true)
                {
                    MessageBox.Show("Mulțumim pentru steluța acordată!");
                    this.sTELUTETableAdapter.InsertQuery_stelute("1", x);
                }
                radioButton1.Checked = false;
                comboBox2.SelectedItem = "Total stelute";
                listBox4.Items.Clear();
                {
                    listBox4.Items.Add("Total stelute: ");

                    listBox4.Items.Add("★★★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 5 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("5"));

                    listBox4.Items.Add("★★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 4 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("4"));

                    listBox4.Items.Add("★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 3 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("3"));

                    listBox4.Items.Add("★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 2 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("2"));

                    listBox4.Items.Add("★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 1 steluta= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("1"));

                    //Statistica stelute
                    chart1.Visible = true;
                    int maxim = 0;
                    int one = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("1").ToString());
                    int two = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("2").ToString());
                    if (one > two) maxim = one;
                    else maxim = two;
                    int three = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("3").ToString());
                    if (maxim < three) maxim = three;
                    int four = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("4").ToString());
                    if (maxim < four) maxim = four;
                    int five = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("5").ToString());
                    if (maxim < five) maxim = five;


                    this.chart1.Series["1 STEA"].Points.AddXY(2, one);

                    this.chart1.Series["2 STELE"].Points.AddXY(2.5, two);

                    this.chart1.Series["3 STELE"].Points.AddXY(3, three);

                    this.chart1.Series["4 STELE"].Points.AddXY(3.5, four);

                    this.chart1.Series["5 STELE"].Points.AddXY(4, five);

                    chart1.Series["1 STEA"]["PixelPointWidth"] = "170";
                    chart1.Series["2 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["3 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["4 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["5 STELE"]["PixelPointWidth"] = "170";
                
                }
            }
            else if (steluta != 0 && radioButton1.Checked == true)
            {
                MessageBox.Show("Nu poti acorda stelute de mai multe ori! ");
                radioButton1.Checked = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)//2 stelute
        {

            if (steluta == 0 && radioButton2.Checked == true)
            {
                steluta = 2;
                DateTime x = DateTime.Now;
                if (radioButton2.Checked == true)
                {
                    MessageBox.Show("Mulțumim pentru steluțele acordate!");
                    this.sTELUTETableAdapter.InsertQuery_stelute("2", x);
                }
                radioButton2.Checked = false;
                comboBox2.SelectedItem = "Total stelute";
                listBox4.Items.Clear();
                {
                    listBox4.Items.Add("Total stelute: ");

                    listBox4.Items.Add("★★★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 5 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("5"));

                    listBox4.Items.Add("★★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 4 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("4"));

                    listBox4.Items.Add("★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 3 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("3"));

                    listBox4.Items.Add("★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 2 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("2"));

                    listBox4.Items.Add("★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 1 steluta= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("1"));

                    //Statistica stelute
                    chart1.Visible = true;
                    int maxim = 0;
                    int one = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("1").ToString());
                    int two = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("2").ToString());
                    if (one > two) maxim = one;
                    else maxim = two;
                    int three = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("3").ToString());
                    if (maxim < three) maxim = three;
                    int four = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("4").ToString());
                    if (maxim < four) maxim = four;
                    int five = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("5").ToString());
                    if (maxim < five) maxim = five;


                    this.chart1.Series["1 STEA"].Points.AddXY(2, one);

                    this.chart1.Series["2 STELE"].Points.AddXY(2.5, two);

                    this.chart1.Series["3 STELE"].Points.AddXY(3, three);

                    this.chart1.Series["4 STELE"].Points.AddXY(3.5, four);

                    this.chart1.Series["5 STELE"].Points.AddXY(4, five);

                    chart1.Series["1 STEA"]["PixelPointWidth"] = "170";
                    chart1.Series["2 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["3 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["4 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["5 STELE"]["PixelPointWidth"] = "170";

                }
            }
            else if (steluta != 0 && radioButton2.Checked == true)
            {
                MessageBox.Show("Nu poti acorda stelute de mai multe ori! ");
                radioButton2.Checked = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)//3 stelute
        {

            if (steluta == 0 && radioButton3.Checked == true)
            {
                steluta = 3;
                DateTime x = DateTime.Now;
                if (radioButton3.Checked == true)
                {
                    MessageBox.Show("Mulțumim pentru steluțele acordate!");
                    this.sTELUTETableAdapter.InsertQuery_stelute("3", x);
                }
                radioButton3.Checked = false;
                comboBox2.SelectedItem = "Total stelute";
                listBox4.Items.Clear();
                {
                    listBox4.Items.Add("Total stelute: ");

                    listBox4.Items.Add("★★★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 5 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("5"));

                    listBox4.Items.Add("★★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 4 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("4"));

                    listBox4.Items.Add("★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 3 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("3"));

                    listBox4.Items.Add("★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 2 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("2"));

                    listBox4.Items.Add("★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 1 steluta= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("1"));

                    //Statistica stelute
                    chart1.Visible = true;
                    int maxim = 0;
                    int one = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("1").ToString());
                    int two = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("2").ToString());
                    if (one > two) maxim = one;
                    else maxim = two;
                    int three = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("3").ToString());
                    if (maxim < three) maxim = three;
                    int four = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("4").ToString());
                    if (maxim < four) maxim = four;
                    int five = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("5").ToString());
                    if (maxim < five) maxim = five;


                    this.chart1.Series["1 STEA"].Points.AddXY(2, one);

                    this.chart1.Series["2 STELE"].Points.AddXY(2.5, two);

                    this.chart1.Series["3 STELE"].Points.AddXY(3, three);

                    this.chart1.Series["4 STELE"].Points.AddXY(3.5, four);

                    this.chart1.Series["5 STELE"].Points.AddXY(4, five);

                    chart1.Series["1 STEA"]["PixelPointWidth"] = "170";
                    chart1.Series["2 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["3 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["4 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["5 STELE"]["PixelPointWidth"] = "170";

                }
            }
            else if (steluta != 0 && radioButton3.Checked == true)
            {
                MessageBox.Show("Nu poti acorda stelute de mai multe ori! ");
                radioButton3.Checked = false;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)//4 stelute
        {

            if (steluta == 0&&radioButton4.Checked==true)
            {
                steluta = 4;
                DateTime x = DateTime.Now;

                if (radioButton4.Checked == true)
                {
                    MessageBox.Show("Mulțumim pentru steluțele acordate!");
                    this.sTELUTETableAdapter.InsertQuery_stelute("4", x);
                }
                radioButton4.Checked = false;
                comboBox2.SelectedItem = "Total stelute";
                listBox4.Items.Clear();
                {
                    listBox4.Items.Add("Total stelute: ");

                    listBox4.Items.Add("★★★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 5 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("5"));

                    listBox4.Items.Add("★★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 4 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("4"));

                    listBox4.Items.Add("★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 3 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("3"));

                    listBox4.Items.Add("★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 2 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("2"));

                    listBox4.Items.Add("★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 1 steluta= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("1"));

                    //Statistica stelute
                    chart1.Visible = true;
                    int maxim = 0;
                    int one = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("1").ToString());
                    int two = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("2").ToString());
                    if (one > two) maxim = one;
                    else maxim = two;
                    int three = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("3").ToString());
                    if (maxim < three) maxim = three;
                    int four = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("4").ToString());
                    if (maxim < four) maxim = four;
                    int five = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("5").ToString());
                    if (maxim < five) maxim = five;


                    this.chart1.Series["1 STEA"].Points.AddXY(2, one);

                    this.chart1.Series["2 STELE"].Points.AddXY(2.5, two);

                    this.chart1.Series["3 STELE"].Points.AddXY(3, three);

                    this.chart1.Series["4 STELE"].Points.AddXY(3.5, four);

                    this.chart1.Series["5 STELE"].Points.AddXY(4, five);

                    chart1.Series["1 STEA"]["PixelPointWidth"] = "170";
                    chart1.Series["2 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["3 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["4 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["5 STELE"]["PixelPointWidth"] = "170";
                
                }
            }
            else if (steluta != 0 && radioButton4.Checked == true)
            {
                MessageBox.Show("Nu poti acorda stelute de mai multe ori! ");
                radioButton4.Checked = false;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)//5 stelute
        {
            if (steluta == 0 && radioButton5.Checked == true)
            {
                steluta = 5;
                DateTime x = DateTime.Now;
                if (radioButton5.Checked == true)
                {
                    MessageBox.Show("Mulțumim pentru steluțele acordate!");
                    this.sTELUTETableAdapter.InsertQuery_stelute("5", x);
                }
                radioButton5.Checked = false;
                comboBox2.SelectedItem = "Total stelute";
                listBox4.Items.Clear();
                {
                    listBox4.Items.Add("Total stelute: ");

                    listBox4.Items.Add("★★★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 5 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("5"));

                    listBox4.Items.Add("★★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 4 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("4"));

                    listBox4.Items.Add("★★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 3 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("3"));

                    listBox4.Items.Add("★★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 2 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("2"));

                    listBox4.Items.Add("★");
                    listBox4.Items.Add("Nr utilizatori care au acordat 1 steluta= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("1"));


                    //Statistica stelute
                    chart1.Visible = true;
                    int maxim = 0;
                    int one = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("1").ToString());
                    int two = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("2").ToString());
                    if (one > two) maxim = one;
                    else maxim = two;
                    int three = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("3").ToString());
                    if (maxim < three) maxim = three;
                    int four = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("4").ToString());
                    if (maxim < four) maxim = four;
                    int five = int.Parse(this.sTELUTETableAdapter.ScalarQuerynumar_stelute("5").ToString());
                    if (maxim < five) maxim = five;


                    this.chart1.Series["1 STEA"].Points.AddXY(2, one);

                    this.chart1.Series["2 STELE"].Points.AddXY(2.5, two);

                    this.chart1.Series["3 STELE"].Points.AddXY(3, three);

                    this.chart1.Series["4 STELE"].Points.AddXY(3.5, four);

                    this.chart1.Series["5 STELE"].Points.AddXY(4, five);

                    chart1.Series["1 STEA"]["PixelPointWidth"] = "170";
                    chart1.Series["2 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["3 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["4 STELE"]["PixelPointWidth"] = "170";
                    chart1.Series["5 STELE"]["PixelPointWidth"] = "170";
                
                }
            }
            else if (steluta != 0 && radioButton5.Checked == true)
            {
                MessageBox.Show("Nu poti acorda stelute de mai multe ori! ");
                radioButton5.Checked = false;
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//combobox1-categorii animal
        {
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            int categ = int.Parse(this.cATEGORIITableAdapter.ScalarQueryidc_categorie(comboBox1.SelectedItem.ToString()).ToString());
            this.sPECIITableAdapter.FillBycategorie_animal(this.database1DataSet.SPECII,categ);
            label8.Text = ("Specii de " + comboBox1.SelectedItem.ToString());
            DataTable dt1 = this.database1DataSet.SPECII;
          //  listBox2.Items.Add(comboBox1.SelectedItem.ToString()+":"); 
            //listBox2.Items.Add("");   
            for (int i = 0; i < dt1.Rows.Count; i++)
                listBox2.Items.Add(dt1.Rows[i]["Specie"]);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)//listbox2-specii categorie animal
        {
            listBox3.Items.Clear();
            label9.Text=("Subspecii de "+listBox2.SelectedItem.ToString());
            int ids_specie = int.Parse(this.sPECIITableAdapter.ScalarQueryids_specie_data(listBox2.SelectedItem.ToString()).ToString());
            this.sUBSPECIITableAdapter.FillBysubspecie_specie_data(this.database1DataSet.SUBSPECII, ids_specie);
            DataTable dt = this.database1DataSet.SUBSPECII;
            //listBox3.Items.Add("Subspecii "+listBox2.SelectedItem.ToString());
           // listBox3.Items.Add("");
            for (int i = 0; i < dt.Rows.Count; i++)
                listBox3.Items.Add(dt.Rows[i]["Subspecie"]);
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)//listbox3- subspecii animal
        {
            button3.Text = "INAPOI LA CATEGORII";
            tabControl1.SelectedIndex = 5;
            this.sUBSPECIITableAdapter.FillBysubspecie_animal(this.database1DataSet.SUBSPECII,listBox3.SelectedItem.ToString());
            DataTable dt = this.database1DataSet.SUBSPECII;

            label23.Text = dt.Rows[0]["Subspecie"].ToString();
            if (dt.Rows[0]["Origine"].ToString() == "") textBox3.Text = "Nu exista informatii cunoscute.";
            else textBox3.Text = dt.Rows[0]["Origine"].ToString();

            if (dt.Rows[0]["Durata_de_viata"].ToString() == "") textBox4.Text = "Nu exista informatii cunoscute.";
            else textBox4.Text = dt.Rows[0]["Durata_de_viata"].ToString();

            if (dt.Rows[0]["Hrana"].ToString() == "") textBox5.Text = "Nu exista informatii cunoscute.";
            else textBox5.Text = dt.Rows[0]["Hrana"].ToString();

            if (dt.Rows[0]["Caracteristici"].ToString() == "") textBox6.Text = "Nu exista informatii cunoscute.";
            else textBox6.Text = dt.Rows[0]["Caracteristici"].ToString();

            if (dt.Rows[0]["Curiozitati"].ToString() == "") textBox7.Text = "Nu exista informatii cunoscute.";
            else textBox7.Text = dt.Rows[0]["Curiozitati"].ToString();

            //this.pozeleTableAdapter.Fill(this.pozeDataSet.pozele);
            //DataTable dt = this.pozeDataSet.pozele;
            //int i = comboBox1.SelectedIndex;
            Bitmap image = new Bitmap("E:\\PARADISUL ANIMAL\\imagini\\" + dt.Rows[0]["Poza"].ToString());
            pictureBox5.Image = (Image)image;
                    
        }

        private void cATEGORIIANIMALEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button3_Click(object sender, EventArgs e)//INAPOI LA CATEGORII
        {
            if (button3.Text == "INAPOI LA PAGINA PRINCIPALA") tabControl1.SelectedIndex = 0;
            else 
            tabControl1.SelectedIndex = 3;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)//combobox2- statistici stelute
        {
            listBox4.Items.Clear();
            if (comboBox2.SelectedItem.ToString() == "Total stelute".ToString())//var 1
            {
               // listBox4.Items.Clear();
                listBox4.Items.Add("Total stelute: ");
                
                listBox4.Items.Add("★★★★★");
                listBox4.Items.Add("Nr utilizatori care au acordat 5 stelute= "+this.sTELUTETableAdapter.ScalarQuerynumar_stelute("5"));

                listBox4.Items.Add("★★★★");
                listBox4.Items.Add("Nr utilizatori care au acordat 4 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("4"));

                listBox4.Items.Add("★★★");
                listBox4.Items.Add("Nr utilizatori care au acordat 3 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("3"));

                listBox4.Items.Add("★★");
                listBox4.Items.Add("Nr utilizatori care au acordat 2 stelute= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("2"));

                listBox4.Items.Add("★");
                listBox4.Items.Add("Nr utilizatori care au acordat 1 steluta= " + this.sTELUTETableAdapter.ScalarQuerynumar_stelute("1"));
            }

            if (comboBox2.SelectedItem.ToString() == "Stelute acordate azi".ToString())//var 2
            {
                //listBox4.Items.Clear();
                listBox4.Items.Add("Stelute acordate azi: ");
                DateTime x=DateTime.Now;
                int ziua_actuala = x.Day;
                int luna_actuala = x.Month;
                int anul_actual = x.Year;
                this.sTELUTETableAdapter.Fill(this.database1DataSet.STELUTE);
                int s5=0, s4=0, s3=0, s2=0, s1=0;//stelute
                DataTable dt = this.database1DataSet.STELUTE;
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    DateTime data = DateTime.Parse(dt.Rows[i]["Data_adaugarii"].ToString());
                    if (ziua_actuala == data.Day && luna_actuala == data.Month && anul_actual == data.Year)
                    {
                        if (dt.Rows[i]["Nr_stelute"].ToString() == "5") s5++;
                        if (dt.Rows[i]["Nr_stelute"].ToString() == "4") s4++;
                        if (dt.Rows[i]["Nr_stelute"].ToString() == "3") s3++;
                        if (dt.Rows[i]["Nr_stelute"].ToString() == "2") s2++;
                        if (dt.Rows[i]["Nr_stelute"].ToString() == "1") s1++;
                    }

                   

                }
                int ok = 0;//verific daca au fost adaugate azi stelute
                    if (s5 != 0)
                    {
                        listBox4.Items.Add("★★★★★");
                        if (s5 == 1) listBox4.Items.Add("Azi, un vizitator a acordat 5 stelute");
                        else
                        listBox4.Items.Add("Azi, "+s5+ " vizitatori au acordat 5 stelute");
                        ok = 1;
                    }

                if (s4 != 0)
                {
                    listBox4.Items.Add("★★★★");
                    if (s4 == 1) listBox4.Items.Add("Azi, un vizitator a acordat 4 stelute.");
                    else
                        listBox4.Items.Add("Azi, " + s4 + " vizitatori au acordat 4 stelute.");
                    ok = 1;
                }

                if (s3 != 0)
                {
                    listBox4.Items.Add("★★★");
                    if (s3 == 1) listBox4.Items.Add("Azi, un vizitator a acordat 3 stelute.");
                    else
                        listBox4.Items.Add("Azi, " + s3 + " vizitatori au acordat 3 stelute.");
                    ok = 1;
                }

                if (s2 != 0)
                {
                    listBox4.Items.Add("★★");
                    if (s2 == 1) listBox4.Items.Add("Azi, un vizitator a acordat 2 stelute.");
                    else
                        listBox4.Items.Add("Azi, " + s2 + " vizitatori au acordat 2 stelute.");
                    ok = 1;
                }

                if (s1 != 0)
                {
                    listBox4.Items.Add("★");
                    if (s1 == 1) listBox4.Items.Add("Azi, un vizitator a acordat o steluta.");
                    else
                        listBox4.Items.Add("Azi, " + s1 + " vizitatori au acordat o steluta.");
                    ok = 1;
                }
                if(ok==0) listBox4.Items.Add("Azi nu au fost adaugate stelute.");
            }

            if (comboBox2.SelectedItem.ToString() == "Stelute acordate in aceasta luna")//var 3
            {
                listBox4.Items.Add("Stelute acordate in aceasta luna: ");
                DateTime x = DateTime.Now;
                int luna_actuala=x.Month;
                int anul_actual = x.Year;
                this.sTELUTETableAdapter.Fill(this.database1DataSet.STELUTE);
                int s5 = 0, s4 = 0, s3 = 0, s2 = 0, s1 = 0;//stelute
                DataTable dt = this.database1DataSet.STELUTE;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DateTime data = DateTime.Parse(dt.Rows[i]["Data_adaugarii"].ToString());
                    if (luna_actuala == data.Month && anul_actual == data.Year)
                    {
                        if (dt.Rows[i]["Nr_stelute"].ToString() == "5") s5++;
                        if (dt.Rows[i]["Nr_stelute"].ToString() == "4") s4++;
                        if (dt.Rows[i]["Nr_stelute"].ToString() == "3") s3++;
                        if (dt.Rows[i]["Nr_stelute"].ToString() == "2") s2++;
                        if (dt.Rows[i]["Nr_stelute"].ToString() == "1") s1++;
                    }



                }
                int ok = 0;//verific daca au fost adaugate in aceasta luna stelute
                if (s5 != 0)
                {
                    listBox4.Items.Add("★★★★★");
                    if (s5 == 1) listBox4.Items.Add("Luna aceasta un vizitator a acordat 5 stelute");
                    else
                        listBox4.Items.Add("Luna aceasta," + s5 + " vizitatori au acordat 5 stelute");
                    ok = 1;
                }

                if (s4 != 0)
                {
                    listBox4.Items.Add("★★★★");
                    if (s4 == 1) listBox4.Items.Add("Luna aceasta un vizitator a acordat 4 stelute.");
                    else
                        listBox4.Items.Add("Luna aceasta, " + s4 + " vizitatori au acordat 4 stelute.");
                    ok = 1;
                }

                if (s3 != 0)
                {
                    listBox4.Items.Add("★★★");
                    if (s3 == 1) listBox4.Items.Add("Luna aceasta un vizitator a acordat 3 stelute.");
                    else
                        listBox4.Items.Add("Luna aceasta, " + s3 + " vizitatori au acordat 3 stelute.");
                    ok = 1;
                }

                if (s2 != 0)
                {
                    listBox4.Items.Add("★★");
                    if (s2 == 1) listBox4.Items.Add("Luna aceasta un vizitator a acordat 2 stelute.");
                    else
                        listBox4.Items.Add("Luna aceasta, " + s2 + " vizitatori au acordat 2 stelute.");
                    ok = 1;
                }

                if (s1 != 0)
                {
                    listBox4.Items.Add("★");
                    if (s1 == 1) listBox4.Items.Add("Luna aceasta un vizitator a acordat o steluta.");
                    else
                        listBox4.Items.Add("Luna aceasta, " + s1 + " vizitatori au acordat o steluta.");
                    ok = 1;
                }
                if (ok == 0) listBox4.Items.Add("Luna aceasta nu au fost adaugate stelute.");
            }
           
        }

        private void button6_Click(object sender, EventArgs e)//locatia
        {
            tabControl1.SelectedIndex = 7;
          
        }

        private void button5_Click(object sender, EventArgs e)//program vizite
        {
            tabControl1.SelectedIndex = 6;
        }

        private void button4_Click(object sender, EventArgs e)//tarife
        {
            tabControl1.SelectedIndex = 8;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

      

        private void pictureBox17_Click(object sender, EventArgs e)//picturebox: animalul zilei
        {
            button3.Text = "INAPOI LA PAGINA PRINCIPALA";
            tabControl1.SelectedIndex = 5;
            this.sUBSPECIITableAdapter.Fill(this.database1DataSet.SUBSPECII);
            DataTable dt = this.database1DataSet.SUBSPECII;
            label23.Text = dt.Rows[random_nr]["Subspecie"].ToString();
           // if (dt.Rows[random_nr]["Subspecie"].ToString() != "")
            {
                if (dt.Rows[random_nr]["Origine"].ToString() == "") textBox3.Text = "Nu exista informatii cunoscute.";
                else textBox3.Text = dt.Rows[random_nr]["Origine"].ToString();

                if (dt.Rows[random_nr]["Durata_de_viata"].ToString() == "") textBox4.Text = "Nu exista informatii cunoscute.";
                else textBox4.Text = dt.Rows[random_nr]["Durata_de_viata"].ToString();

                if (dt.Rows[random_nr]["Hrana"].ToString() == "") textBox5.Text = "Nu exista informatii cunoscute.";
                else textBox5.Text = dt.Rows[random_nr]["Hrana"].ToString();

                if (dt.Rows[random_nr]["Caracteristici"].ToString() == "") textBox6.Text = "Nu exista informatii cunoscute.";
                else textBox6.Text = dt.Rows[random_nr]["Caracteristici"].ToString();

                if (dt.Rows[random_nr]["Curiozitati"].ToString() == "") textBox7.Text = "Nu exista informatii cunoscute.";
                else textBox7.Text = dt.Rows[random_nr]["Curiozitati"].ToString();

                Bitmap image = new Bitmap("E:\\PARADISUL ANIMAL\\imagini\\" + dt.Rows[random_nr]["Poza"].ToString());
                pictureBox5.Image = (Image)image;
            }
          
        }

        private void button10_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            button15.Visible = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            button16.Visible = true;

        }

        private void button12_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            button17.Visible = true;
            comboBox1.Text = "";
            listBox2.Items.Clear();
            listBox3.Items.Clear();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            this.oPINIITableAdapter.Fill(this.database1DataSet.OPINII);
            DataTable dt = this.database1DataSet.OPINII;
            listBox1.Items.Add("PĂRERI ANTERIOARE:");
            for (int i = 0; i < dt.Rows.Count; i++)
                listBox1.Items.Add(dt.Rows[i]["Nume"] + ":" + dt.Rows[i]["Opinie"]);
            tabControl1.SelectedIndex = 4;
            button18.Visible = true;
        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }


        private void aNIMALEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            textBox8.Text = "";
            comboBox1.Text = "";
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox5.Items.Clear();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button14_Click(object sender, EventArgs e)//CAUTA ANIMAL
        {
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            int categorie = 0;
            string retin ="";
            if (textBox8.Text.ToString() == "") MessageBox.Show("Te rugam sa introduci un animal.");
            else
            {
                label30.Visible = true;
                string text = textBox8.Text.ToString();
                text = text.ToString().ToLower();//facem initial toate litere mici
                //vom transforma apoi, pe rand, prima litera din fiecare cuvant in litera mare
                //daca textul introdus contine mai multe cuvinte, le vom cauta pe rand pe toate

                text = text + " ";
                int ind = 0;
                List<string> animale = new List<string>();

                while (ind != text.Length)//cat timp exista cuvinte de preluat
                {

                    string x = "";//reinitializam pt fiecare cuvant
                    while (text[ind] != ' ' && ind <= text.Length)
                    {
                        x = x + text[ind];
                        ind++;
                    }
                    ind++;
                    string cuv = "";
                    //facem prima litera UPPERCASE
                    cuv = (x[0].ToString().ToUpper());
                    for (int j = 1; j < x.Length; j++)
                             cuv = cuv + x[j];

                        
                    this.sUBSPECIITableAdapter.Fill(this.database1DataSet.SUBSPECII);
                    DataTable dt = this.database1DataSet.SUBSPECII;
                    int ok = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                        if (dt.Rows[i]["Subspecie"].ToString().IndexOf(cuv) != -1) { adaugare_lista(animale, dt.Rows[i]["Subspecie"].ToString()); ok = 1; }
          
                    if (ok == 0)
                    {
                        this.cATEGORIITableAdapter.FillByorder_categorie(this.database1DataSet.CATEGORII);
                        DataTable dt1 = this.database1DataSet.CATEGORII;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                            if (dt1.Rows[i]["Categorie"].ToString().IndexOf(cuv) != -1)
                            {
                                listBox3.Items.Clear();
                                listBox2.Items.Clear();
                                categorie = 1;
                                retin = dt1.Rows[i]["Categorie"].ToString();
                                int categ = int.Parse(this.cATEGORIITableAdapter.ScalarQueryidc_categorie(dt1.Rows[i]["Categorie"].ToString()).ToString());
                                this.sPECIITableAdapter.FillBycategorie_animal(this.database1DataSet.SPECII, categ);
                                DataTable dt2 = this.database1DataSet.SPECII;
                                for (int j = 0; j < dt2.Rows.Count; j++)
                                {
                                    //listBox3.Items.Add(dt2.Rows[j]["Specie"].ToString());
                                    adaugare_lista(animale, dt2.Rows[j]["Specie"].ToString());
                                }
                            }
                    }

                    //vom face verificarile si pt litere mici
                    cuv = x;
                    this.sUBSPECIITableAdapter.Fill(this.database1DataSet.SUBSPECII);
                    ok = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                        if (dt.Rows[i]["Subspecie"].ToString().IndexOf(cuv) != -1) { adaugare_lista(animale, dt.Rows[i]["Subspecie"].ToString()); ok = 1; }
                    if (ok == 0)
                    {
                        this.cATEGORIITableAdapter.FillByorder_categorie(this.database1DataSet.CATEGORII);
                        DataTable dt1 = this.database1DataSet.CATEGORII;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                            if (dt1.Rows[i]["Categorie"].ToString().IndexOf(cuv) != -1)
                            {
                                categorie = 1;
                                retin = dt1.Rows[i]["Categorie"].ToString();
                                listBox3.Items.Clear();
                                listBox2.Items.Clear();
                                int categ = int.Parse(this.cATEGORIITableAdapter.ScalarQueryidc_categorie(dt1.Rows[i]["Categorie"].ToString()).ToString());
                                this.sPECIITableAdapter.FillBycategorie_animal(this.database1DataSet.SPECII, categ);
                                DataTable dt2 = this.database1DataSet.SPECII;
                                for (int j = 0; j < dt2.Rows.Count; j++)
                                {

                                    adaugare_lista(animale, dt2.Rows[j]["Specie"].ToString());
                                }
                            }
                    }
                }
                int vida = 0;
                foreach (string s in animale)
                    vida++;
                label30.Text = ("Rezultatele cautarii pt " + text);
                listBox5.Items.Clear();
                if(categorie==1)
                listBox5.Items.Add("Specii de " + retin);
                if (vida == 0) listBox5.Items.Add("Nu s-au gasit rezultate.");
                afisare_lista(animale);
            }  
            
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)// REZULTATELE CAUTARII
        {
            int yes = 0;
            if (listBox5.Text != "")
            {
                string text = listBox5.SelectedItem.ToString();
                this.sUBSPECIITableAdapter.Fill(this.database1DataSet.SUBSPECII);
                DataTable dt1 = this.database1DataSet.SUBSPECII;
                for (int i = 0; i < dt1.Rows.Count; i++)
                    if (text.IndexOf(dt1.Rows[i]["Subspecie"].ToString()) != -1)
                    {
                        yes = 1;
                        button3.Text = "INAPOI LA CATEGORII";
                        tabControl1.SelectedIndex = 5;
                        this.sUBSPECIITableAdapter.FillBysubspecie_animal(this.database1DataSet.SUBSPECII, text);
                        DataTable dt = this.database1DataSet.SUBSPECII;

                        label23.Text = dt.Rows[0]["Subspecie"].ToString();
                        if (dt.Rows[0]["Origine"].ToString() == "") textBox3.Text = "Nu exista informatii cunoscute.";
                        else textBox3.Text = dt.Rows[0]["Origine"].ToString();

                        if (dt.Rows[0]["Durata_de_viata"].ToString() == "") textBox4.Text = "Nu exista informatii cunoscute.";
                        else textBox4.Text = dt.Rows[0]["Durata_de_viata"].ToString();

                        if (dt.Rows[0]["Hrana"].ToString() == "") textBox5.Text = "Nu exista informatii cunoscute.";
                        else textBox5.Text = dt.Rows[0]["Hrana"].ToString();

                        if (dt.Rows[0]["Caracteristici"].ToString() == "") textBox6.Text = "Nu exista informatii cunoscute.";
                        else textBox6.Text = dt.Rows[0]["Caracteristici"].ToString();

                        if (dt.Rows[0]["Curiozitati"].ToString() == "") textBox7.Text = "Nu exista informatii cunoscute.";
                        else textBox7.Text = dt.Rows[0]["Curiozitati"].ToString();


                        Bitmap image = new Bitmap("E:\\PARADISUL ANIMAL\\imagini\\" + dt.Rows[0]["Poza"].ToString());
                        pictureBox5.Image = (Image)image;

                    }
                if (yes == 0)
                {
                    this.sPECIITableAdapter.Fill(this.database1DataSet.SPECII);
                    DataTable dt2 = this.database1DataSet.SPECII;
                    for (int i = 0; i < dt2.Rows.Count; i++)
                        if (dt2.Rows[i]["Specie"].ToString().IndexOf(text) != -1)
                        {
                            listBox3.Items.Clear();
                            label9.Text = ("Subspecii de " + listBox5.SelectedItem.ToString());
                            int ids_specie = int.Parse(this.sPECIITableAdapter.ScalarQueryids_specie_data(listBox5.SelectedItem.ToString()).ToString());
                            this.sUBSPECIITableAdapter.FillBysubspecie_specie_data(this.database1DataSet.SUBSPECII, ids_specie);
                            DataTable dt0 = this.database1DataSet.SUBSPECII;
                            for (int j = 0; j < dt0.Rows.Count; j++)
                                listBox3.Items.Add(dt0.Rows[j]["Subspecie"]);
                        }

                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            button15.Visible = false;
          
        }

        private void button17_Click(object sender, EventArgs e)
        {
            button17.Visible = false;
            tabControl1.SelectedIndex = 0;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            button16.Visible = false;
            tabControl1.SelectedIndex = 0;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            button18.Visible = false;
            tabControl1.SelectedIndex = 0;
        }

        private void label30_Click(object sender, EventArgs e)
        {
            
        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        int counter = -1;
        private void timer1_Tick(object sender, EventArgs e)//Timer
        {
            counter++;
            if (counter % 5 == 0)
                label1.ForeColor = Color.BlueViolet;
            if (counter % 5 == 1)
                label1.ForeColor = Color.Orchid;
            if (counter % 5 == 2)
                label1.ForeColor = Color.DarkOrange;
            if (counter % 5 == 3)
                label1.ForeColor = Color.Salmon;
            if (counter % 5 == 4)
                label1.ForeColor = Color.MediumVioletRed;
            

            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        
       


        
   

   
 
        

    

       
    }
}
