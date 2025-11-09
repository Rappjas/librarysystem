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
    public partial class add_books : Form
    {
        public add_books()
        {
            InitializeComponent();
        }

        private void Form13_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text;
            string author = txtAuthor.Text;
            string genre = cbGenre.Text;
            int pubYear = int.Parse(txtYear.Text);
            int copies = int.Parse(txtCopies.Text);

            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"INSERT INTO tbl_books (title, author, genre, publication_year) " +
                               $"VALUES (@title, @author, @genre, @pubYear)", conn);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@genre", genre);
                cmd.Parameters.AddWithValue("@pubYear", pubYear);
                cmd.ExecuteNonQuery();

                long bookId = cmd.LastInsertedId;

                for (int i = 0; i < copies; i++)
                {
                    MySqlCommand cmdCopy = new MySqlCommand(
                        $"INSERT INTO tbl_book_copies (book_id) VALUES (@bookId)", conn);
                    cmdCopy.Parameters.AddWithValue("@bookId", bookId);
                    cmdCopy.ExecuteNonQuery();
                }
                MessageBox.Show("Book added successfully!");
            }
        }
    }
}
