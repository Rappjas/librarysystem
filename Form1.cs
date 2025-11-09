using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace librarysystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connection = "server=127.0.0.1; database=pasiglibrarydb; uid=root; pwd=;";
            string userid = IDGenerator.GenerateUserID();
            string role = "Member";
            string name = textBox2.Text.Trim();
            string username = textBox1.Text.Trim();
            string password = textBox3.Text.Trim();
            string email = textBox4.Text.Trim();
            string joined = DateTime.Now.ToString("MM/dd/yyyy");

            if (name == "" || username == "" || password == "")
            {
                MessageBox.Show("Please enter all fields.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connection))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO users (User_ID, role, fullname, username, email, Password, date_registered) VALUES (@userid, @role, @Name, @Username, @email, @Password, @datejoined)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@datejoined", joined);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        MessageBox.Show("Registration successful!");
                    else
                        MessageBox.Show("Registration failed.");

                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                Form2 form2 = new Form2();
                form2.Show();
                this.Hide();
            }
        }
    }
}
