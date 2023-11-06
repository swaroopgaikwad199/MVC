using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RedXML;

namespace REDTR.UTILS
{
    public class LineSettings
    {
        public static List<LineImplementation> Lines = new List<LineImplementation>();

        public static List<LineImplementation> LoadLineSettings()
        {
            if (File.Exists(REDTR.UTILS.SettingsPath.LineConfig) && (!string.IsNullOrEmpty(File.ReadAllText(REDTR.UTILS.SettingsPath.LineConfig))))
            {
                Lines = GenericXmlSerializer<List<LineImplementation>>.Deserialize(REDTR.UTILS.SettingsPath.LineConfig);
            }
            //CreateDefaultRedCamForRC(); //If RC is Not set by user It will pick up default Redcam settings..
            return Lines;
        }

        public static void SaveLineSettings(List<LineImplementation> devLst)
        {
            GenericXmlSerializer<List<LineImplementation>>.Serialize(devLst, REDTR.UTILS.SettingsPath.LineConfig);
        }

    }

    public class LineImplementation
    {
        string _LineName;

        public string LineName
        {
            get { return _LineName; }
            set { _LineName = value; }
        }

        string _LineCode;

        public string LineCode
        {
            get { return _LineCode; }
            set { _LineCode = value; }
        }

        string _Server;

        public string Server
        {
            get { return _Server; }
            set { _Server = value; }
        }

        string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        private string _Database;


        public string Database //Added this property by Arvind.
        {
            get { return _Database; }
            set { _Database = value; }
        }
    }
}
