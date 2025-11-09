using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace librarysystem
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dbconnection = "server=127.0.0.1; database=book_db; uid=root;";

            user_data.currentuser = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (user_data.currentuser == "" || password == "")
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(dbconnection))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM users WHERE username=@Username AND Password=@Password";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Username", user_data.currentuser);
                    cmd.Parameters.AddWithValue("@Password", password);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user_data.user_id = reader.GetString("User_ID");
                        user_data.email = reader.GetString("email");
                        user_data.real_name = reader.GetString("fullname");
                        string role = reader.GetString("role");
                        MessageBox.Show("Login successful!");

                        if (role == "Admin")
                        {
                            manage_books bookmanageform = new manage_books();
                            bookmanageform.Show();
                            this.Hide();
                        }
                        else {
                            Form3 form3 = new Form3();
                            form3.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.");
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}
