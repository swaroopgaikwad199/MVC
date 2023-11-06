using System;
using System.Collections.Generic;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public class MLNOBLL
    {
        public enum MLNOOp
        {
            GetMLNOs = 1,
            GetMLNO = 2
        }
        private MLNODAO _MLNODAO;

        public MLNODAO MLNODAO
        {
            get { return _MLNODAO; }
            set { _MLNODAO = value; }
        }

        public MLNOBLL()
        {
            MLNODAO = new MLNODAO();
        }
        public List<MLNO> GetMLNOs()
        {
            try
            {
                return MLNODAO.GetMLNOs((int)MLNOOp.GetMLNOs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public MLNO GetMLNO(Decimal ID)
        {
            try
            {
                return MLNODAO.GetMLNO((int)MLNOOp.GetMLNO, ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdateMLNO(MLNO oMLNO)
        {
            try
            {
                return MLNODAO.InsertOrUpdateMLNO(oMLNO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveMLNO(Decimal ID)
        {
            try
            {
                return MLNODAO.RemoveMLNO(ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<MLNO> DeserializeMLNOs(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<MLNO>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializeMLNOs(string Path, List<MLNO> MLNOs)
        {
            try
            {
                GenericXmlSerializer<List<MLNO>>.Serialize(MLNOs, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
