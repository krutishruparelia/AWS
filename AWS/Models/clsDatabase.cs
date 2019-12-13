using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;

namespace AWS.Models
{
    class clsDatabase
    {
      

        public string strModuleName = "Database Routines";
        public string strFunctionName = "";
        public string strExceptionMessage = "";

        string appath = Path.GetDirectoryName(HttpRuntime.AppDomainAppPath); //(Application.ExecutablePath);

        public SqlConnectionStringBuilder Sbu = new SqlConnectionStringBuilder();
        
        //public static SqlConnection Con = new SqlConnection(); 
        //public String GlobalConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=DL_CNFG_SFT_DB;Integrated Security=True;Asynchronous Processing=true;MultipleActiveResultSets=True";
        //public String GlobalConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog="++"DL_CNFG_SFT_DB;Integrated Security=True;Asynchronous Processing=true;MultipleActiveResultSets=True";

        public String GlobalConnectionString = "";

        public static SqlConnection GlobalConnection = new SqlConnection();

        public static SqlConnection GlobalConnection_Web = new SqlConnection();

        public SqlConnectionStringBuilder Sbu_Web = new SqlConnectionStringBuilder();
        
        public bool OpenConnection()
        {
            strFunctionName = "Open Database Connection";

            lock (this)
            {
                try
                {
                    appath = Path.GetDirectoryName(HttpRuntime.AppDomainAppPath);  //(Application.ExecutablePath);
                    appath = @appath + "\\" + "App_Data\\ARSDB.mdf";
                 
                    if (GlobalConnection.State.ToString().ToUpper() == "OPEN")
                    {
                        GlobalConnection.Close();
                    }

                    Sbu.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + appath + ";Integrated Security=True;Asynchronous Processing=true;MultipleActiveResultSets=True; Connect Timeout=300;";

                    //Sbu.ConnectionString = "Data Source=184.168.194.70;Initial Catalog=ph19324034461_;Persist Security Info=True;User ID=smbdb;Password=***********";

                    //Sbu.ConnectionString = "Data Source=azistatst.c7cwycb3wgvd.us-east-2.rds.amazonaws.com;Initial Catalog=myDb;Persist Security Info=True;User ID=awsazistaindustries;Password=azista123";

                    GlobalConnection.ConnectionString = Sbu.ConnectionString;

                    GlobalConnection.Open();

                    return true;
                }
                catch (Exception Ex)
                {
                    strExceptionMessage = Ex.Message;


                    return false;
                }
            }
        }

        public bool Connection_Web()
        {
            strFunctionName = "Web Database Connection";

            lock (this)
            {
                try
                {
                    if (GlobalConnection_Web.State.ToString().ToUpper() == "OPEN")
                    {
                        GlobalConnection_Web.Close();
                    }
                    //Sbu_Web.DataSource = @"AZISTAAERO039\SQLEXPRESS";
                   // Sbu_Web.DataSource = @"AZISTAAERO011\SQLEXPRESS";
                    //Sbu_Web.DataSource = @"SERVER2\SQLEXPRESS";
                    Sbu_Web.DataSource = @"3.13.189.69";
                    //Sbu_Web.InitialCatalog = "myDb";
                    Sbu_Web.InitialCatalog = "AWS";
                    //Sbu_Web.InitialCatalog = "Remote_AITS_DB";
                    Sbu_Web.IntegratedSecurity = false;
                    //Sbu_Web.IntegratedSecurity = true;
                    Sbu_Web.MultipleActiveResultSets = true;
                    //Sbu_Web.PersistSecurityInfo = false;
                    Sbu_Web.ConnectTimeout = 0;


                    Sbu_Web.UserID = "sa";
                    Sbu_Web.Password = "azista@1";

                    GlobalConnection_Web.ConnectionString = Sbu_Web.ConnectionString;
                    GlobalConnection_Web.Open();

                    return true;
                }
                catch (Exception Ex)
                {
                    strExceptionMessage = Ex.Message;


                    return false;
                }
            }
        }
        public bool OpenConnection_Web()
        {
            strFunctionName = "Open Web Database Connection";

            lock (this)
            {
                try
                {
                    if (GlobalConnection_Web.State.ToString().ToUpper() == "OPEN")
                    {
                        GlobalConnection_Web.Close();
                    }

                    //Sbu_Web.DataSource = @"AZISTAAERO039\SQLEXPRESS";
                    //Sbu_Web.DataSource = @"AZISTAAERO011\SQLEXPRESS";
                    //Sbu_Web.DataSource = @"SERVER2\SQLEXPRESS";
                    Sbu_Web.DataSource = @"172.22.1.131,1433";
                    //Sbu_Web.InitialCatalog = "myDb";
                    //Sbu_Web.InitialCatalog = "ATS";
                    Sbu_Web.InitialCatalog = "AWS";
                    Sbu_Web.IntegratedSecurity = false;
                    //Sbu_Web.InitialCatalog = "Remote_AITS_DB";
                    //Sbu_Web.IntegratedSecurity = true;
                    Sbu_Web.MultipleActiveResultSets = true;
                    //Sbu_Web.PersistSecurityInfo = false;
                    Sbu_Web.ConnectTimeout = 0;

                    Sbu_Web.UserID = "AWS";
                    Sbu_Web.Password = "Azista@1";

                    GlobalConnection_Web.ConnectionString = Sbu_Web.ConnectionString;
                    GlobalConnection_Web.Open();

                    return true;
                }
                catch (Exception Ex)
                {
                    strExceptionMessage = Ex.Message;


                    return false;
                }
            }
        }



        public bool CreateTable(string query, string strConnType)
        {
            strFunctionName = "Update Into Database-1";

            lock (this)
            {
                SqlCommand CreateCmd = new SqlCommand();

                try
                {
                    //UpdtCmd.Connection = GlobalConnection;

                    if (strConnType.Trim().ToUpper() == "LOCAL")
                    {
                        if (GlobalConnection.State.ToString().ToUpper() == "CLOSE")
                        {
                            OpenConnection();
                        }

                        CreateCmd.Connection = GlobalConnection;
                    }
                    else if (strConnType.Trim().ToUpper() == "WEB")
                    {
                        Connection_Web();
                        CreateCmd.Connection = GlobalConnection_Web;
                    }

                    CreateCmd.CommandText = query;

                    CreateCmd.ExecuteNonQuery();

                    if (CreateCmd != null)
                    {
                        CreateCmd.Dispose();
                    }

                    return true;
                }
                catch (Exception Ex)
                {
                    if (CreateCmd != null)
                    {
                        CreateCmd.Dispose();
                    }

                    strExceptionMessage = Ex.Message;

                   

                    return false;
                }
                finally
                {
                    if (CreateCmd != null)
                    {
                        CreateCmd.Dispose();
                    }
                }
            }
        }
        public DataSet FetchData_Table(string spname, string strConnType)
        {
            strFunctionName = "Fetch Data From Store Procedure - I";

            lock (this)
            {
                SqlDataAdapter da = null;
                DataSet ds = null;

                SqlCommand SelCmd = new SqlCommand();

                try
                {
                    if (strConnType.Trim().ToUpper() == "LOCAL")
                    {
                        Connection_Web();
                        SelCmd.Connection = GlobalConnection;
                    }
                    else if (strConnType.Trim().ToUpper() == "WEB")
                    {
                        Connection_Web();
                        SelCmd.Connection = GlobalConnection_Web;
                    }
                    SelCmd.CommandText = spname;
                

                    da = new SqlDataAdapter(SelCmd);
                    ds = new DataSet();

                    da.Fill(ds);

                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }

                    return ds;

                }
                catch (Exception Ex)
                {
                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }


                    strExceptionMessage = Ex.Message;


                    return ds;
                }
                finally
                {
                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }

                }
            }
        }
        public DataSet FetchData_SP(string spname, string strConnType)
        {
            strFunctionName = "Fetch Data From Store Procedure - I";

            lock (this)
            {
                SqlDataAdapter da = null;
                DataSet ds = null;

                SqlCommand SelCmd = new SqlCommand();

                try
                {
                    if (strConnType.Trim().ToUpper() == "LOCAL")
                    {
                        Connection_Web();
                        SelCmd.Connection = GlobalConnection;
                    }
                    else if (strConnType.Trim().ToUpper() == "WEB")
                    {
                        Connection_Web();
                        SelCmd.Connection = GlobalConnection_Web;
                    }
                    SelCmd.CommandText = spname;
                    SelCmd.CommandType = CommandType.StoredProcedure;
                    
                    da = new SqlDataAdapter(SelCmd);
                    ds = new DataSet();

                    da.Fill(ds);

                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }

                    return ds;
                  
                }
                catch (Exception Ex)
                {
                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }

                  
                    strExceptionMessage = Ex.Message;


                    return ds;
                }
                finally
                {
                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }

                }
            }
        }

        public SqlDataReader FetchData(string query, string strConnType)
        {
            strFunctionName = "Fetch Data From Database-1";

            lock (this)
            {
                SqlDataReader SelRdr = null;

                SqlCommand SelCmd = new SqlCommand();

                try
                {
                    //SelCmd.Connection = GlobalConnection;

                    if (strConnType.Trim().ToUpper() == "LOCAL")
                    {
                        if (GlobalConnection.State.ToString().ToUpper() == "CLOSE")
                        {
                            OpenConnection();
                        }

                        SelCmd.Connection = GlobalConnection;
                    }
                    else if (strConnType.Trim().ToUpper() == "WEB")
                    {
                        if (GlobalConnection_Web.State.ToString().ToUpper() == "CLOSE")
                        {
                            OpenConnection_Web();
                        }

                        SelCmd.Connection = GlobalConnection_Web;
                    }

                    SelCmd.CommandText = query;

                    SelRdr = SelCmd.ExecuteReader();

                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }

                    /////As return "Reader" value
                    //if (SelRdr != null)
                    //{
                    //    SelRdr.Dispose();
                    //}

                    return SelRdr;
                }
                catch (Exception Ex)
                {
                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }

                    /////As return "Reader" value
                    //if (SelRdr != null)
                    //{
                    //    SelRdr.Dispose();
                    //}

                    strExceptionMessage = Ex.Message;

                   // bool ExcpMesg = clsEDR.WriteIntoExceptionFile(strModuleName, strFunctionName, strExceptionMessage, clsGlobalData.strExceptionFileName, clsGlobalData.strLoc_ExceptionFile);

                    return SelRdr;
                }
                finally
                {
                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }

                    /////As return "Reader" value
                    //if (SelRdr != null)
                    //{
                    //    SelRdr.Dispose();
                    //}
                }
            }
        }
        public DataSet FetchDataset(string query, string strConnType)
        {
            strFunctionName = "Fetch Data From Database-2";

            lock (this)
            {
                SqlDataAdapter da = null;
                DataSet ds = null;

                SqlCommand SelCmd = new SqlCommand();

                try
                {
                    //SelCmd.Connection = GlobalConnection;

                    if (strConnType.Trim().ToUpper() == "LOCAL")
                    {
                        //if (GlobalConnection.State.ToString().ToUpper() == "CLOSE")
                        //{
                        //    OpenConnection();
                        //}
                        Connection_Web();
                        SelCmd.Connection = GlobalConnection;
                    }
                    else if (strConnType.Trim().ToUpper() == "WEB")
                    {
                        //if (GlobalConnection_Web.State.ToString().ToUpper() == "CLOSE")
                        //{
                        //    OpenConnection_Web();
                        //}
                        Connection_Web();
                        SelCmd.Connection = GlobalConnection_Web;
                    }

                    SelCmd.CommandText = query;

                    //SelRdr = SelCmd.ExecuteReader();

                    //SqlCommand com = new SqlCommand("Select * from tbl_category", con);

                    da = new SqlDataAdapter(SelCmd);

                    ds = new DataSet();

                    da.Fill(ds);




                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }

                    /////As return "Reader" value
                    //if (SelRdr != null)
                    //{
                    //    SelRdr.Dispose();
                    //}

                    //return SelRdr;
                    return ds;
                }
                catch (Exception Ex)
                {
                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }

                    /////As return "Reader" value
                    //if (SelRdr != null)
                    //{
                    //    SelRdr.Dispose();
                    //}

                    strExceptionMessage = Ex.Message;

                  //  bool ExcpMesg = clsEDR.WriteIntoExceptionFile(strModuleName, strFunctionName, strExceptionMessage, clsGlobalData.strExceptionFileName, clsGlobalData.strLoc_ExceptionFile);

                    return ds;
                }
                finally
                {
                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }

                    /////As return "Reader" value
                    //if (SelRdr != null)
                    //{
                    //    SelRdr.Dispose();
                    //}
                }
            }
        }

        public DataSet FetchData_SP_columnName(string spname, string tblname, string strConnType)
        {
            strFunctionName = "Fetch Data From Database-1";

            lock (this)
            {
                SqlDataAdapter da = null;
                DataSet ds = null;

                SqlCommand SelCmd = new SqlCommand();

                try
                {


                    if (strConnType.Trim().ToUpper() == "LOCAL")
                    {

                        Connection_Web();
                        SelCmd.Connection = GlobalConnection;
                    }
                    else if (strConnType.Trim().ToUpper() == "WEB")
                    {

                        Connection_Web();
                        SelCmd.Connection = GlobalConnection_Web;
                    }



                    SelCmd.CommandText = spname;
                    SelCmd.CommandType = CommandType.StoredProcedure;

                    SelCmd.Parameters.AddWithValue("@TableName", tblname);

                    da = new SqlDataAdapter(SelCmd);

                    ds = new DataSet();

                    da.Fill(ds);

                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }


                    return ds;

                }
                catch (Exception Ex)
                {
                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }



                    strExceptionMessage = Ex.Message;

                   

                    return ds;
                }
                finally
                {
                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }


                }
            }
        }

        public DataSet FetchData_SP_MonthlyWeeklyData(string spname, string tblname,string ColumnName, string strConnType)
        {
            strFunctionName = "Fetch Data From Database-1";

            lock (this)
            {
                SqlDataAdapter da = null;
                DataSet ds = null;

                SqlCommand SelCmd = new SqlCommand();

                try
                {


                    if (strConnType.Trim().ToUpper() == "LOCAL")
                    {

                        Connection_Web();
                        SelCmd.Connection = GlobalConnection;
                    }
                    else if (strConnType.Trim().ToUpper() == "WEB")
                    {

                        Connection_Web();
                        SelCmd.Connection = GlobalConnection_Web;
                    }



                    SelCmd.CommandText = spname;
                    SelCmd.CommandType = CommandType.StoredProcedure;

                    SelCmd.Parameters.AddWithValue("@tb", tblname);
                    SelCmd.Parameters.AddWithValue("@ColumnName", ColumnName);




                    da = new SqlDataAdapter(SelCmd);

                    ds = new DataSet();

                    da.Fill(ds);

                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }


                    return ds;

                }
                catch (Exception Ex)
                {
                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }



                    strExceptionMessage = Ex.Message;



                    return ds;
                }
                finally
                {
                    if (SelCmd != null)
                    {
                        SelCmd.Dispose();
                    }


                }
            }
        }
    }
}
