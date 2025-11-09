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
        private int copyId;
        private string bookTitle;
        private string bookAuthor;
        private string borrowerName;
        private string dateBorrowed;
        private decimal fineAmount = 250.00m;

        public Form11(int copyId, string bookTitle, string bookAuthor, string borrowerName, string dateBorrowed, int borrowerId)
        {
            InitializeComponent();

            this.copyId = copyId;
            this.bookTitle = bookTitle;
            this.bookAuthor = bookAuthor;
            this.borrowerName = borrowerName;
            this.dateBorrowed = dateBorrowed;

            lblTitle.Text = bookTitle;
            lblAuthor.Text = bookAuthor;
            lblName.Text = borrowerName;
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

                MySqlCommand cmdupdate = new MySqlCommand($"UPDATE tbl_book_copies SET status='LOST', lost_date=@lostDate, " +
                    $"fine=@fine WHERE copy_id=@copyId", conn);
                cmdupdate.Parameters.AddWithValue("@lostDate", lostDate);
                cmdupdate.Parameters.AddWithValue("@fine", fine);
                cmdupdate.Parameters.AddWithValue("@copyId", copyId);
                cmdupdate.ExecuteNonQuery();

                MySqlCommand cmdinsert = new MySqlCommand($"INSERT INTO tbl_lost_books (copy_id, borrower_id, lost_date, fine) " +
                    $"VALUES (@copyId, @borrowerId, @lostDate, @fine)", conn);
                cmdinsert.Parameters.AddWithValue("@copyId", copyId);
                cmdinsert.Parameters.AddWithValue("@borrowerId", borrowerId);
                cmdinsert.Parameters.AddWithValue("@lostDate", lostDate);
                cmdinsert.Parameters.AddWithValue("@fine", fine);

                MessageBox.Show("Book marked as lost successfully.");
                this.Close();
            }
        }
    }
}
