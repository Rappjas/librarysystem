using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace librarysystem
{
    public partial class Form6 : Form
    {
        private int borrowerId;
        public Form6()
        {
            InitializeComponent();
            LoadBook();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            cmbCategory.Items.AddRange(new string[] { "All", "Title", "Author", "Genre" });
            cmbCategory.SelectedIndex = 0; // Default to All
            LoadBook();

            lblWelcome.Text = "Welcome, " + user_data.currentuser + "!";
        }

        private void LoadBook(string keyword = "", string category = "All")
        {
            string sql = $"SELECT b.book_id, b.title, b.author, b.genre, b.publication_year, " +
             $"(SELECT COUNT(*) FROM tbl_book_copies bc WHERE bc.book_id = b.book_id AND bc.status = 'AVAILABLE') AS available_copies " +
             $"FROM tbl_books b";

            if (!string.IsNullOrEmpty(keyword))
            {
                if (category == "All")
                {
                    sql += " WHERE b.title LIKE @keyword OR b.author LIKE @keyword OR b.genre LIKE @keyword";
                }
                else if (category == "Title")
                {
                    sql += " WHERE b.title LIKE @keyword";
                }
                else if (category == "Author")
                {
                    sql += " WHERE b.author LIKE @keyword";
                }
                else if (category == "Genre")
                {
                    sql += " WHERE b.genre LIKE @keyword";
                }
            }
            using (MySqlConnection conn = new MySqlConnection(connector.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                if (!string.IsNullOrEmpty(keyword))
                {
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                }

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (!dt.Columns.Contains("images"))
                {
                    dt.Columns.Add("images", typeof(Bitmap));
                }

                string folderPath = Path.Combine(Application.StartupPath, "images");

                foreach (DataRow row in dt.Rows)
                {
                    string genre = row["genre"].ToString().ToLower();
                    string imageFile = "default.jpg";

                    if (genre.Contains("programming"))
                        imageFile = "programming.png";
                    else if (genre.Contains("horror"))
                        imageFile = "horror.png";

                    string imagePath = Path.Combine(folderPath, imageFile);

                    if (File.Exists(imagePath))
                        using (var original = new Bitmap(imagePath))
                        {
                            int newWidth = 60;   
                            int newHeight = 60;  
                            Bitmap resized = new Bitmap(original, new Size(newWidth, newHeight));
                            row["images"] = resized;
                        }
                    else
                        row["images"] = null;
                }

                dataGridView1.DataSource = dt;

                // pagdagdag ng column(temporary) hindi connected sa db
                if (!dataGridView1.Columns.Contains("images"))
                {
                    DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
                    imgCol.Name = "images";
                    imgCol.HeaderText = "Image";
                    imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    imgCol.DataPropertyName = "images"; 
                    dataGridView1.Columns.Add(imgCol);
                }
                dataGridView1.RowTemplate.Height = 100;
                dataGridView1.Columns["images"].Width = 100;

                dataGridView1.ClearSelection();
                conn.Close();
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            string category = cmbCategory.SelectedItem.ToString();
            LoadBook(keyword, category);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (borrowerId == 0)
            {
                MessageBox.Show("You have not borrowed any books yet.");
                return;
            }

            Form9 bview = new Form9(borrowerId);
            bview.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to borrow");
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            int bookId = Convert.ToInt32(row.Cells["book_id"].Value);
            String title = row.Cells["title"].Value.ToString();
            String author = row.Cells["author"].Value.ToString();
            int availableCopies = Convert.ToInt32(row.Cells["available_copies"].Value);

            if (availableCopies <= 0)
            {
                MessageBox.Show("No available copies to borrow for this book.");
                return;
            }

            Form7 brform = new Form7(bookId, title, author);
            brform.Show();
            this.Hide();

            if (brform.BorrowerId > 0)
            {
                borrowerId = brform.BorrowerId;
            }
            LoadBook();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            this.Hide();
        }

        private void button_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form10 form10 = new Form10();
            form10.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a book to reserve");
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            int availableCopies = Convert.ToInt32(row.Cells["available_copies"].Value);

            if (availableCopies > 0)
            {
                MessageBox.Show("This book has available copies. Please borrow instead.");
                return;
            }

            int bookId = Convert.ToInt32(row.Cells["book_id"].Value);
            string title = row.Cells["title"].Value.ToString();
            string author = row.Cells["author"].Value.ToString();
            int borrowerId = this.borrowerId;


            Form12 form12 = new Form12(bookId, borrowerId, title, author);
            form12.Show();
            this.Hide();

            LoadBook();
        }
    }
}
