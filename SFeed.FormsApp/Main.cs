using AutoMapper;
using log4net;
using SFeed.Business.MapperConfig;
using SFeed.Business.Providers;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models.Wall;
using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace SFeed.FormsApp
{
    public partial class MainForm : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainForm));
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                RegisterEntityToViewModelMapper.Register(cfg);
                cfg.CreateMap<WallPostModel, WallPostGridViewModel>();
            });

            SetUsername();
            FillFollowers();
            GenerateWallPostGrid();
            WallSelectionComboBox.SelectedIndex = 0;
            FollowingTypeCombobox.SelectedIndex = 0;
            StopFollowingMenuItem.Click += StopFollowingMenuItem_Click;

        }

        private void StopFollowingMenuItem_Click(object sender, EventArgs e)
        {
            var item = FollowingListBox.SelectedItem.ToString();

            var parts = item.Split(' ');
            var length = parts.Length;
            var toUnfollow = string.Empty;
            for (int i = 0; i < length - 1; i++)
            {
                toUnfollow += parts[i];
            }

            var followerProvider = new FollowerProvider();
            if (item.EndsWith("Grup)"))
            {
                followerProvider.UnfollowGroup(FormHelper.Username, toUnfollow);
            }
            else
            {
                followerProvider.UnfollowUser(FormHelper.Username, toUnfollow);
            }
            FillFollowers();
        }

        private void ChangeUsernameButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                MessageBox.Show("Kullanıcı adı girmediniz!");
                return;
            }
            FormHelper.Username = UsernameTextBox.Text;
            FillFollowers();
            GenerateWallPostGrid();
        }

        private void SetUsername()
        {
            var currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();

            if (currentUser != null && !string.IsNullOrWhiteSpace(currentUser.Name))
            {
                var username = currentUser.Name;
                if (username.Contains("\\"))
                {
                    username = username.Split('\\')[1];
                }
                FormHelper.Username = username;
                UsernameTextBox.Text = username;
            }

        }

        private void ShareToOwnWallCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            WallOwnerTextBox.Visible = !ShareToOwnWallCheckbox.Checked;
            WallOwnerLabel.Visible = !ShareToOwnWallCheckbox.Checked;
            WallSelectionComboBox.Visible = !ShareToOwnWallCheckbox.Checked;
        }

        private void ShareButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ShareTextBox.Text))
            {
                MessageBox.Show("İçerik girmediniz!");
                return;
            }

            var request = new WallPostCreateRequest();
            request.Body = ShareTextBox.Text;
            request.PostedBy = FormHelper.Username;
            request.PostType = WallPostType.text;
            if (ShareToOwnWallCheckbox.Checked)
            {
                request.TargetWall = new WallModel { OwnerId = FormHelper.Username, WallOwnerType = WallType.user };
            }
            else
            {
                var ownerType = WallSelectionComboBox.Text == "Grup" ? WallType.group : WallType.user;
                request.TargetWall = new WallModel { OwnerId = WallOwnerTextBox.Text, WallOwnerType = ownerType };
            }

            var provider = new WallPostProvider();
            var postId = provider.AddPost(request);
            var feedProvider = new NewsfeedProvider();
            var newsFeedEntry = new NewsfeedItem
            {
                By = request.PostedBy,
                ReferencePostId = postId,
                FeedType = NewsfeedActionType.wallpost,
                WallOwner = new NewsfeedWallModel { IsPublic = true, OwnerId = request.PostedBy, WallOwnerType = WallType.user }
            };
            feedProvider.AddNewsfeedItem(newsFeedEntry);
        }

        private void FillFollowers()
        {
            var followerProvider = new FollowerProvider();
            var followers = followerProvider.GetUserFollowers(FormHelper.Username).ToList();
            var cachedFollowers = followerProvider.GetUserFollowersCached(FormHelper.Username).ToList();
            var followingUsers = followerProvider.GetFollowingUsersPaged(FormHelper.Username, 0, 1000).Records.Select(p=>string.Concat(p," (Kullanıcı)")).ToList();
            var followingGroups = followerProvider.GetFollowingGroupsPaged(FormHelper.Username, 0, 1000).Records.Select(p=>string.Concat(p," (Grup)")).ToList();
            var following = followingUsers.Union(followingGroups).ToList();

            FollowersListBox.DataSource = null;
            FollowersListBox.Items.Clear();
            FollowersListBox.DataSource = followers;
            CachedFollowersListBox.DataSource = null;
            CachedFollowersListBox.Items.Clear();
            CachedFollowersListBox.DataSource = cachedFollowers;
            FollowingListBox.DataSource = null;
            FollowingListBox.Items.Clear();
            FollowingListBox.DataSource = following;
        }

        private void FollowUserButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FollowUserTextBox.Text))
            {
                MessageBox.Show("Takip edilecek kullanıcı/grup seçmediniz!");
                return;
            }
            var followerProvider = new FollowerProvider();
            if (FollowingTypeCombobox.Text == "Grup")
            {
                followerProvider.FollowGroup(FormHelper.Username, FollowUserTextBox.Text);
            }
            else
            {
                followerProvider.FollowUser(FormHelper.Username, FollowUserTextBox.Text);
            }
            FillFollowers();
        }

        private void RefreshFollowersButton_Click(object sender, EventArgs e)
        {
            FillFollowers();
        }

        private void GenerateWallPostGrid()
        {
            WallPostGridView.Rows.Clear();
            WallPostGridView.Columns.Clear();
            WallPostGridView.Columns.Add("Id", "Id");
            WallPostGridView.Columns.Add("Body", "İçerik");
            WallPostGridView.Columns.Add("PostedBy", "Paylaşan");
            WallPostGridView.Columns.Add("CreatedDate", "Paylaşım Tarihi");
            WallPostGridView.Columns.Add("ModifiedDate", "Güncellenme Tarihi");
            WallPostGridView.Columns.Add("LikeCount", "Beğeni Sayısı");
            WallPostGridView.Columns.Add("CommentCount", "Yorum Sayısı");

            var likeButton = new DataGridViewCheckBoxColumn();
            likeButton.Name = "Like";
            likeButton.HeaderText = "Beğen";
            WallPostGridView.Columns.Add(likeButton);
            var commentButton = new DataGridViewButtonColumn();
            commentButton.Name = "Comment";
            commentButton.HeaderText = "Comment";
            commentButton.Text = "Beğen";
            WallPostGridView.Columns.Add(commentButton);
            var commentCountButton = new DataGridViewButtonColumn();
            commentCountButton.Name = "CommentDetails";
            commentCountButton.HeaderText = "Yorumlar";
            commentCountButton.Text = "Yorumlar";
            WallPostGridView.Columns.Add(commentCountButton);


            var provider = new WallPostProvider();
            var posts = provider.GetUserWallDetailed(FormHelper.Username, DateTime.Now, 1000);


            foreach (var post in posts)
            {
                WallPostGridView.Rows.Add(post.Id, post.Body, post.PostedBy, post.CreatedDate, post.ModifiedDate, post.LikeCount, post.CommentCount, false);
            }
        }

        private void GenerateNewsFeedGrid()
        {
            NewsFeedGridView.Rows.Clear();
            NewsFeedGridView.Columns.Clear();
            NewsFeedGridView.Columns.Add("Id", "Id");
            NewsFeedGridView.Columns.Add("Body", "İçerik");
            NewsFeedGridView.Columns.Add("PostedBy", "Paylaşan");
            NewsFeedGridView.Columns.Add("CreatedDate", "Paylaşım Tarihi");
            NewsFeedGridView.Columns.Add("ModifiedDate", "Güncellenme Tarihi");
            NewsFeedGridView.Columns.Add("LikeCount", "Beğeni Sayısı");
            NewsFeedGridView.Columns.Add("CommentCount", "Yorum Sayısı");
            NewsFeedGridView.Columns.Add("Events", "Aksiyonlar");



            var provider = new NewsfeedProvider();
            var posts = provider.GetUserNewsfeed(FormHelper.Username, 0, 1000);


            foreach (var post in posts)
            {
                var events = post.FeedDescription.Select(p => string.Concat(p.By, ":", Convert.ToString(p.Action)));
                var actions = string.Empty;

                if (events != null)
                {
                    actions = string.Join(";", events);
                }

                NewsFeedGridView.Rows.Add(post.Id, post.Body, post.PostedBy, post.CreatedDate, post.ModifiedDate, post.LikeCount, post.CommentCount, actions);
            }
        }

        private void WallPostGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            var likeColumnIndex = WallPostGridView.Columns["Like"].Index;
            if (e.ColumnIndex == likeColumnIndex && e.RowIndex >= 0)
            {
                WallPostGridView.EndEdit();
                var row = WallPostGridView.Rows[e.RowIndex];
                var isLiked = Convert.ToBoolean((row.Cells["Like"] as DataGridViewCheckBoxCell).Value);
                var id = Convert.ToString(row.Cells["Id"].Value);
                var likeProvider = new EntryLikeProvider();
                var feedProvider = new NewsfeedProvider();
                if (isLiked)
                {
                    likeProvider.LikePost(id, FormHelper.Username);

                    var newsFeedEntry = new NewsfeedItem
                    {
                        By = FormHelper.Username,
                        ReferencePostId = id,
                        FeedType = NewsfeedActionType.like,
                        WallOwner = new Core.Models.Wall.NewsfeedWallModel { IsPublic = true, OwnerId = FormHelper.Username, WallOwnerType = Core.Models.Wall.WallType.user }
                    };

                    feedProvider.AddNewsfeedItem(newsFeedEntry);
                }
                else
                {
                    likeProvider.UnlikePost(id, FormHelper.Username);
                    var newsFeedEntry = new NewsfeedItem
                    {
                        By = FormHelper.Username,
                        ReferencePostId = id,
                        FeedType = NewsfeedActionType.like,
                        WallOwner = new Core.Models.Wall.NewsfeedWallModel { IsPublic = true, OwnerId = FormHelper.Username, WallOwnerType = Core.Models.Wall.WallType.user }
                    };

                    feedProvider.RemoveNewsfeedItem(newsFeedEntry);
                }
            }
        }

        private void WallPostGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var commentColumnIndex = WallPostGridView.Columns["Comment"].Index;
                var commentDetailsColumnIndex = WallPostGridView.Columns["CommentDetails"].Index;
                if (e.ColumnIndex == commentColumnIndex)
                {
                    var row = WallPostGridView.Rows[e.RowIndex];
                    var id = Convert.ToString(row.Cells["Id"].Value);
                    CommentAddForm commentForm = new CommentAddForm(id);
                    commentForm.Show();
                }
                else if (e.ColumnIndex == commentDetailsColumnIndex)
                {
                    var row = WallPostGridView.Rows[e.RowIndex];
                    var id = Convert.ToString(row.Cells["Id"].Value);
                    CommentDisplayForm commentForm = new CommentDisplayForm(id);
                    commentForm.Show();
                }
            }
        }

        private void RefreshWallPostButton_Click(object sender, EventArgs e)
        {
            GenerateWallPostGrid();
        }

        private void RefreshNewsFeedButton_Click(object sender, EventArgs e)
        {
            GenerateNewsFeedGrid();
        }

        private void RefreshGroupWallButton_Click(object sender, EventArgs e)
        {
            var groupName = GroupWallGroupNameTextBox.Text;


            GroupWallGridView.Rows.Clear();
            GroupWallGridView.Columns.Clear();
            GroupWallGridView.Columns.Add("Id", "Id");
            GroupWallGridView.Columns.Add("Body", "İçerik");
            GroupWallGridView.Columns.Add("PostedBy", "Paylaşan");
            GroupWallGridView.Columns.Add("CreatedDate", "Paylaşım Tarihi");
            GroupWallGridView.Columns.Add("ModifiedDate", "Güncellenme Tarihi");
            GroupWallGridView.Columns.Add("LikeCount", "Beğeni Sayısı");
            GroupWallGridView.Columns.Add("CommentCount", "Yorum Sayısı");

            var likeButton = new DataGridViewCheckBoxColumn();
            likeButton.Name = "Like";
            likeButton.HeaderText = "Beğen";
            GroupWallGridView.Columns.Add(likeButton);
            var commentButton = new DataGridViewButtonColumn();
            commentButton.Name = "Comment";
            commentButton.HeaderText = "Comment";
            commentButton.Text = "Beğen";
            GroupWallGridView.Columns.Add(commentButton);
            var commentCountButton = new DataGridViewButtonColumn();
            commentCountButton.Name = "CommentDetails";
            commentCountButton.HeaderText = "Yorumlar";
            commentCountButton.Text = "Yorumlar";
            GroupWallGridView.Columns.Add(commentCountButton);


            var provider = new WallPostProvider();
            var posts = provider.GetGroupWall(groupName, 0, 100);


            foreach (var post in posts)
            {
                GroupWallGridView.Rows.Add(post.Id, post.Body, post.PostedBy, post.CreatedDate, post.ModifiedDate, post.LikeCount, post.CommentCount, false);
            }
        }

        private void FollowingListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var index = FollowingListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                FollowingListBox.SelectedIndex = index;
                FollowingContextMenuStrip.Visible = true;
            }
            else
            {
                FollowingContextMenuStrip.Visible = false;
            }
        }

        private void ShareMultipleButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ShareTextBox.Text))
            {
                MessageBox.Show("İçerik girmediniz!");
                return;
            }

            var request = new WallPostCreateRequest();
            request.Body = ShareTextBox.Text;
            request.PostedBy = FormHelper.Username;
            request.PostType = WallPostType.text;
            if (ShareToOwnWallCheckbox.Checked)
            {
                request.TargetWall = new WallModel { OwnerId = FormHelper.Username, WallOwnerType = WallType.user };
            }
            else
            {
                var ownerType = WallSelectionComboBox.Text == "Grup" ? WallType.group : WallType.user;
                request.TargetWall = new WallModel { OwnerId = WallOwnerTextBox.Text, WallOwnerType = ownerType };
            }

            var provider = new WallPostProvider();

            for (int i = 0; i < 100; i++)
            {
                var postId = provider.AddPost(request);
                var feedProvider = new NewsfeedProvider();
                var newsFeedEntry = new NewsfeedItem
                {
                    By = request.PostedBy,
                    ReferencePostId = postId,
                    FeedType = NewsfeedActionType.wallpost,
                    WallOwner = new NewsfeedWallModel { IsPublic = true, OwnerId = request.PostedBy, WallOwnerType = WallType.user }
                };
                feedProvider.AddNewsfeedItem(newsFeedEntry);
            }
        }
    }
}
