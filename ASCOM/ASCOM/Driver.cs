//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Focuser driver for SympleAstroFocus
//
// Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
//				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
//				erat, sed diam voluptua. At vero eos et accusam et justo duo 
//				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
//				sanctus est Lorem ipsum dolor sit amet.
//
// Implements:	ASCOM Focuser interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// dd-mmm-yyyy	XXX	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//


// This is used to define code in the template that is specific to one class implementation
// unused code can be deleted and this definition removed.
#define Focuser

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.ComponentModel;

using System.Net;
using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using System.Collections.Specialized;

using HidSharp;
using System.Linq;



namespace ASCOM.SympleAstroFocus
{
    //
    // Your driver's DeviceID is ASCOM.SympleAstroFocus.Focuser
    //
    // The Guid attribute sets the CLSID for ASCOM.SympleAstroFocus.Focuser
    // The ClassInterface/None attribute prevents an empty interface called
    // _SympleAstroFocus from being created and used as the [default] interface
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM Focuser Driver for SympleAstroFocus.
    /// </summary>
    [Guid("d1b57fc2-7af3-42a6-a9fe-3c6b41e6bf70")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Focuser : IFocuserV3
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.SympleAstroFocus.Focuser";
        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "SympleAstroFocus";

        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";
        

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        internal TraceLogger tl;

        FilteredDeviceList devices;
        HidDevice device;
        HidStream stream;

        BackgroundWorker usbBgWorker;

        private Mutex usbMut;

        #region IFocuser variables
        //TODO: figure out out the most "C#" way of maintaining these variables
        //small class, tuple, array per var?
        private bool deviceNeedsUpdating;
        private uint appCurrentPos = 0; 
        private uint deviceCurrentPos = 0;
        private uint appSetPos = 0;
        private uint deviceSetPos = 0;
        private uint appMaxPos = 0;
        private uint deviceMaxPos = 0;
        private Constants.Status_Dword_Bits status_flags;
        private Constants.Command_Dword_Bits commands;
        private uint driverStatus = 0;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SympleAstroFocus"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Focuser()
        {
            tl = new TraceLogger("", "SympleAstroFocus");
            ReadProfile(); // Read device configuration from the ASCOM Profile store

            tl.LogMessage("Focuser", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro-utilities object




            devices = new FilteredDeviceList();
            devices.Add(DeviceList.Local);
            updateConnectionStatus();

            

            tl.LogMessage("Focuser", "Completed initialisation");
        }


        protected virtual void OnDevicesChanged(EventArgs e)
        {
            updateConnectionStatus();
        }

        public bool updateConnectionStatus()
        {
            bool existingConnectedState = connectedState;
            if (connectedState == false)
            {

                //nothing connected, try to connect
                IEnumerable<HidDevice> candidate_devices = devices.GetHidDevices(56, 78);


                device = candidate_devices.FirstOrDefault();
                if (device != null)
                {
                    HidSharp.Reports.ReportDescriptor desc =  device.GetReportDescriptor();
                    Console.WriteLine(desc.ToString());


                    connectedState = true;
                    //
                    updateStateFromDevice();
                    syncAppFromDeviceState();
                    deviceNeedsUpdating = false;
                    usbMut = new Mutex();
                    usbBgWorker = new BackgroundWorker();
                    usbBgWorker.DoWork += new DoWorkEventHandler(bgThread);
                    usbBgWorker.RunWorkerAsync();
                }
            } else {

                //we're already connected - make sure its still there
                //TODO
            }



            if (connectedState == existingConnectedState) {
                return false;
            }

            return true;
        }

        public event EventHandler UpdateRecievdFromDevice;

        private void bgThread(object sender,
            DoWorkEventArgs e)
        {   
            while (true)
            {
                usbMut.WaitOne();
                updateStateFromDevice();
                usbMut.ReleaseMutex();

                usbMut.WaitOne();
                updateDeviceFromHost();
                usbMut.ReleaseMutex();

                Thread.Sleep(100);
            }
        }

        private int updateStateFromDevice()
        {

            int bytes_read = 0;
            if (!device.TryOpen(out stream))
            {
                Console.WriteLine("Failed to open device.");
                throw new ASCOM.NotConnectedException("Couldn't open a stream for reading latest state from device");
            }

            using (stream)
            {

                byte[] bytes;
                bytes = new byte[65];

                UInt32[] dwords_from_dev;
                dwords_from_dev = new UInt32[16];
                try
                {
                    bytes_read = stream.Read(bytes);
                    Console.WriteLine(bytes);

                    for (int i = 1; i < bytes_read-4; i=i+4) //starting at 1 is weird - might be HidSharp or uC code's fault
                    {
                        int dword = BitConverter.ToInt32(bytes, i);
                        //dword = IPAddress.HostToNetworkOrder(dword);
                        dwords_from_dev[i / 4] = unchecked((uint)dword);
                    }

                    switch (dwords_from_dev[Constants.STATE_ID_DWORD])
                    {
                        case Constants.STATE_ID_0:
                            decodeStateId0(dwords_from_dev);
                            break;
                        default:
                            Console.WriteLine("Unrecognised state word type");
                            break;
                    }
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Read timed out.");
                }

            }
            UpdateRecievdFromDevice?.Invoke(this, EventArgs.Empty);
            return bytes_read;
        }

        private void decodeStateId0(uint[] state_words)
        {
            deviceCurrentPos   = state_words[Constants.CURRENT_POSITION_DWORD];
            deviceMaxPos       = state_words[Constants.MAX_POSITION_DWORD];
            deviceSetPos       = state_words[Constants.SET_POSITION_DWORD];

            status_flags = (Constants.Status_Dword_Bits)state_words[Constants.STATUS_DWORD];
            driverStatus = state_words[Constants.DRIVER_STATUS_DWORD];
        }

        private void updateDeviceFromHost()
        {
            if (deviceNeedsUpdating)
            {
                if (!device.TryOpen(out stream))
                {
                    Console.WriteLine("Failed to open device.");
                    throw new ASCOM.NotConnectedException("Couldn't open a stream for writing latest state to device");
                }

                using (stream)
                {

                    byte[] bytes;
                    bytes = new byte[65];

                    uint[] dwords_to_dev;
                    dwords_to_dev = new uint[16];

                    dwords_to_dev[Constants.STATE_ID_DWORD] = Constants.STATE_ID_0;
                    dwords_to_dev[Constants.SET_POSITION_DWORD] = appSetPos;
                    dwords_to_dev[Constants.MAX_POSITION_DWORD] = appMaxPos;
                    dwords_to_dev[Constants.COMMAND_DWORD] = (uint)commands;
                    for (int i = 0; i < dwords_to_dev.Length; i++)
                    {
                        byte[] dword_as_bytes;
                        dword_as_bytes = BitConverter.GetBytes(dwords_to_dev[i]);
                        for (int j = 0; j < 4; j++)
                        {
                            bytes[1 + (i * 4) + j] = dword_as_bytes[j];
                        }
                    }
                    bytes[0] = 1;
                    stream.Write(bytes);

                    commands = 0;

                }
                deviceNeedsUpdating = false;
            }
            
        }

        private void syncAppFromDeviceState()
        {
            appMaxPos = deviceMaxPos;
            appSetPos = deviceSetPos;
            appCurrentPos = deviceCurrentPos;
        }
        //
        // PUBLIC COM INTERFACE IFocuserV3 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            using (SetupDialogForm F = new SetupDialogForm(this, tl))
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // TODO The optional CommandBlind method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBlind must send the supplied command to the mount and return immediately without waiting for a response

            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            // TODO The optional CommandBool method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBool must send the supplied command to the mount, wait for a response and parse this to return a True or False value

            // string retString = CommandString(command, raw); // Send the command and wait for the response
            // bool retBool = XXXXXXXXXXXXX; // Parse the returned string and create a boolean True / False value
            // return retBool; // Return the boolean value to the client

            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // TODO The optional CommandString method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandString must send the supplied command to the mount and wait for a response before returning this to the client

            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            // Clean up the trace logger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        public bool Connected
        {
            get
            {
                LogMessage("Connected", "Get {0}", IsConnected);
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected", "Set {0}", value);
                if (value == IsConnected)
                    return;

   
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "3");
                return Convert.ToInt16("3");
            }
        }

        public string Name
        {
            get
            {
                string name = "Short driver name - please customise";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region IFocuser Implementation


        public bool Absolute
        {
            get
            {
                return true; // This is an absolute focuser
            }
        }

        public void Halt()
        {
            commands |= Constants.Command_Dword_Bits.HALT_MOTOR_BIT;
            deviceNeedsUpdating = true;
        }

        public bool IsMoving
        {
            get
            {
                tl.LogMessage("IsMoving Get", false.ToString());
                return status_flags.HasFlag(Constants.Status_Dword_Bits.STATUS_IS_MOVING_BIT);
            }
        }

        public bool Link
        {
            get
            {
                tl.LogMessage("Link Get", this.Connected.ToString());
                return this.Connected; // Direct function to the connected method, the Link method is just here for backwards compatibility
            }
            set
            {
                tl.LogMessage("Link Set", value.ToString());
                this.Connected = value; // Direct function to the connected method, the Link method is just here for backwards compatibility
            }
        }

        public int MaxIncrement
        {
            get
            {
                return Convert.ToInt32(deviceMaxPos); // Maximum change in one move
            }

            set
            {
                appMaxPos = Convert.ToUInt32(value); // Set the focuser position
                deviceNeedsUpdating = true;
            }
        }

        public int MaxStep
        {
            get
            {
                return Convert.ToInt32(deviceMaxPos);
            }
        }

        public void Move(int Position)
        {
            tl.LogMessage("Move", Position.ToString());
            Console.WriteLine(Position.ToString());

            //usbMut.WaitOne();
            appSetPos = Convert.ToUInt32(Position); // Set the focuser position
            deviceNeedsUpdating = true;
            //usbMut.ReleaseMutex();
        }

        public int Position
        {
            get
            {
                return Convert.ToInt32(deviceCurrentPos); // Return the focuser position
            }
        }

        public double StepSize
        {
            get
            {
                tl.LogMessage("StepSize Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("StepSize", false);
            }
        }

        public bool TempComp
        {
            get
            {
                tl.LogMessage("TempComp Get", false.ToString());
                return false;
            }
            set
            {
                tl.LogMessage("TempComp Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("TempComp", false);
            }
        }

        public bool TempCompAvailable
        {
            get
            {
                tl.LogMessage("TempCompAvailable Get", false.ToString());
                return false; // Temperature compensation is not available in this driver
            }
        }

        public double Temperature
        {
            get
            {
                tl.LogMessage("Temperature Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Temperature", false);
            }
        }

        #endregion

        #region SympleAstro specific information
        
        public string SerialNumber
        {
            get
            {
                return device.GetSerialNumber();
            }
        }

        public void ToggleReverse()
        {

            //usbMut.WaitOne();
            commands |= Constants.Command_Dword_Bits.TOGGLE_REVERSE_BIT;
            deviceNeedsUpdating = true;
            //usbMut.ReleaseMutex();
        }

        public bool ReversedMotor
        {
            get
            {
                return status_flags.HasFlag(Constants.Status_Dword_Bits.STATUS_REVERSE_BIT);
            }
        }

        public void SetZero()
        {
            //usbMut.WaitOne();
            commands |= Constants.Command_Dword_Bits.SET_ZERO_BIT;
            deviceNeedsUpdating = true;
            //usbMut.ReleaseMutex();
            appSetPos = 0;

        }

        public int SG_RESULT
        {
            get
            {
                return Convert.ToInt32((driverStatus & Constants.DRIVER_STATUS_SG_RESULT_MASK) >> Constants.DRIVER_STATUS_SG_RESULT_SHIFT);
            }
        }

        public int CS_ACTUAL
        {
            get
            {
                return Convert.ToInt32((driverStatus & Constants.DRIVER_STATUS_CS_ACTUAL_MASK) >> Constants.DRIVER_STATUS_CS_ACTUAL_SHIFT);
            }
        }
        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Focuser";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Focuser";
                driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());
            }
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }
        #endregion
    }
}
