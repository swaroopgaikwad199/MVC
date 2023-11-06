using System;
using REDTR.HELPER;

namespace REDTR.CODEMGR
{
    public class SSCCMgr1
    {
        int m_PI = 3; // Default Value By GS1
        string m_GS1CompCode = null;
        string m_LineCode = null;
        int LastSSCC = 0;

        public enum SSCCIncreamentMode
        {
            PreFetch,
            PostFetch
        }
        private DbHelper m_DbHelper = new DbHelper();

        public SSCCMgr1(int pi)
        {
            if (pi < 0 || pi > 9) pi = 3;
            m_PI = pi;
            m_GS1CompCode = m_DbHelper.GetGS1CompCode();  //"8901107";
            m_LineCode = m_DbHelper.GetLineCode();  //"01";
            if (m_LineCode.Length < 2)
            {
                m_LineCode = m_LineCode.PadLeft(2, '0');
            }
            LastSSCC = m_DbHelper.GetLastSSCC(m_PI);
        }
        public int GetPI()
        {
            return m_PI;
        }

        public string GetNextSSCC(SSCCIncreamentMode ssccIncrementMode)
        {
            string SSCC = "";

            int rmnSSCCLen = 17 - (m_PI.ToString().Length + m_GS1CompCode.Length + m_LineCode.Length);

            SSCC = String.Format("{0}{1}{2}", m_PI, m_GS1CompCode, m_LineCode);

            if (ssccIncrementMode == SSCCIncreamentMode.PostFetch)
            {
                SSCC = SSCC + (LastSSCC + 1).ToString().PadLeft(rmnSSCCLen, '0');
            }
            else
            {
                // LastSSCC will be treated as current SSCC to use directly. So system should increament it before sending to Print
                SSCC = SSCC + (LastSSCC).ToString().PadLeft(rmnSSCCLen, '0');
            }

            int checksum = GS1Mgr.GetGS1CheckSum(SSCC);
            SSCC += checksum.ToString();
            //Trace.TraceInformation("{0},GetNextSSCC LastSSCC#{1},SSCC{2}", DateTime.Now.ToString(), LastSSCC, SSCC);
            return SSCC;
        }
        public bool SetNextSSCC()
        {
            bool res = false;
            int curSSCC = LastSSCC + 1;
            m_DbHelper.SetLastSSCC(m_PI, curSSCC);
            LastSSCC = curSSCC;
          
            //Trace.TraceInformation("{0},SetNextSSCC2DB LastSSCC#{1}", DateTime.Now.ToString(), LastSSCC);
            return res;
        }

        public static string GetTestSSCC(int PI) //internal
        {
            string SSCC = "";

            DbHelper oDbHelper = new DbHelper();
            string GS1CompCode = oDbHelper.GetGS1CompCode();  //"8901107";
            string LineCode = oDbHelper.GetLineCode();  //"001";

            int rmnSSCCLen = 17 - (PI.ToString().Length + GS1CompCode.Length + LineCode.Length);

            SSCC = String.Format("{0}{1}{2}", PI, GS1CompCode, LineCode);

            SSCC = SSCC + (0).ToString().PadLeft(rmnSSCCLen, '0');

            int checksum = GS1Mgr.GetGS1CheckSum(SSCC);
            SSCC += checksum.ToString();
            return SSCC;
        }
    }

    public class DavaRunningSeqNoMgr1
    {
          int m_PI = 1; // Default Value for Running Seq Id
          int LastRunnningSeqNo = 0;
          private DbHelper m_DbHelper = new DbHelper();

          public DavaRunningSeqNoMgr1()
          {
              LastRunnningSeqNo = m_DbHelper.GetLastDavaFileRunningSeqNo(m_PI);

          }
        
        public bool SetNextRunnningSeqNo()
        {
            bool res = false;
               int curSSCC=0;
            if (LastRunnningSeqNo == 999) {  curSSCC =1; }
            else
            { curSSCC = LastRunnningSeqNo + 1;}
            m_DbHelper.SetLastDavaFileRunningSeqNo(curSSCC);
            LastRunnningSeqNo = curSSCC;             
            //Trace.TraceInformation("{0},SetNextSSCC2DB LastSSCC#{1}", DateTime.Now.ToString(), LastSSCC);
            return res;
        }

        public int GetNextRunnningSeqNo(REDTR.CODEMGR.SSCCMgr.SSCCIncreamentMode ssccIncrementMode)
        {
            int RunnningSeqNo = 0;

            if (ssccIncrementMode == REDTR.CODEMGR.SSCCMgr.SSCCIncreamentMode.PostFetch)
            {
                RunnningSeqNo = RunnningSeqNo + (LastRunnningSeqNo + 1);
            }
            else
            {
                // LastRunnningSeqNo will be treated as current RunnningSeqNo to use directly. So system should increament it before sending to Print
                RunnningSeqNo = RunnningSeqNo + (LastRunnningSeqNo);
            }


            return RunnningSeqNo;
        }
    
    }

}
