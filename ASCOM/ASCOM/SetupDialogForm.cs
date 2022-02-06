using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.SympleAstroFocus;

namespace ASCOM.SympleAstroFocus
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        TraceLogger tl; // Holder for a reference to the driver's trace logger
        Focuser f;
        public SetupDialogForm(Focuser f, TraceLogger tlDriver)
        {
            InitializeComponent();

            // Save the provided trace logger for use within the setup dialogue
            tl = tlDriver;
            this.f = f;

            this.f.UpdateRecievdFromDevice += f_DataFromDevice;
            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue

        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("https://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {



            updateDisplayedValues();
            debugPanel.Visible = false;
            configPanel.Visible = false;

        }

        //There has to be a better way of doing this besides maintaining two function?
        private void updateDisplayedValues()
        {
            serialNumberVal.Text = "";

            if (f.Connected == true)
            {

                connectedState.Text = "CONNECTED";
                serialNumberVal.Text = f.SerialNumber;

            }
        }

        private void InvokeUpdateDisplayedValues()
        {

            Invoke(new Action(() => { serialNumberVal.Text = ""; }));

            if (f.Connected == true)
            {

                Invoke(new Action(() => { connectedState.Text = "CONNECTED"; }));
                Invoke(new Action(() => { serialNumberVal.Text = f.SerialNumber; }));


                Invoke(new Action(() => { CurrentPositionValue.Text = f.Position.ToString(); }));

                Invoke(new Action(() => { MaxPositionValue.Text = f.MaxStep.ToString(); }));

                Invoke(new Action(() => { reversedValLabel.Text = f.ReversedMotor.ToString(); }));

                Invoke(new Action(() => { motorLoadVal.Text = f.SG_RESULT.ToString(); }));

                Invoke(new Action(() => { csActualVal.Text = f.CS_ACTUAL.ToString(); }));


                Invoke(new Action(() => { homingDirectionsCurrVals.SetItemChecked(0, f.HomingTowardsZeroEnabled); }));

                Invoke(new Action(() => { homingDirectionsCurrVals.SetItemChecked(1, f.HomingTowardsMaxEnabled); }));


                Invoke(new Action(() => { irunCurrentVal.Text = f.IRUN.ToString(); }));


                Invoke(new Action(() => { iholdCurrentVal.Text = f.IHOLD.ToString(); }));
            }
        }

        void f_DataFromDevice(object sender, EventArgs e)
        {
            InvokeUpdateDisplayedValues();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click_2(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            f.Move(Int32.Parse(MoveVal.Text));
        }

        private void MoveVal_TextChanged(object sender, EventArgs e)
        {

        }

        private void toggleDirection_Click(object sender, EventArgs e)
        {
            f.ToggleReverse();
        }

        private void refreshUi_Click(object sender, EventArgs e)
        {
            updateDisplayedValues();
        }

        private void setZeroPosButton_Click(object sender, EventArgs e)
        {
            f.SetZero();
        }

        private void haltButton_Click(object sender, EventArgs e)
        {
            f.Halt();
        }

        private void label2_Click_3(object sender, EventArgs e)
        {

        }

        private void updateMaxButton_Click(object sender, EventArgs e)
        {
            int new_max;

            if (Int32.TryParse(newMaxPos.Text, out new_max))
            {
                f.MaxIncrement = new_max;
            }
        }


        private void debugPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void visibilityButtonsTable_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toggleConfigVisibilitybutton_Click_1(object sender, EventArgs e)
        {
            configPanel.Visible = !configPanel.Visible;
        }

        private void toggleDebugVisibilitybutton_Click(object sender, EventArgs e)
        {
            debugPanel.Visible = !debugPanel.Visible;
        }

        private void homingDirectionsCurrVals_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void updateHomingDirs_Click(object sender, EventArgs e)
        {
            f.HomingTowardsZeroEnabled = homingDirectionsSetVals.GetItemChecked(0);
            f.HomingTowardsMaxEnabled = homingDirectionsSetVals.GetItemChecked(1);
        }

        private void updateIrunButton_Click(object sender, EventArgs e)
        {
            f.IRUN = Decimal.ToUInt32(irunSetVal.Value);
        }

        private void updateIholdButton_Click(object sender, EventArgs e)
        {
            f.IHOLD = Decimal.ToUInt32(iholdSetVal.Value);
        }

        private void triggerHomingButton_Click(object sender, EventArgs e)
        {
            f.TriggerHoming();
        }
    }
}