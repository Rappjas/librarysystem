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
        private int bookId;
        private string bookTitle;
        private string bookAuthor;
        private int selectedCopyId;
        public int BorrowerId { get; private set; }
        public Form7(int Id, string title, string author)
        {
            InitializeComponent();
            bookId = Id;
            bookTitle = title;
            bookAuthor = author;

            txtBookId.Text = Id.ToString();
            txtTitle.Text = title;
            txtAuthor.Text = author;

            txtBorrowed.Text = DateTime.Now.ToString("MM/dd/yyyy");

            dateReturn.Value = DateTime.Now.AddDays(7);

        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            //string borrowerName = txtName.Text.Trim();
            //string email = txtEmail.Text.Trim();
            
            string dateBorrowed = txtBorrowed.Text.Trim();
            string Datreturn = dateReturn.Value.ToString("MM/dd/yyyy");

            //if (user_data.real_name == "")
            //{
            //MessageBox.Show("Please enter the borrower's name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            // return;
            //}

            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();

                MySqlCommand insertcmd = new MySqlCommand($"INSERT INTO tbl_borrowers (borrower_name, email) " +
                    $"VALUES (@name, @email)", conn);
                insertcmd.Parameters.AddWithValue("@name", user_data.real_name);
                insertcmd.Parameters.AddWithValue("@email", user_data.email);
                insertcmd.ExecuteNonQuery();

                int borrowerId = (int)insertcmd.LastInsertedId;
                BorrowerId = borrowerId;

                MySqlCommand cmdupd = new MySqlCommand($"UPDATE tbl_book_copies SET status='BORROWED', " +
                    $"borrower_id=@borrowerId, date_borrow=@dateBorrow, " +
                    $"date_return=@dateReturn WHERE copy_id=@copyId", conn);
                cmdupd.Parameters.AddWithValue("@borrowerId", borrowerId);
                cmdupd.Parameters.AddWithValue("@dateBorrow", dateBorrowed);
                cmdupd.Parameters.AddWithValue("@dateReturn", Datreturn);
                cmdupd.Parameters.AddWithValue("@copyId", selectedCopyId);
                cmdupd.ExecuteNonQuery();

                MessageBox.Show("Book borrowed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form5 form5 = new Form5();
                form5.Show();
                this.Hide();
            }
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            txtEmail.Text = user_data.email;
            txtName.Text = user_data.real_name;
            txtEmail.ReadOnly = true;
            txtName.ReadOnly = true;

            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();
                MySqlCommand cmdSelect = new MySqlCommand($"SELECT copy_id FROM tbl_book_copies " +
                    $"WHERE book_id = @bookId AND status = 'AVAILABLE' LIMIT 1", conn);
                cmdSelect.Parameters.AddWithValue("@bookId", bookId);

                MySqlDataReader reader = cmdSelect.ExecuteReader();
                if (reader.Read())
                {
                    selectedCopyId = reader.GetInt32("copy_id");
                }
                else
                {
                    MessageBox.Show("No available copies to borrow for this book.");
                    this.Close();
                }
                reader.Close();
                conn.Close();
            }

     
        }
    }
}
