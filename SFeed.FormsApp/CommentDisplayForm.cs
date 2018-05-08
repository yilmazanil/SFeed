using SFeed.Business.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SFeed.FormsApp
{
    public partial class CommentDisplayForm : Form
    {
        public string FormId { get; set; }

        public CommentDisplayForm(string formId)
        {
            this.FormId = formId;
            InitializeComponent();
            InitializeComments();
        }

        private void InitializeComments()
        {
            var provider = new CommentProvider();
            var comments = provider.GetComments(FormId, 0, 1000);

            CommentsGridView.Columns.Add("Id", "Id");
            CommentsGridView.Columns.Add("Body", "İçerik");
            CommentsGridView.Columns.Add("CreatedBy", "Paylaşan");
            CommentsGridView.Columns.Add("CreatedDate", "Paylaşım Tarihi");
            CommentsGridView.Columns.Add("ModifiedDate", "Güncellenme Tarihi");
            CommentsGridView.Columns.Add("LikeCount", "Beğeni Sayısı");

            var likeButton = new DataGridViewCheckBoxColumn();
            likeButton.Name = "Like";
            likeButton.HeaderText = "Beğen";
            CommentsGridView.Columns.Add(likeButton);

            foreach (var comment in comments)
            {
                CommentsGridView.Rows.Add(comment.Id, comment.Body, comment.CreatedBy, comment.CreatedDate, comment.ModifiedDate, comment.LikeCount, false);
            }

        }
    }
}
