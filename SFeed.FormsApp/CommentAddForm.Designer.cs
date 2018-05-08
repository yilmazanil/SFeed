namespace SFeed.FormsApp
{
    partial class CommentAddForm
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
            this.CommentBodyTextBox = new System.Windows.Forms.TextBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CommentBodyTextBox
            // 
            this.CommentBodyTextBox.Location = new System.Drawing.Point(13, 13);
            this.CommentBodyTextBox.Multiline = true;
            this.CommentBodyTextBox.Name = "CommentBodyTextBox";
            this.CommentBodyTextBox.Size = new System.Drawing.Size(259, 209);
            this.CommentBodyTextBox.TabIndex = 0;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(197, 226);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Kaydet";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CommentAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.CommentBodyTextBox);
            this.Name = "CommentAddForm";
            this.Text = "CommentAddForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox CommentBodyTextBox;
        private System.Windows.Forms.Button SaveButton;
    }
}