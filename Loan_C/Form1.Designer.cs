namespace Loan_C
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
            this.btnBank = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbank = new System.Windows.Forms.TextBox();
            this.btnupload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBank
            // 
            this.btnBank.Location = new System.Drawing.Point(459, 57);
            this.btnBank.Name = "btnBank";
            this.btnBank.Size = new System.Drawing.Size(44, 23);
            this.btnBank.TabIndex = 0;
            this.btnBank.Text = "Bank";
            this.btnBank.UseVisualStyleBackColor = true;
            this.btnBank.Click += new System.EventHandler(this.btnBank_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(216, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Bank Ac";
            // 
            // txtbank
            // 
            this.txtbank.Location = new System.Drawing.Point(277, 60);
            this.txtbank.Name = "txtbank";
            this.txtbank.Size = new System.Drawing.Size(176, 20);
            this.txtbank.TabIndex = 2;
            // 
            // btnupload
            // 
            this.btnupload.Location = new System.Drawing.Point(552, 63);
            this.btnupload.Name = "btnupload";
            this.btnupload.Size = new System.Drawing.Size(75, 23);
            this.btnupload.TabIndex = 3;
            this.btnupload.Text = "Upload";
            this.btnupload.UseVisualStyleBackColor = true;
            this.btnupload.Click += new System.EventHandler(this.btnupload_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 502);
            this.Controls.Add(this.btnupload);
            this.Controls.Add(this.txtbank);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBank);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBank;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtbank;
        private System.Windows.Forms.Button btnupload;
    }
}

