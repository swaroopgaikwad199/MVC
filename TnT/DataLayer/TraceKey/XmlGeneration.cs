using EPCIS_XMLs_Generation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using TnT.Models;
using TnT.Models.EPCIS;
using TnT.Models.Job;
using System.Data;
using TnT.Models.Code;
using System.IO;

namespace TnT.DataLayer.TraceKey
{
    public class XmlGeneration
    {
        private string EventTimeZoneOffset = null;
        private ApplicationDbContext db = new ApplicationDbContext();
        public string convertToEventOld(EpcisEventDetails evnt, string SenderGLN)
        {
            StringBuilder writer;
            try
            {
                int day = TimeZoneInfo.Local.BaseUtcOffset.Days;
                int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                int minutes = TimeZoneInfo.Local.BaseUtcOffset.Minutes;
                string localZone = "";
                if (hours > 0)
                    localZone = string.Format("+{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                else if (hours < 0)
                    localZone = string.Format("-{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                this.EventTimeZoneOffset = localZone;
                writer = new StringBuilder();
                writer.Append("<" + evnt.EventType + ">");
                DateTime evt = Convert.ToDateTime(evnt.EventTime);
                writer.Append("<eventTime>" + evt.ToString("yyyy-MM-ddTHH:mm:ss.fff") + EventTimeZoneOffset + "</eventTime>");


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
                else if (evnt.EventType == EpcisEnums.EPCISEventType.AggregationEvent.ToString())
                {
                    writer.Append("<parentID>");
                    writer.Append(evnt.ParentID);
                    writer.Append("</parentID>");
                    writer.Append("<childEPCs>");
                    var epcs = evnt.ChildEPC.Split(',');
                    foreach (var epc in epcs)
                    {
                        writer.Append("<epc>" + epc + "</epc>");
                    }
                    writer.Append("</childEPCs>");
                }
                writer.Append("<action>" + evnt.Action + "</action>");
                writer.Append("<bizStep>urn:epcglobal:cbv:bizstep:" + evnt.BizStep + "</bizStep>");
                writer.Append("<disposition>urn:epcglobal:cbv:disp:" + evnt.Disposition + "</disposition>");

                if (evnt.Action.ToUpper().CompareTo("OBSERVE") == 0 && evnt.EventType == EpcisEnums.EPCISEventType.ObjectEvent.ToString() && evnt.ExtensionData1 != null)
                {
                    EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
                    var setting = db.Settings.FirstOrDefault();
                    var receiver = db.M_EPCISReceiver.Where(x => x.CompanyName == evnt.ExtensionData1).FirstOrDefault();
                    string receiverGLN = "";
                    if (receiver != null)
                    {
                        receiverGLN = epcisConf.GetEPCGLN(receiver.GLN, receiver.Extension, setting.CompanyCode.Length);
                    }

                    writer.Append("<extension>");
                    writer.Append("<sourceList>");
                    writer.Append("<source type=\"urn: epcglobal:cbv: sdt:owning_party\">" + receiverGLN + "</source>");
                    writer.Append("	<source type=\"urn: epcglobal:cbv: sdt:location\">" + SenderGLN + "</source>");
                    writer.Append("</sourceList>");
                    writer.Append("<destinationList>");
                    writer.Append("<destination type=\"urn: epcglobal:cbv: sdt:owning_party\">" + receiverGLN + "</destination>");
                    writer.Append("<destination type=\"urn: epcglobal:cbv: sdt:location\">" + receiverGLN + "</destination>");
                    writer.Append("</destinationList>");
                    writer.Append("</extension>");
                }

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

        public string GenerateEPCISHeader(Job jb, string SenderGLN, string ReceiverGLN, string TypeVersion, string ProductionDocument, string gtin)
        {
            int day = TimeZoneInfo.Local.BaseUtcOffset.Days;
            int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
            int minutes = TimeZoneInfo.Local.BaseUtcOffset.Minutes;
            string localZone = "";
            if (hours > 0)
                localZone = string.Format("+{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
            else if (hours < 0)
                localZone = string.Format("-{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
            this.EventTimeZoneOffset = localZone;
            StringBuilder writer;

            writer = new StringBuilder();
            writer.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            writer.Append("<gs1:EPCISDocument xmlns:cbvmda=\"urn:epcglobal:cbv:mda\"");
            writer.Append(" xmlns:gs1=\"urn:epcglobal:epcis:xsd:1\" xmlns:tk=\"urn:tracekey:extension:xsd\"");
            writer.Append(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
            writer.Append(" xmlns:sbdh=\"http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader\"");
            writer.Append(" creationDate =\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff") + EventTimeZoneOffset + "\" schemaVersion=\"1.2\">");
            writer.Append("<EPCISHeader>");
            writer.Append("<StandardBusinessDocumentHeader");
            writer.Append(" xmlns=\"http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader\"> ");
            writer.Append("<HeaderVersion/>");
            writer.Append("<Sender>");
            writer.Append("<Identifier Authority=\"SGLN\">" + SenderGLN + "</Identifier>");
            writer.Append("</Sender>");
            writer.Append("<Receiver>");
            writer.Append("<Identifier Authority=\"SGLN\">" + ReceiverGLN + "</Identifier>");
            writer.Append("</Receiver>");
            writer.Append("<DocumentIdentification>");
            writer.Append("<Standard>EPCIS Standard with tracekey extension</Standard>");
            writer.Append("<TypeVersion>" + TypeVersion + "</TypeVersion>");
            writer.Append("<InstanceIdentifier>0987654321</InstanceIdentifier>");
            writer.Append("<Type>" + ProductionDocument + "</Type>");
            writer.Append("<CreationDateAndTime>" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff") + EventTimeZoneOffset + "</CreationDateAndTime>");
            writer.Append("</DocumentIdentification>");
            writer.Append("</StandardBusinessDocumentHeader>");
            writer.Append("<extension>");
            writer.Append("<EPCISMasterData>");
            writer.Append("<VocabularyList>");
            writer.Append("<Vocabulary type=\"urn:epcglobal:epcis:vtype:tk:Vocabulary\">");
            writer.Append("<VocabularyElementList>");
            writer.Append("<VocabularyElement id=\"tk:Upload\">");
            writer.Append("<attribute id=\"urn:epcglobal:cbv:mda#lot\">" + jb.BatchNo + "</attribute>");
            writer.Append("<attribute id=\"urn:epcglobal:cbv:mda#gtin\">" + gtin + "</attribute>");
            writer.Append("<attribute id=\"urn:epcglobal:cbv:mda#quantity\">" + jb.Quantity + "</attribute>");
            writer.Append("<attribute id =\"urn:epcglobal:cbv:mda#expirationDate\" >" + jb.ExpDate.ToString("yyyy-MM-dd") + "</attribute >");
            writer.Append("</VocabularyElement>");
            writer.Append("</VocabularyElementList>");
            writer.Append("</Vocabulary>");
            writer.Append("</VocabularyList>");
            writer.Append("</EPCISMasterData>");
            writer.Append("</extension>");
            writer.Append("</EPCISHeader>");
            writer.Append("<EPCISBody>");
            writer.Append("<EventList>");

            string Header = "";
            Header = writer.ToString();
            return Header;
        }

        public string EndDocument()
        {
            StringBuilder writer;
            writer = new StringBuilder();
            writer.Append("</EventList>");
            writer.Append("</EPCISBody>");
            writer.Append("</gs1:EPCISDocument>");
            string EndDocument = "";
            EndDocument = writer.ToString();
            return EndDocument;
        }

        public string CommissionEvent(string Filename, Job selectedJob, List<string> epclist, DateTime evtTime)
        {
            //StringBuilder writer;
            try
            {
                using (StreamWriter sw = new StreamWriter(Filename, true))
                {
                    int day = TimeZoneInfo.Local.BaseUtcOffset.Days;
                    int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                    int minutes = TimeZoneInfo.Local.BaseUtcOffset.Minutes;
                    string localZone = "";
                    if (hours > 0)
                        localZone = string.Format("+{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                    else if (hours < 0)
                        localZone = string.Format("-{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                    this.EventTimeZoneOffset = localZone;
                    //writer = new StringBuilder();

                    DateTime evt = evtTime;
                    sw.WriteLine("<ObjectEvent>");
                    sw.WriteLine("<eventTime>" + evt.ToString("yyyy-MM-ddTHH:mm:ss.fff") + EventTimeZoneOffset + "</eventTime>");
                    sw.WriteLine("<eventTimeZoneOffset>" + EventTimeZoneOffset + "</eventTimeZoneOffset>");
                    sw.WriteLine("<epcList>");
                    foreach (var item in epclist)
                    {
                        sw.WriteLine("<epc>" + item + "</epc>");
                    }
                    sw.WriteLine("</epcList>");
                    sw.WriteLine("<action>OBSERVE</action>");
                    sw.WriteLine("<bizStep>urn:epcglobal:cbv:bizstep:commissioning</bizStep>");
                    sw.WriteLine("<disposition>urn:epcglobal:cbv:disp:active</disposition>");
                    sw.WriteLine("<tk:extension><tk:ilmd>");
                    sw.WriteLine("<tk:lotNumber>" + selectedJob.BatchNo + "</tk:lotNumber>");
                    sw.WriteLine("<tk:itemExpirationDate>" + selectedJob.ExpDate.ToString("yyyy-MM-dd") + "</tk:itemExpirationDate>");
                    sw.WriteLine("<tk:productionDate>" + selectedJob.MfgDate.ToString("yyyy-MM-dd") + "</tk:productionDate>");
                    sw.WriteLine("<tk:productionOrder>productionOrder</tk:productionOrder>");
                    sw.WriteLine("</tk:ilmd></tk:extension>");
                    sw.WriteLine("</ObjectEvent>");
                    string ObjectEvent = "";
                    //ObjectEvent = writer.ToString();
                }
                return Filename;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AggregationEvent(string Filename, string parentID, List<string> childs, DateTime evtTime)
        {
            //StringBuilder writer;
            try
            {
                using (StreamWriter sw = new StreamWriter(Filename, true))
                {

                    int day = TimeZoneInfo.Local.BaseUtcOffset.Days;
                    int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                    int minutes = TimeZoneInfo.Local.BaseUtcOffset.Minutes;
                    string localZone = "";
                    if (hours > 0)
                        localZone = string.Format("+{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                    else if (hours < 0)
                        localZone = string.Format("-{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                    this.EventTimeZoneOffset = localZone;
                    //writer = new StringBuilder();
                    EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();

                    DateTime evt = evtTime;
                     sw.WriteLine("<AggregationEvent>");
                     sw.WriteLine("<eventTime>" + evt.ToString("yyyy-MM-ddTHH:mm:ss.fff") + EventTimeZoneOffset + "</eventTime>");
                     sw.WriteLine("<eventTimeZoneOffset>" + EventTimeZoneOffset + "</eventTimeZoneOffset>");
                     sw.WriteLine("<parentID>" + parentID + "</parentID>");
                    sw.WriteLine("<childEPCs>");
                    foreach (var chld in childs)
                    {
                        if (chld.Length == 18)
                        {
                            sw.WriteLine("<epc>" + epcisConf.GetEPCSSCC(chld) + "</epc>");
                        }
                        else
                        {
                            sw.WriteLine("<epc>" + chld + "</epc>");
                        }
                    }
                     sw.WriteLine("</childEPCs>");
                     sw.WriteLine("<action>ADD</action>");
                    sw.WriteLine("</AggregationEvent>");
                    //string ObjectEvent = "";
                    //ObjectEvent = writer.ToString();
                    //return ObjectEvent;
                    return Filename;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SSCCCommsionEvent(string Filename, List<string> sscc, DateTime evtTime)
        {
            //StringBuilder writer;
            try
            {
                using (StreamWriter sw = new StreamWriter(Filename, true))
                {

                    int day = TimeZoneInfo.Local.BaseUtcOffset.Days;
                    int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                    int minutes = TimeZoneInfo.Local.BaseUtcOffset.Minutes;
                    string localZone = "";
                    if (hours > 0)
                        localZone = string.Format("+{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                    else if (hours < 0)
                        localZone = string.Format("-{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                    this.EventTimeZoneOffset = localZone;
                    //writer = new StringBuilder();
                    EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
                    sw.WriteLine("<ObjectEvent>");
                    sw.WriteLine("<eventTime>" + evtTime.ToString("yyyy-MM-ddTHH:mm:ss.fff") + EventTimeZoneOffset + "</eventTime>");
                    sw.WriteLine("<eventTimeZoneOffset>" + EventTimeZoneOffset + "</eventTimeZoneOffset>");
                    sw.WriteLine("<epcList>");
                    foreach (var chld in sscc)
                    {
                        if (chld.Length == 18)
                        {
                            sw.WriteLine("<epc>" + epcisConf.GetEPCSSCC(chld) + "</epc>");
                        }
                    }
                    sw.WriteLine("</epcList>");
                    sw.WriteLine("<action>ADD</action>");
                    sw.WriteLine("<bizStep>urn:epcglobal:cbv:bizstep:commissioning</bizStep>");
                    sw.WriteLine("<disposition>urn:epcglobal:cbv:disp:active</disposition>");
                    sw.WriteLine("</ObjectEvent>");
                    //string ObjectEvent = "";
                    //ObjectEvent = writer.ToString();
                    return Filename;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DecommissioneEvent(string Filename, Job selectedJob, List<string> epclist, DateTime evtTime)
        {
            //StringBuilder writer;
            try
            {
                using (StreamWriter sw = new StreamWriter(Filename, true))
                {
                    int day = TimeZoneInfo.Local.BaseUtcOffset.Days;
                    int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                    int minutes = TimeZoneInfo.Local.BaseUtcOffset.Minutes;
                    string localZone = "";
                    if (hours > 0)
                        localZone = string.Format("+{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                    else if (hours < 0)
                        localZone = string.Format("-{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                    this.EventTimeZoneOffset = localZone;
                    //writer = new StringBuilder();

                    DateTime evt = evtTime;
                    sw.WriteLine("<ObjectEvent>");
                    sw.WriteLine("<eventTime>" + evt.ToString("yyyy-MM-ddTHH:mm:ss.fff") + EventTimeZoneOffset + "</eventTime>");
                    sw.WriteLine("<eventTimeZoneOffset>" + EventTimeZoneOffset + "</eventTimeZoneOffset>");
                    sw.WriteLine("<epcList>");
                    foreach (var item in epclist)
                    {
                        sw.WriteLine("<epc>" + item + "</epc>");
                    }
                    sw.WriteLine("</epcList>");
                    sw.WriteLine("<action>OBSERVE</action>");
                    sw.WriteLine("<bizStep>urn:epcglobal:cbv:bizstep:destroying</bizStep>");
                    sw.WriteLine("<disposition>urn:epcglobal:cbv:disp:destroyed</disposition>");
                    sw.WriteLine("<tk:extension><tk:ilmd>");
                    sw.WriteLine("<tk:lotNumber>" + selectedJob.BatchNo + "</tk:lotNumber>");
                    sw.WriteLine("<tk:itemExpirationDate>" + selectedJob.ExpDate.ToString("yyyy-MM-dd") + "</tk:itemExpirationDate>");
                    sw.WriteLine("<tk:productionDate>" + selectedJob.MfgDate.ToString("yyyy-MM-dd") + "</tk:productionDate>");
                    sw.WriteLine("<tk:productionOrder>productionOrder</tk:productionOrder>");
                    sw.WriteLine("</tk:ilmd></tk:extension>");
                    sw.WriteLine("</ObjectEvent>");
                    //string ObjectEvent = "";
                    //ObjectEvent = writer.ToString();
                }
                return Filename;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}