using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace librarysystem
{
    public partial class member_management : Form
    {
        public member_management()
        {
            InitializeComponent();
            loadUsers("");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadUsers(textBox1.Text.Trim());
            textBox1.Clear();
        }

        private void loadUsers(string searchTerm)
        {
            try
            {
                string query = "SELECT user_id, role, username, email, fullname, date_registered, borrowed_books, returned_books, fines_fees FROM users";
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query += " WHERE user_id LIKE @search OR " +
                             "username LIKE @search OR " +
                             "fullname LIKE @search OR " +
                             "email LIKE @search";
                }
                using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                    }
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.ClearSelection();
                }
            } catch (Exception ex)
            {
                MessageBox.Show("An error occured while loading user data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBookMng_Click(object sender, EventArgs e)
        {
            manage_books manageBooksForm = new manage_books();
            manageBooksForm.Show();
            this.Hide();
        }
    }
}
