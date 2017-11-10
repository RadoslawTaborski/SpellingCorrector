namespace WindowApp
{
    partial class Form1
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
            this.Check = new System.Windows.Forms.Button();
            this.tbText = new System.Windows.Forms.TextBox();
            this.tbList = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Check
            // 
            this.Check.Location = new System.Drawing.Point(12, 38);
            this.Check.Name = "Check";
            this.Check.Size = new System.Drawing.Size(117, 44);
            this.Check.TabIndex = 1;
            this.Check.Text = "Sprawdź";
            this.Check.UseVisualStyleBackColor = true;
            this.Check.Click += new System.EventHandler(this.Check_Click);
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(12, 12);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(117, 20);
            this.tbText.TabIndex = 2;
            // 
            // tbList
            // 
            this.tbList.Location = new System.Drawing.Point(135, 12);
            this.tbList.Multiline = true;
            this.tbList.Name = "tbList";
            this.tbList.Size = new System.Drawing.Size(139, 237);
            this.tbList.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.tbList);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.Check);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Check;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.TextBox tbList;
    }
}

