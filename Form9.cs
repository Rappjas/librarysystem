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
        public Form9()
        {
            InitializeComponent();
            LoadBorrowedBooks();
        }

        private void LoadBorrowedBooks()
        {
            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();


                MySqlCommand cmd = new MySqlCommand(
                    "SELECT s.StatusId, b.BookID, b.BookTitle, b.Author, s.borrowed_date, s.return_date " +
                    "FROM status s " +
                    "JOIN books b ON s.book_id = b.BookID " +
                    "WHERE s.user_id = @userId AND s.status = 'BORROWED'", conn);
                cmd.Parameters.AddWithValue("@userId", user_data.user_id);

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
            
            // book info
            status_data.statusID = dataGridView1.SelectedRows[0].Cells["StatusID"].Value.ToString();
            book_data.currentbookid = dataGridView1.SelectedRows[0].Cells["BookID"].Value.ToString();
            book_data.currentbookname = dataGridView1.SelectedRows[0].Cells["BookTitle"].Value.ToString();
            book_data.currentbookauthor = dataGridView1.SelectedRows[0].Cells["Author"].Value.ToString();
            string dateBorrowed = dataGridView1.SelectedRows[0].Cells["borrowed_date"].Value.ToString();
            string dateReturn = dataGridView1.SelectedRows[0].Cells["return_date"].Value.ToString();

            Form8 rbook = new Form8(dateBorrowed, dateReturn);
            rbook.Show();
            LoadBorrowedBooks();
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            LoadBorrowedBooks();
                
         }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            book_data.currentbookid = row.Cells["BookID"].Value.ToString();
            book_data.currentbookname = row.Cells["BookTitle"].Value.ToString();
            book_data.currentbookauthor = row.Cells["Author"].Value.ToString();
            string dateBorrowed = row.Cells["borrowed_date"].Value.ToString();

            // Open Form11 for actions like marking lost or extending
            Form11 form11 = new Form11(dateBorrowed);
            form11.Show();
            this.Hide();
        }
    }
}
