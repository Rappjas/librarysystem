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
    public partial class Form8 : Form
    {
        private string bookId;
        public Form8(string dateBorrowed, string dateReturn)
        {
            InitializeComponent();
            txtTitle.Text = book_data.currentbookname;
            txtBorrowedDate.Text = dateBorrowed;
            txtDueDate.Text = dateReturn;
            txtName.Text = user_data.real_name;
            txtCopyId.Text = book_data.currentbookid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int returned = 1;

            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();

                // ✅ Update the books table to mark it as AVAILABLE
                MySqlCommand updateBook = new MySqlCommand(
                    "UPDATE books SET Status='AVAILABLE' WHERE BookID=@bookId", conn);
                updateBook.Parameters.AddWithValue("@bookId", book_data.currentbookid);
                updateBook.ExecuteNonQuery();


                MySqlCommand updateUser = new MySqlCommand(
                    "UPDATE users SET returned_books = returned_books + @returned WHERE user_id=@uid", conn);
                updateUser.Parameters.AddWithValue("@returned", returned);
                updateUser.Parameters.AddWithValue("@uid", user_data.user_id);
                updateUser.ExecuteNonQuery();

                // ✅ Update the status table for this user and book
                MySqlCommand updateStatus = new MySqlCommand(
                    "UPDATE status SET status='RETURNED', return_date=@returnDate " +
                    "WHERE book_id=@bookId AND user_id=@userId AND status='BORROWED'", conn);
                updateStatus.Parameters.AddWithValue("@bookId", book_data.currentbookid);
                updateStatus.Parameters.AddWithValue("@userId", user_data.user_id);
                updateStatus.Parameters.AddWithValue("@returnDate", DateTime.Now.ToString("yyyy-MM-dd"));
                int rowsAffected = updateStatus.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Book returned successfully.");
                    this.Close();
                    Form5 form5 = new Form5();
                    form5.Show();
                }
                else
                {
                    MessageBox.Show("Failed to return the book. Please try again.");
                }

                conn.Close();
            }
        }
    }
}
