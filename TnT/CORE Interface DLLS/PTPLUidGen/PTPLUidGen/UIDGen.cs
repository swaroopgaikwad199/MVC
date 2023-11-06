using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

namespace PTPLUidGen
{
    public  class UIDGen
    {
        public string m_UnitID = "0";
        private int m_LineID = 0;
        public string m_CompanyID = "0";
        private int m_ProductType = 1;
        string alphanumericCharacters ="ABCDEFGHIJKLMNPQRSTUVWXYZ0123456789";

        public void InitUIDGen(int prodType, string AppStartPath)
        {
            rgmyekcil.UID_Info uidInfo = CheckLic_GetUIDInfo(AppStartPath);
            System.Threading.Thread.Sleep(98);
            GetCodeSetupInfo(uidInfo.CompanyID, uidInfo.UnitID, uidInfo.LineID);
            alphanumericCharacters += m_CompanyID.PadLeft(4, '0') + m_UnitID.ToString().PadLeft(2, '0') + m_LineID.ToString().PadLeft(2, '0');
            m_ProductType = prodType;
        }

        private rgmyekcil.UID_Info CheckLic_GetUIDInfo(string AppStartPath, int val)
        {
            rgmyekcil.rgmyekcilDecode keyGen = new rgmyekcil.rgmyekcilDecode();
            string keyFile = rgmyekcil.LicFiles.GetLICFile(AppStartPath);
            bool haslic = keyGen.DecodeLic(keyFile);
            rgmyekcil.UID_Info uidInfo = keyGen.GetUIDInfo();
            return uidInfo;
        }

        private rgmyekcil.UID_Info CheckLic_GetUIDInfo(string AppStartPath)
        {
            rgmyekcil.rgmyekcilDecode keyGen = new rgmyekcil.rgmyekcilDecode();
            string keyFile = rgmyekcil.LicFiles.GetLICFile(AppStartPath);
            bool haslic = keyGen.DecodeLic(keyFile);
            bool HasValidLic = false;
            if (haslic == true)
            {
                bool opt1 = keyGen.HasValidSystem();
                bool opt2 = keyGen.HasExecValidity();

                if (opt1 == true && opt2 == true)
                {
                    HasValidLic = true;
                }
                else
                    Trace.TraceInformation("SystemIntigrityError @ InitUIDGen {0},{1},{2}", DateTime.Now, opt1, opt2);
            }
            Trace.Assert(HasValidLic, "FAILED TO INITIALIZE THE SYSTEM.", "CRITICAL FAILURE");
            if (HasValidLic == false)
            {
                Trace.TraceWarning("SystemIntigrityError @ InitUIDGen, {0},{1},{2},", AppStartPath, haslic, HasValidLic);
                Process proc = Process.GetCurrentProcess();
                proc.Kill();
                return null;
            }

            rgmyekcil.UID_Info uidInfo = keyGen.GetUIDInfo();
            return uidInfo;
        }
        private void GetCodeSetupInfo(int companyID, int unitID, int lineID)
        {
            //if (companyID < 500)
            //    m_CompanyID = companyID;
            //else
            //    //System.Windows.Forms.MessageBox.Show("INVALID COMPANY ID : " + companyID + "\r\n\r\nContact Software Administrator! Usage Not Recomonded");
            //    throw new OverflowException("INVALID COMPANY ID : " + companyID + "\r\n\r\nContact Software Administrator! Usage Not Recomonded");

            //if (unitID < 100)
            //    m_UnitID = unitID;
            //else
            //    throw new OverflowException("INVALID UNIT ID : " + unitID + "\r\n\r\nContact Software Administrator! Usage Not Recomonded");

            if (lineID < 100)
                m_LineID = lineID;
            else
                throw new OverflowException("INVALID LINE ID : " + lineID + "\r\n\r\nContact Software Administrator! Usage Not Recomonded");
        }

        public string GenerateUID(int UIDLength)
        {
            return GetRandomString(UIDLength, alphanumericCharacters);
        }

        private string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            var characterArray = characterSet.Distinct().ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", "characterSet");

            var bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
            return new string(result);
        }
    }

}
