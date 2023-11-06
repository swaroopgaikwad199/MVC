using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace REDTR.CODEMGR.UIDMaker
{
    public class CodeReaderXML
    {
        PasSTransactionsColl dataHCL = null;
        public void GetHCL_XMLData(string filePath)
        {
            //string filePath = @"C:\Documents and Settings\GAM\Desktop\fwdxmlserialsfiles\opsm-11-09-02-01-42-58.xml";
            StreamReader streamReader = new StreamReader(filePath);
            string bufferRead = streamReader.ReadToEnd();
            streamReader.Close();

            dataHCL = ParseURL(bufferRead);
        }

        private static PasSTransactionsColl ParseURL(string xmlBuffer)
        {
            string curNodeElement = "";
            if (String.IsNullOrEmpty(xmlBuffer) == true)
            {
                Trace.TraceInformation("RJXMLParser::ParseURL() {0},{1}{2}", DateTime.Now.ToString(), "Input xmlBuffer Error", xmlBuffer);
                return null;
            }
            try
            {
                PasSTransactionsColl data = new PasSTransactionsColl();

                NameTable nt = new NameTable();
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
                nsmgr.AddNamespace(String.Empty, "");

                //Create the XmlParserContext.
                XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);


                //XmlTextReader reader = new XmlTextReader(strUrl);
                XmlTextReader reader = new XmlTextReader(xmlBuffer, XmlNodeType.Element, context);

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            Hashtable attributes = new Hashtable();
                            string strURI = reader.NamespaceURI;
                            string strName = reader.Name;
                            if (reader.HasAttributes)
                            {
                                for (int i = 0; i < reader.AttributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);
                                    attributes.Add(reader.Name, reader.Value);
                                }
                            }
                            // Handle Elment & Attributes here
                            if (String.Compare(strName, "Command", true) == 0)
                            {

                            }

                            curNodeElement = strName;
                            break;

                        //case XmlNodeType.EndElement:
                        //	Todo
                        case XmlNodeType.Text:
                            // Handle Elment Value Here (reader.Value);
                            switch (curNodeElement)
                            {
                                case "LotId":
                                    data.PasSTransactions.BatchDetails.LotId = reader.Value;
                                    break;
                                case "LotNumber":
                                    data.PasSTransactions.BatchDetails.LotNumber = reader.Value;
                                    break;
                                case "ExpDate":
                                    try
                                    {
                                        string dateRcvd = reader.Value;
                                        DateTime dt = Convert.ToDateTime(dateRcvd);
                                        data.PasSTransactions.BatchDetails.ExpDate = dt.ToString("yyMMdd");
                                    }
                                    catch { }
                                    break;
                                case "ProductCode":
                                    data.PasSTransactions.ProductCode = reader.Value;
                                    break;
                                case "ProductUom":
                                    data.PasSTransactions.ProductUom = reader.Value;
                                    break;
                                case "TransactionDate":
                                    data.PasSTransactions.TransactionDate = reader.Value;
                                    break;
                                case "TransactionLocation":
                                    data.PasSTransactions.TransactionLocation = reader.Value;
                                    break;
                                case "TransactionId":
                                    data.PasSTransactions.TransactionId = reader.Value;
                                    break;
                                case "SerialId":
                                    data.PasSTransactions.PasSTransSerialAssocData.Add(new PasSTransSerialAssoc(reader.Value, ""));
                                    break;
                                case "TransSerialAssocId":
                                    int index = data.PasSTransactions.PasSTransSerialAssocData.Count - 1;
                                    if (index >= 0)
                                        data.PasSTransactions.PasSTransSerialAssocData[index].TransSerialAssocId = reader.Value;
                                    break;
                            }

                            //MessageBox.Show(reader.Value);
                            break;

                        default:
                            break;
                    }
                }
                return data;
            }
            catch (XmlException ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
            return null;
        }

        public class PasSTransSerialAssoc
        {
            public string SerialId;
            public bool Result;
            public string ResultDesc;
            public string InspectionDesc;
            public string NxtLevelSerialId;
            public string TransSerialAssocId;
            public PasSTransSerialAssoc()
            {

            }
            public PasSTransSerialAssoc(string oSerialId, string oTransSerialAssocId)
            {
                SerialId = oSerialId;
                TransSerialAssocId = oTransSerialAssocId;
            }
            public PasSTransSerialAssoc(int testCtr, bool result, string resultDesc, string inspectionDesc)
            {
                //SerialId = String.Format("{0:d15}", testCtr);
                //TransSerialAssocId = String.Format("T{0:d14}", testCtr);
                SerialId = testCtr.ToString();
                TransSerialAssocId = (testCtr + 1001).ToString();
                Result = result;
                ResultDesc = resultDesc;
                InspectionDesc = inspectionDesc;
            }
            public PasSTransSerialAssoc(string oSerialId, string oTransSerialAssocId, bool result, string resultDesc, string inspectionDesc)
            {
                //SerialId = String.Format("{0:d15}", testCtr);
                //TransSerialAssocId = String.Format("T{0:d14}", testCtr);
                SerialId = oSerialId;
                TransSerialAssocId = oTransSerialAssocId;
                Result = result;
                ResultDesc = resultDesc;
                inspectionDesc = InspectionDesc;
            }
        }
        public class BatchInfo
        {
            public string LotId;
            public string LotNumber;
            public string MfgDate;
            public string ExpDate;
        }
        public class BatchDetails
        {
            public string ProdName;
            public string GTIN;
            public string GTINTertairy;
            public string BatchID;
            public string LOT;
            public string BatchExpiry;
            public string BatchMFG;
            public int Quantity;
            public Nullable<int> Surplus_uid;
            public Nullable<int> ShipperStartIndex;
        }
        public class Serials
        {
            public string UID;
            public string PackageTypeCode;
            public string Result;
            public string ResultDesc;
            public string DM2DGrades;
            public string InspectionSkiped;
            public string NextLevelCode;
            public string SSCC;
            public string TransSerialAssocId;

            public Serials()
            {

            }
            public Serials(string oSerialId, string oTransSerialAssocId)
            {
                UID = oSerialId;
                TransSerialAssocId = oTransSerialAssocId;
            }
            public Serials(int testCtr, bool isRejected, string resultDesc, string inspectionSkiped, string dM2DGrades)
            {
                //SerialId = String.Format("{0:d15}", testCtr);
                //TransSerialAssocId = String.Format("T{0:d14}", testCtr);
                UID = testCtr.ToString();
                TransSerialAssocId = (testCtr + 1001).ToString();
                if (isRejected == false)
                    Result = "Good";
                else
                    Result = "Bad";
                ResultDesc = resultDesc;
                InspectionSkiped = inspectionSkiped;
                DM2DGrades = dM2DGrades;
            }
            public Serials(string oSerialId, string oTransSerialAssocId, bool isRejected, string resultDesc, string inspectionSkiped, string dM2DGrades)
            {
                //SerialId = String.Format("{0:d15}", testCtr);
                //TransSerialAssocId = String.Format("T{0:d14}", testCtr);
                UID = oSerialId;
                TransSerialAssocId = oTransSerialAssocId;
                if (isRejected == false)
                    Result = "Good";
                else
                    Result = "Bad";

                ResultDesc = resultDesc;
                InspectionSkiped = inspectionSkiped;
                DM2DGrades = dM2DGrades;
            }
        }
        public class PasSTransactions
        {
            public string ProductCode;
            public string ProductUom;
            public string TransactionDate;
            public string TransactionLocation;
            public string TransactionId;
            public BatchInfo BatchDetails;
            public List<PasSTransSerialAssoc> PasSTransSerialAssocData;
            public PasSTransactions()
            {
                BatchDetails = new BatchInfo();
                PasSTransSerialAssocData = new List<PasSTransSerialAssoc>();
            }
        }
        public class PasSTransactionsColl
        {
            public PasSTransactions PasSTransactions;
            public PasSTransactionsColl()
            {
                PasSTransactions = new PasSTransactions();
            }
        }
        internal string GetNextSerialNo()
        {
            if (dataHCL.PasSTransactions.PasSTransSerialAssocData.Count > 0)
            {

                string data = dataHCL.PasSTransactions.PasSTransSerialAssocData[0].SerialId;
                dataHCL.PasSTransactions.PasSTransSerialAssocData.RemoveAt(0);
                //throw new NotImplementedException();
                return data;
            }
            else
                return "";
        }
        public BatchInfo GetHCL_BatchData()
        {
            return dataHCL.PasSTransactions.BatchDetails;
        }
        public PasSTransactions GetHCL_TransDetails()
        {
            return dataHCL.PasSTransactions;
        }
    }
}
