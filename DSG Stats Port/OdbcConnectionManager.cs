using System;
using System.Data.Odbc;
using Oracle.DataAccess.Client;

namespace Data_Stats_Engine
{
    namespace Data_Stats_Engine
    {
        //ODBC connections - used for certain databases such as SQL Server
        public class OdbcConnectionManager
        {
            //Create a connection string
            public static OdbcConnection con { get; set; }
            public static string connectionString { get; set; }

            public static void Connect()
            {
                con = new OdbcConnection();

                //todo: NEED TO GET ALL PROPER CONNECTION INFO AND SET UP DSNS LOCALLY.

                //todo: generalise this to read connection string information from query parameters

                //ConnectLm – oracle in Orahome92
                //CRMDatamart – SQL Server
                //ETLDatamart – SQL Server
                //Export – mySQL ODBC 5.1 driver
                //Keil –  SQL Server
                //SAPStore – SQLServer
                //All the ODBC drviers that start with KPI are SQL Server 

                con.ConnectionString = "Driver={SQL Server};" +
                                       "Server=CRMDatamart;" +
                                       "DataBase=bo_edge_adw_crm" +
                                       "Uid=bo_read;" +
                                       "Pwd=XX_XXXX;";

                try
                {
                    Console.WriteLine("Now attempting to connect to ODBC database");
                    con.Open();
                    Console.WriteLine("Successfully connected to ODBC Database!");

                    //Instantiate an Oracle command and add our connection details to it
                    OdbcCommand cmd = con.CreateCommand();

                    //todo: read the command out of the existing system and test it here!
                    cmd.CommandText = "SELECT * FROM SYSTEM_LKP";

                    //Execute the command and use the datareader to display the data
                    OdbcDataReader reader = cmd.ExecuteReader();

                    Console.WriteLine("THE RESULTS FROM THE ODBC QUERY ARE: \n");

                    //whilst the read from the database is still occuring
                    int fieldCount = reader.FieldCount;
                    Console.Write(":");
                    for (int i = 0; i < fieldCount; i++)
                    {
                        String fName = reader.GetName(i);
                        Console.Write(fName + ":");
                    }

                    Console.WriteLine();

                    Console.WriteLine("\n\nFinished reading data from the Oracle database.");

                    Console.ReadLine();
                }
                catch (OracleException ex)
                {
                    Console.WriteLine("Error attempting to connect to Oracle data source: " + ex.Message);
                }
            }
        }
    }
}
