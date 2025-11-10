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
    public partial class manage_books : Form
    {
        public manage_books()
        {
            InitializeComponent();
            loadBooks();
        }

        private void loadBooks()
        {
            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM books", conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();
                conn.Close();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            add_books addbooksForm = new add_books();
            addbooksForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to edit.");
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            string bookId = row.Cells["BookID"].Value.ToString();
            string title = row.Cells["BookTitle"].Value.ToString();
            string author = row.Cells["Author"].Value.ToString();
            string genre = row.Cells["Genre"].Value.ToString();
            string publisher = row.Cells["Publisher"].Value.ToString();
            string language = row.Cells["Language"].Value.ToString();

            FormEdit editForm = new FormEdit(bookId, title, author, genre, publisher, language);
            editForm.ShowDialog();
            loadBooks();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to delete.");
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            string bookId = row.Cells["BookID"].Value.ToString();

            if (MessageBox.Show("Are you sure you want to delete this book?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM books WHERE BookID=@bookId", conn);
                    cmd.Parameters.AddWithValue("@bookId", bookId);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    loadBooks();
                    MessageBox.Show("Book deleted successfully.");
                }
            }
        }

        private void btnMemMng_Click(object sender, EventArgs e)
        {
            member_management memberManagementForm = new member_management();
            memberManagementForm.Show();
            this.Hide();
        }

        private void btnCirMng_Click(object sender, EventArgs e)
        {
            circulation circulationManagementForm = new circulation();
            circulationManagementForm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );       

            if (result == DialogResult.Yes)
            {
                Form2 loginForm = new Form2();
                loginForm.Show();
                this.Hide();
            }
        }
    }
}
