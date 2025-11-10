using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace librarysystem
{
    public partial class Form7 : Form
    {
        public int BorrowerId { get; private set; }
        public Form7()
        {
            InitializeComponent();
            

            txtBookId.Text = book_data.currentbookid;
            txtTitle.Text = book_data.currentbookname;
            txtAuthor.Text = book_data.currentbookauthor;

            txtBorrowed.Text = DateTime.Now.ToString("MM/dd/yyyy");

            dateReturn.Value = DateTime.Now.AddDays(7);

        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            string borrowedDate = DateTime.Now.ToString("yyyy-MM-dd");
            string returnDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");

            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();

                // Insert borrow record into status table
                MySqlCommand insertStatus = new MySqlCommand(
                    $"INSERT INTO status (book_id, journal_id, user_id, status, borrowed_date, return_date, reserved_date) " +
                    $"VALUES (@bookId, NULL, @userId, 'BORROWED', @borrowedDate, @returnDate, NULL)", conn);
                insertStatus.Parameters.AddWithValue("@bookId", book_data.currentbookid);
                insertStatus.Parameters.AddWithValue("@userId", user_data.user_id);
                insertStatus.Parameters.AddWithValue("@borrowedDate", borrowedDate);
                insertStatus.Parameters.AddWithValue("@returnDate", returnDate);
                insertStatus.ExecuteNonQuery();

                MySqlCommand updateBookStatus = new MySqlCommand($"UPDATE books SET Status = 'BORROWED' WHERE BookID = @bookId", conn);
                updateBookStatus.Parameters.AddWithValue("@bookId", book_data.currentbookid);
                updateBookStatus.ExecuteNonQuery();

                MessageBox.Show("Book borrowed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Return to main form
                Form6 form6 = new Form6();
                form6.Show();
                this.Hide();
            }
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            // Set user info from logged-in user_data
            txtEmail.Text = user_data.email;
            txtName.Text = user_data.real_name;
            txtEmail.ReadOnly = true;
            txtName.ReadOnly = true;

            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();

                // Check if the book is available
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT Status FROM books WHERE BookID = @bookId", conn);
                cmd.Parameters.AddWithValue("@bookId", book_data.currentbookid);

                string status = cmd.ExecuteScalar()?.ToString();

                if (status == null || status.ToLower() != "available")
                {
                    // Book is not available
                    MessageBox.Show("This book is currently not available to borrow.", "Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }

                conn.Close();
            }

        }
    }
}
