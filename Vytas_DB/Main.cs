using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace Vytas_DB
{
    public partial class Main : Form
    {
        public bool admin = false;
        List<int> list = new List<int>();
        List<GroupBox> listGB = new List<GroupBox>();
        int a;
        int x = 8;
        int y = 40;
        public SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\vytasDB.mdf;Integrated Security=True;Connect Timeout=30");
        string path = AppDomain.CurrentDomain.BaseDirectory + @"images\";
        public Main()
        {
            InitializeComponent();
            comboBox1.Items.Add("Name");
            comboBox1.Items.Add("Cost");
            comboBox1.SelectedIndex = 0;
            FillItems(false);
            FillCounts();
        }

        public void FillCounts()
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand cmd3 = new SqlCommand("KorzinaSelectAll", sqlCon) { CommandType = CommandType.StoredProcedure };
            using (SqlDataReader reader = cmd3.ExecuteReader())
            {
                list.Clear();
                while (reader.Read())
                {
                    list.Add(reader.GetInt32(1));
                }
            }
            korzinaO.Text = list.Count.ToString();
            sqlCon.Close();
        }

        public void FillItems(bool search)
        {
            for (int i = 0; i < listGB.Count; i++)
            {
                Controls.Remove(listGB[i]);
            }
            a = 0;
            x = 8;
            y = 40;
            listGB.Clear();
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand cmd = new SqlCommand("SelectAll", sqlCon) { CommandType = CommandType.StoredProcedure };
            if (search)
            {
                cmd = new SqlCommand("Search", sqlCon) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@ByThis", textBox4.Text);
                cmd.Parameters.AddWithValue("@Search", comboBox1.SelectedIndex);
            }
            SqlDataReader sqlReader = cmd.ExecuteReader();
            while (sqlReader.Read())
            {
                int id = sqlReader.GetInt32(0);
                string name = sqlReader.GetSqlValue(1).ToString().Trim();
                string cost = sqlReader.GetSqlValue(2).ToString();
                string image = sqlReader.GetSqlValue(3).ToString().Trim();

                GroupBox groupBox = new GroupBox();
                listGB.Add(groupBox);
                groupBox.Size = new Size(190, 230);
                groupBox.Location = new Point(x, y);
                groupBox.Text = " " + name + "    " + cost + "€ ";
                Controls.Add(groupBox);
                x = x + 195;
                a++;
                if (a == 3)
                {
                    a = 0;
                    x = x - 195*3;
                    y = y + 235;
                }
                PictureBox pictureBox = new PictureBox();
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Size = new Size(190, 215);
                pictureBox.Location = new Point(0,15);
                if (File.Exists(path + image))
                {
                    pictureBox.Image = Image.FromFile(path + image);
                }
                else
                {
                    pictureBox.Image = Image.FromFile(path + "nofile.png");
                }
                groupBox.Controls.Add(pictureBox);
                groupBox.Click += (e, s) =>
                {
                    if (sqlCon.State == ConnectionState.Closed)
                        sqlCon.Open();
                    SqlCommand cmd2 = new SqlCommand("KorzinaAdd", sqlCon) { CommandType = CommandType.StoredProcedure };
                    cmd2.Parameters.AddWithValue("@id", id);
                    cmd2.ExecuteNonQuery();
                    sqlCon.Close();
                    FillCounts();
                };
                pictureBox.Click += (e, s) =>
                {
                    if (sqlCon.State == ConnectionState.Closed)
                        sqlCon.Open();
                    SqlCommand cmd2 = new SqlCommand("KorzinaAdd", sqlCon) { CommandType = CommandType.StoredProcedure };
                    cmd2.Parameters.AddWithValue("@id", id);
                    cmd2.ExecuteNonQuery();
                    sqlCon.Close();
                    FillCounts();
                };
            }
            sqlCon.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            FillItems(true);
        }

        private void korzinaO_Click(object sender, EventArgs e)
        {
            Korzina korzinaF = new Korzina(this);
            korzinaF.Show();
            korzinaF.FormClosed += (s, i) =>
            {
                FillCounts();
            };
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (admin)
            {
                admin = false;
                AddItem.Visible = false;
                LoginBtn.Text = "Login admin";
            }
            else
            {
                Login korzinaF = new Login(this);
                korzinaF.Show();
            }
        }

        private void AddItem_Click(object sender, EventArgs e)
        {
            AddItemPage korzinaF = new AddItemPage(this);
            korzinaF.Show();
        }
    }
}
