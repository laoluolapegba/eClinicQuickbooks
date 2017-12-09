namespace DataImport
{
    partial class frmMain
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
            this.btnCust = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCust
            // 
            this.btnCust.Location = new System.Drawing.Point(27, 13);
            this.btnCust.Name = "btnCust";
            this.btnCust.Size = new System.Drawing.Size(75, 23);
            this.btnCust.TabIndex = 0;
            this.btnCust.Text = "Customer";
            this.btnCust.UseVisualStyleBackColor = true;
            this.btnCust.Click += new System.EventHandler(this.btnCust_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 190);
            this.Controls.Add(this.btnCust);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCust;
    }
}

