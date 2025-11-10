using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace librarysystem
{
    public partial class circulation : Form
    {
        public circulation()
        {
            InitializeComponent();
            LoadCirculationData();
        }

        private void btnBookMng_Click(object sender, EventArgs e)
        {
            manage_books manageBooksForm = new manage_books();
            manageBooksForm.Show();
            this.Hide();
        }

        private void btnMemMng_Click(object sender, EventArgs e)
        {
            member_management memberManagementForm = new member_management();
            memberManagementForm.Show();
            this.Hide();
        }

        private void LoadCirculationData()
        {
            string sqlQuery = @"
                SELECT
                    s.StatusID AS 'Transaction ID',
                    s.status AS 'Current Status',
                    s.borrowed_date AS 'Borrowed Date',
                    s.return_date AS 'Due Date',
                    s.Actual_Return_Date AS 'Actual Return Date',
                    u.user_id AS 'User ID',
                    u.fullname AS 'Borrower Name',
                    u.fines_fees AS 'Total Fines (₱)',
                    COALESCE(s.book_id, s.journal_id) AS 'Item ID',
                    CASE
                        WHEN s.book_id IS NOT NULL THEN 'Book'
                        WHEN s.journal_id IS NOT NULL THEN 'Journal'
                        ELSE 'Unknown'
                    END AS 'Item Type',
                    COALESCE(b.BookTitle, j.JournalTitle) AS 'Title'
                FROM
                    status s
                LEFT JOIN
                    users u ON UPPER(s.user_id) = UPPER(u.user_id)
                LEFT JOIN
                    books b ON UPPER(s.book_id) = UPPER(b.BookID)
                LEFT JOIN
                    journals j ON UPPER(s.journal_id) = UPPER(j.JournalID)
                WHERE
                    s.status IN ('BORROWED', 'RESERVED')
                ORDER BY
                    s.return_date ASC;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connector.connectionString))
                {
                    connection.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlQuery, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Query executed successfully, but zero active rows were found.", "Debug info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading circulation data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
