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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace librarysystem
{

    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void LoadBook(string keyword = "", string category = "All")
        {
            string sql = "SELECT b.book_id, b.title, b.author, b.genre, b.publication_year " +
                         "FROM tbl_books b";

            if (!string.IsNullOrEmpty(keyword))
            {
                if (category == "All")
                {
                    sql += " WHERE b.title LIKE @keyword OR b.author LIKE @keyword OR b.genre LIKE @keyword";
                }
                else if (category == "Title")
                {
                    sql += " WHERE b.title LIKE @keyword";
                }
                else if (category == "Author")
                {
                    sql += " WHERE b.author LIKE @keyword";
                }
                else if (category == "Genre")
                {
                    sql += " WHERE b.genre LIKE @keyword";
                }
            }

            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                if (!string.IsNullOrEmpty(keyword))
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (!dt.Columns.Contains("Summary"))
                    dt.Columns.Add("Summary", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    string title = row["title"].ToString();
                    string genre = row["genre"].ToString().ToLower();
                    string summary = "";

                    if (genre.Contains("horror"))
                        summary = $"A chilling horror story titled '{title}', filled with suspense and mystery.";
                    else if (genre.Contains("romance"))
                        summary = $"A heartwarming romance novel about love and destiny in '{title}'.";
                    else if (genre.Contains("fantasy"))
                        summary = $"An imaginative fantasy tale with adventure and magic in '{title}'.";
                    else if (genre.Contains("science"))
                        summary = $"A fascinating science-themed book exploring discoveries in '{title}'.";
                    else if (genre.Contains("fiction"))
                        summary = $"A captivating fiction story that brings imagination to life in '{title}'.";
                    else
                        summary = $"An interesting book titled '{title}' from the {genre} genre.";

                    row["Summary"] = summary;
                }

                dataGridView1.DataSource = dt;
                dataGridView1.ReadOnly = true;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

        }
        private void LoadBookCounts()
        {
            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();

                int borrowerId = 0;
                int returnedBooks = 0;

                string getUserQuery = "SELECT borrower_id, return_books FROM tbl_borrowers WHERE borrower_name=@username";
                using (MySqlCommand cmd = new MySqlCommand(getUserQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@username", user_data.currentuser);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            borrowerId = Convert.ToInt32(reader["borrower_id"]);
                            returnedBooks = Convert.ToInt32(reader["return_books"]);
                        }
                        else
                        {
                            // User not found
                            txtBorrowedBooks.Text = "0";
                            txtReturnedBooks.Text = "0";
                            return;
                        }
                    }
                }

                string borrowedQuery = "SELECT COUNT(*) FROM tbl_book_copies WHERE status='BORROWED' AND borrower_id=@id";
                using (MySqlCommand cmd = new MySqlCommand(borrowedQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@id", borrowerId);
                    int borrowedCount = Convert.ToInt32(cmd.ExecuteScalar());
                    txtBorrowedBooks.Text = borrowedCount.ToString();
                }
                txtReturnedBooks.Text = returnedBooks.ToString();
            }

            txtBorrowedBooks.ReadOnly = true;
            txtReturnedBooks.ReadOnly = true;
        }
        private void BookCollectionForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            this.Hide();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            cmbCategory.Items.AddRange(new string[] { "All", "Title", "Author", "Genre" });
            cmbCategory.SelectedIndex = 0; // Default to All
            LoadBook();
            LoadBookCounts();

            lblWelcome.Text = "Welcome, " + user_data.currentuser + "!";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();
            form6.Show();
            this.Hide();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form10 form10 = new Form10();
            form10.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            add_books form13 = new add_books();
            form13.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return;

            string summary = dataGridView1.CurrentRow.Cells["Summary"].Value.ToString();
            txtSummary.Text = summary;
        }

        private void txtSummary_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            string category = cmbCategory.SelectedItem.ToString();
            LoadBook(keyword, category);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void txtBorrowedBooks_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
