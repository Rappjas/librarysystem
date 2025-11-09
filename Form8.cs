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
        private int copyId;
        public int BorrowerId { get; private set; }
        public Form8(int copyId, string title, string borrowerName, string dateBorrowed, string dateReturn)
        {
            InitializeComponent();
            this.copyId = copyId;
            txtCopyId.Text = copyId.ToString();
            txtTitle.Text = title;
            txtName.Text = borrowerName;
            txtBorrowedDate.Text = dateBorrowed;
            txtDueDate.Text = dateReturn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"UPDATE tbl_book_copies SET status='AVAILABLE', " +
                    $"borrower_id=NULL, date_borrow=NULL, date_return=NULL " +
                    $"WHERE copy_id=@copyId", conn);


                cmd.Parameters.AddWithValue("@copyId", copyId);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Book returned successfully.");
                    Form5 form5 = new Form5();
                    this.Close();
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
