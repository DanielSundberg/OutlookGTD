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
            this._conversationIdTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._storeTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _subjectTextBox
            // 
            this._subjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._subjectTextBox.Location = new System.Drawing.Point(15, 40);
            this._subjectTextBox.Name = "_subjectTextBox";
            this._subjectTextBox.Size = new System.Drawing.Size(117, 22);
            this._subjectTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Subject:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Folder:";
            // 
            // _folderTextBox
            // 
            this._folderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._folderTextBox.Location = new System.Drawing.Point(15, 96);
            this._folderTextBox.Name = "_folderTextBox";
            this._folderTextBox.Size = new System.Drawing.Size(117, 22);
            this._folderTextBox.TabIndex = 3;
            // 
            // _conversationIdTextBox
            // 
            this._conversationIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._conversationIdTextBox.Location = new System.Drawing.Point(15, 153);
            this._conversationIdTextBox.Name = "_conversationIdTextBox";
            this._conversationIdTextBox.Size = new System.Drawing.Size(117, 22);
            this._conversationIdTextBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Conversation id:";
            // 
            // _storeTextBox
            // 
            this._storeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._storeTextBox.Location = new System.Drawing.Point(15, 226);
            this._storeTextBox.Name = "_storeTextBox";
            this._storeTextBox.Size = new System.Drawing.Size(117, 22);
            this._storeTextBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 206);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Store:";
            // 
            // TaskGTDView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._storeTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._conversationIdTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._folderTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._subjectTextBox);
            this.Name = "TaskGTDView";
            this.Size = new System.Drawing.Size(150, 259);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _subjectTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _folderTextBox;
        private System.Windows.Forms.TextBox _conversationIdTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _storeTextBox;
        private System.Windows.Forms.Label label4;
    }
}
