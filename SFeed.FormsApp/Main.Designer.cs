namespace SFeed.FormsApp
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.ChangeUsernameButton = new System.Windows.Forms.Button();
            this.ChangeUsernameLabel = new System.Windows.Forms.Label();
            this.ShareTextBox = new System.Windows.Forms.TextBox();
            this.ShareButton = new System.Windows.Forms.Button();
            this.ShareToOwnWallCheckbox = new System.Windows.Forms.CheckBox();
            this.WallOwnerTextBox = new System.Windows.Forms.TextBox();
            this.WallOwnerLabel = new System.Windows.Forms.Label();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.ShareTab = new System.Windows.Forms.TabPage();
            this.WallSelectionComboBox = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.FollowingTypeCombobox = new System.Windows.Forms.ComboBox();
            this.RefreshFollowersButton = new System.Windows.Forms.Button();
            this.FollowingLabel = new System.Windows.Forms.Label();
            this.FollowingListBox = new System.Windows.Forms.ListBox();
            this.FollowingContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.StopFollowingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FollowUserButton = new System.Windows.Forms.Button();
            this.FollowUserTextBox = new System.Windows.Forms.TextBox();
            this.CachedFollowersLabel = new System.Windows.Forms.Label();
            this.CachedFollowersListBox = new System.Windows.Forms.ListBox();
            this.FollowersLabel = new System.Windows.Forms.Label();
            this.FollowersListBox = new System.Windows.Forms.ListBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.RefreshWallPostButton = new System.Windows.Forms.Button();
            this.WallPostGridView = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.RefreshNewsFeedButton = new System.Windows.Forms.Button();
            this.NewsFeedGridView = new System.Windows.Forms.DataGridView();
            this.GroupWallTab = new System.Windows.Forms.TabPage();
            this.GroupWallGridView = new System.Windows.Forms.DataGridView();
            this.RefreshGroupWallButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.GroupWallGroupNameTextBox = new System.Windows.Forms.TextBox();
            this.WallPostBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ShareMultipleButton = new System.Windows.Forms.Button();
            this.MainTabControl.SuspendLayout();
            this.ShareTab.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.FollowingContextMenuStrip.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WallPostGridView)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NewsFeedGridView)).BeginInit();
            this.GroupWallTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GroupWallGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WallPostBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Location = new System.Drawing.Point(1067, 17);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(100, 20);
            this.UsernameTextBox.TabIndex = 0;
            // 
            // ChangeUsernameButton
            // 
            this.ChangeUsernameButton.Location = new System.Drawing.Point(1173, 15);
            this.ChangeUsernameButton.Name = "ChangeUsernameButton";
            this.ChangeUsernameButton.Size = new System.Drawing.Size(84, 23);
            this.ChangeUsernameButton.TabIndex = 1;
            this.ChangeUsernameButton.Text = "Değiştir";
            this.ChangeUsernameButton.UseVisualStyleBackColor = true;
            this.ChangeUsernameButton.Click += new System.EventHandler(this.ChangeUsernameButton_Click);
            // 
            // ChangeUsernameLabel
            // 
            this.ChangeUsernameLabel.AutoSize = true;
            this.ChangeUsernameLabel.Location = new System.Drawing.Point(991, 20);
            this.ChangeUsernameLabel.Name = "ChangeUsernameLabel";
            this.ChangeUsernameLabel.Size = new System.Drawing.Size(70, 13);
            this.ChangeUsernameLabel.TabIndex = 2;
            this.ChangeUsernameLabel.Text = "Kullanıcı Adı: ";
            // 
            // ShareTextBox
            // 
            this.ShareTextBox.Location = new System.Drawing.Point(6, 6);
            this.ShareTextBox.Multiline = true;
            this.ShareTextBox.Name = "ShareTextBox";
            this.ShareTextBox.Size = new System.Drawing.Size(1229, 556);
            this.ShareTextBox.TabIndex = 3;
            // 
            // ShareButton
            // 
            this.ShareButton.Location = new System.Drawing.Point(1036, 576);
            this.ShareButton.Name = "ShareButton";
            this.ShareButton.Size = new System.Drawing.Size(84, 27);
            this.ShareButton.TabIndex = 4;
            this.ShareButton.Text = "Paylaş";
            this.ShareButton.UseVisualStyleBackColor = true;
            this.ShareButton.Click += new System.EventHandler(this.ShareButton_Click);
            // 
            // ShareToOwnWallCheckbox
            // 
            this.ShareToOwnWallCheckbox.AutoSize = true;
            this.ShareToOwnWallCheckbox.Checked = true;
            this.ShareToOwnWallCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShareToOwnWallCheckbox.Location = new System.Drawing.Point(909, 582);
            this.ShareToOwnWallCheckbox.Name = "ShareToOwnWallCheckbox";
            this.ShareToOwnWallCheckbox.Size = new System.Drawing.Size(107, 17);
            this.ShareToOwnWallCheckbox.TabIndex = 5;
            this.ShareToOwnWallCheckbox.Text = "Kendi Duvarımda";
            this.ShareToOwnWallCheckbox.UseVisualStyleBackColor = true;
            this.ShareToOwnWallCheckbox.CheckedChanged += new System.EventHandler(this.ShareToOwnWallCheckbox_CheckedChanged);
            // 
            // WallOwnerTextBox
            // 
            this.WallOwnerTextBox.Location = new System.Drawing.Point(1036, 606);
            this.WallOwnerTextBox.Name = "WallOwnerTextBox";
            this.WallOwnerTextBox.Size = new System.Drawing.Size(109, 20);
            this.WallOwnerTextBox.TabIndex = 6;
            this.WallOwnerTextBox.Visible = false;
            // 
            // WallOwnerLabel
            // 
            this.WallOwnerLabel.AutoSize = true;
            this.WallOwnerLabel.Location = new System.Drawing.Point(1151, 609);
            this.WallOwnerLabel.Name = "WallOwnerLabel";
            this.WallOwnerLabel.Size = new System.Drawing.Size(56, 13);
            this.WallOwnerLabel.TabIndex = 7;
            this.WallOwnerLabel.Text = "Duvarında";
            this.WallOwnerLabel.Visible = false;
            // 
            // MainTabControl
            // 
            this.MainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainTabControl.Controls.Add(this.ShareTab);
            this.MainTabControl.Controls.Add(this.tabPage2);
            this.MainTabControl.Controls.Add(this.tabPage1);
            this.MainTabControl.Controls.Add(this.tabPage3);
            this.MainTabControl.Controls.Add(this.GroupWallTab);
            this.MainTabControl.Location = new System.Drawing.Point(12, 44);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(1265, 671);
            this.MainTabControl.TabIndex = 8;
            // 
            // ShareTab
            // 
            this.ShareTab.Controls.Add(this.ShareMultipleButton);
            this.ShareTab.Controls.Add(this.WallSelectionComboBox);
            this.ShareTab.Controls.Add(this.ShareTextBox);
            this.ShareTab.Controls.Add(this.WallOwnerTextBox);
            this.ShareTab.Controls.Add(this.WallOwnerLabel);
            this.ShareTab.Controls.Add(this.ShareButton);
            this.ShareTab.Controls.Add(this.ShareToOwnWallCheckbox);
            this.ShareTab.Location = new System.Drawing.Point(4, 22);
            this.ShareTab.Name = "ShareTab";
            this.ShareTab.Padding = new System.Windows.Forms.Padding(3);
            this.ShareTab.Size = new System.Drawing.Size(1257, 645);
            this.ShareTab.TabIndex = 0;
            this.ShareTab.Text = "Paylaş";
            this.ShareTab.UseVisualStyleBackColor = true;
            // 
            // WallSelectionComboBox
            // 
            this.WallSelectionComboBox.FormattingEnabled = true;
            this.WallSelectionComboBox.Items.AddRange(new object[] {
            "Kullanıcı",
            "Grup"});
            this.WallSelectionComboBox.Location = new System.Drawing.Point(909, 605);
            this.WallSelectionComboBox.Name = "WallSelectionComboBox";
            this.WallSelectionComboBox.Size = new System.Drawing.Size(121, 21);
            this.WallSelectionComboBox.TabIndex = 8;
            this.WallSelectionComboBox.Visible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.FollowingTypeCombobox);
            this.tabPage2.Controls.Add(this.RefreshFollowersButton);
            this.tabPage2.Controls.Add(this.FollowingLabel);
            this.tabPage2.Controls.Add(this.FollowingListBox);
            this.tabPage2.Controls.Add(this.FollowUserButton);
            this.tabPage2.Controls.Add(this.FollowUserTextBox);
            this.tabPage2.Controls.Add(this.CachedFollowersLabel);
            this.tabPage2.Controls.Add(this.CachedFollowersListBox);
            this.tabPage2.Controls.Add(this.FollowersLabel);
            this.tabPage2.Controls.Add(this.FollowersListBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1257, 645);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Takip";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // FollowingTypeCombobox
            // 
            this.FollowingTypeCombobox.FormattingEnabled = true;
            this.FollowingTypeCombobox.Items.AddRange(new object[] {
            "Kullanıcı",
            "Grup"});
            this.FollowingTypeCombobox.Location = new System.Drawing.Point(16, 8);
            this.FollowingTypeCombobox.Name = "FollowingTypeCombobox";
            this.FollowingTypeCombobox.Size = new System.Drawing.Size(121, 21);
            this.FollowingTypeCombobox.TabIndex = 9;
            // 
            // RefreshFollowersButton
            // 
            this.RefreshFollowersButton.Location = new System.Drawing.Point(1166, 8);
            this.RefreshFollowersButton.Name = "RefreshFollowersButton";
            this.RefreshFollowersButton.Size = new System.Drawing.Size(75, 23);
            this.RefreshFollowersButton.TabIndex = 8;
            this.RefreshFollowersButton.Text = "Yenile";
            this.RefreshFollowersButton.UseVisualStyleBackColor = true;
            this.RefreshFollowersButton.Click += new System.EventHandler(this.RefreshFollowersButton_Click);
            // 
            // FollowingLabel
            // 
            this.FollowingLabel.AutoSize = true;
            this.FollowingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FollowingLabel.Location = new System.Drawing.Point(403, 37);
            this.FollowingLabel.Name = "FollowingLabel";
            this.FollowingLabel.Size = new System.Drawing.Size(78, 13);
            this.FollowingLabel.TabIndex = 7;
            this.FollowingLabel.Text = "Takip Edilen";
            // 
            // FollowingListBox
            // 
            this.FollowingListBox.ContextMenuStrip = this.FollowingContextMenuStrip;
            this.FollowingListBox.FormattingEnabled = true;
            this.FollowingListBox.Location = new System.Drawing.Point(406, 53);
            this.FollowingListBox.Name = "FollowingListBox";
            this.FollowingListBox.Size = new System.Drawing.Size(172, 550);
            this.FollowingListBox.TabIndex = 6;
            this.FollowingListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FollowingListBox_MouseDown);
            // 
            // FollowingContextMenuStrip
            // 
            this.FollowingContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StopFollowingMenuItem});
            this.FollowingContextMenuStrip.Name = "FollowingContextMenuStrip";
            this.FollowingContextMenuStrip.Size = new System.Drawing.Size(135, 26);
            // 
            // StopFollowingMenuItem
            // 
            this.StopFollowingMenuItem.Name = "StopFollowingMenuItem";
            this.StopFollowingMenuItem.Size = new System.Drawing.Size(134, 22);
            this.StopFollowingMenuItem.Text = "Takibi Bırak";
            // 
            // FollowUserButton
            // 
            this.FollowUserButton.Location = new System.Drawing.Point(253, 6);
            this.FollowUserButton.Name = "FollowUserButton";
            this.FollowUserButton.Size = new System.Drawing.Size(75, 23);
            this.FollowUserButton.TabIndex = 5;
            this.FollowUserButton.Text = "Takip Et";
            this.FollowUserButton.UseVisualStyleBackColor = true;
            this.FollowUserButton.Click += new System.EventHandler(this.FollowUserButton_Click);
            // 
            // FollowUserTextBox
            // 
            this.FollowUserTextBox.Location = new System.Drawing.Point(143, 8);
            this.FollowUserTextBox.Name = "FollowUserTextBox";
            this.FollowUserTextBox.Size = new System.Drawing.Size(100, 20);
            this.FollowUserTextBox.TabIndex = 4;
            // 
            // CachedFollowersLabel
            // 
            this.CachedFollowersLabel.AutoSize = true;
            this.CachedFollowersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.CachedFollowersLabel.Location = new System.Drawing.Point(210, 38);
            this.CachedFollowersLabel.Name = "CachedFollowersLabel";
            this.CachedFollowersLabel.Size = new System.Drawing.Size(118, 13);
            this.CachedFollowersLabel.TabIndex = 3;
            this.CachedFollowersLabel.Text = "Takipçiler (Cached)";
            // 
            // CachedFollowersListBox
            // 
            this.CachedFollowersListBox.FormattingEnabled = true;
            this.CachedFollowersListBox.Location = new System.Drawing.Point(213, 54);
            this.CachedFollowersListBox.Name = "CachedFollowersListBox";
            this.CachedFollowersListBox.Size = new System.Drawing.Size(172, 550);
            this.CachedFollowersListBox.TabIndex = 2;
            // 
            // FollowersLabel
            // 
            this.FollowersLabel.AutoSize = true;
            this.FollowersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FollowersLabel.Location = new System.Drawing.Point(13, 37);
            this.FollowersLabel.Name = "FollowersLabel";
            this.FollowersLabel.Size = new System.Drawing.Size(63, 13);
            this.FollowersLabel.TabIndex = 1;
            this.FollowersLabel.Text = "Takipçiler";
            // 
            // FollowersListBox
            // 
            this.FollowersListBox.FormattingEnabled = true;
            this.FollowersListBox.Location = new System.Drawing.Point(16, 53);
            this.FollowersListBox.Name = "FollowersListBox";
            this.FollowersListBox.Size = new System.Drawing.Size(172, 550);
            this.FollowersListBox.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.RefreshWallPostButton);
            this.tabPage1.Controls.Add(this.WallPostGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1257, 645);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Duvarım";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // RefreshWallPostButton
            // 
            this.RefreshWallPostButton.Location = new System.Drawing.Point(1166, 21);
            this.RefreshWallPostButton.Name = "RefreshWallPostButton";
            this.RefreshWallPostButton.Size = new System.Drawing.Size(75, 23);
            this.RefreshWallPostButton.TabIndex = 1;
            this.RefreshWallPostButton.Text = "Yenile";
            this.RefreshWallPostButton.UseVisualStyleBackColor = true;
            this.RefreshWallPostButton.Click += new System.EventHandler(this.RefreshWallPostButton_Click);
            // 
            // WallPostGridView
            // 
            this.WallPostGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.WallPostGridView.Location = new System.Drawing.Point(6, 50);
            this.WallPostGridView.Name = "WallPostGridView";
            this.WallPostGridView.Size = new System.Drawing.Size(1245, 589);
            this.WallPostGridView.TabIndex = 0;
            this.WallPostGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.WallPostGridView_CellContentClick);
            this.WallPostGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.WallPostGridView_CellMouseUp);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.RefreshNewsFeedButton);
            this.tabPage3.Controls.Add(this.NewsFeedGridView);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1257, 645);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Haber Kaynağım";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // RefreshNewsFeedButton
            // 
            this.RefreshNewsFeedButton.Location = new System.Drawing.Point(1166, 15);
            this.RefreshNewsFeedButton.Name = "RefreshNewsFeedButton";
            this.RefreshNewsFeedButton.Size = new System.Drawing.Size(75, 23);
            this.RefreshNewsFeedButton.TabIndex = 1;
            this.RefreshNewsFeedButton.Text = "Yenile";
            this.RefreshNewsFeedButton.UseVisualStyleBackColor = true;
            this.RefreshNewsFeedButton.Click += new System.EventHandler(this.RefreshNewsFeedButton_Click);
            // 
            // NewsFeedGridView
            // 
            this.NewsFeedGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.NewsFeedGridView.Location = new System.Drawing.Point(7, 54);
            this.NewsFeedGridView.Name = "NewsFeedGridView";
            this.NewsFeedGridView.Size = new System.Drawing.Size(1244, 585);
            this.NewsFeedGridView.TabIndex = 0;
            // 
            // GroupWallTab
            // 
            this.GroupWallTab.Controls.Add(this.GroupWallGridView);
            this.GroupWallTab.Controls.Add(this.RefreshGroupWallButton);
            this.GroupWallTab.Controls.Add(this.label1);
            this.GroupWallTab.Controls.Add(this.GroupWallGroupNameTextBox);
            this.GroupWallTab.Location = new System.Drawing.Point(4, 22);
            this.GroupWallTab.Name = "GroupWallTab";
            this.GroupWallTab.Padding = new System.Windows.Forms.Padding(3);
            this.GroupWallTab.Size = new System.Drawing.Size(1257, 645);
            this.GroupWallTab.TabIndex = 4;
            this.GroupWallTab.Text = "Grup Duvarı";
            this.GroupWallTab.UseVisualStyleBackColor = true;
            // 
            // GroupWallGridView
            // 
            this.GroupWallGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GroupWallGridView.Location = new System.Drawing.Point(8, 35);
            this.GroupWallGridView.Name = "GroupWallGridView";
            this.GroupWallGridView.Size = new System.Drawing.Size(1233, 595);
            this.GroupWallGridView.TabIndex = 3;
            // 
            // RefreshGroupWallButton
            // 
            this.RefreshGroupWallButton.Location = new System.Drawing.Point(1166, 6);
            this.RefreshGroupWallButton.Name = "RefreshGroupWallButton";
            this.RefreshGroupWallButton.Size = new System.Drawing.Size(75, 23);
            this.RefreshGroupWallButton.TabIndex = 2;
            this.RefreshGroupWallButton.Text = "Yenile";
            this.RefreshGroupWallButton.UseVisualStyleBackColor = true;
            this.RefreshGroupWallButton.Click += new System.EventHandler(this.RefreshGroupWallButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(993, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Grup Adı:";
            // 
            // GroupWallGroupNameTextBox
            // 
            this.GroupWallGroupNameTextBox.Location = new System.Drawing.Point(1060, 6);
            this.GroupWallGroupNameTextBox.Name = "GroupWallGroupNameTextBox";
            this.GroupWallGroupNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.GroupWallGroupNameTextBox.TabIndex = 0;
            // 
            // ShareMultipleButton
            // 
            this.ShareMultipleButton.Location = new System.Drawing.Point(1126, 576);
            this.ShareMultipleButton.Name = "ShareMultipleButton";
            this.ShareMultipleButton.Size = new System.Drawing.Size(81, 27);
            this.ShareMultipleButton.TabIndex = 9;
            this.ShareMultipleButton.Text = "Paylaş (100x)";
            this.ShareMultipleButton.UseVisualStyleBackColor = true;
            this.ShareMultipleButton.Click += new System.EventHandler(this.ShareMultipleButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1293, 741);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.ChangeUsernameLabel);
            this.Controls.Add(this.ChangeUsernameButton);
            this.Controls.Add(this.UsernameTextBox);
            this.Name = "MainForm";
            this.Text = "SFeed";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainTabControl.ResumeLayout(false);
            this.ShareTab.ResumeLayout(false);
            this.ShareTab.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.FollowingContextMenuStrip.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WallPostGridView)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NewsFeedGridView)).EndInit();
            this.GroupWallTab.ResumeLayout(false);
            this.GroupWallTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GroupWallGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WallPostBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.Button ChangeUsernameButton;
        private System.Windows.Forms.Label ChangeUsernameLabel;
        private System.Windows.Forms.TextBox ShareTextBox;
        private System.Windows.Forms.Button ShareButton;
        private System.Windows.Forms.CheckBox ShareToOwnWallCheckbox;
        private System.Windows.Forms.TextBox WallOwnerTextBox;
        private System.Windows.Forms.Label WallOwnerLabel;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage ShareTab;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label FollowersLabel;
        private System.Windows.Forms.ListBox FollowersListBox;
        private System.Windows.Forms.Label CachedFollowersLabel;
        private System.Windows.Forms.ListBox CachedFollowersListBox;
        private System.Windows.Forms.Button FollowUserButton;
        private System.Windows.Forms.TextBox FollowUserTextBox;
        private System.Windows.Forms.Button RefreshFollowersButton;
        private System.Windows.Forms.Label FollowingLabel;
        private System.Windows.Forms.ListBox FollowingListBox;
        private System.Windows.Forms.DataGridView WallPostGridView;
        private System.Windows.Forms.BindingSource WallPostBindingSource;
        private System.Windows.Forms.Button RefreshWallPostButton;
        private System.Windows.Forms.Button RefreshNewsFeedButton;
        private System.Windows.Forms.DataGridView NewsFeedGridView;
        private System.Windows.Forms.ComboBox WallSelectionComboBox;
        private System.Windows.Forms.TabPage GroupWallTab;
        private System.Windows.Forms.Button RefreshGroupWallButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox GroupWallGroupNameTextBox;
        private System.Windows.Forms.DataGridView GroupWallGridView;
        private System.Windows.Forms.ComboBox FollowingTypeCombobox;
        private System.Windows.Forms.ContextMenuStrip FollowingContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem StopFollowingMenuItem;
        private System.Windows.Forms.Button ShareMultipleButton;
    }
}

