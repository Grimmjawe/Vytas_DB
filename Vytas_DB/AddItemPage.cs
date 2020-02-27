using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Vytas_DB
{
    public partial class AddItemPage : Form
    {
        Main main;
        string fileDirectory;
        SqlConnection sqlCon;
        string path = AppDomain.CurrentDomain.BaseDirectory + @"images\";
        public AddItemPage(Main mainn)
        {
            main = mainn;
            sqlCon = mainn.sqlCon;
            InitializeComponent();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg|PNG|*.png", ValidateNames = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                    fileDirectory = ofd.FileName;
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            TryAddItem();
        }

        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            TryAddItem();
        }

        void TryAddItem()
        {
            string name = textBox1.Text.Trim();
            string cost = textBox2.Text.Trim();
            if (name != "" && cost != "" && pictureBox1.Image != null)
            {
                string newname = name + "_" + cost + ".jpg";
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand cmd2 = new SqlCommand("AddItem", sqlCon) { CommandType = CommandType.StoredProcedure };
                cmd2.Parameters.AddWithValue("@name", name);
                cmd2.Parameters.AddWithValue("@cost", Convert.ToDouble(cost));
                cmd2.Parameters.AddWithValue("@image", newname);
                cmd2.ExecuteNonQuery();
                sqlCon.Close();
                System.IO.File.Copy(fileDirectory, path + newname);
                main.FillItems(false);
                Close();
            }
        }
    }
}
