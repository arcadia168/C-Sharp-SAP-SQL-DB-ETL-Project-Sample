using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Data_Stats_Engine
{

    class MySqlStagingConnectionManager
    {

        public string connectionString { get; set; }

        public void importDataIntoStagingArea(DataTable inputTable)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    //construct a string command using the ado.net data table

                    //todo: iterate over ado.net data table and build up an insert statement


                    string sql = "SHOW DATABASES";

                    MySqlConnection stagingConnection = new MySqlConnection(connectionString);

                    using (MySqlCommand cmd = new MySqlCommand(sql, stagingConnection))
                    {
                        //Open up a temporary connection to the database using the connection string.
                        stagingConnection.Open();

                        //Need to use the non-query to insert data
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException exception)
            {
                //log the exception that occured when trying to connect and throw to caller
                Console.WriteLine("MYSQLERROR OCCURED: " + exception.Message);

                //Throw this error back up the callstack
                throw;
            }
            
        }
    }
}
