using SFeed.Business.Providers;
using SFeed.Core.Models.Comments;
using SFeed.Core.Models.Newsfeed;
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
    public partial class CommentAddForm : Form
    {
        public string FormId { get; set; }

        public CommentAddForm(string formId)
        {
            this.FormId = formId;
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var commentProvider = new CommentProvider();

            var request = new CommentCreateRequest();
            request.Body = CommentBodyTextBox.Text;
            request.CreatedBy = FormHelper.Username;
            request.WallPostId = FormId;
            commentProvider.AddComment(request);

            var feedProvider = new NewsfeedProvider();
            var newsFeedEntry = new NewsfeedItem
            {
                By = FormHelper.Username,
                ReferencePostId = FormId,
                FeedType = NewsfeedActionType.comment,
                WallOwner = new Core.Models.Wall.NewsfeedWallModel { IsPublic = true, OwnerId = FormHelper.Username, WallOwnerType = Core.Models.Wall.WallType.user }
            };

            feedProvider.AddNewsfeedItem(newsFeedEntry);

            this.Close();
        }
    }
}
