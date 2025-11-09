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
    public partial class Form9 : Form
    {
        private int borrowerId;
        public Form9(int borrowerId)
        {
            InitializeComponent();
            this.borrowerId = borrowerId;
            LoadBorrowedBooks();
        }

        private void LoadBorrowedBooks()
        {
            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT bc.copy_id, b.title, bc.date_borrow, bc.date_return, br.borrower_name " +
                        $"FROM tbl_book_copies bc " +
                        $"JOIN tbl_books b ON bc.book_id = b.book_id " +
                        $"JOIN tbl_borrowers br ON bc.borrower_id = br.borrower_id " +
                        $"WHERE bc.borrower_id = @borrowerId AND bc.status = 'BORROWED'", conn);
                cmd.Parameters.AddWithValue("@borrowerId", borrowerId);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();

                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("You have not borrowed any books yet.");
                }
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a borrowed book first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int copyId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["copy_id"].Value);
            string title = dataGridView1.SelectedRows[0].Cells["title"].Value.ToString();
            string borrowerName = dataGridView1.SelectedRows[0].Cells["borrower_name"].Value.ToString();
            string dateBorrowed = dataGridView1.SelectedRows[0].Cells["date_borrow"].Value.ToString();
            string dateReturn = dataGridView1.SelectedRows[0].Cells["date_return"].Value.ToString();

            Form8 rbook = new Form8(copyId, title, borrowerName, dateBorrowed, dateReturn);
            rbook.Show();
            LoadBorrowedBooks();
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT bc.copy_id, b.title, bc.date_borrow, bc.date_return, br.borrower_name " +
                        $"FROM tbl_book_copies bc " +
                        $"JOIN tbl_books b ON bc.book_id = b.book_id " +
                        $"JOIN tbl_borrowers br ON bc.borrower_id = br.borrower_id " +
                        $"WHERE bc.borrower_id = @borrowerId AND bc.status = 'BORROWED'", conn);
                cmd.Parameters.AddWithValue("@borrowerId", borrowerId);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();

                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("You have not borrowed any books yet.");
                }
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int copyId = Convert.ToInt32(row.Cells["copy_id"].Value);
                string title = row.Cells["title"].Value.ToString();
                string borrowerName = row.Cells["borrower_name"].Value.ToString();
                string dateBorrowed = row.Cells["date_borrow"].Value.ToString();
                string author = row.Cells["author"]?.Value.ToString() ?? "Unknown Author";

                Form11 form11 = new Form11(copyId, title, author, borrowerName, dateBorrowed, borrowerId);
                form11.Show();
                this.Hide();

                LoadBorrowedBooks();
            }
        }
    }
}
