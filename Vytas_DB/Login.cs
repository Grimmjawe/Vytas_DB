using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Vytas_DB
{
    public partial class Login : Form
    {
        Main main;
        SqlConnection sqlCon;
        public Login(Main mainn)
        {
            main = mainn;
            sqlCon = mainn.sqlCon;
            InitializeComponent();
            label2.Visible = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            TryLogin();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TryLogin();
            }
        }

        void TryLogin()
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand cmd = new SqlCommand("TestLogin", sqlCon) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@username", textBox1.Text);
            cmd.Parameters.AddWithValue("@password", textBox2.Text);
            using (SqlDataReader sqlReader = cmd.ExecuteReader())
            {
                while (sqlReader.Read())
                {
                    main.admin = true;
                    main.AddItem.Visible = true;
                    main.LoginBtn.Text = "Logout admin";
                    Close();
                }
            };
            label2.Visible = true;
        }
    }
}
