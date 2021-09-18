namespace ASCOM.SympleAstroFocus
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.vidLabel = new System.Windows.Forms.TextBox();
            this.vidVal = new System.Windows.Forms.TextBox();
            this.pidLabel = new System.Windows.Forms.TextBox();
            this.pidVal = new System.Windows.Forms.TextBox();
            this.connectedState = new System.Windows.Forms.Label();
            this.serialNumberLabel = new System.Windows.Forms.Label();
            this.serialNumberVal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(577, 257);
            this.cmdOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(88, 37);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(577, 303);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(88, 38);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 48);
            this.label1.TabIndex = 2;
            this.label1.Text = "Construct your driver\'s setup dialog here.";
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.SympleAstroFocus.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(593, 14);
            this.picASCOM.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(577, 80);
            this.chkTrace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(97, 24);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // vidLabel
            // 
            this.vidLabel.Location = new System.Drawing.Point(12, 128);
            this.vidLabel.Name = "vidLabel";
            this.vidLabel.ReadOnly = true;
            this.vidLabel.Size = new System.Drawing.Size(100, 26);
            this.vidLabel.TabIndex = 7;
            this.vidLabel.Text = "Vendor ID";
            this.vidLabel.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // vidVal
            // 
            this.vidVal.Location = new System.Drawing.Point(130, 128);
            this.vidVal.Name = "vidVal";
            this.vidVal.ReadOnly = true;
            this.vidVal.Size = new System.Drawing.Size(100, 26);
            this.vidVal.TabIndex = 8;
            this.vidVal.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // pidLabel
            // 
            this.pidLabel.Location = new System.Drawing.Point(12, 160);
            this.pidLabel.Name = "pidLabel";
            this.pidLabel.ReadOnly = true;
            this.pidLabel.Size = new System.Drawing.Size(100, 26);
            this.pidLabel.TabIndex = 9;
            this.pidLabel.Text = "Product ID";
            this.pidLabel.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // pidVal
            // 
            this.pidVal.Location = new System.Drawing.Point(130, 160);
            this.pidVal.Name = "pidVal";
            this.pidVal.ReadOnly = true;
            this.pidVal.Size = new System.Drawing.Size(100, 26);
            this.pidVal.TabIndex = 10;
            this.pidVal.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // connectedState
            // 
            this.connectedState.AutoSize = true;
            this.connectedState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.connectedState.Location = new System.Drawing.Point(12, 93);
            this.connectedState.Name = "connectedState";
            this.connectedState.Size = new System.Drawing.Size(138, 22);
            this.connectedState.TabIndex = 11;
            this.connectedState.Text = "DISCONNECTED";
            this.connectedState.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // serialNumberLabel
            // 
            this.serialNumberLabel.AutoSize = true;
            this.serialNumberLabel.Location = new System.Drawing.Point(11, 193);
            this.serialNumberLabel.Name = "serialNumberLabel";
            this.serialNumberLabel.Size = new System.Drawing.Size(109, 20);
            this.serialNumberLabel.TabIndex = 12;
            this.serialNumberLabel.Text = "Serial Number";
            this.serialNumberLabel.Click += new System.EventHandler(this.label2_Click_2);
            // 
            // serialNumberVal
            // 
            this.serialNumberVal.AutoSize = true;
            this.serialNumberVal.Location = new System.Drawing.Point(126, 193);
            this.serialNumberVal.Name = "serialNumberVal";
            this.serialNumberVal.Size = new System.Drawing.Size(109, 20);
            this.serialNumberVal.TabIndex = 13;
            this.serialNumberVal.Text = "Serial Number";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 354);
            this.Controls.Add(this.serialNumberVal);
            this.Controls.Add(this.serialNumberLabel);
            this.Controls.Add(this.connectedState);
            this.Controls.Add(this.pidVal);
            this.Controls.Add(this.pidLabel);
            this.Controls.Add(this.vidVal);
            this.Controls.Add(this.vidLabel);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SympleAstroFocus Setup";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.TextBox vidLabel;
        private System.Windows.Forms.TextBox vidVal;
        private System.Windows.Forms.TextBox pidLabel;
        private System.Windows.Forms.TextBox pidVal;
        private System.Windows.Forms.Label connectedState;
        private System.Windows.Forms.Label serialNumberLabel;
        private System.Windows.Forms.Label serialNumberVal;
    }
}