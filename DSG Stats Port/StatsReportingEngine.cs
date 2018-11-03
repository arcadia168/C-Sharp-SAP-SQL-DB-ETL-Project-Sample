using System;
using System.Data;
using Data_Stats_Engine.Data_Stats_Engine;

namespace Data_Stats_Engine
{
    class SAP_Test_Connector
    {
        static void Main(string[] args)
        {
            //testOracleQuery();
            //testSAPQuery();
            //testODBCQuery();

            //test that the SAP pre-written queries in the transfer class allow for retrieval of information properly

            TransferOperations.GetOpex();
            // TransferOperations.GetPos();
            // TransferOperations.GetSalesItemTypes();
            //TransferOperations.GetCustomers();
        }


        public static void testODBCQuery()
        {
            //Instantiate an ODBC Connection object
            OdbcConnectionManager.Connect();
        }

        public static void testOracleQuery()
        {
            //Test the class used to connect to Oracle databases            
            OracleConnectionManager oracleTestConnection = new OracleConnectionManager();
            
            //Connect to the oracle data source 
            oracleTestConnection.oracleConnectionString = "User Id=BO_READ;Password=XXXXXXX;Data Source=CONPRO";

            //Instantiate the Oracle query object
            Query oracleQuery = new Query("PARAMETER_LKP")
            {
                Fields =
                    new string[7]
                    {"PL_ID", "FK_CATEGORY", "PL_NAME", "PL_DESC", "PL_ACTIVE", "FK_PARAMETER_TYPE", "PL_VALUE_TO_SHOW"}
            };

            //Populate the query object

            //Run the Oracle query passing in the above query object
            DataTable oracleQueryResults = new DataTable();

            //Do something with the results. Maybe save them to a file like with the SAP results.
            try
            {
                //run the oracle query - surround in a try as query operation may not work properly.
                oracleQueryResults = oracleTestConnection.RunOracleQuery(oracleQuery);

                //Parse the oracle results into a file

                //Write the results into a file
                string filePath = Environment.CurrentDirectory + oracleQuery.QueryTable + "TableParsedSAPData.xml";
                oracleQueryResults.WriteXml(filePath);

                //In addition to writing the results into a file, attempt to import them into the MySQL staging area
                MySqlStagingConnectionManager stagingConnection = new MySqlStagingConnectionManager();

                //Connect to MySQL DB and then attempt to insert the ADO.NET data table
                stagingConnection.connectionString = "server=localhost;user=root;database=devsys_ops;port=3306;password=XXXXXXXX";

                //Now attempt to feed the result ADO.NET data table into the appropriate table in the MySQL staging area
                stagingConnection.importDataIntoStagingArea(oracleQueryResults);

            }
            catch(Exception exception)
            {

                Console.WriteLine(exception.Message);
            }     
        }

        public static void testSAPQuery()
        {

            //Set up the connection to SAP Server 
            SapConnectionManager.SetUpSapConnection();

            //Now the connection has been set up, test the connection to the SAP server
            Boolean canConnect = SapConnectionManager.TestConnection("PRO");

            //Now try and retrieve some information
            if (canConnect)
            {
                //Attempt to make an RFC call to SAP Server and retrieve some data

                //Instantiate a query object and populate it. 

                //Parse the parameters into a query object
                Query queryObject = new Query("TVAPT")
                {
                    Delimiter = '~',
                    Fields = new string[3] { "PSTYV", "VTEXT", "SPRAS" }
                };

                //Attempt to run the query
                DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

                //Write the results into a file
                string filePath = Environment.CurrentDirectory + queryObject.QueryTable + "TableParsedSAPData.xml";
                testTable.WriteXml(filePath);
            }
        }

        public static void testStagingArea()
        {
            //Instantiate a connection to the staging area and attempt to read data
        }
    }
}
