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
            this.moveButt = new System.Windows.Forms.Button();
            this.MoveVal = new System.Windows.Forms.TextBox();
            this.toggleDirection = new System.Windows.Forms.Button();
            this.haltButton = new System.Windows.Forms.Button();
            this.reverseLabel = new System.Windows.Forms.Label();
            this.refreshUi = new System.Windows.Forms.Button();
            this.setZeroPosButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(516, 332);
            this.cmdOK.Margin = new System.Windows.Forms.Padding(4);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(78, 30);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(516, 368);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(4);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(78, 30);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 38);
            this.label1.TabIndex = 2;
            this.label1.Text = "Construct your driver\'s setup dialog here.";
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.SympleAstroFocus.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(530, 11);
            this.picASCOM.Margin = new System.Windows.Forms.Padding(4);
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
            this.chkTrace.Location = new System.Drawing.Point(513, 64);
            this.chkTrace.Margin = new System.Windows.Forms.Padding(4);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(87, 21);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // vidLabel
            // 
            this.vidLabel.Location = new System.Drawing.Point(11, 102);
            this.vidLabel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.vidLabel.Name = "vidLabel";
            this.vidLabel.ReadOnly = true;
            this.vidLabel.Size = new System.Drawing.Size(89, 22);
            this.vidLabel.TabIndex = 7;
            this.vidLabel.Text = "Vendor ID";
            this.vidLabel.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // vidVal
            // 
            this.vidVal.Location = new System.Drawing.Point(116, 102);
            this.vidVal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.vidVal.Name = "vidVal";
            this.vidVal.ReadOnly = true;
            this.vidVal.Size = new System.Drawing.Size(89, 22);
            this.vidVal.TabIndex = 8;
            this.vidVal.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // pidLabel
            // 
            this.pidLabel.Location = new System.Drawing.Point(11, 128);
            this.pidLabel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pidLabel.Name = "pidLabel";
            this.pidLabel.ReadOnly = true;
            this.pidLabel.Size = new System.Drawing.Size(89, 22);
            this.pidLabel.TabIndex = 9;
            this.pidLabel.Text = "Product ID";
            this.pidLabel.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // pidVal
            // 
            this.pidVal.Location = new System.Drawing.Point(116, 128);
            this.pidVal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pidVal.Name = "pidVal";
            this.pidVal.ReadOnly = true;
            this.pidVal.Size = new System.Drawing.Size(89, 22);
            this.pidVal.TabIndex = 10;
            this.pidVal.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // connectedState
            // 
            this.connectedState.AutoSize = true;
            this.connectedState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.connectedState.Location = new System.Drawing.Point(11, 74);
            this.connectedState.Name = "connectedState";
            this.connectedState.Size = new System.Drawing.Size(118, 19);
            this.connectedState.TabIndex = 11;
            this.connectedState.Text = "DISCONNECTED";
            this.connectedState.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // serialNumberLabel
            // 
            this.serialNumberLabel.AutoSize = true;
            this.serialNumberLabel.Location = new System.Drawing.Point(10, 154);
            this.serialNumberLabel.Name = "serialNumberLabel";
            this.serialNumberLabel.Size = new System.Drawing.Size(98, 17);
            this.serialNumberLabel.TabIndex = 12;
            this.serialNumberLabel.Text = "Serial Number";
            this.serialNumberLabel.Click += new System.EventHandler(this.label2_Click_2);
            // 
            // serialNumberVal
            // 
            this.serialNumberVal.AutoSize = true;
            this.serialNumberVal.Location = new System.Drawing.Point(112, 154);
            this.serialNumberVal.Name = "serialNumberVal";
            this.serialNumberVal.Size = new System.Drawing.Size(98, 17);
            this.serialNumberVal.TabIndex = 13;
            this.serialNumberVal.Text = "Serial Number";
            // 
            // moveButt
            // 
            this.moveButt.Location = new System.Drawing.Point(116, 336);
            this.moveButt.Name = "moveButt";
            this.moveButt.Size = new System.Drawing.Size(75, 23);
            this.moveButt.TabIndex = 14;
            this.moveButt.Text = "Move";
            this.moveButt.UseVisualStyleBackColor = true;
            this.moveButt.Click += new System.EventHandler(this.button1_Click);
            // 
            // MoveVal
            // 
            this.MoveVal.Location = new System.Drawing.Point(10, 337);
            this.MoveVal.Name = "MoveVal";
            this.MoveVal.Size = new System.Drawing.Size(100, 22);
            this.MoveVal.TabIndex = 15;
            this.MoveVal.TextChanged += new System.EventHandler(this.MoveVal_TextChanged);
            // 
            // toggleDirection
            // 
            this.toggleDirection.Location = new System.Drawing.Point(115, 187);
            this.toggleDirection.Name = "toggleDirection";
            this.toggleDirection.Size = new System.Drawing.Size(90, 23);
            this.toggleDirection.TabIndex = 16;
            this.toggleDirection.Text = "Reverse Direction";
            this.toggleDirection.UseVisualStyleBackColor = true;
            this.toggleDirection.Click += new System.EventHandler(this.toggleDirection_Click);
            // 
            // haltButton
            // 
            this.haltButton.Location = new System.Drawing.Point(197, 337);
            this.haltButton.Name = "haltButton";
            this.haltButton.Size = new System.Drawing.Size(75, 23);
            this.haltButton.TabIndex = 17;
            this.haltButton.Text = "Halt";
            this.haltButton.UseVisualStyleBackColor = true;
            this.haltButton.Click += new System.EventHandler(this.haltButton_Click);
            // 
            // reverseLabel
            // 
            this.reverseLabel.AutoSize = true;
            this.reverseLabel.Location = new System.Drawing.Point(8, 190);
            this.reverseLabel.Name = "reverseLabel";
            this.reverseLabel.Size = new System.Drawing.Size(73, 17);
            this.reverseLabel.TabIndex = 18;
            this.reverseLabel.Text = "Reversed:";
            // 
            // refreshUi
            // 
            this.refreshUi.Location = new System.Drawing.Point(503, 91);
            this.refreshUi.Name = "refreshUi";
            this.refreshUi.Size = new System.Drawing.Size(89, 44);
            this.refreshUi.TabIndex = 19;
            this.refreshUi.Text = "Debug: Refresh UI";
            this.refreshUi.UseVisualStyleBackColor = true;
            this.refreshUi.Click += new System.EventHandler(this.refreshUi_Click);
            // 
            // setZeroPosButton
            // 
            this.setZeroPosButton.Location = new System.Drawing.Point(5, 216);
            this.setZeroPosButton.Name = "setZeroPosButton";
            this.setZeroPosButton.Size = new System.Drawing.Size(200, 27);
            this.setZeroPosButton.TabIndex = 20;
            this.setZeroPosButton.Text = "Set current position as Zero";
            this.setZeroPosButton.UseVisualStyleBackColor = true;
            this.setZeroPosButton.Click += new System.EventHandler(this.setZeroPosButton_Click);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 409);
            this.Controls.Add(this.setZeroPosButton);
            this.Controls.Add(this.refreshUi);
            this.Controls.Add(this.reverseLabel);
            this.Controls.Add(this.haltButton);
            this.Controls.Add(this.toggleDirection);
            this.Controls.Add(this.MoveVal);
            this.Controls.Add(this.moveButt);
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
            this.Margin = new System.Windows.Forms.Padding(4);
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
        private System.Windows.Forms.Button moveButt;
        private System.Windows.Forms.TextBox MoveVal;
        private System.Windows.Forms.Button toggleDirection;
        private System.Windows.Forms.Button haltButton;
        private System.Windows.Forms.Label reverseLabel;
        private System.Windows.Forms.Button refreshUi;
        private System.Windows.Forms.Button setZeroPosButton;
    }
}