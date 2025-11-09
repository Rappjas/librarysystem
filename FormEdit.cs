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
    public partial class FormEdit : Form
    {
        private string bookId;
        public FormEdit(string bookId, string title, string author, string genre, string publisher, string language)
        {
            InitializeComponent();
            this.bookId = bookId;
            txtTitle.Text = title;
            txtAuthor.Text = author;
            txtGenre.Text = genre;
            txtPublisher.Text = publisher;
            txtLanguage.Text = language;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE books SET BookTitle=@title, Author=@author, Genre=@genre, Publisher=@publisher, Language=@language WHERE BookID=@bookId", conn);

                cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                cmd.Parameters.AddWithValue("@author", txtAuthor.Text.Trim());
                cmd.Parameters.AddWithValue("@genre", txtGenre.Text.Trim());
                cmd.Parameters.AddWithValue("@publisher", txtPublisher.Text.Trim());
                cmd.Parameters.AddWithValue("@language", txtLanguage.Text.Trim());
                cmd.Parameters.AddWithValue("@bookId", bookId);

                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Book details updated successfully.");
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
