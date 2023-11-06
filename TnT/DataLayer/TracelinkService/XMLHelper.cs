using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using TnT.Models.TraceLinkImporter;
using TnT.Models.Job;
using TnT.Models.SettingsNUtility;
using TnT.Models.Tracelink;
using TnT.Models.Product;
using TnT.Models.Customer;
using TnT.Models.Code;
using TnT.Models;

namespace TnT.DataLayer.TracelinkService
{
    public class XMLHelper
    {
        DataSet ds = new DataSet();
        XMLDBHelper XmlData = new XMLDBHelper();
        XmlWriter writer = null;
        XmlWriterSettings settings = new XmlWriterSettings();
        string xmlDoc = string.Empty;
        StringBuilder sw = new StringBuilder();
        ApplicationDbContext db = new ApplicationDbContext();

        public StringBuilder getSOMXMLOld(int jid, M_SOM som,bool IsMOC)
        {
            var data = (from j in db.Job join cm in db.M_Customer on j.CustomerId equals cm.Id where j.JID == jid select new { SenderId = cm.SenderId, ReceiverId = cm.ReceiverId }).FirstOrDefault();
            DateTime fdate = getGMT0DateTime(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            string FileControlNumber = fdate.ToString("yyyyMMddhhmmss");
            var job = db.Job.Where(x => x.JID == jid).FirstOrDefault();
            ds = XmlData.SOMData(job);
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;


            if (ds.Tables[0].Rows.Count > 0)
            {
                using (writer = XmlWriter.Create(sw, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("som", "SOMSalesShipmentMessage", "urn:tracelink:mapper:sl:serialized_operations_manager");
                    writer.WriteAttributeString("xmlns", "cmn", null, "urn:tracelink:mapper:sl:commontypes");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");

                    writer.WriteStartElement("som", "ControlFileHeader", null);
                    if (data.SenderId != "")
                    {
                        writer.WriteElementString("cmn", "FileSenderNumber", null, data.SenderId);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "FileSenderNumber", null, "");
                    }
                    if (data.ReceiverId != "")
                    {
                        writer.WriteElementString("cmn", "FileReceiverNumber", null, data.ReceiverId);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "FileReceiverNumber", null, "");
                    }
                    writer.WriteElementString("cmn", "FileControlNumber", null, FileControlNumber);
                    writer.WriteElementString("cmn", "FileDate", null, fdate.ToString("yyyy-MM-dd"));
                    writer.WriteElementString("cmn", "FileTime", null, fdate.ToString("HH:mm:ss"));
                    writer.WriteEndElement();

                    writer.WriteStartElement("som", "MessageBody", null);
                    //writer.WriteElementString("cmn", "LocationId", null, "8888882028655");
                    writer.WriteElementString("cmn", "DeliveryNumber", null, som.DeliveryNumber);
                    string DCFlag = string.Empty;
                    if (som.DeliveryCompleteFlag.ToString() == "True")
                    {
                        DCFlag = "true";
                    }
                    else
                    {
                        DCFlag = "false";
                    }
                    writer.WriteElementString("cmn", "DeliveryCompleteFlag", null, DCFlag);

                    writer.WriteStartElement("som", "OrderDetails", null);
                    writer.WriteStartElement("cmn", "TransactionIdentifier", null);
                    writer.WriteAttributeString("type", "PO");
                    writer.WriteString(som.TransactionIdentifier);
                    writer.WriteEndElement();

                    //writer.WriteStartElement("cmn", "TransactionIdentifier", null);
                    //writer.WriteAttributeString("type", "ASN");
                    //writer.WriteString("D70301");
                    //writer.WriteEndElement();

                    writer.WriteElementString("cmn", "TransactionDate", null, som.TransactionDate.ToString("yyyy-MM-dd"));
                    //writer.WriteElementString("cmn", "TransactionTime", null, "06:30:55");
                    var shiptocountry = db.Country.Where(x => x.CountryName == som.SFLI_Country).FirstOrDefault();
                    if (shiptocountry != null)
                    {
                        writer.WriteElementString("cmn", "ShipToCountryCode", null, shiptocountry.TwoLetterAbbreviation);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "ShipToCountryCode", null, som.SFLI_Country);
                    }

                    var shipfromcountry = db.Country.Where(x => x.CountryName == som.Country).FirstOrDefault();
                    if (shipfromcountry != null)
                    {
                        writer.WriteElementString("cmn", "ShipFromCountryCode", null, shipfromcountry.TwoLetterAbbreviation);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "ShipFromCountryCode", null, som.Country);
                    }
                    writer.WriteElementString("cmn", "SalesDistributionType", null, som.SalesDistributionType.ToUpper());

                    ////writer.WriteStartElement("cmn", "OrderItemList", null);

                    ////writer.WriteStartElement("cmn", "OrderItem", null);
                    //writer.WriteStartElement("cmn", "PackagingItemCode", null);
                    //writer.WriteAttributeString("type", "GTIN-14");
                    //writer.WriteString(ds.Tables[0].Rows[0].ItemArray[0].ToString());
                    //writer.WriteEndElement();
                    //writer.WriteElementString("cmn", "InternalMaterialCode", null, ds.Tables[0].Rows[0].ItemArray[5].ToString());
                    //writer.WriteElementString("cmn", "LotNumber", null, ds.Tables[0].Rows[0].ItemArray[2].ToString());
                    //writer.WriteElementString("cmn", "Quantity", null, ds.Tables[0].Rows[0].ItemArray[4].ToString());
                    //string IsSer = string.Empty;
                    //if (som.IsSerialized.ToString() == "True")
                    //{
                    //    IsSer = "true";
                    //}
                    //else
                    //{
                    //    IsSer = "false";
                    //}

                    //writer.WriteElementString("cmn", "IsSerialized", null, IsSer);
                    //writer.WriteEndElement();//Order Item

                    //writer.WriteEndElement();//Order ListItem

                    writer.WriteStartElement("cmn", "SenderInfo", null);
                    writer.WriteStartElement("cmn", "FromBusinessPartyLookupId", null);
                    writer.WriteAttributeString("type", "GCP");
                    writer.WriteString(som.FromBusinessPartyLookupId);
                    writer.WriteEndElement();
                    writer.WriteStartElement("cmn", "ShipFromLocationLookupId", null);
                    writer.WriteAttributeString("type", "SGLN");
                    writer.WriteString(som.ShipFromLocationLookupId);
                    writer.WriteEndElement();
                    //writer.WriteStartElement("cmn", "FromBusinessPartyInfo", null);
                    //writer.WriteStartElement("cmn", "BusinessId", null);
                    //writer.WriteAttributeString("type", "GLN");
                    //writer.WriteString(som.BusinessId);
                    //writer.WriteEndElement();

                    //writer.WriteStartElement("cmn", "BusinessInfo", null);
                    //writer.WriteElementString("cmn", "BusinessName", null, som.BusinessName);
                    //writer.WriteElementString("cmn", "Street1", null, som.Street1);
                    //writer.WriteElementString("cmn", "City", null, som.City);
                    //writer.WriteElementString("cmn", "StateOrRegion", null, som.StateOrRegion);
                    //writer.WriteElementString("cmn", "PostalCode", null, som.PostalCode);
                    //writer.WriteElementString("cmn", "Country", null, som.Country);
                    //writer.WriteEndElement();//BusinessInfo
                    //writer.WriteEndElement();//FromBusinessPartInfo

                    //writer.WriteStartElement("cmn", "ShipFromLocationInfo", null);

                    //writer.WriteStartElement("cmn", "FacilityId", null);
                    //writer.WriteAttributeString("type", "GLN");
                    //writer.WriteString(som.FacilityId_GLN);
                    //writer.WriteEndElement();

                    //writer.WriteStartElement("cmn", "FacilityId", null);
                    //writer.WriteAttributeString("type", "SGLN");
                    //writer.WriteString(som.FacilityId_SGLN);
                    //writer.WriteEndElement();

                    //writer.WriteStartElement("cmn", "LocationInfo", null);
                    //writer.WriteElementString("cmn", "BusinessName", null, som.SFLI_BusinessName);
                    //writer.WriteElementString("cmn", "Street1", null, som.SFLI_Street1);
                    //writer.WriteElementString("cmn", "City", null, som.SFLI_City);
                    //writer.WriteElementString("cmn", "StateOrRegion", null, som.SFLI_StateOrRegion);
                    //writer.WriteElementString("cmn", "PostalCode", null, som.SFLI_PostalCode);
                    //writer.WriteElementString("cmn", "Country", null, som.SFLI_Country);

                    //writer.WriteEndElement();//LocationInfo
                    //writer.WriteStartElement("cmn", "LicenseNumber", null);
                    //writer.WriteAttributeString("state", "TN");
                    //writer.WriteAttributeString("agency", "BOP");
                    //writer.WriteString("8888800003604");
                    //writer.WriteEndElement();

                    //writer.WriteStartElement("cmn", "ContactInfo", null);
                    //writer.WriteElementString("cmn", "Name", null, "Customer Service");
                    //writer.WriteElementString("cmn", "Telephone", null, "+1-781-555-5624");
                    //writer.WriteElementString("cmn", "Email", null, "support@kendallpharma.com");

                    //writer.WriteEndElement();//ContactInfo


                    //writer.WriteEndElement();//ShipFromLocationInfo

                    writer.WriteEndElement();//SenderInfo

                    writer.WriteStartElement("cmn", "ReceiverInfo", null);
                    writer.WriteStartElement("cmn", "ToBusinessPartyLookupId", null);
                    writer.WriteAttributeString("type", "GCP");
                    writer.WriteString(som.ToBusinessPartyLookupId);
                    writer.WriteEndElement();
                    writer.WriteStartElement("cmn", "ShipToLocationLookupId", null);
                    writer.WriteAttributeString("type", "SGLN");
                    writer.WriteString(som.ShipToLocationLookupId);
                    writer.WriteEndElement();
                    //writer.WriteStartElement("cmn", "ShipToLocationLookupId", null);
                    //writer.WriteAttributeString("type", "COMPANYSITEID");
                    //writer.WriteString("3333331013657");
                    //writer.WriteEndElement();

                    writer.WriteEndElement();//ReceiverInfo
                    writer.WriteEndElement();//OrderDetails

                    //writer.WriteElementString("cmn", "WarehouseOperatorName", null, ds.Tables[0].Rows[0].ItemArray[3].ToString());
                    if (!IsMOC)
                    {
                        writer.WriteStartElement("som", "AddPickedItems", null);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            writer.WriteElementString("cmn", "Serial", null, "00" + ds.Tables[0].Rows[i]["SSCC"].ToString());
                        }
                        writer.WriteEndElement();//AddPickedItems
                    }

                    writer.WriteStartElement("som", "AddPickedLot", null);

                    writer.WriteStartElement("cmn", "ProductLotNumbers", null);

                    //writer.WriteStartElement("cmn", "CountryDrugCode", null);
                    //writer.WriteAttributeString("type", "US_NDC442");
                    //writer.WriteString(ds.Tables[0].Rows[0].ItemArray[6].ToString());
                    //writer.WriteEndElement();
                    writer.WriteElementString("cmn", "InternalMaterialCode", null, ds.Tables[0].Rows[0].ItemArray[5].ToString());
                    writer.WriteElementString("cmn", "LotNumber", null, ds.Tables[0].Rows[0].ItemArray[2].ToString());

                    writer.WriteEndElement();//ProductLotNumbers
                    writer.WriteEndElement();//AddPickedLot
                    writer.WriteEndElement();//MessageBody
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                return sw;
            }
            else
            {

                return sw;
            }

        }

        public StringBuilder getSOMXML(int jid, M_SOM som, bool IsMOC)
        {
            EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
            var data = (from j in db.Job join cm in db.M_Customer on j.CustomerId equals cm.Id where j.JID == jid select new { SenderId = cm.SenderId, ReceiverId = cm.ReceiverId }).FirstOrDefault();
            DateTime fdate = getGMT0DateTime(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            string FileControlNumber = fdate.ToString("yyyyMMddhhmmss");
            var job = db.Job.Where(x => x.JID == jid).FirstOrDefault();
            var setting = db.Settings.FirstOrDefault();
            var cust = db.M_Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();
            ds = XmlData.SOMData(job);
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;


            if (ds.Tables[0].Rows.Count > 0)
            {
                using (writer = XmlWriter.Create(sw, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("som", "SOMSalesShipmentMessage", "urn:tracelink:mapper:sl:serialized_operations_manager");
                    writer.WriteAttributeString("xmlns", "cmn", null, "urn:tracelink:mapper:sl:commontypes");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");

                    writer.WriteStartElement("som", "ControlFileHeader", null);
                    if (data.SenderId != "")
                    {
                        writer.WriteElementString("cmn", "FileSenderNumber", null, data.SenderId);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "FileSenderNumber", null, "");
                    }
                    if (data.ReceiverId != "")
                    {
                        writer.WriteElementString("cmn", "FileReceiverNumber", null, data.ReceiverId);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "FileReceiverNumber", null, "");
                    }
                    writer.WriteElementString("cmn", "FileControlNumber", null, FileControlNumber);
                    writer.WriteElementString("cmn", "FileDate", null, fdate.ToString("yyyy-MM-dd"));
                    writer.WriteElementString("cmn", "FileTime", null, fdate.ToString("hh:mm:ss"));
                    writer.WriteEndElement();//ControlFileHeader

                    writer.WriteStartElement("som", "MessageBody", null);

                    writer.WriteElementString("cmn", "DeliveryNumber", null, som.DeliveryNumber);
                    string DCFlag = string.Empty;
                    if (som.DeliveryCompleteFlag.ToString() == "True")
                    {
                        DCFlag = "true";
                    }
                    else
                    {
                        DCFlag = "false";
                    }
                    writer.WriteElementString("cmn", "DeliveryCompleteFlag", null, DCFlag);

                    writer.WriteStartElement("som", "OrderDetails", null);
                    writer.WriteStartElement("cmn", "TransactionIdentifier", null);
                    writer.WriteAttributeString("type", "PO");
                    writer.WriteString(som.TransactionIdentifier);
                    writer.WriteEndElement();

                    writer.WriteElementString("cmn", "TransactionDate", null, som.TransactionDate.ToString("yyyy-MM-dd"));

                    var shiptocountry = db.Country.Where(x => x.CountryName == som.SFLI_Country).FirstOrDefault();
                    if (shiptocountry != null)
                    {
                        writer.WriteElementString("cmn", "ShipToCountryCode", null, shiptocountry.TwoLetterAbbreviation);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "ShipToCountryCode", null, som.SFLI_Country);
                    }

                    var shipfromcountry = db.Country.Where(x => x.CountryName == som.Country).FirstOrDefault();
                    if (shipfromcountry != null)
                    {
                        writer.WriteElementString("cmn", "ShipFromCountryCode", null, shipfromcountry.TwoLetterAbbreviation);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "ShipFromCountryCode", null, som.Country);
                    }
                    writer.WriteElementString("cmn", "SalesDistributionType", null, som.SalesDistributionType.ToUpper());



                    writer.WriteStartElement("cmn", "SenderInfo", null);
                    writer.WriteStartElement("cmn", "FromBusinessPartyInfo", null);
                    writer.WriteStartElement("cmn", "BusinessId", null);
                    writer.WriteAttributeString("type", "GLN");
                    writer.WriteString(som.FromBusinessPartyLookupId);
                    writer.WriteEndElement();
                    writer.WriteStartElement("cmn", "BusinessInfo", null);
                    writer.WriteElementString("cmn", "BusinessName", null, setting.CompanyName);
                    writer.WriteElementString("cmn", "Street1", null, setting.Street);
                    if (setting.District != null)
                    {
                        writer.WriteElementString("cmn", "District", null, setting.District);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "District", null, null);
                    }
                    writer.WriteElementString("cmn", "City", null, setting.City);
                    if (setting.StateOrRegion != 0)
                    {
                        writer.WriteElementString("cmn", "StateOrRegion", null, setting.M_State.StateName);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "StateOrRegion", null, null);
                    }
                    writer.WriteElementString("cmn", "PostalCode", null, setting.PostalCode);
                    writer.WriteElementString("cmn", "Country", null, setting.M_Country.TwoLetterAbbreviation);
                    writer.WriteEndElement();//BusinessInfo
                    writer.WriteEndElement();//FromBusinessPartyInfo

                    writer.WriteStartElement("cmn", "ShipFromLocationInfo", null);
                    writer.WriteStartElement("cmn", "FacilityId", null);
                    writer.WriteAttributeString("type", "GLN");
                    writer.WriteString(som.FromBusinessPartyLookupId);
                    writer.WriteEndElement();
                    writer.WriteStartElement("cmn", "FacilityId", null);
                    writer.WriteAttributeString("type", "SGLN");
                    writer.WriteString(som.ShipFromLocationLookupId);
                    writer.WriteEndElement();
                    writer.WriteStartElement("cmn", "LocationInfo", null);
                    writer.WriteElementString("cmn", "BusinessName", null, setting.CompanyName);
                    writer.WriteElementString("cmn", "Street1", null, setting.Street);
                    if (setting.District != null)
                    {
                        writer.WriteElementString("cmn", "District", null, setting.District);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "District", null, null);
                    }
                    writer.WriteElementString("cmn", "City", null, setting.City);
                    if (setting.StateOrRegion != 0)
                    {
                        writer.WriteElementString("cmn", "StateOrRegion", null, setting.M_State.StateName);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "StateOrRegion", null, null);
                    }
                    writer.WriteElementString("cmn", "PostalCode", null, setting.PostalCode);
                    writer.WriteElementString("cmn", "Country", null, setting.M_Country.TwoLetterAbbreviation);
                    writer.WriteEndElement();//LocationInfo
                    writer.WriteEndElement();//ShipFromLocationInfo
                    writer.WriteEndElement();//SenderInfo

                    writer.WriteStartElement("cmn", "ReceiverInfo", null);

                    writer.WriteStartElement("cmn", "ToBusinessPartyInfo", null);
                    writer.WriteStartElement("cmn", "BusinessId", null);
                    writer.WriteAttributeString("type", "GLN");
                    writer.WriteString(som.ToBusinessPartyLookupId);
                    writer.WriteEndElement();

                    writer.WriteStartElement("cmn", "BusinessInfo", null);
                    writer.WriteElementString("cmn", "BusinessName", null, cust.CompanyName);
                    writer.WriteElementString("cmn", "Street1", null, cust.street1);
                    writer.WriteElementString("cmn", "City", null, cust.city);
                    writer.WriteElementString("cmn", "StateOrRegion", null, cust.M_State.StateName);
                    writer.WriteElementString("cmn", "PostalCode", null, cust.postalCode);
                    writer.WriteElementString("cmn", "Country", null, cust.M_Country.TwoLetterAbbreviation);
                    writer.WriteEndElement();//BusinessInfo

                    writer.WriteEndElement();//ToBusinessPartyInfo


                    writer.WriteStartElement("cmn", "ShipToLocationInfo", null);
                    writer.WriteStartElement("cmn", "FacilityId", null);
                    writer.WriteAttributeString("type", "SGLN");
                    writer.WriteString(som.ShipToLocationLookupId);
                    writer.WriteEndElement();
                    writer.WriteStartElement("cmn", "LocationInfo", null);

                    writer.WriteElementString("cmn", "BusinessName", null, cust.CompanyName);
                    writer.WriteElementString("cmn", "Street1", null, cust.street1);
                    writer.WriteElementString("cmn", "City", null, cust.city);
                    writer.WriteElementString("cmn", "StateOrRegion", null, cust.M_State.StateName);
                    writer.WriteElementString("cmn", "PostalCode", null, cust.postalCode);
                    writer.WriteElementString("cmn", "Country", null, cust.M_Country.TwoLetterAbbreviation);

                    writer.WriteEndElement();//LocationInfo
                    writer.WriteEndElement();//ShipToLocationInfo


                    writer.WriteEndElement();//ReceiverInfo

                    writer.WriteEndElement();//OrderDetails

                    //writer.WriteElementString("cmn", "WarehouseOperatorName", null, ds.Tables[0].Rows[0].ItemArray[3].ToString());
                    if (!IsMOC)
                    {
                        writer.WriteStartElement("som", "AddPickedItems", null);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {


                            writer.WriteElementString("cmn", "Serial", null, "00" + ds.Tables[0].Rows[i]["SSCC"].ToString());


                        }
                        writer.WriteEndElement();//AddPickedItems
                    }

                    writer.WriteStartElement("som", "AddPickedLot", null);

                    writer.WriteStartElement("cmn", "ProductLotNumbers", null);


                    writer.WriteElementString("cmn", "InternalMaterialCode", null, ds.Tables[0].Rows[0].ItemArray[5].ToString());
                    writer.WriteElementString("cmn", "LotNumber", null, ds.Tables[0].Rows[0].ItemArray[2].ToString());

                    writer.WriteEndElement();//ProductLotNumbers
                    writer.WriteEndElement();//AddPickedLot

                    writer.WriteEndElement();//MessageBody
                    writer.WriteEndElement();//SOMSalesShipmentMessage

                    writer.WriteEndDocument();
                }

                return sw;
            }
            else
            {

                return null;
            }

        }

        public StringBuilder getDispositionUpdateXml(int jid, bool IsMOC)
        {
            try
            {
                ds = XmlData.DispositionData(jid, IsMOC);
                var data = (from j in db.Job join cm in db.M_Customer on j.CustomerId equals cm.Id where j.JID == jid select new { SenderId = cm.SenderId, ReceiverId = cm.ReceiverId }).FirstOrDefault();
                var companySettings = db.Settings.FirstOrDefault();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;
                settings.OmitXmlDeclaration = true;
                string FileControlNumber = DateTime.Now.ToString("yyyyMMddhhmmss");

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                using (writer = XmlWriter.Create(sw, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("SNXDispositionUpdatedMessage", "urn:tracelink:mapper:sl:serial_number_exchange");
                    writer.WriteAttributeString("xmlns", "snx", null, "urn:tracelink:mapper:sl:serial_number_exchange");
                    writer.WriteAttributeString("xmlns", "cmn", null, "urn:tracelink:mapper:sl:commontypes");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");

                    writer.WriteStartElement("snx", "ControlFileHeader", null);
                    if (data.SenderId != "")
                    {
                        writer.WriteElementString("cmn", "FileSenderNumber", null, data.SenderId);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "FileSenderNumber", null, "");
                    }
                    if (data.ReceiverId != "")
                    {
                        writer.WriteElementString("cmn", "FileReceiverNumber", null, data.ReceiverId);
                    }
                    else
                    {
                        writer.WriteElementString("cmn", "FileReceiverNumber", null, "");
                    }
                    writer.WriteElementString("cmn", "FileControlNumber", null, FileControlNumber);
                    writer.WriteElementString("cmn", "FileDate", null, DateTime.Now.Date.ToString("yyyy-MM-dd"));
                    writer.WriteElementString("cmn", "FileTime", null, DateTime.Now.ToString("hh:mm:ss"));
                    writer.WriteEndElement();//ControlFileHeader

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        writer.WriteStartElement("MessageBody");
                        DateTime EDate = getGMT0DateTime(Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[1]));
                        writer.WriteElementString("cmn", "EventDateTime", null, EDate.ToString("yyyy-MM-dd") + "T" + EDate.ToString("HH:mm:ss.ff"));
                        writer.WriteElementString("cmn", "EventTimeZoneOffset", null, "+00:00");

                        writer.WriteStartElement("snx", "SerialNumbers", null);
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                string serialno = ds.Tables[1].Rows[i].ItemArray[0].ToString();
                                var gtin = db.X_TracelinkUIDStore.Where(x => x.SerialNo == serialno).FirstOrDefault();
                                if (gtin != null)
                                {
                                    writer.WriteElementString("cmn", "Serial", null, "01" + gtin.GTIN + "21" + serialno);
                                }

                            }
                        }
                        writer.WriteEndElement();//SerialNumbers
                        writer.WriteElementString("cmn", "PackagingSerialNumberStatus", null, "DEACTIVATED");
                        writer.WriteElementString("cmn", "EventLocation", null, companySettings.GLN);
                        //writer.WriteElementString("cmn", "ProductionLineId", null, ds.Tables[0].Rows[0].ItemArray[3].ToString());
                        //writer.WriteElementString("cmn", "LineManagerName", null, ds.Tables[0].Rows[0].ItemArray[2].ToString());
                        writer.WriteElementString("cmn", "ReasonDescription", null, "Rejected/Damaged/Sampled codes in Plant or Warehouse");
                        writer.WriteEndElement();//MessageBody
                    }


                    writer.WriteEndElement();//SNXDispositionUpdatedMessage
                    writer.WriteEndDocument();
                }
                return sw;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #region Disposition Assign
        /// <summary>
        /// Generate Dispostion Assign File
        /// </summary>
        /// <param name="jid">Job Id</param>
        /// <param name="IsMoc">File should be only for MOC</param>
        /// <returns></returns>
        public string getDispositionXML(int jid, bool IsMoc)
        {
            try
            {
                string header = string.Empty;
                header = DispAssheader(jid);
                string footer = string.Empty;
                footer = DispAssFooter();
                string compEvent = string.Empty;
                compEvent = CommissionEvent(jid, IsMoc);
                string xml = header + compEvent + footer;
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Header for Disposition Assign File
        /// </summary>
        /// <param name="jid">Job Id</param>
        /// <returns></returns>
        public string DispAssheader(int jid)
        {
            StringBuilder writer;
            try
            {
                var data = (from j in db.Job join cm in db.M_Customer on j.CustomerId equals cm.Id where j.JID == jid select new { SenderId = cm.SenderId, ReceiverId = cm.ReceiverId }).FirstOrDefault();
                string FileControlNumber = DateTime.Now.ToString("yyyyMMddhhmmss");

                string header = string.Empty;
                writer = new StringBuilder();
                writer.Append("<?xml version=\"1.0\"?>");
                writer.Append("<SNXDispositionAssignedMessage ");
                writer.Append("xmlns:snx=\"urn:tracelink:mapper:sl:serial_number_exchange\"");
                writer.Append(" xmlns:cmn=\"urn:tracelink:mapper:sl:commontypes\"");
                writer.Append(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
                writer.Append(" xmlns=\"urn:tracelink:mapper:sl:serial_number_exchange\">");
                writer.Append("<snx:ControlFileHeader>");
                writer.Append("<cmn:FileSenderNumber>" + data.SenderId + "</cmn:FileSenderNumber>");
                writer.Append("<cmn:FileReceiverNumber>" + data.ReceiverId + "</cmn:FileReceiverNumber>");
                writer.Append("<cmn:FileControlNumber>" + FileControlNumber + "</cmn:FileControlNumber>");
                writer.Append("<cmn:FileDate>" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "</cmn:FileDate>");
                writer.Append("<cmn:FileTime>" + DateTime.Now.ToString("hh:mm:ss") + "</cmn:FileTime>");
                writer.Append("</snx:ControlFileHeader>");
                writer.Append("<snx:MessageBody>");
                header = writer.ToString();


                return header;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Footer for Dispostion Assign File
        /// </summary>
        /// <returns></returns>
        public string DispAssFooter()
        {
            StringBuilder writer;
            try
            {
                string footer = string.Empty;
                writer = new StringBuilder();
                writer.Append("</snx:MessageBody>");
                writer.Append("</SNXDispositionAssignedMessage>");
                footer = writer.ToString();
                return footer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Generate CommissionEventDetail,CommissionCommonAttributes and AggregationEventDetail String
        /// </summary>
        /// <param name="jid"></param>
        /// <param name="IsMoc"></param>
        /// <returns></returns>
        public string CommissionEvent(int jid, bool IsMoc)//Create by Pranita - CommissionEventDetail loop
        {

            try
            {
                DataRow[] results = null;
                string LastpckLevel = string.Empty;
                bool lstlvl = false;
                string commEvent = string.Empty;
                string compEventDetails = string.Empty;
                string compAttr = string.Empty;
                DataTable dt = XmlData.DispAssCommEvent(jid, IsMoc); //Data for the CommissionEventDetail and AggregationEvent 
                string pklvl = string.Empty;
                var lvls = ProductPackageHelper.getAllDeck(jid.ToString());
                lvls = ProductPackageHelper.sorttheLevelsDesc(lvls); //All the levels for the selected batch sorted in Desc order
                LastpckLevel = lvls.First();
                var jbdetail = db.JobDetails.Where(x => x.JD_JobID == jid && x.JD_Deckcode == "MOC").FirstOrDefault();
                DataTable dtCompAttr = XmlData.DispAssCommAttr(jid, IsMoc); //Data for the CommissionCommonAttributes
                bool isGTIN = false;
                if (jbdetail.JD_NTIN == "" || jbdetail.JD_NTIN == null) //Check if the Job is of type NTIN or GTIN
                {
                    isGTIN = true;
                }
                else
                {
                    isGTIN = false;
                }
                var settings = db.Settings.FirstOrDefault();
                if (IsMoc) //Only for MOC CommissionEventDetail
                {
                    results = dt.Select("PackageTypeCode='MOC' and JD_Deckcode='MOC'");
                    pklvl = "EA";
                    DataRow drLineCode = dtCompAttr.Select("JD_Deckcode='MOC'").FirstOrDefault();
                    compEventDetails = compEventDetails + CompEvtDtl(results, lstlvl, pklvl);
                    compEventDetails = compEventDetails + CompAttr(drLineCode, isGTIN, settings, lstlvl);
                    return compEventDetails;
                }


                for (int i = 0; i < lvls.Count; i++)
                {
                    results = dt.Select("PackageTypeCode='" + lvls[i] + "' and JD_Deckcode='" + lvls[i] + "' and isLoose is null");
                    if (LastpckLevel == lvls[i] && LastpckLevel != "MOC") //Check to get the Last Level
                    {
                        lstlvl = true;
                    }
                    else
                    {
                        lstlvl = false;
                    }
                    if (lvls[i] == "MOC")
                    {
                        pklvl = "EA";
                    }

                    if (lvls[i] == "ISH")
                    {
                        pklvl = "CA";
                    }
                    if (lvls[i] == "OBX")
                    {
                        pklvl = "PK";
                    }
                    if (lvls[i] == "PAL")
                    {
                        pklvl = "PL";
                    }

                    DataRow drLineCode = dtCompAttr.Select("JD_Deckcode='" + lvls[i] + "'").FirstOrDefault();
                    if (results.Count() > 0)
                    {

                        compEventDetails = compEventDetails + CompEvtDtl(results, lstlvl, pklvl);
                        compEventDetails = compEventDetails + CompAttr(drLineCode, isGTIN, settings, lstlvl);

                    }
                    if (lvls[i] != "MOC")//This Loop is for CommissionEventDetail for Loose SSCC
                    {

                        results = dt.Select("PackageTypeCode='" + lvls[i] + "' and JD_Deckcode='" + lvls[i] + "' and isLoose=true");
                        if (results.Count() > 0)
                        {
                            lstlvl = true;
                            compEventDetails = compEventDetails + CompEvtDtl(results, lstlvl, pklvl);
                            compEventDetails = compEventDetails + CompAttr(drLineCode, isGTIN, settings, lstlvl);

                        }
                    }
                }

                string Agg = AggEvent(dt, LastpckLevel, lvls, settings); //AggregationEvent starts here
                compEventDetails = compEventDetails + Agg;
                return compEventDetails;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// CommissionEventDetail
        /// </summary>
        /// <param name="dr">DataRow for CommissionEventDetail</param>
        /// <param name="lastlvl">Is Last Level</param>
        /// <param name="pklvl">Package type Code</param>
        /// <returns></returns>
        public string CompEvtDtl(DataRow[] dr, bool lastlvl, string pklvl)
        {
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                string compevtdtl = string.Empty;
                string timezone = "+00:00";
                writer.Append("<snx:CommissionEvent>");
                for (int i = 0; i < dr.Count(); i++)
                {
                    DateTime mfg = getGMT0DateTime(Convert.ToDateTime(dr[i]["CreatedDate"]));
                    string mfgdate = mfg.ToString("yyyy-MM-dd") + "T" + mfg.ToString("HH:mm:ss.ff");
                    writer.Append("<snx:CommissionEventDetail>");
                    writer.Append("<cmn:EventDateTime>" + mfgdate + "</cmn:EventDateTime>");
                    writer.Append("<cmn:EventTimeZoneOffset>" + timezone + "</cmn:EventTimeZoneOffset>");
                    if (lastlvl)
                    {
                        if (dr[i]["remarks"].ToString() != "")
                        {
                            writer.Append("<cmn:SerialNumber>00" + dr[i]["remarks"] + "</cmn:SerialNumber>");
                            string filterval = dr[i].ItemArray[11].ToString().Substring(0, 1);
                            writer.Append("<cmn:FilterValue>" + filterval + "</cmn:FilterValue>");
                        }
                        else
                        {
                            writer.Append("<cmn:SerialNumber>00" + dr[i]["SSCC"] + "</cmn:SerialNumber>");
                            string filterval = dr[i]["SSCC"].ToString().Substring(0, 1);
                            writer.Append("<cmn:FilterValue>" + filterval + "</cmn:FilterValue>");
                        }

                    }
                    else
                    {
                        writer.Append("<cmn:SerialNumber>01" + dr[i]["JD_GTIN"] + "21" + dr[i]["Code"] + "</cmn:SerialNumber>");
                    }

                    writer.Append("<cmn:PackagingLevel>" + pklvl + "</cmn:PackagingLevel>");
                    writer.Append("</snx:CommissionEventDetail>");
                }
                compevtdtl = writer.ToString();
                return compevtdtl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// CommissionCommonAttributes
        /// </summary>
        /// <param name="dr">DataRow for CommissionCommonAttributes Event</param>
        /// <param name="isGTIN">Job Type is GTIN or NTIN</param>
        /// <param name="companySettings">Setting Table </param>
        /// <returns></returns>
        public string CompAttr(DataRow dr, bool isGTIN, Settings companySettings, bool lstlvl)
        {

            try
            {
                DateTime mfgd = Convert.ToDateTime(dr["MfgDate"]);
                string mdate = mfgd.ToString("yyyy-MM-dd");

                DateTime expd = Convert.ToDateTime(dr["ExpDate"]);
                string edate = expd.ToString("yyyy-MM-dd");

                StringBuilder writer;
                string compAttribute = "";
                writer = new StringBuilder();
                writer.Append("<snx:CommissionCommonAttributes>");
                writer.Append("<cmn:EventLocation>" + companySettings.GLN + "</cmn:EventLocation>");
                writer.Append("<cmn:ProductionLineId>" + dr["LineCode"] + "</cmn:ProductionLineId>");
                if (lstlvl == true)
                {
                    writer.Append("<cmn:CompanyPrefix>" + companySettings.CompanyCode + "</cmn:CompanyPrefix>");
                }
                writer.Append("<ItemDetail>");
                writer.Append("<cmn:InternalMaterialCode>" + dr["InternalMaterialCode"] + "</cmn:InternalMaterialCode>");
                if (isGTIN)
                {
                    writer.Append("<cmn:ItemCode type=\"GTIN-14\">" + dr["JD_GTIN"] + "</cmn:ItemCode>");
                }
                else
                {
                    writer.Append("<cmn:ItemCode type=\"NTIN\">" + dr["JD_NTIN"] + "</cmn:ItemCode>");
                }
                writer.Append("<cmn:LotNumber>" + dr["BatchNo"] + "</cmn:LotNumber>");
                writer.Append("<cmn:ExpirationDate>" + edate + "</cmn:ExpirationDate>");
                writer.Append("<cmn:ManufacturingDate>" + mdate + "</cmn:ManufacturingDate>");
                writer.Append("</ItemDetail>");
                writer.Append("</snx:CommissionCommonAttributes>");
                writer.Append("</snx:CommissionEvent>");
                compAttribute = writer.ToString();
                return compAttribute;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// itemArray from DataRow 0-PackageTypeCode,1-Code,2-NextLevelCode,3-JD_GTIN,4-ExpPackDate,5-BatchNo,6-JobStartTime,7-LastUpdatedDate,
        /// 8-SSCC,9-JD_Deckcode,10-IsLoose
        /// </summary>
        /// <param name="dt">Data Table for AggregationEvent</param>
        /// <param name="LastpckLevel">Last Level Packagetype Code</param>
        /// <param name="lvls">All the Levels</param>
        /// <param name="compSetting">Settings data</param>
        /// <returns></returns>

        public string AggEvent(DataTable dt, string LastpckLevel, List<string> lvls, Settings compSetting)
        {
            try
            {
                StringBuilder writer;
                string ParentSerialNumber = "";
                string aggEvent = "", PcktypeCode = string.Empty;
                writer = new StringBuilder();
                writer.Append("<snx:AggregationEvent>");
                DataRow[] drLastAggData = dt.Select("PackageTypeCode='" + LastpckLevel + "'");
                if (drLastAggData.Count() > 0) //Last Level AggregationEvent
                {
                    for (int i = 0; i <= drLastAggData.Count() - 1; i++) //Loop for last level-eg PAL
                    {
                        DateTime mfg = getGMT0DateTime(Convert.ToDateTime(drLastAggData[i]["CreatedDate"]).AddMinutes(2));
                        string mfgdate = mfg.ToString("yyyy-MM-dd") + "T" + mfg.ToString("HH:mm:ss.ff");
                        if (drLastAggData[i]["SSCC"].ToString() != "")
                        {
                            if (drLastAggData[i]["remarks"].ToString() != "")
                            {
                                ParentSerialNumber = "00" + drLastAggData[i]["remarks"].ToString();
                            }
                            else
                            {
                                ParentSerialNumber = "00" + drLastAggData[i]["SSCC"].ToString();
                            }
                        }
                        else
                        {
                            ParentSerialNumber = "01" + drLastAggData[i]["JD_GTIN"].ToString() + "21" + drLastAggData[i]["Code"].ToString();
                        }
                        string Nextlvlcode = drLastAggData[i]["Code"].ToString();
                        int cnt = lvls.Count();

                        if (cnt - 2 > 0)
                        {
                            PcktypeCode = lvls[1];
                        }
                        else
                        {
                            PcktypeCode = "MOC";
                        }

                        var serialNumber = "";

                        DataRow[] tblSerialNumber2ndLvl = dt.Select("NextLevelCode='" + Nextlvlcode + "' and PackageTypeCode='" + PcktypeCode + "'");
                        writer.Append("<AggregationEventDetail>");
                        writer.Append("<cmn:EventDateTime>" + mfgdate + "</cmn:EventDateTime>");
                        writer.Append("<cmn:EventTimeZoneOffset>+00:00</cmn:EventTimeZoneOffset>");
                        writer.Append("<cmn:EventLocation>" + compSetting.GLN + "</cmn:EventLocation>");
                        writer.Append("<cmn:ParentSerialNumber>" + ParentSerialNumber + "</cmn:ParentSerialNumber>");
                        writer.Append("<cmn:Quantity>" + tblSerialNumber2ndLvl.Count() + "</cmn:Quantity>");
                        writer.Append("<cmn:SerialNumberList>");
                        if (tblSerialNumber2ndLvl.Count() > 0)
                        {
                            for (int j = 0; j < tblSerialNumber2ndLvl.Count(); j++)
                            {

                                if (tblSerialNumber2ndLvl[j]["SSCC"].ToString() == "")
                                {
                                    serialNumber = "01" + tblSerialNumber2ndLvl[j]["JD_GTIN"] + "21" + tblSerialNumber2ndLvl[j]["Code"];
                                }
                                else
                                {
                                    if (tblSerialNumber2ndLvl[j]["remarks"].ToString() == "")
                                    {
                                        serialNumber = "00" + tblSerialNumber2ndLvl[j]["SSCC"];
                                    }
                                    else
                                    {
                                        serialNumber = "00" + tblSerialNumber2ndLvl[j]["remarks"];
                                    }
                                }
                                writer.Append("<cmn:SerialNumber>" + serialNumber + "</cmn:SerialNumber>");
                            }
                        }
                        writer.Append("</cmn:SerialNumberList>");
                        writer.Append("</AggregationEventDetail>");


                        if (tblSerialNumber2ndLvl.Count() > 0)//2nd Last level AggregationEvent
                        {
                            for (int l = 0; l < tblSerialNumber2ndLvl.Count(); l++)//Loop for 2nd Last level-eg-OSH
                            {
                                if (cnt - 3 >= 0)
                                {

                                    if (tblSerialNumber2ndLvl[l]["SSCC"].ToString() != "")
                                    {
                                        if (tblSerialNumber2ndLvl[l]["remarks"].ToString() == "")
                                        {
                                            ParentSerialNumber = "00" + tblSerialNumber2ndLvl[l]["SSCC"].ToString();
                                        }
                                        else
                                        {
                                            ParentSerialNumber = "00" + tblSerialNumber2ndLvl[l]["remarks"].ToString();
                                        }

                                    }
                                    else
                                    {
                                        ParentSerialNumber = "01" + tblSerialNumber2ndLvl[l]["JD_GTIN"].ToString() + "21" + tblSerialNumber2ndLvl[l]["Code"].ToString();
                                    }
                                    Nextlvlcode = tblSerialNumber2ndLvl[l].ItemArray[1].ToString();
                                    mfg = getGMT0DateTime(Convert.ToDateTime(tblSerialNumber2ndLvl[l]["CreatedDate"]).AddMinutes(2));
                                    mfgdate = mfg.ToString("yyyy-MM-dd") + "T" + mfg.ToString("HH:mm:ss.ff");
                                    PcktypeCode = lvls[2];
                                    DataRow[] tblSerialNumber3rdLvl = dt.Select("NextLevelCode='" + Nextlvlcode + "' and PackageTypeCode='" + PcktypeCode + "'");
                                    writer.Append("<AggregationEventDetail>");
                                    writer.Append("<cmn:EventDateTime>" + mfgdate + "</cmn:EventDateTime>");
                                    writer.Append("<cmn:EventTimeZoneOffset>+00:00</cmn:EventTimeZoneOffset>");
                                    writer.Append("<cmn:EventLocation>" + compSetting.GLN + "</cmn:EventLocation>");
                                    writer.Append("<cmn:ParentSerialNumber>" + ParentSerialNumber + "</cmn:ParentSerialNumber>");
                                    writer.Append("<cmn:Quantity>" + tblSerialNumber3rdLvl.Count() + "</cmn:Quantity>");
                                    writer.Append("<cmn:SerialNumberList>");
                                    if (tblSerialNumber3rdLvl.Count() > 0)
                                    {
                                        for (int k = 0; k < tblSerialNumber3rdLvl.Count(); k++)
                                        {

                                            if (tblSerialNumber3rdLvl[k]["SSCC"].ToString() == "")
                                            {
                                                serialNumber = "01" + tblSerialNumber3rdLvl[k]["JD_GTIN"] + "21" + tblSerialNumber3rdLvl[k]["Code"];
                                            }
                                            else
                                            {
                                                if (tblSerialNumber3rdLvl[k]["remarks"].ToString() == "")
                                                {
                                                    serialNumber = "00" + tblSerialNumber3rdLvl[k]["SSCC"];
                                                }
                                                else
                                                {
                                                    serialNumber = "00" + tblSerialNumber3rdLvl[k]["remarks"];
                                                }
                                            }
                                            writer.Append("<cmn:SerialNumber>" + serialNumber + "</cmn:SerialNumber>");
                                        }
                                    }
                                    writer.Append("</cmn:SerialNumberList>");
                                    writer.Append("</AggregationEventDetail>");
                                    if (tblSerialNumber3rdLvl.Count() > 0)//3rd last level for AggregationEvent
                                    {
                                        for (int m = 0; m > tblSerialNumber3rdLvl.Count(); m++)//Loop for 3rd Last level-eg ISH
                                        {
                                            if (cnt - 4 >= 0)
                                            {
                                                if (tblSerialNumber3rdLvl[m]["SSCC"].ToString() != "")
                                                {
                                                    if (tblSerialNumber3rdLvl[m]["remarks"].ToString() == "")
                                                    {
                                                        ParentSerialNumber = "00" + tblSerialNumber3rdLvl[m]["SSCC"].ToString();
                                                    }
                                                    else
                                                    {
                                                        ParentSerialNumber = "00" + tblSerialNumber3rdLvl[m]["remarks"].ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    ParentSerialNumber = "01" + tblSerialNumber3rdLvl[m]["JD_GTIN"] + "21" + tblSerialNumber3rdLvl[m]["Code"]; ;
                                                }
                                                Nextlvlcode = tblSerialNumber3rdLvl[m]["Code"].ToString();
                                                mfg = getGMT0DateTime(Convert.ToDateTime(tblSerialNumber3rdLvl[m]["CreatedDate"]).AddMinutes(2));
                                                mfgdate = mfg.ToString("yyyy-MM-dd") + "T" + mfg.ToString("HH:mm:ss.ff");
                                                PcktypeCode = lvls[3];
                                                DataRow[] tblSerialNumber4thLvl = dt.Select("NextLevelCode='" + Nextlvlcode + "' and PackageTypeCode='" + PcktypeCode + "'");
                                                writer.Append("<AggregationEventDetail>");
                                                writer.Append("<cmn:EventDateTime>" + mfgdate + "</cmn:EventDateTime>");
                                                writer.Append("<cmn:EventTimeZoneOffset>+00:00</cmn:EventTimeZoneOffset>");
                                                writer.Append("<cmn:EventLocation>" + compSetting.GLN + "</cmn:EventLocation>");
                                                writer.Append("<cmn:ParentSerialNumber>" + ParentSerialNumber + "</cmn:ParentSerialNumber>");
                                                writer.Append("<cmn:Quantity>" + tblSerialNumber4thLvl.Count() + "</cmn:Quantity>");
                                                writer.Append("<cmn:SerialNumberList>");
                                                if (tblSerialNumber4thLvl.Count() > 0)
                                                {
                                                    for (int n = 0; n < tblSerialNumber4thLvl.Count(); n++)
                                                    {

                                                        if (tblSerialNumber4thLvl[n]["SSCC"].ToString() == "")
                                                        {
                                                            serialNumber = "01" + tblSerialNumber4thLvl[n]["JD_GTIN"] + "21" + tblSerialNumber4thLvl[n]["Code"];
                                                        }
                                                        else
                                                        {
                                                            if (tblSerialNumber4thLvl[n]["remarks"].ToString() == "")
                                                            {
                                                                serialNumber = "00" + tblSerialNumber4thLvl[n]["SSCC"];
                                                            }
                                                            else
                                                            {
                                                                serialNumber = "00" + tblSerialNumber4thLvl[n]["remarks"];
                                                            }
                                                        }
                                                        writer.Append("<cmn:SerialNumber>" + serialNumber + "</cmn:SerialNumber>");
                                                    }
                                                }
                                                writer.Append("</cmn:SerialNumberList>");
                                                writer.Append("</AggregationEventDetail>");
                                                if (tblSerialNumber4thLvl.Count() > 0)//4th Last Level AggregationEvent
                                                {
                                                    for (int o = 0; o < tblSerialNumber4thLvl.Count(); o++)//Loop for 4th Last Level-eg OBX
                                                    {
                                                        if (cnt - 5 >= 0)
                                                        {
                                                            if (tblSerialNumber4thLvl[o]["SSCC"].ToString() != "")
                                                            {
                                                                if (tblSerialNumber4thLvl[o]["remarks"].ToString() != "")
                                                                {
                                                                    ParentSerialNumber = "00" + tblSerialNumber4thLvl[o]["remarks"].ToString();
                                                                }
                                                                else
                                                                {
                                                                    ParentSerialNumber = "00" + tblSerialNumber4thLvl[o]["SSCC"].ToString();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ParentSerialNumber = "01" + tblSerialNumber4thLvl[o]["JD_GTIN"].ToString() + "21" + tblSerialNumber4thLvl[o]["Code"].ToString();
                                                            }
                                                            Nextlvlcode = tblSerialNumber4thLvl[o]["Code"].ToString();
                                                            mfg = getGMT0DateTime(Convert.ToDateTime(tblSerialNumber4thLvl[o]["CreatedDate"]).AddMinutes(2));
                                                            mfgdate = mfg.ToString("yyyy-MM-dd") + "T" + mfg.ToString("HH:mm:ss.ff");
                                                            PcktypeCode = lvls[4];
                                                            DataRow[] tblSerialNumber5thLvl = dt.Select("NextLevelCode='" + Nextlvlcode + "' and PackageTypeCode='" + PcktypeCode + "'");
                                                            writer.Append("<AggregationEventDetail>");
                                                            writer.Append("<cmn:EventDateTime>" + mfgdate + "</cmn:EventDateTime>");
                                                            writer.Append("<cmn:EventTimeZoneOffset>+00:00</cmn:EventTimeZoneOffset>");
                                                            writer.Append("<cmn:EventLocation>" + compSetting.GLN + "</cmn:EventLocation>");
                                                            writer.Append("<cmn:ParentSerialNumber>" + ParentSerialNumber + "</cmn:ParentSerialNumber>");
                                                            writer.Append("<cmn:Quantity>" + tblSerialNumber5thLvl.Count() + "</cmn:Quantity>");
                                                            writer.Append("<cmn:SerialNumberList>");
                                                            if (tblSerialNumber5thLvl.Count() > 0)
                                                            {
                                                                for (int p = 0; p < tblSerialNumber5thLvl.Count(); p++)//1st level-eg MOC
                                                                {

                                                                    if (tblSerialNumber5thLvl[p]["SSCC"].ToString() == "")
                                                                    {
                                                                        serialNumber = "01" + tblSerialNumber5thLvl[p].ItemArray[3] + "21" + tblSerialNumber5thLvl[p].ItemArray[1];
                                                                    }
                                                                    else
                                                                    {
                                                                        if (tblSerialNumber5thLvl[p]["remarks"].ToString() == "")
                                                                        {
                                                                            serialNumber = "00" + tblSerialNumber5thLvl[p].ItemArray[8];
                                                                        }
                                                                        else
                                                                        {
                                                                            serialNumber = "00" + tblSerialNumber5thLvl[p].ItemArray[11];
                                                                        }
                                                                    }
                                                                    writer.Append("<cmn:SerialNumber>" + serialNumber + "</cmn:SerialNumber>");
                                                                }
                                                            }
                                                            writer.Append("</cmn:SerialNumberList>");
                                                            writer.Append("</AggregationEventDetail>");
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }

                            }
                        }
                    }
                }
                writer.Append("</snx:AggregationEvent>");
                aggEvent = writer.ToString();
                return aggEvent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public StringBuilder getDispositionXMLold(int jid, bool IsMoc)
        {
            int comevent = 0;
            bool isSSCC = false;
            string text = string.Empty;
            List<string> lstcmpifix = new List<string>();
            int t = 1;
            DateTime mdtime;
            string FileControlNumber = DateTime.Now.ToString("yyyyMMddhhmmss");
            var data = (from j in db.Job join cm in db.M_Customer on j.CustomerId equals cm.Id where j.JID == jid select new { SenderId = cm.SenderId, ReceiverId = cm.ReceiverId }).FirstOrDefault();
            var companySettings = db.Settings.FirstOrDefault();
            ds = XmlData.AssignedData(jid, IsMoc);
            var job = db.Job.Where(x => x.JID == jid).FirstOrDefault();

            var cust = db.M_Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();
            if (job.ProviderId == 2)
            {
                if (cust.IsSSCC)
                {
                    isSSCC = true;
                }
                else
                {
                    isSSCC = false;
                }
            }

            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;
            using (writer = XmlWriter.Create(sw, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("SNXDispositionAssignedMessage", "urn:tracelink:mapper:sl:serial_number_exchange");
                writer.WriteAttributeString("xmlns", "snx", null, "urn:tracelink:mapper:sl:serial_number_exchange");
                writer.WriteAttributeString("xmlns", "cmn", null, "urn:tracelink:mapper:sl:commontypes");
                writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");

                writer.WriteStartElement("snx", "ControlFileHeader", null);
                if (data.SenderId != "")
                {
                    writer.WriteElementString("cmn", "FileSenderNumber", null, data.SenderId);
                }
                else
                {
                    writer.WriteElementString("cmn", "FileSenderNumber", null, "");
                }

                if (data.ReceiverId != "")
                {
                    writer.WriteElementString("cmn", "FileReceiverNumber", null, data.ReceiverId);
                }
                else
                {
                    writer.WriteElementString("cmn", "FileReceiverNumber", null, "");
                }
                writer.WriteElementString("cmn", "FileControlNumber", null, FileControlNumber);
                writer.WriteElementString("cmn", "FileDate", null, DateTime.Now.Date.ToString("yyyy-MM-dd"));
                writer.WriteElementString("cmn", "FileTime", null, DateTime.Now.ToString("hh:mm:ss"));
                writer.WriteEndElement();

                writer.WriteStartElement("snx", "MessageBody", null);

                DataTable tblTypeName = ds.Tables["TypeName"];
                string LastpckLevel = string.Empty;// = tblTypeName.Rows[tblTypeName.Rows.Count - 1].ItemArray[0].ToString();
                if (tblTypeName.Rows.Count > 0)
                {
                    var lvls = ProductPackageHelper.getAllDeck(jid.ToString());
                    lvls = ProductPackageHelper.sorttheLevels(lvls);
                    LastpckLevel = lvls.Last();
                    //tblTypeName.Rows[tblTypeName.Rows.Count - 1].ItemArray[0].ToString();
                    for (int i = 0; i < tblTypeName.Rows.Count; i++)
                    {
                        string pkgtype = tblTypeName.Rows[i].ItemArray[0].ToString();

                        DataTable tblPacktype = ds.Tables["Packtype"];
                        DataRow[] results = null;
                        if (pkgtype == LastpckLevel && pkgtype != "MOC")
                        {

                            results = tblPacktype.Select("PackageTypeCode='" + tblTypeName.Rows[i].ItemArray[0] + "' and JD_Deckcode='" + pkgtype + "' and SSCC is not null and (IsLoose is null or IsLoose=0)");

                        }
                        else
                        {
                            results = tblPacktype.Select("PackageTypeCode='" + tblTypeName.Rows[i].ItemArray[0] + "' and JD_Deckcode='" + pkgtype + "' and SSCC is null");
                        }
                        DataRow[] loosresult = null;
                        if (pkgtype != "MOC")
                        {

                            loosresult = tblPacktype.Select("PackageTypeCode='" + tblTypeName.Rows[i].ItemArray[0] + "' and JD_Deckcode='" + pkgtype + "' and SSCC is not null  and IsLoose=1");
                        }
                        if (results.Count() > 0)
                        {

                            writer.WriteStartElement("snx", "CommissionEvent", null);
                            for (int j = 0; j < results.Count(); j++)
                            {
                                if (results[j].ItemArray[9].ToString() == "" || results[j].ItemArray[9].ToString() == "False")
                                {
                                    comevent++;
                                    DateTime mfg = getGMT0DateTime(Convert.ToDateTime(results[j].ItemArray[6]));
                                    t++;
                                    string mfgdate = mfg.ToString("yyyy-MM-dd") + "T" + mfg.ToString("HH:mm:ss.ff");
                                    var offset = TimeZoneInfo.Local.BaseUtcOffset;
                                    string timezone = "+00:00";

                                    //  string GTIN = results[j].ItemArray[2].ToString();
                                    string GTIN = string.Empty;
                                    string serialnumber = results[j].ItemArray[1].ToString();
                                    var gtin = db.X_TracelinkUIDStore.Where(x => x.SerialNo == serialnumber).FirstOrDefault();
                                    if (gtin != null)
                                    {
                                        GTIN = gtin.GTIN;
                                    }
                                    string Batchno = results[j].ItemArray[4].ToString();
                                    string SSCC = string.Empty;
                                    if (isSSCC)
                                    {
                                        if (results[j].ItemArray[10].ToString() != "")
                                        {
                                            SSCC = results[j].ItemArray[10].ToString();
                                        }
                                        else
                                        {
                                            SSCC = results[j].ItemArray[7].ToString();
                                        }
                                    }
                                    else
                                    {
                                        SSCC = results[j].ItemArray[7].ToString();
                                    }

                                    DateTime Exp = Convert.ToDateTime(results[j].ItemArray[3]);
                                    string expdate = Exp.ToString("yyMMdd");

                                    writer.WriteStartElement("snx", "CommissionEventDetail", null);
                                    writer.WriteElementString("cmn", "EventDateTime", null, mfgdate);
                                    writer.WriteElementString("cmn", "EventTimeZoneOffset", null, timezone);
                                    if (pkgtype != LastpckLevel || pkgtype == "MOC")
                                    {
                                        if (gtin != null)
                                        {
                                            writer.WriteElementString("cmn", "SerialNumber", null, "01" + GTIN + "21" + serialnumber);
                                        }
                                        else
                                        {
                                            lstcmpifix.Add(pkgtype);
                                            writer.WriteElementString("cmn", "SerialNumber", null, "00" + serialnumber);
                                            string filterval = serialnumber.Substring(0, 1);
                                            writer.WriteElementString("cmn", "FilterValue", null, filterval);
                                        }
                                    }
                                    else
                                    {
                                        writer.WriteElementString("cmn", "SerialNumber", null, "00" + SSCC);
                                        string filterval = SSCC.Substring(0, 1);
                                        writer.WriteElementString("cmn", "FilterValue", null, filterval);
                                    }
                                    //writer.WriteElementString("cmn", "FilterValue", null, "0");
                                    if (pkgtype == "MOC")
                                    {
                                        writer.WriteElementString("cmn", "PackagingLevel", null, "EA");
                                    }

                                    if (pkgtype == "ISH")
                                    {
                                        writer.WriteElementString("cmn", "PackagingLevel", null, "CA");
                                    }

                                    if (pkgtype == "OBX")
                                    {
                                        writer.WriteElementString("cmn", "PackagingLevel", null, "PK");
                                    }

                                    if (pkgtype == "PAL")
                                    {
                                        writer.WriteElementString("cmn", "PackagingLevel", null, "PL");
                                    }

                                    writer.WriteEndElement();//End CommissionEventDetail

                                }
                            }


                            DataTable tblLine = ds.Tables["LineCode"];
                            DataRow[] drLineCode = tblLine.Select("JD_Deckcode='" + tblTypeName.Rows[i].ItemArray[0] + "'");
                            if (drLineCode.Count() > 0)
                            {
                                if (comevent > 0)
                                {
                                    writer.WriteStartElement("snx", "CommissionCommonAttributes", null);
                                    DateTime mfgd = Convert.ToDateTime(drLineCode[0].ItemArray[2]);
                                    string mdate = mfgd.ToString("yyyy-MM-dd");
                                    mdtime = mfgd;
                                    DateTime expd = Convert.ToDateTime(drLineCode[0].ItemArray[3]);
                                    string edate = expd.ToString("yyyy-MM-dd");
                                    string ProductionLineId = drLineCode[0].ItemArray[0].ToString();
                                    string LineManagerName = drLineCode[0].ItemArray[5].ToString();
                                    string ItemCode = drLineCode[0].ItemArray[6].ToString();
                                    string LotNumber = drLineCode[0].ItemArray[1].ToString();
                                    string POLine = drLineCode[0].ItemArray[0].ToString();
                                    string InternalMaterialCode = drLineCode[0].ItemArray[8].ToString();
                                    string CountryDrugCode = drLineCode[0].ItemArray[9].ToString();

                                    writer.WriteElementString("cmn", "EventLocation", null, companySettings.GLN);
                                    writer.WriteElementString("cmn", "ProductionLineId", null, ProductionLineId);
                                    //writer.WriteElementString("cmn", "LineManagerName", null, LineManagerName);

                                    if (pkgtype == LastpckLevel && pkgtype != "MOC")
                                    {
                                        writer.WriteElementString("cmn", "CompanyPrefix", null, companySettings.CompanyCode);

                                    }
                                    else if (lstcmpifix.Contains(pkgtype))
                                    {
                                        writer.WriteElementString("cmn", "CompanyPrefix", null, companySettings.CompanyCode);
                                    }
                                    writer.WriteStartElement("ItemDetail");
                                    if (InternalMaterialCode != "")
                                    {
                                        writer.WriteElementString("cmn", "InternalMaterialCode", null, InternalMaterialCode);
                                    }
                                    else
                                    {
                                        writer.WriteElementString("cmn", "InternalMaterialCode", null, "");
                                    }
                                    //writer.WriteElementString("cmn","ItemCode",null, ds2.Tables[0].Rows[i].ItemArray[6].ToString());
                                    writer.WriteStartElement("cmn", "ItemCode", null);
                                    // var job = db.Job.Where(x => x.JID == jid).FirstOrDefault();
                                    var MOCGTIN = db.JobDetails.Where(x => x.JD_JobID == jid && x.JD_Deckcode == "MOC").FirstOrDefault();
                                    if (job.ProviderId != 4)
                                    {
                                        if (MOCGTIN.JD_NTIN != null && MOCGTIN.JD_NTIN != "")
                                        {

                                            writer.WriteAttributeString("type", "NTIN");
                                        }
                                        else if (MOCGTIN.JD_GTIN != "")
                                        {
                                            writer.WriteAttributeString("type", "GTIN-14");
                                        }
                                        //var GTINorNTIN = db.M_TracelinkRequest.Where(tls => tls.GTIN == MOCGTIN.JD_GTIN).FirstOrDefault().SrnoType;

                                        //if (GTINorNTIN == "GTIN")
                                        //{
                                        //    writer.WriteAttributeString("type", "GTIN-14");
                                        //}
                                        //else
                                        //{
                                        //    writer.WriteAttributeString("type", "NTIN");
                                        //}

                                    }
                                    else
                                    {
                                        writer.WriteAttributeString("type", "GTIN-14");
                                    }


                                    writer.WriteString(ItemCode);
                                    writer.WriteEndElement();

                                    writer.WriteElementString("cmn", "LotNumber", null, LotNumber);
                                    writer.WriteElementString("cmn", "ExpirationDate", null, edate);
                                    writer.WriteElementString("cmn", "ManufacturingDate", null, mdate);
                                    writer.WriteEndElement();//End ItemDetail


                                    writer.WriteEndElement();//CommissionCommonAttributes
                                    writer.WriteEndElement();//End CommissionEvent
                                    comevent = 0;
                                }
                            }
                        }

                        if (pkgtype != "MOC")
                        {
                            if (loosresult.Count() > 0)
                            {
                                writer.WriteStartElement("snx", "CommissionEvent", null);
                                for (int j = 0; j < loosresult.Count(); j++)
                                {
                                    DateTime mfg = getGMT0DateTime(Convert.ToDateTime(loosresult[j].ItemArray[6]));
                                    t++;
                                    string mfgdate = mfg.ToString("yyyy-MM-dd") + "T" + mfg.ToString("HH:mm:ss.ff");
                                    var offset = TimeZoneInfo.Local.BaseUtcOffset;
                                    string timezone = "+00:00";

                                    //  string GTIN = results[j].ItemArray[2].ToString();
                                    string GTIN = string.Empty;
                                    string serialnumber = loosresult[j].ItemArray[1].ToString();
                                    var gtin = db.X_TracelinkUIDStore.Where(x => x.SerialNo == serialnumber).FirstOrDefault();
                                    if (gtin != null)
                                    {
                                        GTIN = gtin.GTIN;
                                    }
                                    string Batchno = loosresult[j].ItemArray[4].ToString();
                                    string SSCC = string.Empty;
                                    if (isSSCC)
                                    {
                                        if (loosresult[j].ItemArray[10].ToString() != "")
                                        {
                                            SSCC = loosresult[j].ItemArray[10].ToString();
                                        }
                                        else
                                        {
                                            SSCC = loosresult[j].ItemArray[7].ToString();
                                        }
                                    }
                                    else
                                    {
                                        SSCC = loosresult[j].ItemArray[7].ToString();
                                    }

                                    DateTime Exp = Convert.ToDateTime(loosresult[j].ItemArray[3]);
                                    string expdate = Exp.ToString("yyMMdd");

                                    writer.WriteStartElement("snx", "CommissionEventDetail", null);
                                    writer.WriteElementString("cmn", "EventDateTime", null, mfgdate);
                                    writer.WriteElementString("cmn", "EventTimeZoneOffset", null, timezone);

                                    writer.WriteElementString("cmn", "SerialNumber", null, "00" + SSCC);
                                    string filterval = serialnumber.Substring(0, 1);
                                    writer.WriteElementString("cmn", "FilterValue", null, filterval);



                                    //writer.WriteElementString("cmn", "FilterValue", null, "0");
                                    if (pkgtype == "MOC")
                                    {
                                        writer.WriteElementString("cmn", "PackagingLevel", null, "EA");
                                    }

                                    if (pkgtype == "ISH")
                                    {
                                        writer.WriteElementString("cmn", "PackagingLevel", null, "CA");
                                    }

                                    if (pkgtype == "OBX")
                                    {
                                        writer.WriteElementString("cmn", "PackagingLevel", null, "PK");
                                    }

                                    if (pkgtype == "PAL")
                                    {
                                        writer.WriteElementString("cmn", "PackagingLevel", null, "PL");
                                    }

                                    writer.WriteEndElement();//End CommissionEventDetail

                                }


                                DataTable tblLine = ds.Tables["LineCode"];
                                DataRow[] drLineCode = tblLine.Select("JD_Deckcode='" + tblTypeName.Rows[i].ItemArray[0] + "'");
                                if (drLineCode.Count() > 0)
                                {
                                    writer.WriteStartElement("snx", "CommissionCommonAttributes", null);
                                    DateTime mfgd = Convert.ToDateTime(drLineCode[0].ItemArray[2]);
                                    string mdate = mfgd.ToString("yyyy-MM-dd");
                                    mdtime = mfgd;
                                    DateTime expd = Convert.ToDateTime(drLineCode[0].ItemArray[3]);
                                    string edate = expd.ToString("yyyy-MM-dd");
                                    string ProductionLineId = drLineCode[0].ItemArray[0].ToString();
                                    string LineManagerName = drLineCode[0].ItemArray[5].ToString();
                                    string ItemCode = drLineCode[0].ItemArray[6].ToString();
                                    string LotNumber = drLineCode[0].ItemArray[1].ToString();
                                    string POLine = drLineCode[0].ItemArray[0].ToString();
                                    string InternalMaterialCode = drLineCode[0].ItemArray[8].ToString();
                                    string CountryDrugCode = drLineCode[0].ItemArray[9].ToString();

                                    writer.WriteElementString("cmn", "EventLocation", null, companySettings.GLN);
                                    writer.WriteElementString("cmn", "ProductionLineId", null, ProductionLineId);
                                    //writer.WriteElementString("cmn", "LineManagerName", null, LineManagerName);

                                    writer.WriteElementString("cmn", "CompanyPrefix", null, companySettings.CompanyCode);

                                    writer.WriteStartElement("ItemDetail");
                                    if (InternalMaterialCode != "")
                                    {
                                        writer.WriteElementString("cmn", "InternalMaterialCode", null, InternalMaterialCode);
                                    }
                                    else
                                    {
                                        writer.WriteElementString("cmn", "InternalMaterialCode", null, "");
                                    }
                                    //writer.WriteElementString("cmn","ItemCode",null, ds2.Tables[0].Rows[i].ItemArray[6].ToString());
                                    writer.WriteStartElement("cmn", "ItemCode", null);
                                    writer.WriteAttributeString("type", "GTIN-14");
                                    writer.WriteString(ItemCode);
                                    writer.WriteEndElement();

                                    writer.WriteElementString("cmn", "LotNumber", null, LotNumber);
                                    writer.WriteElementString("cmn", "ExpirationDate", null, edate);
                                    writer.WriteElementString("cmn", "ManufacturingDate", null, mdate);
                                    writer.WriteEndElement();//End ItemDetail


                                    writer.WriteEndElement();//CommissionCommonAttributes
                                    writer.WriteEndElement();//End CommissionEvent
                                }
                            }
                        }
                    }
                }


                string PcktypeCode = string.Empty;
                decimal jobid = Convert.ToDecimal(jid);
                if (!IsMoc && LastpckLevel != "MOC")
                {
                    writer.WriteStartElement("snx", "AggregationEvent", null);
                    DataTable tblLastAggregationLevelData = ds.Tables["LastLeveSSCC"];
                    DataRow[] drLastAggData = tblLastAggregationLevelData.Select("PackageTypeCode='" + LastpckLevel + "'");
                    if (drLastAggData.Count() > 0)
                    {
                        for (int i = 0; i < drLastAggData.Count(); i++)
                        {

                            DateTime mfg = getGMT0DateTime(Convert.ToDateTime(drLastAggData[i].ItemArray[2]).AddMinutes(2));
                            t++;
                            string GTIN = string.Empty;
                            string mfgdate = mfg.ToString("yyyy-MM-dd") + "T" + mfg.ToString("HH:mm:ss.ff");
                            string ParentSerialNumber = "";
                            if (isSSCC)
                            {
                                if (drLastAggData[i].ItemArray[6].ToString() == "")
                                {
                                    ParentSerialNumber = drLastAggData[i].ItemArray[1].ToString();
                                }
                                else
                                {
                                    ParentSerialNumber = drLastAggData[i].ItemArray[6].ToString();
                                }
                            }
                            else
                            {
                                ParentSerialNumber = drLastAggData[i].ItemArray[1].ToString();
                            }
                            string Nextlevelcode = drLastAggData[i].ItemArray[0].ToString();

                            int cnt = tblTypeName.Rows.Count;

                            if (cnt - 2 > 0)
                            {
                                PcktypeCode = tblTypeName.Rows[1].ItemArray[0].ToString();
                            }
                            else
                            {
                                PcktypeCode = "MOC";
                            }
                            writer.WriteStartElement("AggregationEventDetail");
                            writer.WriteElementString("cmn", "EventDateTime", null, mfgdate);
                            writer.WriteElementString("cmn", "EventTimeZoneOffset", null, "+00:00");
                            writer.WriteElementString("cmn", "EventLocation", null, companySettings.GLN);
                            writer.WriteElementString("cmn", "ParentSerialNumber", null, "00" + ParentSerialNumber);
                            //writer.WriteElementString("cmn", "PackedStatus", "");

                            DataRow[] tblSerialNumber = tblLastAggregationLevelData.Select("NextLevelCode='" + Nextlevelcode + "' and PackageTypeCode='" + PcktypeCode + "'");

                            string Quantity = tblSerialNumber.Count().ToString();
                            writer.WriteElementString("cmn", "Quantity", null, Quantity);
                            writer.WriteStartElement("cmn", "SerialNumberList", null);
                            for (int j = 0; j < tblSerialNumber.Count(); j++)
                            {
                                string serialno = string.Empty;
                                if (isSSCC)
                                {
                                    if (tblSerialNumber[j].ItemArray[6].ToString() == "")
                                    {
                                        serialno = tblSerialNumber[j].ItemArray[0].ToString();
                                    }
                                    else
                                    {
                                        serialno = tblSerialNumber[j].ItemArray[6].ToString();
                                    }
                                }
                                else
                                {
                                    serialno = tblSerialNumber[j].ItemArray[0].ToString();
                                }
                                var gtin = db.X_TracelinkUIDStore.Where(x => x.SerialNo == serialno).FirstOrDefault();
                                if (gtin != null)
                                {
                                    if (tblSerialNumber[j].ItemArray[6].ToString() == "" && tblSerialNumber[j].ItemArray[1].ToString() == "")
                                    {
                                        writer.WriteElementString("cmn", "SerialNumber", null, "01" + gtin.GTIN + "21" + serialno);
                                    }
                                    else
                                    {
                                        if (gtin.Type == "LSSCC" || gtin.Type == "SSCC")
                                        {
                                            writer.WriteElementString("cmn", "SerialNumber", null, "00" + serialno);
                                        }
                                        else
                                        {
                                            writer.WriteElementString("cmn", "SerialNumber", null, "01" + gtin.GTIN + "21" + serialno);
                                        }
                                    }
                                }
                                else
                                {
                                    writer.WriteElementString("cmn", "SerialNumber", null, "00" + serialno);
                                }
                            }
                            writer.WriteEndElement();//End SerialNumberList
                            writer.WriteEndElement();

                            if (tblSerialNumber.Count() > 0)
                            {
                                for (int l = 0; l < tblSerialNumber.Count(); l++)
                                {
                                    if (cnt - 3 >= 0)
                                    {
                                        ParentSerialNumber = tblSerialNumber[l].ItemArray[0].ToString();

                                        var mdate = db.PackagingDetails.Where(x => x.Code == ParentSerialNumber && x.JobID == jid).FirstOrDefault();
                                        mfg = getGMT0DateTime(Convert.ToDateTime(mdate.LastUpdatedDate).AddMinutes(2));
                                        //mfg =Convert.ToDateTime(tblSerialNumber[0].ItemArray[2]) ;
                                        t++;
                                        mfgdate = mfg.ToString("yyyy-MM-dd") + "T" + mfg.ToString("HH:mm:ss.ff");

                                        var gtin = db.X_TracelinkUIDStore.Where(x => x.SerialNo == ParentSerialNumber).FirstOrDefault();

                                        PcktypeCode = tblTypeName.Rows[2].ItemArray[0].ToString();
                                        if (isSSCC)
                                        {
                                            if (tblSerialNumber[l].ItemArray[6].ToString() == "" && tblSerialNumber[l].ItemArray[1].ToString() == "")
                                            {
                                                ParentSerialNumber = tblSerialNumber[l].ItemArray[0].ToString();
                                                Nextlevelcode = ParentSerialNumber;
                                            }
                                            else
                                            {
                                                ParentSerialNumber = tblSerialNumber[l].ItemArray[6].ToString();
                                                Nextlevelcode = tblSerialNumber[l].ItemArray[0].ToString();
                                            }
                                        }
                                        else
                                        {
                                            ParentSerialNumber = tblSerialNumber[l].ItemArray[0].ToString();
                                            Nextlevelcode = tblSerialNumber[l].ItemArray[0].ToString();
                                        }
                                        writer.WriteStartElement("AggregationEventDetail");
                                        writer.WriteElementString("cmn", "EventDateTime", null, mfgdate);
                                        writer.WriteElementString("cmn", "EventTimeZoneOffset", null, "+00:00");
                                        writer.WriteElementString("cmn", "EventLocation", null, companySettings.GLN);
                                        if (gtin != null)
                                        {
                                            if (tblSerialNumber[l].ItemArray[6].ToString() == "" && tblSerialNumber[l].ItemArray[1].ToString() == "")
                                            {
                                                writer.WriteElementString("cmn", "ParentSerialNumber", null, "01" + gtin.GTIN + "21" + ParentSerialNumber);
                                            }
                                            else
                                            {
                                                if (gtin.Type == "LSSCC" || gtin.Type == "SSCC")
                                                {
                                                    writer.WriteElementString("cmn", "ParentSerialNumber", null, "00" + ParentSerialNumber);
                                                }
                                                else
                                                {
                                                    writer.WriteElementString("cmn", "ParentSerialNumber", null, "01" + gtin.GTIN + "21" + ParentSerialNumber);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            writer.WriteElementString("cmn", "ParentSerialNumber", null, "00" + ParentSerialNumber);
                                        }
                                        //writer.WriteElementString("cmn", "PackedStatus", "");

                                        DataRow[] tblSerialNumber1 = tblLastAggregationLevelData.Select("NextLevelCode='" + Nextlevelcode + "' and PackageTypeCode='" + PcktypeCode + "'");
                                        Quantity = tblSerialNumber1.Count().ToString();
                                        writer.WriteElementString("cmn", "Quantity", null, Quantity);
                                        writer.WriteStartElement("cmn", "SerialNumberList", null);
                                        for (int j = 0; j < tblSerialNumber1.Count(); j++)
                                        {
                                            string serialno = tblSerialNumber1[j].ItemArray[0].ToString();
                                            var gtin1 = db.X_TracelinkUIDStore.Where(x => x.SerialNo == serialno).FirstOrDefault();
                                            if (gtin1 != null)
                                            {

                                                writer.WriteElementString("cmn", "SerialNumber", null, "01" + gtin1.GTIN + "21" + serialno);
                                            }
                                            else
                                            {
                                                writer.WriteElementString("cmn", "SerialNumber", null, "00" + serialno);
                                            }
                                        }
                                        writer.WriteEndElement();
                                        writer.WriteEndElement();

                                        if (cnt - 4 >= 0)
                                        {
                                            if (tblSerialNumber1.Count() > 0)
                                            {
                                                for (int m = 0; m < tblSerialNumber1.Count(); m++)
                                                {
                                                    ParentSerialNumber = tblSerialNumber1[m].ItemArray[0].ToString();
                                                    var mdate1 = db.PackagingDetails.Where(x => x.Code == ParentSerialNumber && x.JobID == jid).FirstOrDefault();
                                                    mfg = getGMT0DateTime(Convert.ToDateTime(mdate1.LastUpdatedDate).AddMinutes(2));
                                                    // mfg = Convert.ToDateTime(tblSerialNumber1[0].ItemArray[2]);
                                                    t++;
                                                    mfgdate = mfg.ToString("yyyy-MM-dd") + "T" + mfg.ToString("HH:mm:ss.ff");

                                                    var gtin2 = db.X_TracelinkUIDStore.Where(x => x.SerialNo == ParentSerialNumber).FirstOrDefault();

                                                    PcktypeCode = tblTypeName.Rows[3].ItemArray[0].ToString();

                                                    writer.WriteStartElement("AggregationEventDetail");
                                                    writer.WriteElementString("cmn", "EventDateTime", null, mfgdate);
                                                    writer.WriteElementString("cmn", "EventTimeZoneOffset", null, "+00:00");
                                                    writer.WriteElementString("cmn", "EventLocation", null, companySettings.GLN);
                                                    if (gtin2 != null)
                                                    {
                                                        writer.WriteElementString("cmn", "ParentSerialNumber", null, "01" + gtin2.GTIN + "21" + ParentSerialNumber);
                                                    }
                                                    else
                                                    {
                                                        writer.WriteElementString("cmn", "ParentSerialNumber", null, "00" + ParentSerialNumber);
                                                    }

                                                    DataRow[] tblSerialNumber2 = tblLastAggregationLevelData.Select("NextLevelCode='" + ParentSerialNumber + "' and PackageTypeCode='" + PcktypeCode + "'");
                                                    Quantity = tblSerialNumber2.Count().ToString();
                                                    writer.WriteElementString("cmn", "Quantity", null, Quantity);
                                                    writer.WriteStartElement("cmn", "SerialNumberList", null);
                                                    for (int j = 0; j < tblSerialNumber2.Count(); j++)
                                                    {
                                                        string serialno = tblSerialNumber2[j].ItemArray[0].ToString();
                                                        var gtin1 = db.X_TracelinkUIDStore.Where(x => x.SerialNo == serialno).FirstOrDefault();
                                                        if (gtin1 != null)
                                                        {
                                                            writer.WriteElementString("cmn", "SerialNumber", null, "01" + gtin1.GTIN + "21" + serialno);
                                                        }
                                                        else
                                                        {
                                                            writer.WriteElementString("cmn", "SerialNumber", null, "00" + serialno);
                                                        }
                                                    }
                                                    writer.WriteEndElement();
                                                    writer.WriteEndElement();





                                                    if (cnt - 5 >= 0)
                                                    {
                                                        if (tblSerialNumber2.Count() > 0)
                                                        {
                                                            for (int n = 0; n < tblSerialNumber2.Count(); n++)
                                                            {
                                                                ParentSerialNumber = tblSerialNumber2[n].ItemArray[0].ToString();
                                                                var mdate2 = db.PackagingDetails.Where(x => x.Code == ParentSerialNumber && x.JobID == jid).FirstOrDefault();
                                                                mfg = getGMT0DateTime(Convert.ToDateTime(mdate2.LastUpdatedDate).AddMinutes(2));
                                                                // mfg = Convert.ToDateTime(tblSerialNumber2[0].ItemArray[2]);
                                                                t++;
                                                                mfgdate = mfg.ToString("yyyy-MM-dd") + "T" + mfg.ToString("HH:mm:ss.ff");

                                                                var gtin3 = db.X_TracelinkUIDStore.Where(x => x.SerialNo == ParentSerialNumber).FirstOrDefault();

                                                                PcktypeCode = tblTypeName.Rows[4].ItemArray[0].ToString();

                                                                writer.WriteStartElement("AggregationEventDetail");
                                                                writer.WriteElementString("cmn", "EventDateTime", null, mfgdate);
                                                                writer.WriteElementString("cmn", "EventTimeZoneOffset", null, "+00:00");
                                                                writer.WriteElementString("cmn", "EventLocation", null, companySettings.GLN);
                                                                if (gtin3 != null)
                                                                {
                                                                    writer.WriteElementString("cmn", "ParentSerialNumber", null, "01" + gtin3.GTIN + "21" + ParentSerialNumber);
                                                                }
                                                                else
                                                                {
                                                                    writer.WriteElementString("cmn", "ParentSerialNumber", null, "00" + ParentSerialNumber);
                                                                }


                                                                DataRow[] tblSerialNumber3 = tblLastAggregationLevelData.Select("NextLevelCode='" + ParentSerialNumber + "' and PackageTypeCode='" + PcktypeCode + "'");
                                                                Quantity = tblSerialNumber3.Count().ToString();
                                                                writer.WriteElementString("cmn", "Quantity", null, Quantity);
                                                                writer.WriteStartElement("cmn", "SerialNumberList", null);
                                                                for (int j = 0; j < tblSerialNumber3.Count(); j++)
                                                                {
                                                                    string serialno = tblSerialNumber3[j].ItemArray[0].ToString();
                                                                    var gtin1 = db.X_TracelinkUIDStore.Where(x => x.SerialNo == serialno).FirstOrDefault();
                                                                    if (gtin1 != null)
                                                                    {
                                                                        writer.WriteElementString("cmn", "SerialNumber", null, "01" + gtin1.GTIN + "21" + serialno);
                                                                    }
                                                                    else
                                                                    {
                                                                        writer.WriteElementString("cmn", "SerialNumber", null, "00" + serialno);
                                                                    }
                                                                }
                                                                writer.WriteEndElement();
                                                                writer.WriteEndElement();

                                                                if (cnt - 6 >= 0)
                                                                {
                                                                    if (tblSerialNumber3.Count() > 0)
                                                                    {
                                                                        for (int o = 0; o < tblSerialNumber3.Count(); o++)
                                                                        {
                                                                            ParentSerialNumber = tblSerialNumber3[o].ItemArray[0].ToString();
                                                                            var mdate3 = db.PackagingDetails.Where(x => x.Code == ParentSerialNumber && x.JobID == jid).FirstOrDefault();
                                                                            mfg = getGMT0DateTime(Convert.ToDateTime(mdate3.LastUpdatedDate).AddMinutes(2));
                                                                            //  mfg = Convert.ToDateTime(tblSerialNumber3[0].ItemArray[2]);
                                                                            t++;
                                                                            mfgdate = mfg.ToString("yyyy-MM-dd") + "T" + mfg.ToString("HH:mm:ss.ff");

                                                                            var gtin4 = db.X_TracelinkUIDStore.Where(x => x.SerialNo == ParentSerialNumber).FirstOrDefault();

                                                                            PcktypeCode = tblTypeName.Rows[5].ItemArray[0].ToString();

                                                                            writer.WriteStartElement("AggregationEventDetail");
                                                                            writer.WriteElementString("cmn", "EventDateTime", null, mfgdate);
                                                                            writer.WriteElementString("cmn", "EventTimeZoneOffset", null, "+00:00");
                                                                            writer.WriteElementString("cmn", "EventLocation", null, companySettings.GLN);
                                                                            if (gtin4 != null)
                                                                            {
                                                                                writer.WriteElementString("cmn", "ParentSerialNumber", null, "01" + gtin4.GTIN + "21" + ParentSerialNumber);
                                                                            }
                                                                            else
                                                                            {
                                                                                writer.WriteElementString("cmn", "ParentSerialNumber", null, "00" + ParentSerialNumber);
                                                                            }
                                                                            //writer.WriteElementString("cmn", "PackedStatus", "");

                                                                            DataRow[] tblSerialNumber4 = tblLastAggregationLevelData.Select("NextLevelCode='" + ParentSerialNumber + "' and PackageTypeCode='" + PcktypeCode + "'");
                                                                            Quantity = tblSerialNumber4[0].ItemArray[3].ToString();
                                                                            writer.WriteElementString("cmn", "Quantity", null, Quantity);
                                                                            writer.WriteStartElement("cmn", "SerialNumberList", null);
                                                                            for (int j = 0; j < tblSerialNumber4.Count(); j++)
                                                                            {
                                                                                string serialno = tblSerialNumber4[j].ItemArray[0].ToString();
                                                                                var gtin1 = db.X_TracelinkUIDStore.Where(x => x.SerialNo == serialno).FirstOrDefault();
                                                                                if (gtin1 != null)
                                                                                {
                                                                                    writer.WriteElementString("cmn", "SerialNumber", null, "01" + gtin1.GTIN + "21" + serialno);
                                                                                }
                                                                                else
                                                                                {
                                                                                    writer.WriteElementString("cmn", "SerialNumber", null, "00" + serialno);
                                                                                }
                                                                            }
                                                                            writer.WriteEndElement();
                                                                            writer.WriteEndElement();
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }


                    }



                    writer.WriteEndElement();//End AggreagationEvent
                }
                writer.WriteEndElement();//End MessageBody
                writer.WriteEndElement();//End SNXDispositionAssignedMessage
                writer.WriteEndDocument();//End Document
            }
            return sw;
        }


        public StringBuilder getProductDetailXML(Job jb, Settings st, TLProductDetailViewModel vm, M_Customer cm, PackagingAsso pkg, PackagingAssoDetails pkgDetails)
        {

            //   ds = XmlData.ProductDetails(Convert.ToInt32(jb.JID));
            var country = db.Country.Where(x => x.TwoLetterAbbreviation == vm.CountryName).FirstOrDefault();
            string FileControlNumber = DateTime.Now.ToString("yyyyMMddhhmmss");
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;


            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    DateTime Exp = Convert.ToDateTime(ds.Tables["ProductDetails"].Rows[0].ItemArray[4]);
            //    string expdate = Exp.ToString("yyyyMMdd");
            using (writer = XmlWriter.Create(sw, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("snx", "SNXProductionOrderMessage", "urn:tracelink:mapper:sl:serial_number_exchange");
                writer.WriteAttributeString("xmlns", "cmn", null, "urn:tracelink:mapper:sl:commontypes");
                writer.WriteStartElement("snx", "ControlFileHeader", null);
                writer.WriteElementString("cmn", "FileSenderNumber", null, cm.SenderId);
                writer.WriteElementString("cmn", "FileReceiverNumber", null, cm.ReceiverId);
                writer.WriteElementString("cmn", "FileControlNumber", null, FileControlNumber);
                writer.WriteElementString("cmn", "FileDate", null, DateTime.Now.Date.ToString("yyyy-MM-dd"));
                writer.WriteElementString("cmn", "FileTime", null, DateTime.Now.ToString("hh:mm:ss"));
                writer.WriteEndElement();

                writer.WriteStartElement("snx", "MessageBody", null);
                writer.WriteElementString("cmn", "BatchStatus", null, "Active");
                writer.WriteStartElement("snx", "Product", null);
                writer.WriteElementString("cmn", "ProductionDate", null, pkg.CreatedDate.ToString("yyyy-MM-dd"));

                writer.WriteStartElement("cmn", "ManufacturerId", null);
                writer.WriteAttributeString("type", "GCP");
                writer.WriteString(st.CompanyCode);
                writer.WriteEndElement();

                writer.WriteElementString("cmn", "InternalMaterialCode", null, pkg.InternalMaterialCode);

                writer.WriteStartElement("cmn", "CountryDrugCode", null);
                writer.WriteAttributeString("type", "IN_PRODUCT_CODE");
                writer.WriteString(pkg.ProductCode);
                writer.WriteEndElement();

                writer.WriteElementString("cmn", "LotNumber", null, jb.BatchNo);
                writer.WriteElementString("cmn", "ExpirationDate", null, jb.ExpDate.ToString("yyyy-MM-dd"));

                //writer.WriteStartElement("cmn", "ReferenceDocuments", null);
                //writer.WriteElementString("cmn", "PONumber", null, jb.JobName);
                //writer.WriteElementString("cmn", "POLine", null, jb.LineCode);
                //writer.WriteElementString("cmn", "WorkOrderNumber", null, "98522888");
                //writer.WriteElementString("cmn", "ReferenceIdentifier", null, "6512888");
                //writer.WriteEndElement();//ReferenceDocuments

                //for (int i = 0; i < ds.Tables[""].Rows.Count; i++) ;
                writer.WriteStartElement("snx", "Quantities", null);
                string pkgType = "";
                if (pkgDetails.PackageTypeCode == "MOC")
                {
                    pkgType = "EA";
                }

                writer.WriteStartElement("cmn", "PackagingItemCode", null);
                string GTINorNTIN = pkgDetails.GTIN;
                if (jb.ProviderId != 4)
                {

                    if (GTINorNTIN != "")
                    {
                        writer.WriteAttributeString("type", "GTIN-14");
                    }
                    else
                    {
                        writer.WriteAttributeString("type", "NTIN");
                    }
                }
                else
                {
                    writer.WriteAttributeString("type", "GTIN-14");
                }
                if (GTINorNTIN != "")
                {
                    writer.WriteString(pkgDetails.GTIN);
                }
                else
                {
                    writer.WriteString(pkgDetails.NTIN);
                }
                writer.WriteEndElement();
                writer.WriteElementString("cmn", "PackagingLevel", null, pkgType);
                writer.WriteElementString("cmn", "Quantity", null, jb.Quantity.ToString());
                writer.WriteEndElement();//Quantities
                writer.WriteElementString("cmn", "CountryOfManufacture", null, "IN");
                //string forexport = string.Empty;
                //if (jb.ForExport.ToString() == "True")
                //{
                //    forexport = "true";
                //}
                //else
                //{
                //    forexport = "false";
                //}
                string TMrktExemption = string.Empty;
                if (vm.TMrktExemption.ToString() == "True")
                {
                    TMrktExemption = "true";
                }
                else
                {
                    TMrktExemption = "false";
                }
                writer.WriteElementString("cmn", "ForExport", null, TMrktExemption);
                if (TMrktExemption == "true")
                {
                    writer.WriteStartElement("snx", "ExportMarket", null);
                    writer.WriteElementString("cmn", "ExportToCountry", null, country.TwoLetterAbbreviation);

                    writer.WriteElementString("cmn", "TargetMarketExemption", null, TMrktExemption);
                    writer.WriteElementString("cmn", "TargetMarketExemptionReference", null, vm.TMrktRefNo);
                    writer.WriteElementString("cmn", "TargetMarketExemptionDate", null, vm.TMrktExemptionDate.ToString("yyyy-MM-dd"));


                    writer.WriteEndElement();//ExportMarket
                }
                writer.WriteStartElement("cmn", "UnitPrice", null);
                writer.WriteAttributeString("currencyCode", vm.currencyCode);
                writer.WriteString(vm.currencyRate);
                writer.WriteEndElement();

                writer.WriteEndElement();//Product
                writer.WriteEndElement();//MessageBody
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            // }
            return sw;
        }

        private DateTime getGMT0DateTime(DateTime input)
        {
            TimeSpan tss = TimeSpan.Parse("05:30");
            TimeSpan tsd = TimeSpan.Parse("00:00");
            TimeZoneInfo tsource = TimeZoneInfo.CreateCustomTimeZone("id1", tss, "dsp", "std");
            TimeZoneInfo tdest = TimeZoneInfo.CreateCustomTimeZone("id2", tsd, "dsp", "std");

            DateTime s = TimeZoneInfo.ConvertTime(input, tsource, tdest);

            return s;
        }
    }
}