namespace librarysystem
{
    partial class circulation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(circulation));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnMemMng = new System.Windows.Forms.Button();
            this.btnBookMng = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(691, 445);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // btnMemMng
            // 
            this.btnMemMng.Location = new System.Drawing.Point(62, 144);
            this.btnMemMng.Name = "btnMemMng";
            this.btnMemMng.Size = new System.Drawing.Size(105, 50);
            this.btnMemMng.TabIndex = 11;
            this.btnMemMng.Text = "Member Management";
            this.btnMemMng.UseVisualStyleBackColor = true;
            this.btnMemMng.Click += new System.EventHandler(this.btnMemMng_Click);
            // 
            // btnBookMng
            // 
            this.btnBookMng.Location = new System.Drawing.Point(66, 85);
            this.btnBookMng.Name = "btnBookMng";
            this.btnBookMng.Size = new System.Drawing.Size(101, 43);
            this.btnBookMng.TabIndex = 12;
            this.btnBookMng.Text = "Book Management";
            this.btnBookMng.UseVisualStyleBackColor = true;
            this.btnBookMng.Click += new System.EventHandler(this.btnBookMng_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(192, 169);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(450, 178);
            this.dataGridView1.TabIndex = 13;
            // 
            // circulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 445);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnBookMng);
            this.Controls.Add(this.btnMemMng);
            this.Controls.Add(this.pictureBox1);
            this.Name = "circulation";
            this.Text = "circulation";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnMemMng;
        private System.Windows.Forms.Button btnBookMng;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}