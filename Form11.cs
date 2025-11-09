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
    public partial class Form11 : Form
    {
        private int borrowerId;
        private string bookId;
        private string bookTitle;
        private string bookAuthor;
        private string dateBorrowed;
        private decimal fineAmount = 250.00m;

        public Form11(string bookId, string title, string author, int borrowerId, string dateBorrowed)
        {
            InitializeComponent();

            this.bookId = bookId;
            this.bookTitle = bookTitle;
            this.bookAuthor = bookAuthor;
            this.dateBorrowed = dateBorrowed;

            lblTitle.Text = bookTitle;
            lblAuthor.Text = bookAuthor;
            lblName.Text = user_data.real_name;
            lblDateBorrow.Text = dateBorrowed;

            lblLostDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            txtFineAmount.Text = fineAmount.ToString("0.00");
            this.borrowerId = borrowerId;
        }

        private void Form11_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string lostDate = DateTime.Now.ToString("yyyy-MM-dd");
            string fineText = txtFineAmount.Text.Trim();

            if (!decimal.TryParse(fineText, out decimal fine))
            {
                MessageBox.Show("Please enter a valid fine amount.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();

                //Update books table
                MySqlCommand updateBook = new MySqlCommand(
                    "UPDATE books SET Status='LOST' WHERE BookID=@bookId", conn);
                updateBook.Parameters.AddWithValue("@bookId", bookId);
                updateBook.ExecuteNonQuery();

                //Update status table
                MySqlCommand updateStatus = new MySqlCommand(
                    "UPDATE status SET status='LOST', return_date=@lostDate " +
                    "WHERE book_id=@bookId AND user_id=@borrowerId AND status='BORROWED'", conn);
                updateStatus.Parameters.AddWithValue("@bookId", bookId);
                updateStatus.Parameters.AddWithValue("@borrowerId", borrowerId);
                updateStatus.Parameters.AddWithValue("@lostDate", lostDate);
                updateStatus.ExecuteNonQuery();

                MessageBox.Show("Book marked as lost successfully.");
                this.Close();
            }
        }   
    }
}
