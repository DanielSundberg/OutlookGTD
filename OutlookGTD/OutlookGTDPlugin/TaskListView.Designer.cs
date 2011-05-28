namespace OutlookGTDPlugin
{
    partial class TaskListView
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
            this._listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._okButton = new System.Windows.Forms.Button();
            this._showAllButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _listView
            // 
            this._listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this._listView.FullRowSelect = true;
            this._listView.Location = new System.Drawing.Point(12, 12);
            this._listView.Name = "_listView";
            this._listView.Size = new System.Drawing.Size(407, 403);
            this._listView.TabIndex = 0;
            this._listView.UseCompatibleStateImageBehavior = false;
            this._listView.View = System.Windows.Forms.View.Details;
            this._listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this._listView_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Subject";
            this.columnHeader1.Width = 279;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Due date";
            this.columnHeader2.Width = 123;
            // 
            // _okButton
            // 
            this._okButton.Location = new System.Drawing.Point(263, 421);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 23);
            this._okButton.TabIndex = 1;
            this._okButton.Text = "&Ok";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this._okButton_Click);
            // 
            // _showAllButton
            // 
            this._showAllButton.Location = new System.Drawing.Point(12, 421);
            this._showAllButton.Name = "_showAllButton";
            this._showAllButton.Size = new System.Drawing.Size(75, 23);
            this._showAllButton.TabIndex = 2;
            this._showAllButton.Text = "&Show all";
            this._showAllButton.UseVisualStyleBackColor = true;
            this._showAllButton.Click += new System.EventHandler(this._showAllButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.Location = new System.Drawing.Point(344, 421);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "&Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // TaskListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 456);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._showAllButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._listView);
            this.Name = "TaskListView";
            this.Text = "Select task";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView _listView;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button _showAllButton;
        private System.Windows.Forms.Button _cancelButton;
    }
}