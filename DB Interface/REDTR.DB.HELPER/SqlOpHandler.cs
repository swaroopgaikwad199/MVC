using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Data;
using System.Windows.Forms;
using REDTR.UTILS;

namespace REDTR.HELPER
{
   public class SqlDBHandler
    {
       public static string GetLogin(string databaseServer, string userName, string userPass, string database)
       {
           return "server=" + databaseServer + ";database=" + database + ";User ID=" + userName + ";Password=" + userPass + ";PersistSecurityInfo=true;pooling=false";
       }

       public static string GetScript(string Scriptname)
       {
           //Assembly asm = Assembly.GetExecutingAssembly();
           //Stream str = asm.GetManifestResourceStream(asm.GetName().Name + "." + Scriptname);
           StreamReader reader = new StreamReader(Scriptname);//str);
           return reader.ReadToEnd();
       } 
       public static bool IsDBExists(string databaseServer, string userName, string userPass, string database)
       {
           string conStr = SqlDBHandler.GetLogin(
            databaseServer,
            userName,
            userPass,
             "master");

           SqlConnection sqlCon = new SqlConnection(conStr);
           string CMD = "SELECT name FROM master.dbo.sysdatabases WHERE name = N'" + database + "'";

           SqlCommand cmd = new SqlCommand(CMD, sqlCon);
           sqlCon.Open();
           SqlDataReader RD = cmd.ExecuteReader();
           while (RD.Read())
           {
               if (RD["name"] != null)
               {
                   return true;
               }
           }
           RD.Close();
           return false;
       }
       public static bool ISTableExist(string conStr, string Table)
       {
           SqlConnection sqlCon = new SqlConnection(conStr);
           string CMD = "SELECT * FROM sys.Tables WHERE name LIKE '%" + Table + "%'";

           SqlCommand cmd = new SqlCommand(CMD, sqlCon);
           sqlCon.Open();
           SqlDataReader RD = cmd.ExecuteReader();
           while (RD.Read())
           {
               if (RD["name"] != null)
               {
                   return true;
               }
           }
           RD.Close();
           return false;
       }
       
       public static void CreateFreshDB(string databaseServer, string userName, string userPass, string database)
       {

           string conStr = SqlDBHandler.GetLogin(
                databaseServer,
                userName,
                userPass,
                 "master");

           string Path = SettingsPath.DatabaseDirectory + "\\" + database;
           string Ext_DATA = ".mdf";
           string Ext_LOG = ".ldf";
           string DB_FILEPATH = Path + Ext_DATA;
           string DBLOG_FILEPATH = Path + Ext_LOG;
           string CREATEDB_CMD = "CREATE DATABASE [" + database + "] ON (NAME = '" + database + "DB_DATA" + "',FILENAME = '" + DB_FILEPATH + "',SIZE = 5 MB , FILEGROWTH = 5 MB) LOG ON (NAME = '" + database + "DB_LOG" + "',FILENAME = '" + DBLOG_FILEPATH + "',SIZE = 1 MB, FILEGROWTH = 5 MB)";
         
           SqlConnection sqlCon = new SqlConnection(conStr);
           try
           {
               string SqlDBCreation = "IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'" + database + "') DROP DATABASE [" + database + "]" + CREATEDB_CMD;//CREATE DATABASE [" + database + "]";
               SqlCommand cmd = new SqlCommand(SqlDBCreation, sqlCon);

               sqlCon.Open();
               cmd.ExecuteNonQuery();
           }
           catch (Exception ex)
           {
               Trace.TraceError("{0}:COMMAND SENT FOR DATABASE CREATION: {1} {2} {3}", DateTime.Now.ToString(), CREATEDB_CMD, ex.Message, ex.StackTrace);
               System.Windows.Forms.MessageBox.Show(ex.Message);
           }
           finally
           {
               if (sqlCon.State != ConnectionState.Closed) 
                   sqlCon.Close();
           }
       }

       public static string[] GetSqlLine(string ScriptPath)
       {
           Regex regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
           string txtSQL = GetScript(ScriptPath);//"InstallTnt.txt");   
           string[]  SqlLine = regex.Split(txtSQL);
           return SqlLine;
       }
       public static void ExecuteSql(string ScriptPath, SqlConnection sqlCon)
       {
           try
           {
               if (sqlCon.State != ConnectionState.Closed) sqlCon.Close();
               sqlCon.Open();

               string[] SqlLine = GetSqlLine(ScriptPath);

               SqlCommand cmd = sqlCon.CreateCommand();
               cmd.Connection = sqlCon;
             
               foreach (string line in SqlLine)
               {
                   if (line.Length > 0)
                   {
                       cmd.CommandText = line;
                       cmd.CommandType = CommandType.Text;
                       try
                       {
                           cmd.ExecuteNonQuery();
                       }
                       catch (SqlException ex)
                       {
                           ExecuteDrop(sqlCon);
                           throw ex;
                           //rollback
                           //break;
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               System.Windows.Forms.MessageBox.Show(ex.Message);
           }
           finally
           {
               if (sqlCon.State != ConnectionState.Closed) sqlCon.Close();
           }
       }
       public static void ExecuteDrop(SqlConnection sqlCon)
        {
            if (sqlCon.State != ConnectionState.Closed) sqlCon.Close();         
            try
            {                 
                sqlCon.Open();
                SqlCommand cmd = sqlCon.CreateCommand();
                cmd.Connection = sqlCon;
                cmd.CommandText = "IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'" + sqlCon.Database + "')	DROP DATABASE " + sqlCon.Database + "";//GetScript(ScriptPath);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlCon.State != ConnectionState.Closed) sqlCon.Close();

            }
        }     
    }

   public class SqlSpComparer
   {
       string mSourceDbConstr = string.Empty;
       string mTargetDBConstr = string.Empty;
       public SqlSpComparer(string SourceDbConstr, string TargetDBConstr)
       {
           mSourceDbConstr = SourceDbConstr;
           mTargetDBConstr = TargetDBConstr;
       }
       public void FunMergeSP()
       {
           int newSPs = 0, removedSPs = 0, changedSPs = 0, nochangeSPs = 0;
           DB DB = new DB();
           Sprocs SourceSp = DB.GetProcedures(mSourceDbConstr);
           Sprocs targetSP = DB.GetProcedures(mTargetDBConstr);
           foreach (SP S in SourceSp.List)
           {
               SP test = targetSP.FindProcedureByName(S.Name);
               if (test == null)
               {
                   DB.ExecuteSp(mTargetDBConstr, S);
                   Console.WriteLine("New SP Added" + S.Name);
                   ++newSPs;
               }
               else
               {
                   if (Sprocs.ProcedureNoChange(S, test) == false)
                   {
                       DB.ExecuteSp(mTargetDBConstr, S);
                       Console.WriteLine("Alter SP" + S.Name);
                       ++changedSPs;
                   }
                   else
                       ++nochangeSPs;
               }
           }
           foreach (SP T in targetSP.List)
           {
               SP S = SourceSp.FindProcedureByName(T.Name);
               if (S == null)
               {
                   DB.DROPSP(mTargetDBConstr, T.Name);
                   Console.WriteLine("Remove Unused SP" + S.Name);
                   ++removedSPs;
               }
           }
           if (newSPs == 0 && removedSPs == 0 && changedSPs == 0)
               Console.WriteLine("STORED PROCEDURES ARE UP TO DATE.NO ANY DIFF FOUND\r\nPRESS ENTER TO EXIT");
           else
               Console.WriteLine(newSPs + "-NEW SP ADDED," + changedSPs + "\r\nALTER SP EXECUSTION", removedSPs + "\r\n REMOVED SP");
          

       }
   }

   public class SqlTableComparer
   {

   }


   #region SupportedClasses
   public class ItemArgs<T> : EventArgs
   {
       public T Item;

       public ItemArgs(T item)
       {
           this.Item = item;
       }
   }
   public class DB
   {
       public DB()//string mConstr)
       {
           //Constr = mConstr;
       }
       public bool DROPSP(string Constr, string SPName)
       {
           try
           {
               SqlConnection SqlConn = new SqlConnection(Constr);
               SqlCommand Cmd = new SqlCommand("IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'" + SPName + "') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) DROP PROCEDURE " + SPName + "", SqlConn);
               Cmd.CommandType = CommandType.Text;
               SqlConn.Open();
               Cmd.ExecuteNonQuery();
               SqlConn.Close();
               return true;
           }
           catch (Exception)
           {
               return false;
           }
       }
       public bool ExecuteSp(string mConstr, SP sp)
       {
           try
           {
               DROPSP(mConstr, sp.Name);

               SqlConnection SqlConn = new SqlConnection(mConstr);
               SqlCommand Cmd = new SqlCommand(sp.Code, SqlConn);
               Cmd.CommandType = CommandType.Text;
               SqlConn.Open();
               Cmd.ExecuteNonQuery();
               return true;
           }
           catch (Exception ex)
           {
               throw ex;
           }
           //return false;
       }
       public Sprocs GetProcedures(string mConstr)
       {
           Sprocs procedures = new Sprocs();
           Sprocs.Connection = mConstr;

           string conString = mConstr;
           string sqlString = "select specific_schema, routine_name from information_schema.routines order by specific_schema, routine_name;  ";

           SqlConnection con = new SqlConnection(conString);
           SqlCommand cmd = new SqlCommand(sqlString, con);
           cmd.CommandType = CommandType.Text;

           try
           {
               cmd.Connection.Open();
               SqlDataReader rd = cmd.ExecuteReader(CommandBehavior.CloseConnection);
               if (rd.HasRows)
               {
                   while (rd.Read())
                   {
                       SP sp = new SP();
                       sp.Schema = rd["specific_schema"].ToString().Trim();
                       sp.Name = sp.Schema + "." + rd["routine_name"].ToString();
                       GetProcedureCode(sp);
                       procedures.List.Add(sp);
                   }
                   rd.Close();
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
           finally
           {
               if (con.State == ConnectionState.Open)
                   con.Close();
           }

           return procedures;
       }

       public void GetProcedureCode(SP procedure)
       {
           //clear
           procedure.code = "";
           string sqlString = "select	object_name(id) as routine_name " +
                             "         , cast( ctext as nvarchar(max)) as routine_definition " +
                             "         , colid		 " +
                             "from	sys.syscomments " +
                             "where	id = OBJECT_ID('" + procedure.Name + "')";

           SqlConnection con = new SqlConnection(Sprocs.Connection);
           SqlCommand cmd = new SqlCommand(sqlString, con);
           cmd.CommandType = CommandType.Text;

           try
           {
               cmd.Connection.Open();

               SqlDataReader rd = cmd.ExecuteReader(CommandBehavior.CloseConnection);
               if (rd.HasRows)
               {
                   StringBuilder sb = new StringBuilder();
                   while (rd.Read())
                   {
                       sb.Append(rd["routine_definition"].ToString());

                   }
                   procedure.code = sb.ToString().Trim();
               }
               rd.Close();
           }
           catch (Exception ex)
           {
               throw ex;
           }
           finally
           {
               if (con.State == ConnectionState.Open)
                   con.Close();
           }
       }
   }
   public class SP
   {

       internal string code;

       public enum ProcedureType
       {
           Unchanged = 0,
           Removed = 1,
           New = 2,
           Changed = 3
       }

       public string Schema { get; set; }
       public string Name { get; set; }
       public string Code
       {
           //lazy loading :)
           get
           {
               if (this.code == null && Sprocs.Connection != null)
               {
                   // process the sp info
                   if (code != null)
                   {
                       string[] astr = code.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                       //get teh summary
                       Summary = "";
                       string keyword = "--$summary";
                       foreach (string s in astr)
                       {
                           if (s.Contains(keyword))
                           {
                               Summary = (s.Replace(keyword, " ").Trim() + "\r\n");
                           }
                       }

                       //get all the params
                       Parameters = new List<string>();
                       keyword = "--$param";
                       foreach (string s in astr)
                       {
                           if (s.Contains(keyword))
                           {
                               Parameters.Add(s.Replace(keyword, " ").Trim() + "\r\n");
                           }
                       }
                       //get all the return value(s)
                       Returns = new List<string>();
                       keyword = "--$ret";
                       foreach (string s in astr)
                       {
                           if (s.Contains(keyword))
                           {
                               Returns.Add(s.Replace(keyword, " ").Trim() + "\r\n");
                           }
                       }

                       //get all the resultset(s)
                       ResultSets = new List<string>();
                       keyword = "--$result";
                       foreach (string s in astr)
                       {
                           if (s.Contains(keyword))
                           {
                               ResultSets.Add(s.Replace(keyword, " ").Trim().Remove(0, 2) + "\r\n");
                           }
                       }
                   }
               }
               return this.code;
           }
       }
       public List<string> Parameters { get; set; }
       public List<string> Returns { get; set; }
       public List<string> ResultSets { get; set; }
       public string Summary { get; set; }
       public SP OldSproc { get; set; }
       public ProcedureType Type { get; set; }

       public SP()
       {
           this.Schema = string.Empty;
           this.Name = string.Empty;
           this.code = null;
           this.Parameters = new List<string>();
           this.Returns = new List<string>();
           this.ResultSets = new List<string>();

           this.Type = ProcedureType.Unchanged;
       }

       public override string ToString()
       {
           return this.Name;
       } 
   }
   public class Sprocs
   {
       public List<SP> List;
       public static string Connection = string.Empty;
       public Sprocs()
       {
           List = new List<SP>();
       }

       public SP FindProcedureByName(string procName)
       {
           return List.Find(new Predicate<SP>(
               delegate(SP procedure)
               {
                   return procedure.Name == procName;
               }));
       }
       public static bool ProcedureNoChange(SP x, SP y)
       {

           return
               x.Name == x.Name
               &&
               x.Code.Equals(y.Code);
       }

   }
   #endregion END_SupportedClasses

}
