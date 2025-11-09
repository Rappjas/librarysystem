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
            string bookid = IDGenerator.GeneratebookID();
            string isbn = IDGenerator.GenerateISBN();
            string shelf = IDGenerator.GenerateShelfNumber();
            string title = txtTitle.Text;
            string author = txtAuthor.Text;
            string genre = cbGenre.Text;
            int pubYear = int.Parse(txtYear.Text);
            int page = int.Parse(txtpages.Text);
            string publishers = txtpublisher.Text;
            string language = txtlanguage.Text;


            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"INSERT INTO books (BookID, BookTitle, Author, Genre, Pub_Date, pagecount, publisher, language, ISBN, Shelf_Number) " +
                               $"VALUES (@id, @title, @author, @genre, @pubYear, @pages, @publisher, @lang, @isbn, @shelf)", conn);
                cmd.Parameters.AddWithValue("id", bookid);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@genre", genre);
                cmd.Parameters.AddWithValue("@pubYear", pubYear);
                cmd.Parameters.AddWithValue("@pages", page);
                cmd.Parameters.AddWithValue("@publisher", publishers);
                cmd.Parameters.AddWithValue("@lang", language);
                cmd.Parameters.AddWithValue("@isbn", isbn);
                cmd.Parameters.AddWithValue("@shelf", shelf);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Book added successfully!");
            }
        }
    }
}
