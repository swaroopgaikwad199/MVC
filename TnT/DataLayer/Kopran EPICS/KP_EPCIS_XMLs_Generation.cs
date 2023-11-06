using EPCIS_XMLs_Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TnT.Models;
using TnT.Models.EPCIS;

namespace TnT.DataLayer.Kopran_EPICS
{
    public class KP_EPCIS_XMLs_Generation
    {
        private string EventTimeZoneOffset = null;
        private ApplicationDbContext db = new ApplicationDbContext();

        #region Header
        /// <summary>
        /// Header for generating PML FILE
        /// </summary>
        /// <param name="path"></param>
        public string GenerateEpcisHeader(string epcisversion)
        {
            StringBuilder writer;

            writer = new StringBuilder();

            writer.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            writer.Append("<epcis:EPCISDocument ");
            writer.Append("xmlns:epcis=\"urn:epcglobal:epcis:xsd:1\"");
            writer.Append(" xmlns:gs1ushc=\"http://epcis.gs1us.org/hc/ns\"");
            writer.Append(" xmlns:sbdh=\"http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader\"");
            writer.Append(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
            writer.Append(" xsi:schemaLocation=\"urn:epcglobal:epcis:xsd:1EPCglobal-epcis-1_1.xsd\"");
            writer.Append(" creationDate=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "\"> schemaVersion=\"" + epcisversion + "\"");
            writer.Append("<EPCISBody>");
            writer.Append("<EventList>");
            string Header = "";
            Header = writer.ToString();
            return Header;
        }
        #endregion


        #region Objectevent
        /// <summary>
        /// Patch for Object event according to Bizstep
        /// </summary>
        /// <param name="_eventType"></param>
        /// <param name="_bizStep"></param>
        /// <param name="_disposition"></param>
        /// <param name="path"></param>
        /// 

        public string GenerateObjectEvent(string evntType, List<string> allDeck, DateTime LastUpdatedDate, DateTime ExpDate, string Lot, string readPoint, string action, string bizStep, string disposition)
        {
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                writer.Append("<" + evntType + ">");
                DateTime evt = Convert.ToDateTime(LastUpdatedDate);
                writer.Append("<eventTime>" + evt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "</eventTime>");
                int day = TimeZoneInfo.Local.BaseUtcOffset.Days;
                int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                int minutes = TimeZoneInfo.Local.BaseUtcOffset.Minutes;
                string localZone = "";
                if (hours > 0)
                    localZone = string.Format("+{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                else if (hours < 0)
                    localZone = string.Format("-{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                this.EventTimeZoneOffset = localZone;

                writer.Append("<eventTimeZoneOffset>" + EventTimeZoneOffset + "</eventTimeZoneOffset>");
                if (evntType == "ObjectEvent")
                {
                    writer.Append("<epcList>");
                    //var epcs = evnt.EpcList.Split(',');
                    foreach (var epc in allDeck)
                    {
                        writer.Append("<epc>" + epc + "</epc>");
                    }
                    writer.Append("</epcList>");
                }

                writer.Append("<action>" + action + "</action>");
                writer.Append("<bizStep>" + bizStep + "</bizStep>");
                writer.Append("<disposition>" + disposition + "</disposition>");
                writer.Append("<readPoint>");
                writer.Append("<id>" + readPoint + "</id>");
                writer.Append("</readPoint>");
                writer.Append("<bizLocation>");
                writer.Append("<id>" + readPoint + "</id>");
                writer.Append("</bizLocation>");

                if (action == "ADD")
                {
                    writer.Append("<extension>");
                    writer.Append("<ilmd>");
                    writer.Append("<gs1ushc:lotNumber>" + Lot + "</gs1ushc:lotNumber>");
                    writer.Append("<gs1ushc:itemExpirationDate>" + ExpDate.Date.ToString("yyyy-MM-dd") + "</gs1ushc:itemExpirationDate>");
                    writer.Append("</ilmd>");
                    writer.Append("</extension>");
                }

                writer.Append("<gs1ushc:eventID>" + Guid.NewGuid() + "</gs1ushc:eventID>");
                writer.Append("</" + evntType + ">");


                string ObjectEvent = "";
                ObjectEvent = writer.ToString();
                return ObjectEvent;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public string GenerateAggregationEvent(string evntType, string parent, List<string> allChilds, DateTime LastUpdatedDate, string readPoint)
        {
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                writer.Append("<" + evntType + ">");
                DateTime evt = Convert.ToDateTime(LastUpdatedDate);
                writer.Append("<eventTime>" + evt.ToString("yyyy-MM-ddTHH:mm:ss.ff") + "</eventTime>");
                int day = TimeZoneInfo.Local.BaseUtcOffset.Days;
                int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                int minutes = TimeZoneInfo.Local.BaseUtcOffset.Minutes;
                string localZone = "";
                if (hours > 0)
                    localZone = string.Format("+{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                else if (hours < 0)
                    localZone = string.Format("-{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                this.EventTimeZoneOffset = localZone;

                writer.Append("<eventTimeZoneOffset>" + EventTimeZoneOffset + "</eventTimeZoneOffset>");

                writer.Append("<parentID>");
                writer.Append(parent);
                writer.Append("</parentID>");
                writer.Append("<childEPCs>");
                //var epcs = evnt.ChildEPC.Split(',');
                foreach (var epc in allChilds)
                {
                    writer.Append("<epc>" + epc + "</epc>");
                }
                writer.Append("</childEPCs>");


                writer.Append("<action>ADD</action>");
                writer.Append("<bizStep>urn:epcglobal:cbv:bizstep:packing</bizStep>");
                writer.Append("<disposition>urn:epcglobal:cbv:disp:in_progress</disposition>");
                writer.Append("<readPoint>");
                writer.Append("<id>" + readPoint + "</id>");
                writer.Append("</readPoint>");
                writer.Append("<bizLocation>");
                writer.Append("<id>" + readPoint + "</id>");
                writer.Append("</bizLocation>");

                writer.Append("<gs1ushc:eventID>" + Guid.NewGuid() + "</gs1ushc:eventID>");
                writer.Append("</" + evntType + ">");


                string ObjectEvent = "";
                ObjectEvent = writer.ToString();
                return ObjectEvent;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string convertToEvent(EpcisEventDetails evnt, string SenderGLN, string BatchNo, DateTime MfgDate, DateTime ExpDate, string NDC)
        {

            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                writer.Append("<" + evnt.EventType + ">");
                DateTime evt = Convert.ToDateTime(evnt.EventTime);
                writer.Append("<eventTime>" + evt.ToString("yyyy-MM-ddTHH:mm:ssZ") + "</eventTime>");
                int day = TimeZoneInfo.Local.BaseUtcOffset.Days;
                int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                int minutes = TimeZoneInfo.Local.BaseUtcOffset.Minutes;
                string localZone = "";
                if (hours > 0)
                    localZone = string.Format("+{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                else if (hours < 0)
                    localZone = string.Format("-{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                this.EventTimeZoneOffset = localZone;

                writer.Append("<eventTimeZoneOffset>" + EventTimeZoneOffset + "</eventTimeZoneOffset>");
                if (evnt.EventType == "ObjectEvent")
                {
                    writer.Append("<epcList>");
                    var epcs = evnt.EpcList.Split(',');
                    foreach (var epc in epcs)
                    {
                        writer.Append("<epc>" + epc + "</epc>");
                    }
                    writer.Append("</epcList>");
                }
                
              

                writer.Append("<action>" + evnt.Action + "</action>");
                writer.Append("<bizStep>urn:epcglobal:cbv:bizstep:" + evnt.BizStep + "</bizStep>");
                writer.Append("<disposition>urn:epcglobal:cbv:disp:" + evnt.Disposition + "</disposition>");
                writer.Append("<readPoint>");
                writer.Append("<id>" + evnt.ReadPoint + "</id>");
                writer.Append("</readPoint>");
             

                if (!string.IsNullOrEmpty(evnt.BizTransactionList))
                {
                    writer.Append("<bizTransactionList>");

                    string[] stringSeparators = new string[] { ",urn:" };
                    var bizTrList = evnt.BizTransactionList.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < bizTrList.Length; i++)
                    {
                        var bizType = bizTrList[i].Split('=');

                        string str = "<bizTransaction type=\"" + ((i > 0) ? "urn:" : "") + bizType[0] + "\">" + bizType[1] + "</bizTransaction>";

                        writer.Append(str);
                    }
                    writer.Append("</bizTransactionList>");


                }

                if (evnt.Action.ToUpper().CompareTo("OBSERVE") == 0 && evnt.EventType == EpcisEnums.EPCISEventType.ObjectEvent.ToString() && evnt.ExtensionData1 != null)
                {
                    EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
                    var receiver = db.M_EPCISReceiver.Where(x => x.CompanyName == evnt.ExtensionData1).FirstOrDefault();
                    //var receiverGLN = epcisConf.GetEPCGLN(receiver.GLN,receiver.Extension);
                    var receiverGLN = "urn:epc:id:sgln:" + receiver.GLN;
                    writer.Append("<extension>");
                    writer.Append("<sourceList>");
                    writer.Append("<source type=\"urn:epcglobal:cbv:sdt:owning_party\">" + receiverGLN + "</source>");
                    writer.Append("	<source type=\"urn:epcglobal:cbv:sdt:location\">" + SenderGLN + "</source>");
                    writer.Append("</sourceList>");
                    writer.Append("<destinationList>");
                    writer.Append("<destination type=\"urn:epcglobal:cbv:sdt:owning_party\">" + receiverGLN + "</destination>");
                    writer.Append("<destination type=\"urn:epcglobal:cbv:sdt:location\">" + receiverGLN + "</destination>");
                    writer.Append("</destinationList>");
                    writer.Append("</extension>");
                }
                writer.Append("<gs1ushc:eventID>" + evnt.UUID + "</gs1ushc:eventID>");
                writer.Append("</" + evnt.EventType + ">");


                string ObjectEvent = "";
                ObjectEvent = writer.ToString();
                return ObjectEvent;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string EndDocument()
        {
            StringBuilder writer;
            writer = new StringBuilder();
            writer.Append("</EventList>");
            writer.Append("</EPCISBody>");
            writer.Append("</epcis:EPCISDocument>");
            string EndDocument = "";
            EndDocument = writer.ToString();
            return EndDocument;
        }
    }
}