using System;
using System.Data;
using Oracle.DataAccess.Client;

namespace Data_Stats_Engine
{
    //Oracle connections
    public class OracleConnectionManager
    {
        //private class property to hold the connection details.
        private OracleConnection con { get; set; }
        private OracleDataAdapter oraAdapter { get; set; }
        private OracleCommandBuilder oraBuilder { get; set; }
        private DataTable oracleResultsTable { get; set; }
        public String oracleConnectionString { get; set; }

        /// <summary>
        /// Used to connect to a given instance of an Oracle database, using supplied connection parameters.
        /// </summary>
        /// <param name="userId">The User Id with which to connect to the supplied database.</param>
        /// <param name="password">The password with which to connect to the supplied database.</param>
        /// <param name="dataSource">The database server which we are trying to connect to.</param>
        public void Connect(string userId, string password, string dataSource)
        {
            con = new OracleConnection();

            con.ConnectionString = "User Id=" + userId + ";" +
                                   "Password=" + password + ";" +
                                   "Data Source=" + dataSource + ";";

            try
            {
                Console.WriteLine("Now attempting to connect to Oracle database");
                con.Open();
                Console.WriteLine("Successfully connected to Oracle!");
            }
            catch (OracleException ex)
            {
                Console.WriteLine("Error attempting to connect to Oracle data source: " + ex.Message);
            }

        }

        /// <summary>
        /// Closes the connetion to the Oracle database.
        /// </summary>
        public void Close()
        {
            con.Close();
            con.Dispose();
        }

        /// <summary>
        /// Runs a query against the currently connected Oracle database
        /// </summary>
        /// <param name="oracleQuery">A query object to be parsed into the query to be run against the Oracle database</param>
        public DataTable RunOracleQuery(Query oracleQuery)
        {
            try
            {

                //Concatenate the fields into a single comma separated string
                String columnFields = String.Join(",", oracleQuery.Fields);

                string commandString = "SELECT " + columnFields + " FROM " + oracleQuery.QueryTable;

                if (oracleQuery.Options != null)
                {
                    commandString = commandString + "WHERE " + oracleQuery.Options;
                }

                //Instantiate the table to be returned
                DataTable oracleResultsTable = new DataTable(oracleQuery.QueryTable);

                //Go and add all the fields as columns to the table
                foreach (string currentField in oracleQuery.Fields)
                {
                    var column = new DataColumn { ColumnName = currentField };

                    //Add the column to the table
                    oracleResultsTable.Columns.Add(column);
                }

                using (OracleConnection connection = new OracleConnection(oracleConnectionString))
                {
                    Console.WriteLine("Beginning to read data from the database.");

                    OracleCommand command = new OracleCommand(commandString, connection);
                    connection.Open();
                    var reader = command.ExecuteReader();

                    // Always call Read before accessing data. 
                    while (reader.Read())
                    {
                        //Create a new row using the schema for the results table
                        DataRow row = oracleResultsTable.NewRow();

                        //Loop through the fields and print them out
                        foreach (String currentField in oracleQuery.Fields)
                        {
                            //Add the retrieved column into the data row
                            row[currentField] = reader[currentField];
                        }

                        //After adding all the columns to the row, add the row 
                        oracleResultsTable.Rows.Add(row);
                    }

                    // Always call Close when done reading.
                    reader.Close();
                }

                Console.WriteLine("\n\nFinished reading data from the Oracle database.");

                //Return the parsed Oracle data to the caller 
                return oracleResultsTable;
            }
            //handle any errors that might occur as a result of reading from the oracle database
            catch (OracleException oracleException)
            {
                //notify user about specific error and rethrow the exception
                Console.WriteLine("Attempting to read from the Oracle database resulted in the following error: " +
                                  oracleException.Message);

                //add more error handling here


                //rethrow the exception for the caller to handle.
                throw;
            }
            catch (InvalidOperationException connectionException)
            {
                //notify user of invalid connection
                Console.WriteLine(connectionException.Message);

                //add more error handling here

                //rethrow the exception for caller to handle
                throw;
            }
        }
    }
}
