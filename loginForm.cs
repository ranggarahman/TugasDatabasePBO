using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PeramalCuaca
{
    public partial class loginForm : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ASUS\source\repos\PeramalCuaca\LoginDB\LoginDB.mdf;Integrated Security=True;Connect Timeout=30";
        public loginForm()
        {
            InitializeComponent();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            if (txtBoxUsername.Text == "" || txtBoxPassword.Text == "")
            {
                MessageBox.Show("Tidak ada masukan");
            }
            else
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    SqlCommand sqlCmd = new SqlCommand("UserAdd", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@username", txtBoxUsername.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@password", txtBoxPassword.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil Terdaftar");

                    Clear();
                }
            }
        }

        void Clear()
        {
            txtBoxUsername.Text = txtBoxPassword.Text = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "Select * from tbl_Login where username = '" + txtBoxUsername.Text.Trim() + "'and password = '" + txtBoxPassword.Text.Trim() + "'";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlCon);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            if(dataTable.Rows.Count == 1)
            {
                Form1 objForm1 = new Form1();
                this.Hide();
                objForm1.Show();
            }
            else if(txtBoxPassword.Text == "" || txtBoxUsername.Text == "")
            {
                MessageBox.Show("Tidak ada input");
            }
            else
                MessageBox.Show("Username belum terdaftar atau salah");
        }
    }
}
