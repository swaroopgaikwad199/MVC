using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.DataLayer
{

    
    public class UIDVerifier
    {

        #region private vaialbles

        private List<string> m_CodesGen = new List<string>();
        private List<string> List0 = new List<string>();
        private List<string> List1 = new List<string>();
        private List<string> List2 = new List<string>();
        private List<string> List3 = new List<string>();
        private List<string> List4 = new List<string>();
        private List<string> List5 = new List<string>();
        private List<string> List6 = new List<string>();
        private List<string> List7 = new List<string>();
        private List<string> List8 = new List<string>();
        private List<string> List9 = new List<string>();
        private List<string> ListA = new List<string>();
        private List<string> ListB = new List<string>();
        private List<string> ListC = new List<string>();
        private List<string> ListD = new List<string>();
        private List<string> ListE = new List<string>();
        private List<string> ListF = new List<string>();
        private List<string> ListG = new List<string>();
        private List<string> ListH = new List<string>();
        private List<string> ListI = new List<string>();
        private List<string> ListJ = new List<string>();
        private List<string> ListK = new List<string>();
        private List<string> ListL = new List<string>();
        private List<string> ListM = new List<string>();
        private List<string> ListN = new List<string>();
        private List<string> ListP = new List<string>();
        private List<string> ListQ = new List<string>();
        private List<string> ListR = new List<string>();
        private List<string> ListS = new List<string>();
        private List<string> ListT = new List<string>();
        private List<string> ListU = new List<string>();
        private List<string> ListV = new List<string>();
        private List<string> ListW = new List<string>();
        private List<string> ListX = new List<string>();
        private List<string> ListY = new List<string>();
        private List<string> ListZ = new List<string>();


        #endregion

        public List<string> getUniqueIds()
        {
            return m_CodesGen;

        }
        public bool AddCodeGen(string Uid)
        {
            try
            {
                bool bIsRepeate = true;
                switch (Uid.Substring(0, 1))
                {
                    case "0":
                        if (!List0.Contains(Uid))
                        {
                            List0.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "1":
                        if (!List1.Contains(Uid))
                        {
                            List1.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "2":
                        if (!List2.Contains(Uid))
                        {
                            List2.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "3":
                        if (!List3.Contains(Uid))
                        {
                            List3.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "4":
                        if (!List4.Contains(Uid))
                        {
                            List4.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "5":
                        if (!List5.Contains(Uid))
                        {
                            List5.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "6":
                        if (!List6.Contains(Uid))
                        {
                            List6.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "7":
                        if (!List7.Contains(Uid))
                        {
                            List7.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "8":
                        if (!List8.Contains(Uid))
                        {
                            List8.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "9":
                        if (!List9.Contains(Uid))
                        {
                            List9.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "A":
                        if (!ListA.Contains(Uid))
                        {
                            ListA.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "B":
                        if (!ListB.Contains(Uid))
                        {
                            ListB.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "C":
                        if (!ListC.Contains(Uid))
                        {
                            ListC.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "D":
                        if (!ListD.Contains(Uid))
                        {
                            ListD.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "E":
                        if (!ListE.Contains(Uid))
                        {
                            ListE.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "F":
                        if (!ListF.Contains(Uid))
                        {
                            ListF.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "G":
                        if (!ListG.Contains(Uid))
                        {
                            ListG.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "H":
                        if (!ListH.Contains(Uid))
                        {
                            ListH.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "I":
                        if (!ListI.Contains(Uid))
                        {
                            ListI.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "J":
                        if (!ListJ.Contains(Uid))
                        {
                            ListJ.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "K":
                        if (!ListK.Contains(Uid))
                        {
                            ListK.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "L":
                        if (!ListL.Contains(Uid))
                        {
                            ListL.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "M":
                        if (!ListM.Contains(Uid))
                        {
                            ListM.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "N":
                        if (!ListN.Contains(Uid))
                        {
                            ListN.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "P":
                        if (!ListP.Contains(Uid))
                        {
                            ListP.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "Q":
                        if (!ListQ.Contains(Uid))
                        {
                            ListQ.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "R":
                        if (!ListR.Contains(Uid))
                        {
                            ListR.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "S":
                        if (!ListS.Contains(Uid))
                        {
                            ListS.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            // //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "T":
                        if (!ListT.Contains(Uid))
                        {
                            ListT.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "U":
                        if (!ListU.Contains(Uid))
                        {
                            ListU.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "V":
                        if (!ListV.Contains(Uid))
                        {
                            ListV.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "W":
                        if (!ListW.Contains(Uid))
                        {
                            ListW.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "X":
                        if (!ListX.Contains(Uid))
                        {
                            ListX.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "Y":
                        if (!ListY.Contains(Uid))
                        {
                            ListY.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            ////LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                    case "Z":
                        if (!ListZ.Contains(Uid))
                        {
                            ListZ.Add(Uid);
                            m_CodesGen.Add(Uid);

                        }
                        else
                        {
                            bIsRepeate = false;
                            //LogWriter(" Duplicate UID Generated :" + Uid);
                        }
                        break;
                }
                return bIsRepeate;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

    }
}