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
            this.picASCOM = new System.Windows.Forms.PictureBox();
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
            this.setZeroPosButton = new System.Windows.Forms.Button();
            this.CurrentPosLabel = new System.Windows.Forms.Label();
            this.CurrentPositionValue = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MaxPosition = new System.Windows.Forms.Label();
            this.MaxPositionValue = new System.Windows.Forms.Label();
            this.newMaxPos = new System.Windows.Forms.TextBox();
            this.updateMaxButton = new System.Windows.Forms.Button();
            this.reversedValLabel = new System.Windows.Forms.Label();
            this.motorLoadLabel = new System.Windows.Forms.Label();
            this.motorLoadVal = new System.Windows.Forms.Label();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.triggerHomingButton = new System.Windows.Forms.Button();
            this.configPanel = new System.Windows.Forms.Panel();
            this.configTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.iholdSetVal = new System.Windows.Forms.NumericUpDown();
            this.updateIrunButton = new System.Windows.Forms.Button();
            this.moveCurrentLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.irunCurrentVal = new System.Windows.Forms.Label();
            this.iholdCurrentVal = new System.Windows.Forms.Label();
            this.updateIholdButton = new System.Windows.Forms.Button();
            this.updateStepSpeedButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.homingDirectionsCurrVals = new System.Windows.Forms.CheckedListBox();
            this.homingDirectionsSetVals = new System.Windows.Forms.CheckedListBox();
            this.updateHomingDirs = new System.Windows.Forms.Button();
            this.irunSetVal = new System.Windows.Forms.NumericUpDown();
            this.motorSpeedVal = new System.Windows.Forms.Label();
            this.motorSpeedLabel = new System.Windows.Forms.Label();
            this.motorSpeedSetVal = new System.Windows.Forms.NumericUpDown();
            this.debugPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.csActualVal = new System.Windows.Forms.Label();
            this.csActualLabel = new System.Windows.Forms.Label();
            this.stepperDriverErrorLabel = new System.Windows.Forms.Label();
            this.stepperDriverErrorVal = new System.Windows.Forms.Label();
            this.StepperCommsErrorLabel = new System.Windows.Forms.Label();
            this.stepperDriverCommsErrrorVal = new System.Windows.Forms.Label();
            this.stepperDriverEnabledLabel = new System.Windows.Forms.Label();
            this.stepperDriverEnabledVal = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.visibilityButtonsTable = new System.Windows.Forms.TableLayoutPanel();
            this.toggleDebugVisibilitybutton = new System.Windows.Forms.Button();
            this.toggleConfigVisibilitybutton = new System.Windows.Forms.Button();
            this.updateStallThresh = new System.Windows.Forms.Button();
            this.stallThreshVal = new System.Windows.Forms.NumericUpDown();
            this.stallThreshCurrentVal = new System.Windows.Forms.Label();
            this.stallThreshLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.controlPanel.SuspendLayout();
            this.configPanel.SuspendLayout();
            this.configTableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iholdSetVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.irunSetVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.motorSpeedSetVal)).BeginInit();
            this.debugPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.visibilityButtonsTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stallThreshVal)).BeginInit();
            this.SuspendLayout();
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.SympleAstroFocus.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(801, 7);
            this.picASCOM.Margin = new System.Windows.Forms.Padding(4);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // vidLabel
            // 
            this.vidLabel.Location = new System.Drawing.Point(4, 3);
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
            this.vidVal.Location = new System.Drawing.Point(259, 3);
            this.vidVal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.vidVal.Name = "vidVal";
            this.vidVal.ReadOnly = true;
            this.vidVal.Size = new System.Drawing.Size(89, 22);
            this.vidVal.TabIndex = 8;
            this.vidVal.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // pidLabel
            // 
            this.pidLabel.Location = new System.Drawing.Point(4, 30);
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
            this.pidVal.Location = new System.Drawing.Point(259, 30);
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
            this.connectedState.Location = new System.Drawing.Point(18, 9);
            this.connectedState.Name = "connectedState";
            this.connectedState.Size = new System.Drawing.Size(118, 19);
            this.connectedState.TabIndex = 11;
            this.connectedState.Text = "DISCONNECTED";
            this.connectedState.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // serialNumberLabel
            // 
            this.serialNumberLabel.AutoSize = true;
            this.serialNumberLabel.Location = new System.Drawing.Point(4, 55);
            this.serialNumberLabel.Name = "serialNumberLabel";
            this.serialNumberLabel.Size = new System.Drawing.Size(98, 17);
            this.serialNumberLabel.TabIndex = 12;
            this.serialNumberLabel.Text = "Serial Number";
            this.serialNumberLabel.Click += new System.EventHandler(this.label2_Click_2);
            // 
            // serialNumberVal
            // 
            this.serialNumberVal.AutoSize = true;
            this.serialNumberVal.Location = new System.Drawing.Point(259, 55);
            this.serialNumberVal.Name = "serialNumberVal";
            this.serialNumberVal.Size = new System.Drawing.Size(98, 17);
            this.serialNumberVal.TabIndex = 13;
            this.serialNumberVal.Text = "Serial Number";
            // 
            // moveButt
            // 
            this.moveButt.Location = new System.Drawing.Point(229, 64);
            this.moveButt.Name = "moveButt";
            this.moveButt.Size = new System.Drawing.Size(88, 32);
            this.moveButt.TabIndex = 14;
            this.moveButt.Text = "Move";
            this.moveButt.UseVisualStyleBackColor = true;
            this.moveButt.Click += new System.EventHandler(this.button1_Click);
            // 
            // MoveVal
            // 
            this.MoveVal.Location = new System.Drawing.Point(123, 69);
            this.MoveVal.Name = "MoveVal";
            this.MoveVal.Size = new System.Drawing.Size(100, 22);
            this.MoveVal.TabIndex = 15;
            this.MoveVal.TextChanged += new System.EventHandler(this.MoveVal_TextChanged);
            // 
            // toggleDirection
            // 
            this.toggleDirection.Location = new System.Drawing.Point(227, 143);
            this.toggleDirection.Name = "toggleDirection";
            this.toggleDirection.Size = new System.Drawing.Size(90, 29);
            this.toggleDirection.TabIndex = 16;
            this.toggleDirection.Text = "Reverse Direction";
            this.toggleDirection.UseVisualStyleBackColor = true;
            this.toggleDirection.Click += new System.EventHandler(this.toggleDirection_Click);
            // 
            // haltButton
            // 
            this.haltButton.Location = new System.Drawing.Point(330, 66);
            this.haltButton.Name = "haltButton";
            this.haltButton.Size = new System.Drawing.Size(91, 32);
            this.haltButton.TabIndex = 17;
            this.haltButton.Text = "Halt";
            this.haltButton.UseVisualStyleBackColor = true;
            this.haltButton.Click += new System.EventHandler(this.haltButton_Click);
            // 
            // reverseLabel
            // 
            this.reverseLabel.AutoSize = true;
            this.reverseLabel.Location = new System.Drawing.Point(7, 149);
            this.reverseLabel.Name = "reverseLabel";
            this.reverseLabel.Size = new System.Drawing.Size(73, 17);
            this.reverseLabel.TabIndex = 18;
            this.reverseLabel.Text = "Reversed:";
            // 
            // setZeroPosButton
            // 
            this.setZeroPosButton.Location = new System.Drawing.Point(229, 28);
            this.setZeroPosButton.Name = "setZeroPosButton";
            this.setZeroPosButton.Size = new System.Drawing.Size(206, 27);
            this.setZeroPosButton.TabIndex = 20;
            this.setZeroPosButton.Text = "Set current position as Zero";
            this.setZeroPosButton.UseVisualStyleBackColor = true;
            this.setZeroPosButton.Click += new System.EventHandler(this.setZeroPosButton_Click);
            // 
            // CurrentPosLabel
            // 
            this.CurrentPosLabel.AutoSize = true;
            this.CurrentPosLabel.Location = new System.Drawing.Point(7, 38);
            this.CurrentPosLabel.Name = "CurrentPosLabel";
            this.CurrentPosLabel.Size = new System.Drawing.Size(113, 17);
            this.CurrentPosLabel.TabIndex = 21;
            this.CurrentPosLabel.Text = "Current Position:";
            // 
            // CurrentPositionValue
            // 
            this.CurrentPositionValue.AutoSize = true;
            this.CurrentPositionValue.Location = new System.Drawing.Point(120, 38);
            this.CurrentPositionValue.Name = "CurrentPositionValue";
            this.CurrentPositionValue.Size = new System.Drawing.Size(27, 17);
            this.CurrentPositionValue.TabIndex = 22;
            this.CurrentPositionValue.Text = "NA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 17);
            this.label2.TabIndex = 23;
            this.label2.Text = "Set Position:";
            this.label2.Click += new System.EventHandler(this.label2_Click_3);
            // 
            // MaxPosition
            // 
            this.MaxPosition.AutoSize = true;
            this.MaxPosition.Location = new System.Drawing.Point(7, 107);
            this.MaxPosition.Name = "MaxPosition";
            this.MaxPosition.Size = new System.Drawing.Size(91, 17);
            this.MaxPosition.TabIndex = 24;
            this.MaxPosition.Text = "Max Position:";
            // 
            // MaxPositionValue
            // 
            this.MaxPositionValue.AutoSize = true;
            this.MaxPositionValue.Location = new System.Drawing.Point(120, 107);
            this.MaxPositionValue.Name = "MaxPositionValue";
            this.MaxPositionValue.Size = new System.Drawing.Size(27, 17);
            this.MaxPositionValue.TabIndex = 25;
            this.MaxPositionValue.Text = "NA";
            // 
            // newMaxPos
            // 
            this.newMaxPos.Location = new System.Drawing.Point(229, 107);
            this.newMaxPos.Name = "newMaxPos";
            this.newMaxPos.Size = new System.Drawing.Size(90, 22);
            this.newMaxPos.TabIndex = 26;
            // 
            // updateMaxButton
            // 
            this.updateMaxButton.Location = new System.Drawing.Point(330, 102);
            this.updateMaxButton.Name = "updateMaxButton";
            this.updateMaxButton.Size = new System.Drawing.Size(91, 33);
            this.updateMaxButton.TabIndex = 27;
            this.updateMaxButton.Text = "Update Max";
            this.updateMaxButton.UseVisualStyleBackColor = true;
            this.updateMaxButton.Click += new System.EventHandler(this.updateMaxButton_Click);
            // 
            // reversedValLabel
            // 
            this.reversedValLabel.AutoSize = true;
            this.reversedValLabel.Location = new System.Drawing.Point(117, 149);
            this.reversedValLabel.Name = "reversedValLabel";
            this.reversedValLabel.Size = new System.Drawing.Size(27, 17);
            this.reversedValLabel.TabIndex = 28;
            this.reversedValLabel.Text = "NA";
            // 
            // motorLoadLabel
            // 
            this.motorLoadLabel.AutoSize = true;
            this.motorLoadLabel.Location = new System.Drawing.Point(4, 73);
            this.motorLoadLabel.Name = "motorLoadLabel";
            this.motorLoadLabel.Size = new System.Drawing.Size(95, 17);
            this.motorLoadLabel.TabIndex = 29;
            this.motorLoadLabel.Text = "SG_RESULT:";
            // 
            // motorLoadVal
            // 
            this.motorLoadVal.AutoSize = true;
            this.motorLoadVal.Location = new System.Drawing.Point(259, 73);
            this.motorLoadVal.Name = "motorLoadVal";
            this.motorLoadVal.Size = new System.Drawing.Size(27, 17);
            this.motorLoadVal.TabIndex = 30;
            this.motorLoadVal.Text = "NA";
            // 
            // controlPanel
            // 
            this.controlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.controlPanel.Controls.Add(this.triggerHomingButton);
            this.controlPanel.Controls.Add(this.reversedValLabel);
            this.controlPanel.Controls.Add(this.updateMaxButton);
            this.controlPanel.Controls.Add(this.moveButt);
            this.controlPanel.Controls.Add(this.newMaxPos);
            this.controlPanel.Controls.Add(this.MoveVal);
            this.controlPanel.Controls.Add(this.MaxPositionValue);
            this.controlPanel.Controls.Add(this.toggleDirection);
            this.controlPanel.Controls.Add(this.MaxPosition);
            this.controlPanel.Controls.Add(this.haltButton);
            this.controlPanel.Controls.Add(this.label2);
            this.controlPanel.Controls.Add(this.reverseLabel);
            this.controlPanel.Controls.Add(this.CurrentPositionValue);
            this.controlPanel.Controls.Add(this.setZeroPosButton);
            this.controlPanel.Controls.Add(this.CurrentPosLabel);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlPanel.Location = new System.Drawing.Point(3, 3);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(1023, 198);
            this.controlPanel.TabIndex = 31;
            // 
            // triggerHomingButton
            // 
            this.triggerHomingButton.Location = new System.Drawing.Point(513, 16);
            this.triggerHomingButton.Name = "triggerHomingButton";
            this.triggerHomingButton.Size = new System.Drawing.Size(123, 61);
            this.triggerHomingButton.TabIndex = 29;
            this.triggerHomingButton.Text = "Trigger Homing";
            this.triggerHomingButton.UseVisualStyleBackColor = true;
            this.triggerHomingButton.Click += new System.EventHandler(this.triggerHomingButton_Click);
            // 
            // configPanel
            // 
            this.configPanel.AutoSize = true;
            this.configPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.configPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.configPanel.Controls.Add(this.configTableLayout);
            this.configPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configPanel.Location = new System.Drawing.Point(3, 246);
            this.configPanel.Name = "configPanel";
            this.configPanel.Size = new System.Drawing.Size(1023, 208);
            this.configPanel.TabIndex = 32;
            // 
            // configTableLayout
            // 
            this.configTableLayout.AutoSize = true;
            this.configTableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.configTableLayout.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.configTableLayout.ColumnCount = 4;
            this.configTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.configTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.configTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 287F));
            this.configTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 151F));
            this.configTableLayout.Controls.Add(this.updateStallThresh, 3, 5);
            this.configTableLayout.Controls.Add(this.iholdSetVal, 2, 2);
            this.configTableLayout.Controls.Add(this.updateIrunButton, 3, 1);
            this.configTableLayout.Controls.Add(this.moveCurrentLabel, 0, 1);
            this.configTableLayout.Controls.Add(this.label1, 0, 2);
            this.configTableLayout.Controls.Add(this.label3, 1, 0);
            this.configTableLayout.Controls.Add(this.label4, 2, 0);
            this.configTableLayout.Controls.Add(this.irunCurrentVal, 1, 1);
            this.configTableLayout.Controls.Add(this.iholdCurrentVal, 1, 2);
            this.configTableLayout.Controls.Add(this.updateIholdButton, 3, 2);
            this.configTableLayout.Controls.Add(this.updateStepSpeedButton, 3, 3);
            this.configTableLayout.Controls.Add(this.label5, 0, 4);
            this.configTableLayout.Controls.Add(this.homingDirectionsCurrVals, 1, 4);
            this.configTableLayout.Controls.Add(this.homingDirectionsSetVals, 2, 4);
            this.configTableLayout.Controls.Add(this.updateHomingDirs, 3, 4);
            this.configTableLayout.Controls.Add(this.irunSetVal, 2, 1);
            this.configTableLayout.Controls.Add(this.motorSpeedVal, 1, 3);
            this.configTableLayout.Controls.Add(this.motorSpeedLabel, 0, 3);
            this.configTableLayout.Controls.Add(this.motorSpeedSetVal, 2, 3);
            this.configTableLayout.Controls.Add(this.stallThreshVal, 2, 5);
            this.configTableLayout.Controls.Add(this.stallThreshCurrentVal, 1, 5);
            this.configTableLayout.Controls.Add(this.stallThreshLabel, 0, 5);
            this.configTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configTableLayout.Location = new System.Drawing.Point(0, 0);
            this.configTableLayout.Name = "configTableLayout";
            this.configTableLayout.RowCount = 6;
            this.configTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.configTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.configTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.configTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.configTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.configTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.configTableLayout.Size = new System.Drawing.Size(1021, 206);
            this.configTableLayout.TabIndex = 0;
            // 
            // iholdSetVal
            // 
            this.iholdSetVal.AutoSize = true;
            this.iholdSetVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iholdSetVal.Location = new System.Drawing.Point(584, 56);
            this.iholdSetVal.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.iholdSetVal.Name = "iholdSetVal";
            this.iholdSetVal.Size = new System.Drawing.Size(281, 22);
            this.iholdSetVal.TabIndex = 37;
            // 
            // updateIrunButton
            // 
            this.updateIrunButton.AutoSize = true;
            this.updateIrunButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.updateIrunButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updateIrunButton.Location = new System.Drawing.Point(872, 22);
            this.updateIrunButton.Name = "updateIrunButton";
            this.updateIrunButton.Size = new System.Drawing.Size(145, 27);
            this.updateIrunButton.TabIndex = 37;
            this.updateIrunButton.Text = "Update";
            this.updateIrunButton.UseVisualStyleBackColor = true;
            this.updateIrunButton.Click += new System.EventHandler(this.updateIrunButton_Click);
            // 
            // moveCurrentLabel
            // 
            this.moveCurrentLabel.AutoSize = true;
            this.moveCurrentLabel.Location = new System.Drawing.Point(4, 19);
            this.moveCurrentLabel.Name = "moveCurrentLabel";
            this.moveCurrentLabel.Size = new System.Drawing.Size(104, 17);
            this.moveCurrentLabel.TabIndex = 0;
            this.moveCurrentLabel.Text = "Moving Current";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Holding Current";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(154, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Current Value";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(584, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "New Value";
            // 
            // irunCurrentVal
            // 
            this.irunCurrentVal.AutoSize = true;
            this.irunCurrentVal.Location = new System.Drawing.Point(154, 19);
            this.irunCurrentVal.Name = "irunCurrentVal";
            this.irunCurrentVal.Size = new System.Drawing.Size(27, 17);
            this.irunCurrentVal.TabIndex = 38;
            this.irunCurrentVal.Text = "NA";
            // 
            // iholdCurrentVal
            // 
            this.iholdCurrentVal.AutoSize = true;
            this.iholdCurrentVal.Location = new System.Drawing.Point(154, 53);
            this.iholdCurrentVal.Name = "iholdCurrentVal";
            this.iholdCurrentVal.Size = new System.Drawing.Size(27, 17);
            this.iholdCurrentVal.TabIndex = 39;
            this.iholdCurrentVal.Text = "NA";
            // 
            // updateIholdButton
            // 
            this.updateIholdButton.AutoSize = true;
            this.updateIholdButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.updateIholdButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updateIholdButton.Location = new System.Drawing.Point(872, 56);
            this.updateIholdButton.Name = "updateIholdButton";
            this.updateIholdButton.Size = new System.Drawing.Size(145, 27);
            this.updateIholdButton.TabIndex = 40;
            this.updateIholdButton.Text = "Update";
            this.updateIholdButton.UseVisualStyleBackColor = true;
            this.updateIholdButton.Click += new System.EventHandler(this.updateIholdButton_Click);
            // 
            // updateStepSpeedButton
            // 
            this.updateStepSpeedButton.AutoSize = true;
            this.updateStepSpeedButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.updateStepSpeedButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updateStepSpeedButton.Location = new System.Drawing.Point(872, 90);
            this.updateStepSpeedButton.Name = "updateStepSpeedButton";
            this.updateStepSpeedButton.Size = new System.Drawing.Size(145, 27);
            this.updateStepSpeedButton.TabIndex = 41;
            this.updateStepSpeedButton.Text = "Update";
            this.updateStepSpeedButton.UseVisualStyleBackColor = true;
            this.updateStepSpeedButton.Click += new System.EventHandler(this.updateStepSpeedButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(130, 17);
            this.label5.TabIndex = 43;
            this.label5.Text = "Sensorless Homing";
            // 
            // homingDirectionsCurrVals
            // 
            this.homingDirectionsCurrVals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.homingDirectionsCurrVals.FormattingEnabled = true;
            this.homingDirectionsCurrVals.Items.AddRange(new object[] {
            "Towards Zero",
            "Towards Max"});
            this.homingDirectionsCurrVals.Location = new System.Drawing.Point(154, 124);
            this.homingDirectionsCurrVals.Name = "homingDirectionsCurrVals";
            this.homingDirectionsCurrVals.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.homingDirectionsCurrVals.Size = new System.Drawing.Size(423, 44);
            this.homingDirectionsCurrVals.TabIndex = 44;
            this.homingDirectionsCurrVals.SelectedIndexChanged += new System.EventHandler(this.homingDirectionsCurrVals_SelectedIndexChanged);
            // 
            // homingDirectionsSetVals
            // 
            this.homingDirectionsSetVals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.homingDirectionsSetVals.FormattingEnabled = true;
            this.homingDirectionsSetVals.Items.AddRange(new object[] {
            "Towards Zero",
            "Towards Max"});
            this.homingDirectionsSetVals.Location = new System.Drawing.Point(584, 124);
            this.homingDirectionsSetVals.Name = "homingDirectionsSetVals";
            this.homingDirectionsSetVals.Size = new System.Drawing.Size(281, 44);
            this.homingDirectionsSetVals.TabIndex = 45;
            // 
            // updateHomingDirs
            // 
            this.updateHomingDirs.AutoSize = true;
            this.updateHomingDirs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.updateHomingDirs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updateHomingDirs.Location = new System.Drawing.Point(872, 124);
            this.updateHomingDirs.Name = "updateHomingDirs";
            this.updateHomingDirs.Size = new System.Drawing.Size(145, 44);
            this.updateHomingDirs.TabIndex = 46;
            this.updateHomingDirs.Text = "Update";
            this.updateHomingDirs.UseVisualStyleBackColor = true;
            this.updateHomingDirs.Click += new System.EventHandler(this.updateHomingDirs_Click);
            // 
            // irunSetVal
            // 
            this.irunSetVal.AutoSize = true;
            this.irunSetVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.irunSetVal.Location = new System.Drawing.Point(584, 22);
            this.irunSetVal.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.irunSetVal.Name = "irunSetVal";
            this.irunSetVal.Size = new System.Drawing.Size(281, 22);
            this.irunSetVal.TabIndex = 47;
            // 
            // motorSpeedVal
            // 
            this.motorSpeedVal.AutoSize = true;
            this.motorSpeedVal.Location = new System.Drawing.Point(154, 87);
            this.motorSpeedVal.Name = "motorSpeedVal";
            this.motorSpeedVal.Size = new System.Drawing.Size(27, 17);
            this.motorSpeedVal.TabIndex = 42;
            this.motorSpeedVal.Text = "NA";
            // 
            // motorSpeedLabel
            // 
            this.motorSpeedLabel.AutoSize = true;
            this.motorSpeedLabel.Location = new System.Drawing.Point(4, 87);
            this.motorSpeedLabel.Name = "motorSpeedLabel";
            this.motorSpeedLabel.Size = new System.Drawing.Size(89, 17);
            this.motorSpeedLabel.TabIndex = 2;
            this.motorSpeedLabel.Text = "Motor Speed";
            // 
            // motorSpeedSetVal
            // 
            this.motorSpeedSetVal.AutoSize = true;
            this.motorSpeedSetVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.motorSpeedSetVal.Location = new System.Drawing.Point(584, 90);
            this.motorSpeedSetVal.Name = "motorSpeedSetVal";
            this.motorSpeedSetVal.Size = new System.Drawing.Size(281, 22);
            this.motorSpeedSetVal.TabIndex = 48;
            // 
            // debugPanel
            // 
            this.debugPanel.AutoSize = true;
            this.debugPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.debugPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.debugPanel.Controls.Add(this.tableLayoutPanel1);
            this.debugPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugPanel.Location = new System.Drawing.Point(3, 460);
            this.debugPanel.Name = "debugPanel";
            this.debugPanel.Size = new System.Drawing.Size(1023, 165);
            this.debugPanel.TabIndex = 33;
            this.debugPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.debugPanel_Paint);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.vidVal, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.csActualVal, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.vidLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.csActualLabel, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.pidLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.motorLoadVal, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.pidVal, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.motorLoadLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.serialNumberLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.serialNumberVal, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.stepperDriverErrorLabel, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.stepperDriverErrorVal, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.StepperCommsErrorLabel, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.stepperDriverCommsErrrorVal, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.stepperDriverEnabledLabel, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.stepperDriverEnabledVal, 1, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1021, 163);
            this.tableLayoutPanel1.TabIndex = 33;
            // 
            // csActualVal
            // 
            this.csActualVal.AutoSize = true;
            this.csActualVal.Location = new System.Drawing.Point(259, 91);
            this.csActualVal.Name = "csActualVal";
            this.csActualVal.Size = new System.Drawing.Size(27, 17);
            this.csActualVal.TabIndex = 32;
            this.csActualVal.Text = "NA";
            // 
            // csActualLabel
            // 
            this.csActualLabel.AutoSize = true;
            this.csActualLabel.Location = new System.Drawing.Point(4, 91);
            this.csActualLabel.Name = "csActualLabel";
            this.csActualLabel.Size = new System.Drawing.Size(92, 17);
            this.csActualLabel.TabIndex = 31;
            this.csActualLabel.Text = "CS_ACTUAL:";
            // 
            // stepperDriverErrorLabel
            // 
            this.stepperDriverErrorLabel.AutoSize = true;
            this.stepperDriverErrorLabel.Location = new System.Drawing.Point(4, 109);
            this.stepperDriverErrorLabel.Name = "stepperDriverErrorLabel";
            this.stepperDriverErrorLabel.Size = new System.Drawing.Size(189, 17);
            this.stepperDriverErrorLabel.TabIndex = 33;
            this.stepperDriverErrorLabel.Text = "STEPPER_DRIVER_ERROR";
            // 
            // stepperDriverErrorVal
            // 
            this.stepperDriverErrorVal.AutoSize = true;
            this.stepperDriverErrorVal.Location = new System.Drawing.Point(259, 109);
            this.stepperDriverErrorVal.Name = "stepperDriverErrorVal";
            this.stepperDriverErrorVal.Size = new System.Drawing.Size(27, 17);
            this.stepperDriverErrorVal.TabIndex = 34;
            this.stepperDriverErrorVal.Text = "NA";
            this.stepperDriverErrorVal.Click += new System.EventHandler(this.label7_Click);
            // 
            // StepperCommsErrorLabel
            // 
            this.StepperCommsErrorLabel.AutoSize = true;
            this.StepperCommsErrorLabel.Location = new System.Drawing.Point(4, 127);
            this.StepperCommsErrorLabel.Name = "StepperCommsErrorLabel";
            this.StepperCommsErrorLabel.Size = new System.Drawing.Size(248, 17);
            this.StepperCommsErrorLabel.TabIndex = 35;
            this.StepperCommsErrorLabel.Text = "STEPPER_DRIVER_COMMS_ERROR";
            // 
            // stepperDriverCommsErrrorVal
            // 
            this.stepperDriverCommsErrrorVal.AutoSize = true;
            this.stepperDriverCommsErrrorVal.Location = new System.Drawing.Point(259, 127);
            this.stepperDriverCommsErrrorVal.Name = "stepperDriverCommsErrrorVal";
            this.stepperDriverCommsErrrorVal.Size = new System.Drawing.Size(27, 17);
            this.stepperDriverCommsErrrorVal.TabIndex = 36;
            this.stepperDriverCommsErrrorVal.Text = "NA";
            // 
            // stepperDriverEnabledLabel
            // 
            this.stepperDriverEnabledLabel.AutoSize = true;
            this.stepperDriverEnabledLabel.Location = new System.Drawing.Point(4, 145);
            this.stepperDriverEnabledLabel.Name = "stepperDriverEnabledLabel";
            this.stepperDriverEnabledLabel.Size = new System.Drawing.Size(203, 17);
            this.stepperDriverEnabledLabel.TabIndex = 37;
            this.stepperDriverEnabledLabel.Text = "STEPPER_DRIVER_ENABLED";
            // 
            // stepperDriverEnabledVal
            // 
            this.stepperDriverEnabledVal.AutoSize = true;
            this.stepperDriverEnabledVal.Location = new System.Drawing.Point(259, 145);
            this.stepperDriverEnabledVal.Name = "stepperDriverEnabledVal";
            this.stepperDriverEnabledVal.Size = new System.Drawing.Size(27, 17);
            this.stepperDriverEnabledVal.TabIndex = 38;
            this.stepperDriverEnabledVal.Text = "NA";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.visibilityButtonsTable, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.debugPanel, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.controlPanel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.configPanel, 0, 2);
            this.tableLayoutPanel.Location = new System.Drawing.Point(12, 70);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(1029, 628);
            this.tableLayoutPanel.TabIndex = 36;
            // 
            // visibilityButtonsTable
            // 
            this.visibilityButtonsTable.AutoSize = true;
            this.visibilityButtonsTable.ColumnCount = 2;
            this.visibilityButtonsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.visibilityButtonsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.visibilityButtonsTable.Controls.Add(this.toggleDebugVisibilitybutton, 0, 0);
            this.visibilityButtonsTable.Controls.Add(this.toggleConfigVisibilitybutton, 0, 0);
            this.visibilityButtonsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visibilityButtonsTable.Location = new System.Drawing.Point(3, 207);
            this.visibilityButtonsTable.Name = "visibilityButtonsTable";
            this.visibilityButtonsTable.RowCount = 1;
            this.visibilityButtonsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.visibilityButtonsTable.Size = new System.Drawing.Size(1023, 33);
            this.visibilityButtonsTable.TabIndex = 38;
            this.visibilityButtonsTable.Paint += new System.Windows.Forms.PaintEventHandler(this.visibilityButtonsTable_Paint);
            // 
            // toggleDebugVisibilitybutton
            // 
            this.toggleDebugVisibilitybutton.AutoSize = true;
            this.toggleDebugVisibilitybutton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.toggleDebugVisibilitybutton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toggleDebugVisibilitybutton.Location = new System.Drawing.Point(514, 3);
            this.toggleDebugVisibilitybutton.Name = "toggleDebugVisibilitybutton";
            this.toggleDebugVisibilitybutton.Size = new System.Drawing.Size(506, 27);
            this.toggleDebugVisibilitybutton.TabIndex = 36;
            this.toggleDebugVisibilitybutton.Text = "Debug Info";
            this.toggleDebugVisibilitybutton.UseVisualStyleBackColor = true;
            this.toggleDebugVisibilitybutton.Click += new System.EventHandler(this.toggleDebugVisibilitybutton_Click);
            // 
            // toggleConfigVisibilitybutton
            // 
            this.toggleConfigVisibilitybutton.AutoSize = true;
            this.toggleConfigVisibilitybutton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.toggleConfigVisibilitybutton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toggleConfigVisibilitybutton.Location = new System.Drawing.Point(3, 3);
            this.toggleConfigVisibilitybutton.Name = "toggleConfigVisibilitybutton";
            this.toggleConfigVisibilitybutton.Size = new System.Drawing.Size(505, 27);
            this.toggleConfigVisibilitybutton.TabIndex = 35;
            this.toggleConfigVisibilitybutton.Text = "Show Configuration";
            this.toggleConfigVisibilitybutton.UseVisualStyleBackColor = true;
            this.toggleConfigVisibilitybutton.Click += new System.EventHandler(this.toggleConfigVisibilitybutton_Click_1);
            // 
            // updateStallThresh
            // 
            this.updateStallThresh.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.updateStallThresh.AutoSize = true;
            this.updateStallThresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.updateStallThresh.Location = new System.Drawing.Point(872, 175);
            this.updateStallThresh.Name = "updateStallThresh";
            this.updateStallThresh.Size = new System.Drawing.Size(145, 27);
            this.updateStallThresh.TabIndex = 37;
            this.updateStallThresh.Text = "Update";
            this.updateStallThresh.UseVisualStyleBackColor = true;
            // 
            // stallThreshVal
            // 
            this.stallThreshVal.AutoSize = true;
            this.stallThreshVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stallThreshVal.Location = new System.Drawing.Point(584, 175);
            this.stallThreshVal.Name = "stallThreshVal";
            this.stallThreshVal.Size = new System.Drawing.Size(281, 22);
            this.stallThreshVal.TabIndex = 49;
            // 
            // stallThreshCurrentVal
            // 
            this.stallThreshCurrentVal.AutoSize = true;
            this.stallThreshCurrentVal.Location = new System.Drawing.Point(154, 172);
            this.stallThreshCurrentVal.Name = "stallThreshCurrentVal";
            this.stallThreshCurrentVal.Size = new System.Drawing.Size(27, 17);
            this.stallThreshCurrentVal.TabIndex = 50;
            this.stallThreshCurrentVal.Text = "NA";
            // 
            // stallThreshLabel
            // 
            this.stallThreshLabel.Location = new System.Drawing.Point(4, 172);
            this.stallThreshLabel.Name = "stallThreshLabel";
            this.stallThreshLabel.Size = new System.Drawing.Size(143, 30);
            this.stallThreshLabel.TabIndex = 51;
            this.stallThreshLabel.Text = "Stall Threshold\r\n(Lower values are more sensitive)\r\n";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(2562, 1047);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.connectedState);
            this.Controls.Add(this.picASCOM);
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
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            this.configPanel.ResumeLayout(false);
            this.configPanel.PerformLayout();
            this.configTableLayout.ResumeLayout(false);
            this.configTableLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iholdSetVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.irunSetVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.motorSpeedSetVal)).EndInit();
            this.debugPanel.ResumeLayout(false);
            this.debugPanel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.visibilityButtonsTable.ResumeLayout(false);
            this.visibilityButtonsTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stallThreshVal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox picASCOM;
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
        private System.Windows.Forms.Button setZeroPosButton;
        private System.Windows.Forms.Label CurrentPosLabel;
        private System.Windows.Forms.Label CurrentPositionValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label MaxPosition;
        private System.Windows.Forms.Label MaxPositionValue;
        private System.Windows.Forms.TextBox newMaxPos;
        private System.Windows.Forms.Button updateMaxButton;
        private System.Windows.Forms.Label reversedValLabel;
        private System.Windows.Forms.Label motorLoadLabel;
        private System.Windows.Forms.Label motorLoadVal;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Panel configPanel;
        private System.Windows.Forms.Panel debugPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label csActualVal;
        private System.Windows.Forms.Label csActualLabel;
        private System.Windows.Forms.TableLayoutPanel configTableLayout;
        private System.Windows.Forms.Label moveCurrentLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label motorSpeedLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button updateIrunButton;
        private System.Windows.Forms.Label irunCurrentVal;
        private System.Windows.Forms.Label iholdCurrentVal;
        private System.Windows.Forms.Button updateIholdButton;
        private System.Windows.Forms.Button updateStepSpeedButton;
        private System.Windows.Forms.TableLayoutPanel visibilityButtonsTable;
        private System.Windows.Forms.Button toggleDebugVisibilitybutton;
        private System.Windows.Forms.Button toggleConfigVisibilitybutton;
        private System.Windows.Forms.Label motorSpeedVal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckedListBox homingDirectionsCurrVals;
        private System.Windows.Forms.CheckedListBox homingDirectionsSetVals;
        private System.Windows.Forms.Button updateHomingDirs;
        private System.Windows.Forms.NumericUpDown irunSetVal;
        private System.Windows.Forms.NumericUpDown iholdSetVal;
        private System.Windows.Forms.Button triggerHomingButton;
        private System.Windows.Forms.NumericUpDown motorSpeedSetVal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label stepperDriverErrorLabel;
        private System.Windows.Forms.Label stepperDriverErrorVal;
        private System.Windows.Forms.Label StepperCommsErrorLabel;
        private System.Windows.Forms.Label stepperDriverCommsErrrorVal;
        private System.Windows.Forms.Label stepperDriverEnabledLabel;
        private System.Windows.Forms.Label stepperDriverEnabledVal;
        private System.Windows.Forms.Button updateStallThresh;
        private System.Windows.Forms.NumericUpDown stallThreshVal;
        private System.Windows.Forms.Label stallThreshCurrentVal;
        private System.Windows.Forms.Label stallThreshLabel;
    }
}