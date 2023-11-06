using System;
using System.Collections.Generic;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public class SettingsBLL
    {
        public enum SettingsOp
        {
            GETSettingss = 1,
            GETSetting = 2,//[@ID as Setting  ID]      
        }
        private SettingsDAO _SettingsDAO;

        public SettingsDAO SettingsDAO
        {
            get { return _SettingsDAO; }
            set { _SettingsDAO = value; }
        }

        public SettingsBLL()
        {
            SettingsDAO = new SettingsDAO();
        }
        public List<Settings> GetSettingss()
        {
            try
            {
                return SettingsDAO.GetSettingss((int)SettingsOp.GETSettingss);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Settings GetSetting(int SettingID)
        {
            try
            {
                return SettingsDAO.GetSetting((int)SettingsOp.GETSetting, SettingID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddSettings(Settings oSettings)
        {
            try
            {
                return SettingsDAO.AddSettings(oSettings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Settings> DeserializeSettingss(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<Settings>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializeSettingss(string Path, List<Settings> Settingss)
        {
            try
            {
                GenericXmlSerializer<List<Settings>>.Serialize(Settingss, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
