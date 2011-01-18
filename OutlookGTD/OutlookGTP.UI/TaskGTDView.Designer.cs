namespace OutlookGTD.UI
{
    partial class TaskGTDView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._subjectTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._folderTextBox = new System.Windows.Forms.TextBox();
            this._entryIdTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._conversationTtreeView = new System.Windows.Forms.TreeView();
            this._bodyTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _subjectTextBox
            // 
            this._subjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._subjectTextBox.Location = new System.Drawing.Point(11, 25);
            this._subjectTextBox.Margin = new System.Windows.Forms.Padding(2);
            this._subjectTextBox.Name = "_subjectTextBox";
            this._subjectTextBox.Size = new System.Drawing.Size(89, 20);
            this._subjectTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Subject:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "FolderPath:";
            // 
            // _folderTextBox
            // 
            this._folderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._folderTextBox.Location = new System.Drawing.Point(11, 66);
            this._folderTextBox.Margin = new System.Windows.Forms.Padding(2);
            this._folderTextBox.Name = "_folderTextBox";
            this._folderTextBox.Size = new System.Drawing.Size(89, 20);
            this._folderTextBox.TabIndex = 3;
            // 
            // _entryIdTextBox
            // 
            this._entryIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._entryIdTextBox.Location = new System.Drawing.Point(11, 108);
            this._entryIdTextBox.Margin = new System.Windows.Forms.Padding(2);
            this._entryIdTextBox.Name = "_entryIdTextBox";
            this._entryIdTextBox.Size = new System.Drawing.Size(89, 20);
            this._entryIdTextBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 90);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Entry id:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 134);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Mail:";
            // 
            // _conversationTtreeView
            // 
            this._conversationTtreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._conversationTtreeView.Location = new System.Drawing.Point(11, 149);
            this._conversationTtreeView.Margin = new System.Windows.Forms.Padding(2);
            this._conversationTtreeView.Name = "_conversationTtreeView";
            this._conversationTtreeView.Size = new System.Drawing.Size(92, 152);
            this._conversationTtreeView.TabIndex = 7;
            this._conversationTtreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._conversationTtreeView_NodeMouseClick);
            this._conversationTtreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._conversationTtreeView_NodeMouseDoubleClick);
            // 
            // _bodyTextBox
            // 
            this._bodyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._bodyTextBox.Location = new System.Drawing.Point(11, 305);
            this._bodyTextBox.Margin = new System.Windows.Forms.Padding(2);
            this._bodyTextBox.Multiline = true;
            this._bodyTextBox.Name = "_bodyTextBox";
            this._bodyTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._bodyTextBox.Size = new System.Drawing.Size(92, 153);
            this._bodyTextBox.TabIndex = 8;
            // 
            // TaskGTDView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._bodyTextBox);
            this.Controls.Add(this._conversationTtreeView);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._entryIdTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._folderTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._subjectTextBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TaskGTDView";
            this.Size = new System.Drawing.Size(112, 460);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _subjectTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _folderTextBox;
        private System.Windows.Forms.TextBox _entryIdTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TreeView _conversationTtreeView;
        private System.Windows.Forms.TextBox _bodyTextBox;
    }
}
