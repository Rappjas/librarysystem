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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string con = "server=127.0.0.1; database=pasiglibrarydb; uid=root;";
            string user = txtUsername.Text.Trim();
            string newpass = txtNewPass.Text.Trim();
            string confirmpass = txtConPass.Text.Trim();

            if (user == "")
            {
                MessageBox.Show("Enter username");
                return;
            }
            if (newpass == "")
            {
                MessageBox.Show("Enter new password");
                return;
            }
            if (confirmpass == "")
            {
                MessageBox.Show("Confirm your password");
                return;
            }
            if (newpass != confirmpass)
            {
                MessageBox.Show("Password not match");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(con))
            {
                try
                {
                    conn.Open();
                    string check = "SELECT COUNT(*) FROM users WHERE Username=@user";
                    MySqlCommand cmd = new MySqlCommand(check, conn);
                    cmd.Parameters.AddWithValue("@user", user);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        string update = "UPDATE users SET Password=@pass WHERE Username=@user";
                        MySqlCommand upcmd = new MySqlCommand(update, conn);
                        upcmd.Parameters.AddWithValue("@pass", newpass);
                        upcmd.Parameters.AddWithValue("@user", user);
                        upcmd.ExecuteNonQuery();

                        MessageBox.Show("Password changed!");
                        Form2 form2 = new Form2();
                        form2.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Username not found");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
