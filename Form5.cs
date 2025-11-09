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
    public partial class Form5 : Form
    {
        private int borrowerId;

        public Form5()
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
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable data_table = new DataTable();
                adapter.Fill(data_table);
                dataGridView1.DataSource = data_table;
                dataGridView1.ClearSelection();

                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("You have not borrowed any books yet.");
                }
                conn.Close();
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            string dbconnection = "server=127.0.0.1; database=library_db; uid=root; pwd=;";

            using (MySqlConnection conn = new MySqlConnection(dbconnection))
            {
                conn.Open();

                string query = "SELECT Name, Username, Joined FROM tbl_users WHERE User_ID = @userID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userID", user_data.user_id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    label1.Text = reader["Name"].ToString();
                    label2.Text = reader["Username"].ToString();

                    if (reader["Joined"] != DBNull.Value)
                    {
                        label3.Text = Convert.ToDateTime(reader["Joined"]).ToString("MMMM dd, yyyy");
                    }
                    else
                    {
                        label3.Text = "N/A";
                    }
                }

                lblWelcome.Text = "Welcome, " + user_data.currentuser + "!";
            } 
            
            // sa user history ito //
            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT bc.copy_id, b.title, bc.date_borrow, bc.date_return, br.borrower_name " +
                        $"FROM tbl_book_copies bc " +
                        $"JOIN tbl_books b ON bc.book_id = b.book_id " +
                        $"JOIN tbl_borrowers br ON bc.borrower_id = br.borrower_id " +
                        $"WHERE br.borrower_name = @name AND bc.status = 'BORROWED'", conn);
                cmd.Parameters.AddWithValue("@name", user_data.real_name);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();
                conn.Close();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();
            form6.Show();
            this.Hide();
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
    }
}
