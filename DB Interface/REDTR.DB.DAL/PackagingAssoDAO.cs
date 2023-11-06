using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
	public partial class PackagingAssoDAO
	{
		public PackagingAssoDAO()
		{
			DbProviderHelper.GetConnection();
		}
		public List<PackagingAsso> GetPackagingAssos(int Flag, string Param1, string Param2)
		{
			try
			{
				List<PackagingAsso> lstPackagingAssos = new List<PackagingAsso>();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingAsso]", CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Param1));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, Param2));
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					PackagingAsso oPackagingAsso = new PackagingAsso();
					oPackagingAsso.PAID = Convert.ToDecimal(oDbDataReader["PAID"]);

					if (oDbDataReader["Name"] != DBNull.Value)
						oPackagingAsso.Name = Convert.ToString(oDbDataReader["Name"]);

					if (oDbDataReader["ProductCode"] != DBNull.Value)
						oPackagingAsso.ProductCode = Convert.ToString(oDbDataReader["ProductCode"]);

					if (oDbDataReader["FGCode"] != DBNull.Value) // Aparna.
						oPackagingAsso.FGCode = Convert.ToString(oDbDataReader["FGCode"]);

					if (oDbDataReader["VerifyProd"] != DBNull.Value)
						oPackagingAsso.VerifyProd = Convert.ToBoolean(oDbDataReader["VerifyProd"]);

					if (oDbDataReader["Description"] != DBNull.Value)
						oPackagingAsso.Description = Convert.ToString(oDbDataReader["Description"]);

					if (oDbDataReader["Remarks"] != DBNull.Value)
						oPackagingAsso.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

					if (oDbDataReader["IsActive"] != DBNull.Value)
						oPackagingAsso.IsActive = Convert.ToBoolean(oDbDataReader["IsActive"]);

					if (oDbDataReader["DAVAPortalUpload"] != DBNull.Value)
						oPackagingAsso.DAVAPortalUpload = Convert.ToBoolean(oDbDataReader["DAVAPortalUpload"]);
					oPackagingAsso.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);
					oPackagingAsso.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);
									
					if (oDbDataReader["DoseUsage"] != DBNull.Value)
						oPackagingAsso.DoseUsage = Convert.ToString(oDbDataReader["DoseUsage"]);
					if (oDbDataReader["ScheduledDrug"] != DBNull.Value)
						oPackagingAsso.ScheduledDrug = Convert.ToBoolean(oDbDataReader["ScheduledDrug"]);
					if (oDbDataReader["GenericName"] != DBNull.Value)
						oPackagingAsso.GenericName = Convert.ToString(oDbDataReader["GenericName"]);

					if (oDbDataReader["Composition"] != DBNull.Value)
						oPackagingAsso.Composition = Convert.ToString(oDbDataReader["Composition"]);

                    if (oDbDataReader["InternalMaterialCode"] != DBNull.Value)
                        oPackagingAsso.InternalMaterialCode = Convert.ToString(oDbDataReader["InternalMaterialCode"]); // by Vikrant

                    if (oDbDataReader["CountryDrugCode"] != DBNull.Value)
                        oPackagingAsso.CountryDrugCode = Convert.ToString(oDbDataReader["CountryDrugCode"]);    // by Vikrant
                    if (oDbDataReader["SaudiDrugCode"] != DBNull.Value)
                        oPackagingAsso.SaudiDrugCode = Convert.ToString(oDbDataReader["SaudiDrugCode"]);    // by Vikrant
                    if (oDbDataReader["UseExpDay"] != DBNull.Value)
						oPackagingAsso.UseExpDay = Convert.ToBoolean(oDbDataReader["UseExpDay"]);       // BY Ansuman
					if (oDbDataReader["ExpDateFormat"] != DBNull.Value)
						oPackagingAsso.ExpDateFormat = Convert.ToString(oDbDataReader["ExpDateFormat"]); // BY Ansuman

					if (oDbDataReader["ProductImage"] != DBNull.Value)
						oPackagingAsso.ProductImage = Convert.ToString(oDbDataReader["ProductImage"]);
                    if (oDbDataReader["FEACN"] != DBNull.Value)
                        oPackagingAsso.FEACN = Convert.ToString(oDbDataReader["FEACN"]);
                    if (oDbDataReader["NHRN"] != DBNull.Value)
                        oPackagingAsso.NHRN = Convert.ToString(oDbDataReader["NHRN"]);
                    //if (oDbDataReader["PublicKey"] != DBNull.Value)
                    //    oPackagingAsso.PublicKey = Convert.ToString(oDbDataReader["PublicKey"]);
                    //if (oDbDataReader["CompType"] != DBNull.Value)
                    //    oPackagingAsso.CompType = Convert.ToString(oDbDataReader["CompType"]);
                    lstPackagingAssos.Add(oPackagingAsso);
				}
				oDbDataReader.Close();
				return lstPackagingAssos;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public PackagingAsso GetPackagingAsso(int Flag, string Param1,string Param2)
		{
			try
			{
				PackagingAsso oPackagingAsso = new PackagingAsso();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingAsso]", CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Param1));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, Param2)); 
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					oPackagingAsso.PAID = Convert.ToDecimal(oDbDataReader["PAID"]);

					if(oDbDataReader["Name"] != DBNull.Value)
						oPackagingAsso.Name = Convert.ToString(oDbDataReader["Name"]);

					if (oDbDataReader["ProductCode"] != DBNull.Value)
						oPackagingAsso.ProductCode = Convert.ToString(oDbDataReader["ProductCode"]);
					if (oDbDataReader["FGCode"] != DBNull.Value)
						oPackagingAsso.FGCode = Convert.ToString(oDbDataReader["FGCode"]);

					if (oDbDataReader["VerifyProd"] != DBNull.Value)
						oPackagingAsso.VerifyProd = Convert.ToBoolean(oDbDataReader["VerifyProd"]); // Aparna.

					if(oDbDataReader["Description"] != DBNull.Value)
						oPackagingAsso.Description = Convert.ToString(oDbDataReader["Description"]);

					if(oDbDataReader["Remarks"] != DBNull.Value)
						oPackagingAsso.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

					if (oDbDataReader["IsActive"] != DBNull.Value)
						oPackagingAsso.IsActive = Convert.ToBoolean(oDbDataReader["IsActive"]);
					if (oDbDataReader["DAVAPortalUpload"] != DBNull.Value)
						oPackagingAsso.DAVAPortalUpload = Convert.ToBoolean(oDbDataReader["DAVAPortalUpload"]);

					oPackagingAsso.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);
					oPackagingAsso.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);

					if (oDbDataReader["ScheduledDrug"] != DBNull.Value)
						oPackagingAsso.ScheduledDrug = Convert.ToBoolean(oDbDataReader["ScheduledDrug"]);

					if (oDbDataReader["DoseUsage"] != DBNull.Value)
						oPackagingAsso.DoseUsage = Convert.ToString(oDbDataReader["DoseUsage"]);

					if (oDbDataReader["GenericName"] != DBNull.Value)
						oPackagingAsso.GenericName = Convert.ToString(oDbDataReader["GenericName"]);

					if (oDbDataReader["Composition"] != DBNull.Value)
						oPackagingAsso.Composition = Convert.ToString(oDbDataReader["Composition"]);

                    if (oDbDataReader["InternalMaterialCode"] != DBNull.Value)
                        oPackagingAsso.InternalMaterialCode = Convert.ToString(oDbDataReader["InternalMaterialCode"]); // by Vikrant

                    if (oDbDataReader["CountryDrugCode"] != DBNull.Value)
                        oPackagingAsso.CountryDrugCode = Convert.ToString(oDbDataReader["CountryDrugCode"]);    // by Vikrant
                    if (oDbDataReader["SaudiDrugCode"] != DBNull.Value)
                        oPackagingAsso.SaudiDrugCode = Convert.ToString(oDbDataReader["SaudiDrugCode"]);    // by Vikrant
                    if (oDbDataReader["ProductImage"] != DBNull.Value)
						oPackagingAsso.ProductImage = Convert.ToString(oDbDataReader["ProductImage"]);
				}
				oDbDataReader.Close();
				return oPackagingAsso;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int InsertOrUpdatePackagingAsso(int Flag, PackagingAsso oPackagingAsso)
		{
			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdatePackagingAsso]", CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProviderId", DbType.Int16, oPackagingAsso.ProviderId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Name", DbType.String, oPackagingAsso.Name));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductCode", DbType.String, oPackagingAsso.ProductCode));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FGCode", DbType.String, oPackagingAsso.FGCode));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID", DbType.Int32, oPackagingAsso.PAID));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Description", DbType.String, oPackagingAsso.Description));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oPackagingAsso.Remarks));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastUpdatedDate", DbType.DateTime, DateTime.Now));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RetID", DbType.Int64, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsActive", DbType.Boolean, oPackagingAsso.IsActive));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DAVAPortalUpload", DbType.Boolean, oPackagingAsso.DAVAPortalUpload));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@InternalMaterialCode", DbType.String, oPackagingAsso.InternalMaterialCode)); // By Vikrant Kumbhar
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountryDrugCode", DbType.String, oPackagingAsso.CountryDrugCode)); // By Vikrant Kumbhar 
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SaudiDrugCode", DbType.String, oPackagingAsso.SaudiDrugCode)); // By Vikrant Kumbhar 
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DosageForm", DbType.String, oPackagingAsso.DosageForm)); //By Pranita
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FEACN", DbType.String, oPackagingAsso.FEACN)); //By Pranita
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SubType", DbType.String, oPackagingAsso.SubType)); //By Pranita
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SubTypeNo", DbType.String, oPackagingAsso.SubTypeNo)); //By Pranita
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackageSpec", DbType.String, oPackagingAsso.PackageSpec)); //By Pranita
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AuthorizedNo", DbType.String, oPackagingAsso.AuthorizedNo)); //By Pranita
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SubTypeSpec", DbType.String, oPackagingAsso.SubTypeSpec)); //By Pranita
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackUnit", DbType.String, oPackagingAsso.PackUnit)); //By Pranita
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ResProdCode", DbType.String, oPackagingAsso.ResProdCode)); //By Pranita
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Workshop", DbType.String, oPackagingAsso.Workshop)); //By Pranita
                // if (oPackagingAsso.ScheduledDrug.HasValue)
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ScheduledDrug", DbType.Boolean, oPackagingAsso.ScheduledDrug));
			   // else
				 //   oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ScheduledDrug", DbType.Boolean, DBNull.Value));
				//if (oPackagingAsso.DoseUsage != null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DoseUsage", DbType.String, oPackagingAsso.DoseUsage));
                    
                // else
                //     oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DoseUsage", DbType.String, DBNull.Value));
                // if (oPackagingAsso.GenericName != null)
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@GenericName", DbType.String, oPackagingAsso.GenericName));
			   // else
			   //     oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@GenericName", DbType.String, DBNull.Value));
			   // if (oPackagingAsso.Composition != null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Composition", DbType.String, oPackagingAsso.Composition));
			   // else
				//    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Composition", DbType.String, DBNull.Value));
			   // if (oPackagingAsso.ProductImage != null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductImage", DbType.String, oPackagingAsso.ProductImage));
			   // else
				 //   oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductImage", DbType.String, DBNull.Value));
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@VerifyProd", DbType.Boolean, oPackagingAsso.VerifyProd));   // Aparna.

					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UseExpDay", DbType.Boolean, oPackagingAsso.UseExpDay));         // By Ansuman
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExpDateFormat", DbType.String, oPackagingAsso.ExpDateFormat));  // By Ansuman
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@NHRN", DbType.String, oPackagingAsso.NHRN));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PublicKey", DbType.String, oPackagingAsso.PublicKey));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CompType", DbType.String, oPackagingAsso.CompType));
                DbProviderHelper.ExecuteNonQuery(oDbCommand);
				return Convert.ToInt32(oDbCommand.Parameters["@RetID"].Value);
				//return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}	
		public int RemovePackagingAsso(Decimal PAID)
		{
			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_DeletePackagingAsso", CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID",DbType.Decimal,PAID));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
