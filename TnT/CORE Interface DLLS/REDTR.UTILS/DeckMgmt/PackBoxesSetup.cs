using System;
using System.Collections.Generic;
using System.IO;
using REDTR.UTILS.SystemIntegrity;
using RedXML;

namespace REDTR.UTILS
{
    public class PackBoxesSetup
    {
        private DECKs m_Decks;
        public DECKs Decks
        {
            get { return m_Decks; }
            set { m_Decks = value; }
        }
        private string m_DisplayName;
        public string DisplayName
        {
            get { return m_DisplayName; }
            set { m_DisplayName = value; }
        }
        private bool m_IsExist;
        public bool IsExist
        {
            get { return m_IsExist; }
            set { m_IsExist = value; }
        }
        private bool m_IsPrimary;
        public bool IsPrimary
        {
            get { return m_IsPrimary; }
            set { m_IsPrimary = value; }
        }
        private bool m_IsTertiary;
        public bool IsTertiary
        {
            get { return m_IsTertiary; }
            set { m_IsTertiary = value; }
        }
        private int m_SSCCPI;
        public int SSCCPI
        {
            get { return m_SSCCPI; }
            set { m_SSCCPI = value; }
        }
        private bool m_Reprint;
        public bool Reprint
        {
            get { return m_Reprint; }
            set { m_Reprint = value; }
        }
        private bool m_DeckToSet;
        public bool DeckToSet
        {
            get { return m_DeckToSet; }
            set { m_DeckToSet = value; }
        }
    }
    public class PackBoxes
    {
        public static List<PackBoxesSetup> LstPackBoxes = new List<PackBoxesSetup>();

        public void LoadPackBoxesSetup() //For Current Activated File
        {
            if (File.Exists(SettingsPath.PackBoxesSetup))
            {
                LstPackBoxes = GenericXmlSerializer<List<PackBoxesSetup>>.Deserialize(SettingsPath.PackBoxesSetup);
            }
            else
            {
                SaveInfo(SettingsPath.PackBoxesSetup);
            }
            LstPackBoxes = LstPackBoxes.FindAll(item => item.IsExist == true);
        }

        private void SaveInfo(string FilePath)
        {
            PackBoxesSetup FldSetup = new PackBoxesSetup();
            FldSetup.Decks = DECKs.PPB;
            FldSetup.DisplayName = "PrimaryPCK";
            FldSetup.IsExist = true;
            FldSetup.IsPrimary = true;
            FldSetup.IsTertiary = false;
            FldSetup.SSCCPI = 3;
            FldSetup.Reprint = false;
            FldSetup.DeckToSet = false;
            LstPackBoxes.Add(FldSetup);

            FldSetup = new PackBoxesSetup();
            FldSetup.Decks = DECKs.MOC;
            FldSetup.DisplayName = "MonoCARTON";
            FldSetup.IsExist = false;
            FldSetup.IsPrimary = false;
            FldSetup.IsTertiary = false;
            FldSetup.SSCCPI = 3;
            FldSetup.Reprint = false;
            FldSetup.DeckToSet = false;
            LstPackBoxes.Add(FldSetup);

            FldSetup = new PackBoxesSetup();
            FldSetup.Decks = DECKs.OBX;
            FldSetup.DisplayName = "OuterBOX";
            FldSetup.IsExist = false;
            FldSetup.IsPrimary = false;
            FldSetup.IsTertiary = false;
            FldSetup.SSCCPI = 3;
            FldSetup.Reprint = false;
            FldSetup.DeckToSet = false;
            LstPackBoxes.Add(FldSetup);

            FldSetup = new PackBoxesSetup();
            FldSetup.Decks = DECKs.ISH;
            FldSetup.DisplayName = "SHIPPER";
            FldSetup.IsExist = false;
            FldSetup.IsPrimary = false;
            FldSetup.IsTertiary = true;
            FldSetup.SSCCPI = 3;
            FldSetup.Reprint = true;
            FldSetup.DeckToSet = false;
            LstPackBoxes.Add(FldSetup);

            FldSetup = new PackBoxesSetup();
            FldSetup.Decks = DECKs.OSH;
            FldSetup.DisplayName = "OuterSHIPPER";
            FldSetup.IsExist = false;
            FldSetup.IsPrimary = false;
            FldSetup.IsTertiary = true;
            FldSetup.SSCCPI = 3;
            FldSetup.Reprint = true;
            FldSetup.DeckToSet = false;
            LstPackBoxes.Add(FldSetup);

            FldSetup = new PackBoxesSetup();
            FldSetup.Decks = DECKs.PAL;
            FldSetup.DisplayName = "PALLET";
            FldSetup.IsExist = false;
            FldSetup.IsPrimary = false;
            FldSetup.IsTertiary = true;
            FldSetup.SSCCPI = 3;
            FldSetup.Reprint = true;
            FldSetup.DeckToSet = false;
            LstPackBoxes.Add(FldSetup);

            /////***********************************************

            if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(FilePath)) == false)
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(FilePath));
            }
            GenericXmlSerializer<List<PackBoxesSetup>>.Serialize(LstPackBoxes, FilePath);
        }
        public static PackBoxesSetup GetPackBox(DECKs Dck)
        {
            PackBoxesSetup packBoxes = new PackBoxesSetup();
            if (LstPackBoxes != null)
            {
                packBoxes = LstPackBoxes.Find(delegate(PackBoxesSetup o) { return o.Decks == Dck; });
            }
            return packBoxes;
        }
        public static DECKs GET_FirstDeck()
        {
            if (LstPackBoxes.Count > 0)
                return LstPackBoxes[0].Decks;
            return DECKs.None;
        }
        public static DECKs GET_TertiaryDeck()  //considered only one tertiary deck at a time.
        {
            DECKs mDeck = DECKs.None;
            PackBoxesSetup pckSetup = LstPackBoxes.Find(delegate(PackBoxesSetup o) { return o.IsTertiary == true; });
            if (pckSetup != null)
                mDeck = pckSetup.Decks;
            return mDeck;
        }

        public static DECKs GET_CurrentDeck()  //considered only one seperate tertiary deck at a time.
        {
            DECKs mDeck = DECKs.None;
            PackBoxesSetup pckSetup = LstPackBoxes.Find(delegate(PackBoxesSetup o) { return o.IsTertiary == true && o.DeckToSet == true; });
            if (pckSetup != null)
                mDeck = pckSetup.Decks;
            return mDeck;
        }
        public static DECKs GET_DisplaySetDeckType()  //considered only one seperate tertiary deck at a time.
        {
            DECKs mDeck = DECKs.None;
            PackBoxesSetup pckSetup = LstPackBoxes.Find(delegate(PackBoxesSetup o) { return o.IsExist == true && o.DeckToSet == true; });
            if (pckSetup != null)
                mDeck = pckSetup.Decks;
            return mDeck;
        }
        public static string GET_DisplaySetDeck()  //considered only one seperate tertiary deck at a time.
        {
            string mDeckDisplayName = string.Empty;
            PackBoxesSetup pckSetup = LstPackBoxes.Find(delegate(PackBoxesSetup o) { return o.IsExist==true && o.DeckToSet == true; });
            if (pckSetup != null)
                mDeckDisplayName = pckSetup.DisplayName;
            return mDeckDisplayName;
        }
        public static bool IsLastDeck(DECKs deck)
        {
            int Index = LstPackBoxes.FindIndex(item => item.Decks == deck);
            if (Index == LstPackBoxes.Count - 1)
                return true;
            else
                return false;
        }
        public static DECKs GET_DECKCode(string DECKDispName)
        {
            DECKs mDeck = DECKs.None;
            PackBoxesSetup pckSetup = LstPackBoxes.Find(delegate(PackBoxesSetup o) { return o.DisplayName == DECKDispName; });
            if (pckSetup != null)
                mDeck = pckSetup.Decks;
            return mDeck;
        }  
    }
    public static class ExMethodsPackBoxes
    {
        public static string DisplayName(this DECKs deck)
        {
            string mDispName = string.Empty;
            PackBoxesSetup pckSetup = PackBoxes.LstPackBoxes.Find(delegate(PackBoxesSetup o) { return o.Decks == deck; });
            if (pckSetup != null)
                mDispName = pckSetup.DisplayName;
            return mDispName;
        }
        public static bool IsTertiaryDeck(this DECKs mDeck)  //considered only one tertiary deck at a time.
        {
            PackBoxesSetup pckSetup = PackBoxes.LstPackBoxes.Find(delegate(PackBoxesSetup o) { return o.Decks == mDeck; });
            if (pckSetup != null)
                return pckSetup.IsTertiary;
            else
                return false;
        }
        public static bool IsFirstDeck(this DECKs mDeck)
        {
            PackBoxesSetup pckSetup = PackBoxes.LstPackBoxes.Find(delegate(PackBoxesSetup o) { return o.Decks == mDeck; });
            if (pckSetup != null)
                return pckSetup.IsPrimary;
            else
                return false;
        }
        //public static bool IsFirstDeck(this DECKs mDeck)
        //{
        //    if (PackBoxes.LstPackBoxes.Count > 0 && PackBoxes.LstPackBoxes[0].Decks == mDeck)
        //        return true;
        //    else
        //        return false;
        //}
        public static bool IsExists(this DECKs mDeck)
        {
            PackBoxesSetup pckSetup = PackBoxes.LstPackBoxes.Find(item => item.Decks == mDeck);
            if (pckSetup != null)
                return true;
            else
                return false;
        }
        public static bool IsDeckToSet(this DECKs mDeck)
        {
            PackBoxesSetup pckSetup = PackBoxes.LstPackBoxes.Find(item => item.Decks == mDeck && item.DeckToSet==true);
            if (pckSetup != null)
                return true;
            else
                return false;
        }
        public static int index(this DECKs mDeck)
        {
            return PackBoxes.LstPackBoxes.FindIndex(item => item.Decks == mDeck);
        }
        public static DECKs ParseToDeck(this string DECKCode)
        {
            DECKs mDeck = DECKs.None;
            try
            {
                mDeck = (DECKs)Enum.Parse(typeof(DECKs), DECKCode);
            }
            catch (Exception)
            {
                mDeck = DECKs.None;
            }
            return mDeck;
        }
        public static int SSCCPI(this DECKs deck)
        {
            PackBoxesSetup Pck = PackBoxes.GetPackBox(deck);
            if (Pck.SSCCPI == 0)
                Pck.SSCCPI = 3;
            return Pck.SSCCPI;
        }
        public static DECKs PrevDeck(this DECKs deck)
        {
            DECKs Prevdeck = DECKs.None;
            int index = PackBoxes.LstPackBoxes.FindIndex(delegate(PackBoxesSetup item) { return item.Decks == deck; });
            if (index > 0)
                Prevdeck = PackBoxes.LstPackBoxes[--index].Decks;
            return Prevdeck;
        }
        public static DECKs NExtDeck(this DECKs deck)
        {
            DECKs nextdeck = DECKs.None;
            int index = PackBoxes.LstPackBoxes.FindIndex(delegate(PackBoxesSetup item) { return item.Decks == deck; });
            if (index > -1 && index + 1 < PackBoxes.LstPackBoxes.Count)
                nextdeck = PackBoxes.LstPackBoxes[++index].Decks;
            return nextdeck;
        }
    }
}
