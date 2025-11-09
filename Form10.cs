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

namespace librarysystem
{
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            string dbconnection = "server=127.0.0.1; database=library_db; uid=root;";

            using (MySqlConnection conn = new MySqlConnection(dbconnection))
            {
                conn.Open();

                string query = "SELECT Name, Username, email, Joined FROM tbl_users WHERE User_ID = @userID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", user_data.user_id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    textBox1.Text = reader["Name"].ToString();
                    textBox2.Text = reader["Username"].ToString();
                    textBox3.Text = reader["email"].ToString();
                }
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dbconnection = "server=127.0.0.1; database=library_db; uid=root;";
            using (MySqlConnection conn = new MySqlConnection(dbconnection))
            {
                try
                {
                    conn.Open();
                    string updatecmd = $"UPDATE tbl_users SET " +
                    $"`Name` = @name, `Username` = @newUsername, `email` = @email WHERE `User_ID` = @userID";
                    using (MySqlCommand cmd = new MySqlCommand(updatecmd, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", textBox1.Text);
                        cmd.Parameters.AddWithValue("@newUsername", textBox2.Text);
                        cmd.Parameters.AddWithValue("@email", textBox3.Text);
                        cmd.Parameters.AddWithValue("@userID", user_data.user_id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Profile Updated!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error!" + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show(); 
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();
            form6.Show();
            this.Hide();
        }
    }
}
