namespace SFeed.FormsApp
{
    partial class CommentDisplayForm
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
            this.CommentsGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.CommentsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // CommentsGridView
            // 
            this.CommentsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CommentsGridView.Location = new System.Drawing.Point(13, 13);
            this.CommentsGridView.Name = "CommentsGridView";
            this.CommentsGridView.Size = new System.Drawing.Size(690, 498);
            this.CommentsGridView.TabIndex = 0;
            // 
            // CommentDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 538);
            this.Controls.Add(this.CommentsGridView);
            this.Name = "CommentDisplayForm";
            this.Text = "CommentDisplayForm";
            ((System.ComponentModel.ISupportInitialize)(this.CommentsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView CommentsGridView;
    }
}