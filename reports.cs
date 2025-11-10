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
    public partial class reports : Form
    {
        public reports()
        {
            InitializeComponent();
            LoadBookReport();
            LoadInventoryReport();


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBox1.Text.Trim();

            if (selected == "Book Reports")
            {
                LoadBookReport();
                dataGridView1.Visible = true;
                txtMostRead.Visible = true;
            }
            else if (selected == "Book Inventory")
            {
                LoadInventoryReport();

                dataGridView1.Visible = true;
                txtMostRead.Visible = true;
            }

        }
        private void LoadBookReport()
        {
            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();

                
                string activityQuery = @"
                SELECT 
                s.book_id AS BookID,
                b.BookTitle,
                s.user_id AS UserID,
                s.status AS Status,
                s.borrowed_date AS BorrowedDate,
                s.return_date AS ReturnDate,
                s.Actual_Return_Date AS ActualReturnDate
                FROM status s
                JOIN books b ON s.book_id = b.BookID
                WHERE s.status IN ('BORROWED', 'RETURNED', 'LOST')";

                MySqlDataAdapter daActivity = new MySqlDataAdapter(activityQuery, conn);
                DataTable dtActivity = new DataTable();
                daActivity.Fill(dtActivity);
                dataGridView1.DataSource = dtActivity;
                dataGridView1.ClearSelection();

                
                string summaryText = "📚 Most Read Books:\r\n";

                // Most-read
                string popularQuery = @"
                SELECT b.BookTitle, COUNT(*) AS TimesBorrowed
                FROM status s
                JOIN books b ON s.book_id = b.BookID
                WHERE s.status IN ('BORROWED', 'RETURNED')
                GROUP BY b.BookTitle
                ORDER BY TimesBorrowed DESC
                LIMIT 5";

                MySqlCommand cmdPopular = new MySqlCommand(popularQuery, conn);
                using (MySqlDataReader reader = cmdPopular.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        summaryText += $"• {reader["BookTitle"]} — {reader["TimesBorrowed"]} times\r\n";
                    }
                }

                // Genre breakdown
                summaryText += "\r\n🏷️ Genre Breakdown:\r\n";
                string genreQuery = "SELECT Genre, COUNT(*) AS Count FROM books GROUP BY Genre";
                MySqlCommand cmdGenre = new MySqlCommand(genreQuery, conn);
                using (MySqlDataReader reader = cmdGenre.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        summaryText += $"• {reader["Genre"]}: {reader["Count"]} books\r\n";
                    }
                }
                txtMostRead.Text = summaryText;

                conn.Close();
            }
        }

        private void LoadInventoryReport()
        {
            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();

                // Grid data
                string query = @"
            SELECT 
                BookID,
                BookTitle,
                Author,
                Genre,
                Pub_Date,
                Pagecount,
                Publisher,
                Language,
                ISBN,
                status,
                Shelf_Number
            FROM books";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.ClearSelection();

                // Summary
                MySqlCommand cmdTotalBooks = new MySqlCommand("SELECT COUNT(*) FROM books", conn);
                int totalBooks = Convert.ToInt32(cmdTotalBooks.ExecuteScalar());

                MySqlCommand cmdFines = new MySqlCommand(
                "SELECT COUNT(*) FROM users WHERE fines_fees > 0", conn);
                int Fines = Convert.ToInt32(cmdFines.ExecuteScalar());


                string summaryText = "📦 Book Inventory Summary:\r\n\r\n";
                summaryText += $"\r\nTotal Books in Library: {totalBooks}\r\n";
                summaryText += $"\r\nTotal Fines: {Fines}\r\n";

                txtMostRead.Text = summaryText;

                conn.Close();
            }
        }
        private void reports_Load(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            txtMostRead.Visible = false;
        }
    }
    }