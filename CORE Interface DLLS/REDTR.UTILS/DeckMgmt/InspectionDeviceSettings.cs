using System.Collections.Generic;
using System.IO;
using REDTR.UTILS.SystemIntegrity;
using RedXML;
using System.ComponentModel;

namespace REDTR.UTILS
{
    public class InspectionDevice
    {
        public enum CamType
        {
            IC,
            RC,
            Sec
        }

        public enum InspectionCodeType
        {
            Code2D,
            Code1D
        }

        public enum CustDtFrmtIndex4cam
        {
            DD = 0,            
            MM = 1,
            YY = 2,
            MMM = 3,
            UserData = 4,
            YYYY = 5
        }
        public enum GTINFrmt4Cam
        {
            None = 0,
            GS1 = 1,
            CIP = 2,
            Manual = 3
        }

         // For Symbology Type of extra id codes [Sunil 20.11.2015]
        public   enum SymbologySetting
        {
            Auto = 0,
            Learn = 1,
            Code39 = 2,
            [Description("Code 39")]
            Code128 = 3,
            [Description("Code 128")]
            Code125 = 4,
            [Description("Code 12/5")]
            UPC = 5,
            EAN = 6,
            PharmaCode = 7,
            PostNet = 8,
            Planet = 9,
            RSS14 = 10,
            [Description("RSS-14/Stacked")]
            RSSLtd = 11,
            [Description("RSS Ltd")]
            Codabar = 12,
            Code93 = 13,
            [Description("Code 93")]
            UPU4State = 14,
            [Description("UPU 4State")]
            AUS4State = 15,
            [Description("AUS 4State")]
            JAP4State = 16,
            [Description("JAP 4State")]
            USPS4State = 17,
            [Description("USPS 4State")]
            DataMatrix = 18,
            QR = 19,
            RSSCCA = 20,
            [Description("RSS CC-A")]
            RSSCCB = 21,
            [Description("RSS CC-B")]
            PDF = 22,
        }

        string m_DeviceType;
        public string DeviceType
        {
            get { return m_DeviceType; }
            set { m_DeviceType = value; }
        }
        string m_DeviceName;    
        // This is the Camera Name, set to camera flash. Can be changed using Cognex Device Explorer
        public string DeviceName
        {
            get { return m_DeviceName; }
            set { m_DeviceName = value; }
        }
        string m_Name;  // This is User Defined Device Name for easy Identification
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        CamType m_Camtype;
        public CamType Camtype
        {
            get { return m_Camtype; }
            set { m_Camtype = value; }
        }

        InspectionCodeType m_CodeType;

        public InspectionCodeType CodeType
        {
            get { return m_CodeType; }
            set { m_CodeType = value; }
        }
        DECKs m_Deck;
        public DECKs Deck
        {
            get { return m_Deck; }
            set { m_Deck = value; }
        }
        string m_SerialKey;
        public string SerialKey
        {
            get { return m_SerialKey; }
            set { m_SerialKey = value; }
        }
        string m_Address;
        public string Address
        {
            get { return m_Address; }
            set { m_Address = value; }
        }
        private string m_MacAddress;
        public string MacAddress
        {
            get { return m_MacAddress; }
            set { m_MacAddress = value; }
        }
        private string m_BaseJobName;
        public string BaseJobName
        {
            get { return m_BaseJobName; }
            set { m_BaseJobName = value; }
        }
    }
    public class InspectionDeviceSettings
    {
        public const string BaseJobFileName = "rPISTnT_Main.job";
        public const string BaseJobFileNameRC = "rPISTnT_Main-RC.job";
        public const string DefaultDeviceNotSetTag = "NOT-SET";
        public const string DefaultDeviceIDTag = "CAMERA_";
        public static List<InspectionDevice> devices = new List<InspectionDevice>();

        public static List<InspectionDevice> LoadCameraSettings()
        {
            if (File.Exists(REDTR.UTILS.SettingsPath.InspectionDevice) && (!string.IsNullOrEmpty(File.ReadAllText(REDTR.UTILS.SettingsPath.InspectionDevice))))
            {   
                devices = GenericXmlSerializer<List<InspectionDevice>>.Deserialize(REDTR.UTILS.SettingsPath.InspectionDevice);
            }
            CreateDefaultRedCamForRC(); //If RC is Not set by user It will pick up default Redcam settings..
            return devices;
        }

        public static void SaveDevice(List<InspectionDevice> devLst)
        {
            GenericXmlSerializer<List<InspectionDevice>>.Serialize(devLst, REDTR.UTILS.SettingsPath.InspectionDevice);
        }
        public static void CreateDefaultRedCamForRC()
        {
            foreach (PackBoxesSetup pck in PackBoxes.LstPackBoxes)
            {
                if (pck.Decks.IsFirstDeck())
                    continue;
                InspectionDevice iDevice = InspectionDeviceSettings.devices.Find(item => item.Deck == pck.Decks && item.Camtype == InspectionDevice.CamType.RC);
                if (iDevice == null)
                {
                    iDevice = new InspectionDevice();
                    iDevice.Deck = pck.Decks;
                    iDevice.Camtype = InspectionDevice.CamType.RC;
                    iDevice.DeviceType = cRedDevice;
                    iDevice.CodeType = InspectionDevice.InspectionCodeType.Code2D;
                    iDevice.DeviceName = iDevice.Name = cRedDevice + " " + pck.Decks.ToString();
                    //iDevice.DeviceID = "Camera-" + InspectionDeviceSettings.devices.Count + 1;
                    InspectionDeviceSettings.devices.Add(iDevice);
                }
            }
        }
        public bool CreateDefaultSettings(int CameraCount)
        {
            try
            {
                List<InspectionDevice> lstDevices = new List<InspectionDevice>();
                for (int camCount = 0; camCount < CameraCount; camCount++)
                {
                    InspectionDevice device = new InspectionDevice();
                    device.Name = "Camera-" + (camCount + 1).ToString();
                    device.Camtype = InspectionDevice.CamType.IC;
                    device.Deck = DECKs.PPB;
                    device.DeviceType = device.DeviceName = device.SerialKey = device.Address = device.MacAddress = DefaultDeviceNotSetTag;
                    //iDevice.DeviceID = "Camera-" + InspectionDeviceSettings.devices.Count + 1;
                    device.BaseJobName = "rPISTnT_Main.job";
                    lstDevices.Add(device);
                }
                devices = lstDevices;
                return true;
            }
            catch
            {

            }
            return false;
        }

        public static bool IsExistNode4Deck(DECKs deck)
        {
            int index = -1;
            if (devices != null && devices.Count > 0)
            {
                index = devices.FindIndex(item => item.Deck == deck);
                if (index > -1)
                    return true;
            }
            return false;
        }


        public static int GetCamIndex(DECKs deck, InspectionDevice.CamType type)
        {
            int CamIndex = -1;
            if (devices != null && devices.Count > 0)
                CamIndex = devices.FindIndex(delegate(InspectionDevice item) { return item.Deck == deck && item.Camtype == type; });
            return CamIndex;
        }

        public static bool IsExistNode4DecknCamType(DECKs deck, InspectionDevice.CamType type)
        {
            int index = -1;
            if (devices != null && devices.Count > 0)
            {
                index = devices.FindIndex(delegate(InspectionDevice item) { return item.Deck == deck && item.Camtype == type; });
                if (index > -1)
                    return true;
            }
            return false;
        }
        public static bool Replace_Device(InspectionDevice device)
        {
            int index = -1;
            if (devices == null)
                devices = new List<InspectionDevice>();
            if (devices.Count > 0)
            {
                index = devices.FindIndex(item => item.Deck == device.Deck && item.Camtype == device.Camtype);
            }
            if (index == -1)
            {
                devices.Add(device);
                return true;
            }
            else
            {
                devices[index] = device;
                return true;
            }
        }
        public static bool RemoveNode4deck(DECKs deck, InspectionDevice.CamType camtype)
        {
            int index = -1;
            if (InspectionDeviceSettings.devices != null && InspectionDeviceSettings.devices.Count != 0)
            {
                index = InspectionDeviceSettings.devices.FindIndex(item => item.Deck == deck && item.Camtype == camtype);
                if (index != -1)
                {
                    InspectionDeviceSettings.devices.RemoveAt(index);
                    return true;
                }
            }
            return false;
        }
        public static bool HasDuplicateDevice(List<InspectionDevice> oLstDevices)
        {
            bool hasDuplicate = false;
            for (int i = 0; i < oLstDevices.Count; i++)
            {
                InspectionDevice device = oLstDevices[i];
                if (IsRedSensor(device.DeviceType) == true)
                    continue;
                if (string.IsNullOrEmpty(device.MacAddress) == true)
                    return true;
                for (int j = i + 1; j < oLstDevices.Count; j++)
                {
                    InspectionDevice device2Cmp = oLstDevices[j];
                    if (string.Compare(device.MacAddress, device2Cmp.MacAddress, true) == 0)
                        return true;
                }
            }
            return hasDuplicate;
        }
        public static bool HasInvalidNameDevice(List<InspectionDevice> oLstDevices)
        {
            bool hasInvalidNameDevice = false;
            for (int i = 0; i < oLstDevices.Count; i++)
            {
                InspectionDevice device = oLstDevices[i];
                List<InspectionDevice> LstDev = oLstDevices.FindAll(delegate(InspectionDevice item) { return item.Name == device.Name || item.DeviceName == device.DeviceName;});
                if (LstDev != null && LstDev.Count > 1)
                    hasInvalidNameDevice = true;
            }
            return hasInvalidNameDevice;
        }

        public static List<InspectionDevice> GEtDevicesOfdeck(DECKs deck)
        {
            List<InspectionDevice> LstDevices = null;
            if (devices != null)
                LstDevices = devices.FindAll(delegate(InspectionDevice o) { return o.Deck == deck; });
            return LstDevices;
        }
        public static InspectionDevice GEtCamDevice(DECKs deck, InspectionDevice.CamType type)
        {
            InspectionDevice iDevice = null;

            if (devices != null)
                iDevice = devices.Find(item => item.Deck == deck && item.Camtype == type);
            return iDevice;
        }
        public static InspectionDevice GEtCamDevice(string DeviceName)
        {
            InspectionDevice iDevice = null;

            if (devices != null)
                iDevice = devices.Find(item => item.DeviceName == DeviceName);
            return iDevice;
        }
        public static InspectionDevice.CamType[] GetCamTypesOfDeck(DECKs deck)
        {
            InspectionDevice.CamType[] types;

            List<InspectionDevice> Ldevice = InspectionDeviceSettings.GEtDevicesOfdeck(deck);
            types = new InspectionDevice.CamType[Ldevice.Count];

            for (int i = 0; i < Ldevice.Count; i++)
            {
                types[i] = Ldevice[i].Camtype;
            }
            return types;
        }

        public static InspectionDevice.InspectionCodeType GetInspCodeType(InspectionDevice.CamType camType, DECKs deck)
        {
            if (devices != null)
            {
                InspectionDevice iDevice = devices.Find(item => item.Deck == deck && item.Camtype == camType);
                if (iDevice != null)
                    return iDevice.CodeType;
            }
            return InspectionDevice.InspectionCodeType.Code2D;
        }

        private const string cDmDevice = "DataMan";
        private const string cRedDevice = "RedCam";
        private const string cInsightDevice = "InSight";

        public static bool IsDMSensor(string deviceType)
        {
            if (deviceType == null)
                return false;
            if (deviceType.ToUpper().Contains(cDmDevice.ToUpper()))
                return true;
            else
                return false;
        }
        public static bool IsRedSensor(string deviceType)
        {
            if (deviceType == null)
                return false;
            if (deviceType.ToUpper().Contains(cRedDevice.ToUpper()))
                return true;
            else
                return false;
        }
        public static bool IsInsightSensor(string deviceType)
        {
            if (deviceType == null)
                return false;
            if (deviceType.ToUpper().Contains(cInsightDevice.ToUpper()))
                return true;
            else
                return false;
        }
 
    }
}
