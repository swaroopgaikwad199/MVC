using System;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Data;

public class DBConfig
{
    public DBConfig()
	{
	}
    public static string DataSource="SAGAR-PC";
    public static string Database = "UIDDB";
    public static string UserName = "sa";
    public static string Password = "sql";

    public static SqlConnection SqlConn;
    public static SqlCommand SqlComm;
    public static SqlDataAdapter SqlDA;
    public static SqlDataReader SqlDataReader1, SqlDataReader;
    public static DataSet Odbc_DS = new DataSet("myDs");
    public static object SQLRetdata = new object();
    public static DataTable Odbc_myTable = new DataTable();

    public static void ReadDatabaseSettings()  // This function is for getting IP and port from SetupBin Folder.
    {

        string IpFilePath = Application.StartupPath;
        string IpFileName = IpFilePath + "\\" + "SetupBin\\DBConfig.bin";


        StreamReader SR = new StreamReader(IpFileName);
        int i = 0;
        while (!SR.EndOfStream)
        {
            string ReturnedValue = SR.ReadLine();
            if (i == 0)
            {
                string[] str = ReturnedValue.Split('=');
                if (str.Length > 1)
                    DataSource = str[1].ToString();
            }
            else if (i == 1)
            {
                string[] str = ReturnedValue.Split('=');
                if (str.Length > 1)
                    Database = str[1].ToString();
            }
            else if (i == 2)
            {
                string[] str = ReturnedValue.Split('=');
                if (str.Length > 1)
                    UserName = str[1].ToString();
            }
            else if (i == 3)
            {
                string[] str = ReturnedValue.Split('=');
                if (str.Length > 1)
                    Password = str[1].ToString();
            }
            i++;
        }
        SR.Close();
    }

    public static void SQL_connection()
    {
        SqlConn = new SqlConnection("Data Source="+DataSource+ " ;Database="+Database+" ;uid="+UserName+" ;pwd="+Password);
        try
        {
            if (SqlConn.State == ConnectionState.Closed)
            {
                SqlConn.Open();
            }

        }
        catch (SqlException ex)
        {
            MessageBox.Show(ex.Message);//<BR/>   There should be no <BR/>
        }
    }
    public static void ExecuteQuery(string txtQuery)
    {
        try
        {

            SqlComm = SqlConn.CreateCommand();
            SqlComm.CommandText = txtQuery;
            SqlComm.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
             MessageBox.Show(ex.Message );
        }
    }
    public static void ExecuteReader(string txtQuery)
    {
        try
        {

            SqlComm = SqlConn.CreateCommand();
            SqlComm.CommandText = txtQuery;
            SqlDataReader = SqlComm.ExecuteReader();

        }
        catch (Exception)
        {

        }

    }

    public static int ExecuteScalar(string txtQuery)
    {
        try
        {

            SqlComm = SqlConn.CreateCommand();
            SqlComm.CommandText = txtQuery;
            SQLRetdata = SqlComm.ExecuteScalar();
            if (SQLRetdata != null)
            {
                int record = int.Parse(SQLRetdata.ToString());
                return record;
            }
            else
                return 0;
        }
        catch (Exception)
        {
            return -1;
        }

    }



}
