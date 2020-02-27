using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vytas_DB
{
    public partial class Korzina : Form
    {
        Main main;
        List<int> list = new List<int>();
        int a;
        int x = 8;
        int y = 40;
        SqlConnection sqlCon;
        string path = AppDomain.CurrentDomain.BaseDirectory + @"images\";
        public Korzina(Main mainn)
        {
            main = mainn;
            sqlCon = mainn.sqlCon;
            InitializeComponent();
            FillItems();
        }
        private void FillItems()
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand cmd3 = new SqlCommand("KorzinaSelectAll", sqlCon);
            cmd3.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader reader = cmd3.ExecuteReader())
            {
                list.Clear();
                while (reader.Read())
                {
                    list.Add(reader.GetInt32(1));
                }
            }
            SqlCommand cmd = new SqlCommand("SelectAll", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader sqlReader = cmd.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    int id = sqlReader.GetInt32(0);
                    string name = sqlReader.GetSqlValue(1).ToString().Trim();
                    string cost = sqlReader.GetSqlValue(2).ToString();
                    string image = sqlReader.GetSqlValue(3).ToString().Trim();

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (id == list[i])
                        {
                            GroupBox groupBox = new GroupBox();
                            groupBox.Size = new Size(190, 230);
                            groupBox.Location = new Point(x, y);
                            groupBox.Text = " " + name + "    " + cost + "€ ";
                            this.Controls.Add(groupBox);
                            x = x + 195;
                            a++;
                            if (a == 3)
                            {
                                a = 0;
                                x = x - 195 * 3;
                                y = y + 235;
                            }
                            PictureBox pictureBox = new PictureBox();
                            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBox.Size = new Size(190, 215);
                            pictureBox.Location = new Point(0, 15);
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
                                SqlCommand cmd2 = new SqlCommand("KorzinaRemove", sqlCon) { CommandType = CommandType.StoredProcedure };
                                cmd2.Parameters.AddWithValue("@id", id);
                                cmd2.ExecuteNonQuery();
                                sqlCon.Close();
                                Point point = new Point(this.Location.X, this.Location.Y);
                                Korzina korzinaF = new Korzina(main);
                                korzinaF.Show();
                                korzinaF.Location = point;
                                main.FillCounts();
                                Close();
                            };
                            pictureBox.Click += (e, s) =>
                            {
                                if (sqlCon.State == ConnectionState.Closed)
                                    sqlCon.Open();
                                SqlCommand cmd2 = new SqlCommand("KorzinaRemove", sqlCon) { CommandType = CommandType.StoredProcedure };
                                cmd2.Parameters.AddWithValue("@id", id);
                                cmd2.ExecuteNonQuery();
                                sqlCon.Close();
                                Point point = new Point(this.Location.X, this.Location.Y);
                                Korzina korzinaF = new Korzina(main);
                                korzinaF.Show();
                                korzinaF.Location = point;
                                main.FillCounts();
                                Close();
                            };
                        }
                    }
                }
                sqlCon.Close();
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
