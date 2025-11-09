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
    public partial class Form12 : Form
    {
        private string bookId;
        private int borrowerId;
        private string title, author;
        public Form12(string bookId, int borrowerId, string title, string author)
        {
            InitializeComponent();
            this.bookId = bookId;
            this.borrowerId = borrowerId;
            this.title = title;
            this.author = author;

            lblTitle.Text = $"Title : {title}";
            lblAuthor.Text = $"Author : {author}";
            lblDatetoday.Text = $"{DateTime.Now.ToShortDateString()}";
            dateTimePicker1.Value = DateTime.Now.AddDays(7);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime reservationDate = DateTime.Now;
            DateTime validUntil = dateTimePicker1.Value;

            if (validUntil <= reservationDate)
            {
                MessageBox.Show("Reservation valid until date must be in the future!");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();

                // Check the current book status
                MySqlCommand checkCmd = new MySqlCommand("SELECT Status FROM books WHERE BookID=@bookId", conn);
                checkCmd.Parameters.AddWithValue("@bookId", bookId);
                string currentStatus = checkCmd.ExecuteScalar()?.ToString();

                if (string.IsNullOrEmpty(currentStatus))
                {
                    MessageBox.Show("Book not found in the database.");
                    return;
                }

                // Only allow reservation if book is AVAILABLE or BORROWED
                if (currentStatus.ToUpper() == "RESERVED")
                {
                    MessageBox.Show("This book is already reserved by another user.");
                    return;
                }
                else if (currentStatus.ToUpper() != "AVAILABLE" && currentStatus.ToUpper() != "BORROWED")
                {
                    MessageBox.Show("This book cannot be reserved right now.");
                    return;
                }

                // Insert reservation into status table
                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO status (book_id, journal_id, user_id, status, borrowed_date, return_date, reserved_date) " +
                    "VALUES (@bookId, NULL, @userId, 'RESERVED', NULL, NULL, @reservedDate)", conn);
                cmd.Parameters.AddWithValue("@bookId", bookId);
                cmd.Parameters.AddWithValue("@userId", user_data.user_id);
                cmd.Parameters.AddWithValue("@reservedDate", reservationDate.ToString("yyyy-MM-dd"));

                if (cmd.ExecuteNonQuery() > 0)
                {
                    // Only update book status if it's currently AVAILABLE
                    if (currentStatus.ToUpper() == "AVAILABLE")
                    {
                        MySqlCommand updateBookCmd = new MySqlCommand(
                            "UPDATE books SET Status='RESERVED' WHERE BookID=@bookId", conn);
                        updateBookCmd.Parameters.AddWithValue("@bookId", bookId);
                        updateBookCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Book reserved successfully!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to reserve the book. Please try again.");
                }

                conn.Close();
            }


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form12_Load(object sender, EventArgs e)
        {

        }
    }
}
