namespace Molmed.SQAT.GUI
{
    partial class ItemSuccessCutoffForm
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
            this.SuccessExpRadioButton = new System.Windows.Forms.RadioButton();
            this.SuccessNonfailedExpRadioButton = new System.Windows.Forms.RadioButton();
            this.LimitLabel = new System.Windows.Forms.Label();
            this.LimitNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.PercentLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LimitNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // SuccessExpRadioButton
            // 
            this.SuccessExpRadioButton.AutoSize = true;
            this.SuccessExpRadioButton.Location = new System.Drawing.Point(39, 46);
            this.SuccessExpRadioButton.Name = "SuccessExpRadioButton";
            this.SuccessExpRadioButton.Size = new System.Drawing.Size(159, 17);
            this.SuccessExpRadioButton.TabIndex = 0;
            this.SuccessExpRadioButton.Text = "Success rate all experiments";
            this.SuccessExpRadioButton.UseVisualStyleBackColor = true;
            // 
            // SuccessNonfailedExpRadioButton
            // 
            this.SuccessNonfailedExpRadioButton.AutoSize = true;
            this.SuccessNonfailedExpRadioButton.Checked = true;
            this.SuccessNonfailedExpRadioButton.Location = new System.Drawing.Point(39, 89);
            this.SuccessNonfailedExpRadioButton.Name = "SuccessNonfailedExpRadioButton";
            this.SuccessNonfailedExpRadioButton.Size = new System.Drawing.Size(192, 17);
            this.SuccessNonfailedExpRadioButton.TabIndex = 1;
            this.SuccessNonfailedExpRadioButton.TabStop = true;
            this.SuccessNonfailedExpRadioButton.Text = "Success rate nonfailed experiments";
            this.SuccessNonfailedExpRadioButton.UseVisualStyleBackColor = true;
            // 
            // LimitLabel
            // 
            this.LimitLabel.AutoSize = true;
            this.LimitLabel.Location = new System.Drawing.Point(36, 148);
            this.LimitLabel.Name = "LimitLabel";
            this.LimitLabel.Size = new System.Drawing.Size(31, 13);
            this.LimitLabel.TabIndex = 2;
            this.LimitLabel.Text = "Limit:";
            // 
            // LimitNumericUpDown
            // 
            this.LimitNumericUpDown.Location = new System.Drawing.Point(73, 146);
            this.LimitNumericUpDown.Name = "LimitNumericUpDown";
            this.LimitNumericUpDown.Size = new System.Drawing.Size(72, 20);
            this.LimitNumericUpDown.TabIndex = 3;
            // 
            // PercentLabel
            // 
            this.PercentLabel.AutoSize = true;
            this.PercentLabel.Location = new System.Drawing.Point(151, 148);
            this.PercentLabel.Name = "PercentLabel";
            this.PercentLabel.Size = new System.Drawing.Size(15, 13);
            this.PercentLabel.TabIndex = 4;
            this.PercentLabel.Text = "%";
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OKButton.Location = new System.Drawing.Point(15, 226);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(72, 24);
            this.OKButton.TabIndex = 5;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(231, 226);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 6;
            this.CloseButton.Text = "&Cancel";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ItemSuccessCutoffForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(315, 259);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.PercentLabel);
            this.Controls.Add(this.LimitNumericUpDown);
            this.Controls.Add(this.LimitLabel);
            this.Controls.Add(this.SuccessNonfailedExpRadioButton);
            this.Controls.Add(this.SuccessExpRadioButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ItemSuccessCutoffForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Success Rate Cutoff";
            ((System.ComponentModel.ISupportInitialize)(this.LimitNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton SuccessExpRadioButton;
        private System.Windows.Forms.RadioButton SuccessNonfailedExpRadioButton;
        private System.Windows.Forms.Label LimitLabel;
        private System.Windows.Forms.NumericUpDown LimitNumericUpDown;
        private System.Windows.Forms.Label PercentLabel;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CloseButton;
    }
}