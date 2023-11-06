using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Data;
using TnT.Models.EPCIS;
using EPCIS_XMLs_Generation;
using TnT.Models;

namespace EPCIS_XMLs_Generation
{
    public class EventGeneration
    {
        private string EventTimeZoneOffset = null;
        private ApplicationDbContext db = new ApplicationDbContext();
        #region Header
        /// <summary>
        /// Header for generating PML FILE
        /// </summary>
        /// <param name="path"></param>
        public string GenerateEpcisHeader(string SenderGLN, string RecieverGLN,string EpcisVersion)//, string manufacturer, string drugName, List<string> ProductGtins, string dosageForm, string strength, string containersize, string sendercompanyName, string senderstreet1, string senderstreet2, string sendercity, string senderstateorRegion, string sendercountry, string senderpostalcode, string recievercompanyName, string recieverstreet1, string recieverstreet2, string recievercity, string recieverstateorRegion, string recievercountry, string recieverpostalcode, string EpcisVersion)
        {
            StringBuilder writer;

            writer = new StringBuilder();
            writer.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            writer.Append("<epcis:EPCISDocument ");
            writer.Append("xmlns:epcis=\"urn:epcglobal:epcis:xsd:1\"");
            writer.Append(" xmlns:sbdh=\"http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader" + "\"");
            writer.Append(" xmlns:epcismd=\"urn:epcglobal:epcis-masterdata:xsd:1\"");
            writer.Append(" xmlns:gs1ushc=\"http://epcis.gs1us.org/hc/ns" + "\"");
            writer.Append(" schemaVersion=\"1.1\" creationDate=\"" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "\">");
            writer.Append("<EPCISHeader>");
            writer.Append("<sbdh:StandardBusinessDocumentHeader>");
            writer.Append("<sbdh:HeaderVersion>" + EpcisVersion + "</sbdh:HeaderVersion>");
            writer.Append("<sbdh:Sender>");
            writer.Append("<sbdh:Identifier Authority=\"SGLN\">" + SenderGLN + "</sbdh:Identifier>");//Manufatcure or Supplier GLN
            writer.Append("</sbdh:Sender>");
            writer.Append("<sbdh:Receiver>");
            writer.Append("<sbdh:Identifier Authority=\"SGLN\">" + RecieverGLN + "</sbdh:Identifier>");//OWning Party or Recieving Prty GLN
            writer.Append("</sbdh:Receiver>");
            writer.Append("<sbdh:DocumentIdentification>");
            writer.Append("<sbdh:Standard>EPCglobal</sbdh:Standard>");
            writer.Append("<sbdh:TypeVersion>1.0</sbdh:TypeVersion>");
            writer.Append("<sbdh:InstanceIdentifier>1234567890</sbdh:InstanceIdentifier>");
            writer.Append("<sbdh:Type>Events</sbdh:Type>");
            writer.Append("<sbdh:CreationDateAndTime>" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "</sbdh:CreationDateAndTime>");
            writer.Append("</sbdh:DocumentIdentification>");
            writer.Append("</sbdh:StandardBusinessDocumentHeader>");
          
            writer.Append("</EPCISHeader>");
            writer.Append("<EPCISBody>");
            writer.Append("<EventList>");
            string Header = "";
            Header = writer.ToString();
            return Header;

        }


        public string GenerateObjectEventNew(string evntType, List<string> allDeck, DateTime LastUpdatedDate, DateTime MfgDate, DateTime ExpDate, string Lot, string readPoint, string NDC)
        {
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                writer.Append("<" + evntType + ">");
                DateTime evt = Convert.ToDateTime(LastUpdatedDate);
                writer.Append("<eventTime>" + evt.ToString("yyyy-MM-ddTHH:mm:ss") + "</eventTime>");
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

                writer.Append("<action>ADD</action>");
                writer.Append("<bizStep>urn:epcglobal:cbv:bizstep:commissioning</bizStep>");
                writer.Append("<disposition>urn:epcglobal:cbv:disp:active</disposition>");
                writer.Append("<readPoint>");
                writer.Append("<id>" + readPoint + "</id>");
                writer.Append("</readPoint>");
                writer.Append("<bizLocation>");
                writer.Append("<id>" + readPoint + "</id>");
                writer.Append("</bizLocation>");
                writer.Append("<extension>");
                writer.Append("<ilmd>");

                /*if (NDC != "")
                {
                    string IdentificationValue = "NDC";
                    string[] NDClength = NDC.Split('-');
                    for (int i = 0; i < NDClength.Count(); i++)
                    {
                        IdentificationValue = IdentificationValue + NDClength[i].Length;
                    }
                    writer.Append("<gs1ushc:additionalTradeItemIdentification>");
                    writer.Append("<gs1ushc:additionalTradeItemIdentificationValue>" + NDC + "</gs1ushc:additionalTradeItemIdentificationValue>");
                    writer.Append("<gs1ushc:additionalTradeItemIdentificationType>" + IdentificationValue + "</gs1ushc:additionalTradeItemIdentificationType>");
                    writer.Append(" </gs1ushc:additionalTradeItemIdentification>");
                }*/
                writer.Append("<gs1ushc:lotNumber>" + Lot + "</gs1ushc:lotNumber>");
                writer.Append("<gs1ushc:itemExpirationDate>" + ExpDate.Date.ToString("yyyy-MM-dd") + "</gs1ushc:itemExpirationDate>");
                writer.Append("<gs1ushc:itemManufactureDate>" + MfgDate.Date.ToString("yyyy-MM-dd") + "</gs1ushc:itemManufactureDate>");
                writer.Append("</ilmd>");
                writer.Append("</extension>");

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

        public string GenerateAggregationEventNewOld(string evntType, string parent, List<string> allChilds, DateTime LastUpdatedDate, string readPoint)
        {
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                writer.Append("<" + evntType + ">");
                DateTime evt = Convert.ToDateTime(LastUpdatedDate);
                writer.Append("<eventTime>" + evt.ToString("yyyy-MM-ddTHH:mm:ss") + "</eventTime>");
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


        public string GenerateAggregationEventNew(string evntType, string parent, List<string> allChilds, DateTime LastUpdatedDate, string readPoint)
        {
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                writer.Append("<" + evntType + ">");
                DateTime evt = Convert.ToDateTime(LastUpdatedDate);
                writer.Append("<eventTime>" + evt.ToString("yyyy-MM-ddTHH:mm:ss") + "</eventTime>");
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
        #endregion

        #region Objectevent
        /// <summary>
        /// Patch for Object event according to Bizstep
        /// </summary>
        /// <param name="_eventType"></param>
        /// <param name="_bizStep"></param>
        /// <param name="_disposition"></param>
        /// <param name="path"></param>
        public string GenerateObjectEvent(List<string> epc, string UUID, string Batch, string MfgDate, string ExpDate, Enum _action, Enum _eventType, Enum _bizStep, string _disposition, string SenderGLN, string path, string RecieverGLN, string Quantity)
        {
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                writer.Append("<ObjectEvent>");
                writer.Append("<eventTime>" + DateTime.Now.ToString(" yyyy-MM-ddTHH:mm:ss") + "</eventTime>");
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
                writer.Append("<epcList>");
                for (int i = 0; i < epc.Count; i++)
                {
                    writer.Append("<epc>" + epc[i] + "</epc>");
                }
                writer.Append("</epcList>");
                writer.Append("<action>" + _action + "</action>");
                writer.Append("<bizStep>urn:epcglobal:cbv:bizstep:" + _bizStep + "</bizStep>");
                writer.Append("<disposition>urn:epcglobal:cbv:disp:" + _disposition + "</disposition>");
                writer.Append("<readPoint>");
                writer.Append("<id>" + SenderGLN + "</id>");
                writer.Append("</readPoint>");
                writer.Append("<bizLocation>");
                writer.Append("<id>" + SenderGLN + "</id>");
                writer.Append("</bizLocation>");
                if (_bizStep.ToString() == EpcisEnums.bizStep.commissioning.ToString())
                {
                    writer.Append("<gs1ushc:lotNumber>" + Batch + "</gs1ushc:lotNumber>");
                    writer.Append("<gs1ushc:itemExpirationDate>" + MfgDate + "</gs1ushc:itemExpirationDate>");
                    writer.Append("<gs1ushc:itemManufactureDate>" + ExpDate + "</gs1ushc:itemManufactureDate>");
                }
                writer.Append("<gs1ushc:eventID>" + UUID + "</gs1ushc:eventID>");
                writer.Append("</ObjectEvent>");


                string ObjectEvent = "";
                ObjectEvent = writer.ToString();
                return ObjectEvent;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public string GenerateObjectEventNew(string evntType, List<string> allDeck, DateTime LastUpdatedDate, DateTime MfgDate, DateTime ExpDate, string Lot, string readPoint, string NDC, string action, string bizStep, string disposition)
        {
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                writer.Append("<" + evntType + ">");
                DateTime evt = Convert.ToDateTime(LastUpdatedDate);
                writer.Append("<eventTime>" + evt.ToString("yyyy-MM-ddTHH:mm:ss") + "</eventTime>");
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

                    if (NDC != "")
                    {
                        string IdentificationValue = "NDC";
                        string[] NDClength = NDC.Split('-');
                        for (int i = 0; i < NDClength.Count(); i++)
                        {
                            IdentificationValue = IdentificationValue + NDClength[i].Length;
                        }
                        writer.Append("<gs1ushc:additionalTradeItemIdentification>");
                        writer.Append("<gs1ushc:additionalTradeItemIdentificationValue>" + NDC + "</gs1ushc:additionalTradeItemIdentificationValue>");
                        writer.Append("<gs1ushc:additionalTradeItemIdentificationType>" + IdentificationValue + "</gs1ushc:additionalTradeItemIdentificationType>");
                        writer.Append(" </gs1ushc:additionalTradeItemIdentification>");
                    }
                    writer.Append("<gs1ushc:lotNumber>" + Lot + "</gs1ushc:lotNumber>");
                    writer.Append("<gs1ushc:itemExpirationDate>" + ExpDate.Date.ToString("yyyy-MM-dd") + "</gs1ushc:itemExpirationDate>");
                    writer.Append("<gs1ushc:itemManufactureDate>" + MfgDate.Date.ToString("yyyy-MM-dd") + "</gs1ushc:itemManufactureDate>");
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

        #region Aggregationevent
        /// <summary>
        /// Patch for Aggregation event according to Bizstep
        /// </summary>
        /// <param name="_eventType"></param>
        /// <param name="_bizStep"></param>
        /// <param name="_disposition"></param>
        /// <param name="path"></param>        
        public string GenerateAggregationEvent(string ParentID, string UUID, List<string> Childepc, string Batch, string MfgDate, string ExpDate, Enum _action, Enum _eventType, Enum _bizStep, string _disposition, string SenderGLN, string path, string RecieverGLN, string Quantity)
        {
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                writer.Append("<AggregationEvent>");
                writer.Append("<eventTime>" + DateTime.Now.ToString(" yyyy-MM-ddTHH:mm:ss") + "</eventTime>");
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
                writer.Append("<parentID>" + ParentID + "</parentID>");
                writer.Append("<childEPCs>");
                for (int i = 0; i < Childepc.Count; i++)
                {
                    writer.Append("<epc>" + Childepc[i] + "</epc>");
                }
                writer.Append("</childEPCs>");
                writer.Append("<action>" + _action + "</action>");
                writer.Append("<bizStep>urn:epcglobal:cbv:bizstep:" + _bizStep + "</bizStep>");
                writer.Append("<disposition>urn:epcglobal:cbv:disp:" + _disposition + "</disposition>");
                writer.Append("<readPoint>");
                writer.Append("<id>" + SenderGLN + "</id>");
                writer.Append("</readPoint>");
                writer.Append("<bizLocation>");
                writer.Append("<id>" + SenderGLN + "</id>");
                writer.Append("</bizLocation>");
                writer.Append("<gs1ushc:eventID>" + UUID + "</gs1ushc:eventID>");
                writer.Append("</AggregationEvent>");


                string AggregationEvent = "";
                AggregationEvent = writer.ToString();
                return AggregationEvent;

            }
            catch (Exception ex)
            {
                throw ex;
                // Trace.TraceError("{0},{1}{2},{3}", DateTime.Now.ToString(), ex.Message, ex.StackTrace, "FromReport:GenerateSalesWareHouse()-SetDATA");
            }

        }
        #endregion

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


        public string convertToEventOld(EpcisEventDetails evnt, string SenderGLN, string BatchNo, DateTime MfgDate, DateTime ExpDate,string NDC)
        {
            
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                writer.Append("<" + evnt.EventType + ">");
                DateTime evt = Convert.ToDateTime(evnt.EventTime);
                writer.Append("<eventTime>" + evt.ToString("yyyy-MM-ddTHH:mm:ss") + "</eventTime>");
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
                else if (evnt.EventType == EpcisEnums.EPCISEventType.TransactionEvent.ToString())
                {
                    writer.Append("<bizTransactionList>");
                    var bizTrList = evnt.BizTransactionList.Split(',');
                    foreach (var item in bizTrList)
                    {
                        var bizType = item.Split('=');
                        writer.Append("<bizTransaction type=\"" + bizType[0] + "\">" + bizType[1] + "</bizTransaction>");
                    }
                    writer.Append("</bizTransactionList>");

                    writer.Append("<epcList>");
                    var epcs = evnt.EpcList.Split(',');
                    foreach (var epc in epcs)
                    {
                        writer.Append("<epc>" + epc + "</epc>");
                    }
                    writer.Append("</epcList>");
                }
                else if (evnt.EventType == EpcisEnums.EPCISEventType.TransactionEvent.ToString())
                {
                    writer.Append("<bizTransactionList>");
                    var bizTrList = evnt.BizTransactionList.Split(',');
                    foreach (var item in bizTrList)
                    {
                        var bizType = item.Split('=');
                        writer.Append("<bizTransaction type=\"" + bizType[0] + "\">" + bizType[1] + "</bizTransaction>");
                    }
                    writer.Append("</bizTransactionList>");

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
                writer.Append("<bizLocation>");
                writer.Append("<id>" + evnt.BizLocation + "</id>");
                writer.Append("</bizLocation>");
                if (evnt.Action.ToUpper().CompareTo("ADD") == 0 && evnt.EventType == EpcisEnums.EPCISEventType.ObjectEvent.ToString())//evnt.BizStep == EpcisEnums.bizStep.commissioning.ToString() && 
                {
                    
                    writer.Append("<extension>");
                    writer.Append("<ilmd>");
                    if (NDC != "")
                    {
                        string IdentificationValue = "NDC";
                        string[] NDClength = NDC.Split('-');
                        for (int i = 0; i < NDClength.Count(); i++)
                        {
                            IdentificationValue = IdentificationValue + NDClength[i].Length;
                        }
                        writer.Append("<gs1ushc:additionalTradeItemIdentification>");
                        writer.Append("<gs1ushc:additionalTradeItemIdentificationValue>" + NDC + "</gs1ushc:additionalTradeItemIdentificationValue>");
                        writer.Append("<gs1ushc:additionalTradeItemIdentificationType>" + IdentificationValue + "</gs1ushc:additionalTradeItemIdentificationType>");
                        writer.Append(" </gs1ushc:additionalTradeItemIdentification>");
                    }
                    writer.Append("<gs1ushc:lotNumber>" + BatchNo + "</gs1ushc:lotNumber>");
                    writer.Append("<gs1ushc:itemExpirationDate>" + ExpDate.Date.ToString("yyyy-MM-dd") + "</gs1ushc:itemExpirationDate>");
                    writer.Append("<gs1ushc:itemManufactureDate>" + MfgDate.Date.ToString("yyyy-MM-dd") + "</gs1ushc:itemManufactureDate>");
                    writer.Append("</ilmd>");
                    writer.Append("</extension>");
                }
                if (evnt.Action.ToUpper().CompareTo("OBSERVE") == 0 && evnt.EventType == EpcisEnums.EPCISEventType.ObjectEvent.ToString() && evnt.ExtensionData1!=null)
                {
                    //EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
                    //var setting = db.Settings.FirstOrDefault();
                    //var receiver = db.M_EPCISReceiver.Where(x => x.CompanyName == evnt.ExtensionData1).FirstOrDefault();
                    //string receiverGLN = "";
                    //if (receiver != null)
                    //{
                    //    receiverGLN = epcisConf.GetEPCGLN(receiver.GLN, receiver.Extension, setting.CompanyCode.Length);
                    //}
                    //EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
                    var receiver = db.M_EPCISReceiver.Where(x => x.CompanyName == evnt.ExtensionData1).FirstOrDefault();
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


        public string convertToEvent(EpcisEventDetails evnt, string SenderGLN, string BatchNo, DateTime MfgDate, DateTime ExpDate, string NDC)
        {

            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                writer.Append("<" + evnt.EventType + ">");
                DateTime evt = Convert.ToDateTime(evnt.EventTime);
                writer.Append("<eventTime>" + evt.ToString("yyyy-MM-ddTHH:mm:ss") + "</eventTime>");
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
                else if (evnt.EventType == EpcisEnums.EPCISEventType.TransactionEvent.ToString())
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
                writer.Append("<bizLocation>");
                writer.Append("<id>" + evnt.BizLocation + "</id>");
                writer.Append("</bizLocation>");

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

                if (evnt.Action.ToUpper().CompareTo("ADD") == 0 && evnt.EventType == EpcisEnums.EPCISEventType.ObjectEvent.ToString())//evnt.BizStep == EpcisEnums.bizStep.commissioning.ToString() && 
                {
                 
                    writer.Append("<extension>");
                    writer.Append("<ilmd>");
                    writer.Append("<gs1ushc:lotNumber>" + BatchNo + "</gs1ushc:lotNumber>");
                    writer.Append("<gs1ushc:itemExpirationDate>" + ExpDate.Date.ToString("yyyy-MM-dd") + "</gs1ushc:itemExpirationDate>");
                    writer.Append("<gs1ushc:itemManufactureDate>" + MfgDate.Date.ToString("yyyy-MM-dd") + "</gs1ushc:itemManufactureDate>");
                    writer.Append("</ilmd>");
                    writer.Append("</extension>");
                    
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
    }
}
