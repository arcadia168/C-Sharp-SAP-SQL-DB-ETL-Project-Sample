
using System;
using System.Data;
using System.Runtime.CompilerServices;
using SAP.Middleware.Connector;

namespace Data_Stats_Engine
{
    public class SapConnectionManager
    {
        //public  DataSet parsedSapData { get; set; }

        private static RfcDestination rfcDestination;
        public static bool destinationIsInitialised;

        //Call this method to establish config to SAP server
        public static void SetUpSapConnection()
        {
            string destinationConfigName = "QAS";
            IDestinationConfiguration destinationConfiguration = null;

            if (!destinationIsInitialised)
            {
                destinationConfiguration = new SapDestinationConfig();
                destinationConfiguration.GetParameters(destinationConfigName);

                if (RfcDestinationManager.TryGetDestination(destinationConfigName) == null)
                {
                    RfcDestinationManager.RegisterDestinationConfiguration(destinationConfiguration);
                    destinationIsInitialised = true;
                }
            }
        }

        //Method to test the connection and configuration to SAP server
        public static bool TestConnection(string destinationName)
        {
            bool result = false;

            try
            {
                rfcDestination = RfcDestinationManager.GetDestination(destinationName);
                if (rfcDestination != null)
                {
                    rfcDestination.Ping();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception("Connection failure error:" + ex.Message);
            }

            return result;
        }

        //Function to attempt to read a data table from sap if connected
        public static DataTable RetrieveSapDataSet(string destinationName, Query query)
        {

            //Create a new ADO.NET record set
            DataTable parsedSapData = new DataTable();

            try
            {
                //Check to see if a connection has been set up and if not, set one up manually
                if (!destinationIsInitialised)
                {
                    SetUpSapConnection();
                }

                //If the SAP RFC object has not been set up, do so now
                if (rfcDestination == null)
                {
                    rfcDestination = RfcDestinationManager.GetDestination(destinationName);
                }

                RfcRepository rfcRepository = rfcDestination.Repository;

                //Create a SAP RFC function object for the SAP function we have created on our SAP server.
                IRfcFunction rfcFunction = rfcRepository.CreateFunction("RFC_READ_TABLE");

                //Set the appropriate query values on the SAP function before making the query

                //to set the fields, get the fable from the query object and populate with the relevant field rows you wish to retrieve from the query
                IRfcTable fieldsRfcTable = rfcFunction.GetTable("FIELDS");

                //Add select option values to MATNRSELECTION table

                //Check to see if there are any fields as of yet to avoid a null reference exception
                if (query.Fields != null)
                {                    
                    foreach (String field in query.Fields)
                    {
                        //Create new FIELDS row
                        fieldsRfcTable.Append();

                        //Populate current FIELDS row with data from list

                        //PROBLEM IS WITH THIS LINE
                        fieldsRfcTable.SetValue("FIELDNAME", field);
                    }
                }

                //Just using test values for now.

                //todo: only set values here if they have been provided in the query
                rfcFunction.SetValue("DELIMITER", query.Delimiter);
                rfcFunction.SetValue("NO_DATA", query.NoData);
                rfcFunction.SetValue("QUERY_TABLE", query.QueryTable);
                //rfcFunction.SetValue("ROWCOUNT", query.RowCount);
                rfcFunction.SetValue("ROWSKIPS", query.RowSkips);

                //Now that the query has been constructed, attempt to make the call
                rfcFunction.Invoke(rfcDestination);

                //Declare loosely typed variables to hold SAP data structures
                IRfcTable sapDataTable = rfcFunction.GetTable("DATA");
                IRfcTable sapFieldsTable = rfcFunction.GetTable("FIELDS");
                IRfcTable sapOptionsTable = rfcFunction.GetTable("OPTIONS");

                DataTable adoDataTable = sapDataTable.ToDataTable("DATA");
                DataTable adoFieldsTable = sapFieldsTable.ToDataTable("FIELDS");
                DataTable adoOptionsTable = sapOptionsTable.ToDataTable("OPTIONS");

                //Parse returned data into sensible format using field and data table
                //parsedSapData = ParseSapData(adoDataTable, adoFieldsTable, query.QueryTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine("RFC_READ_TABLE ERROR: " + ex.Message);

                //return the exception back up to the caller
                throw;
            }

            return null;
            //return parsedSapData;
        }

        //Function to take the data returned by SAP and output an ADO.NET DataTable with correct column values
        private static DataTable ParseSapData(DataTable tableDataRows, DataTable tableColumns, string tableName)
        {
            //Define the table to be returned by the function
            DataTable parsedData = new DataTable(tableName);

            //Iterate over each of the fields returned (one field per row) and add each as a column to the result data table
            foreach (DataRow fieldRow in tableColumns.Rows)
            {
                //Add column to data table
                string currentFieldName = fieldRow[0].ToString();
                parsedData.Columns.Add(currentFieldName);
            }

            //Now split each row string of info into separate column data and populate table
            foreach (DataRow sapRow in tableDataRows.Rows)
            {
                //Define a new table row to add to the table
                DataRow row = parsedData.NewRow();

                //Extract the singular raw data row from the returned SAP data
                string unparsedRowString = sapRow.ItemArray[0].ToString();

                //Iterate over each of the fields by iterating over each of the ROWS in the table
                foreach (DataRow field in tableColumns.Rows)
                {
                    //Define variable to store column value in
                    string parsedColumn = null;

                    //Get the offset and the length from the columns within each of the field's ROWS.
                    String offset = field[1].ToString();
                    String length = field[2].ToString();

                    offset = offset.TrimStart('0').Length > 0 ? offset.TrimStart('0') : "0";
                    length = length.TrimStart('0').Length > 0 ? length.TrimStart('0') : "0";

                    //Convert the length and offset values to strings
                    int offsetNum;
                    int lengthNum;

                    if (Int32.TryParse(offset, out offsetNum) && Int32.TryParse(length, out lengthNum))
                    {
                        //Extract the column value from the row (outer loop) starting from offset and running to the length
                        parsedColumn = unparsedRowString.Substring(offsetNum, lengthNum);
                    }

                    //Get the name of the column 
                    string columnName = (string)field["Fieldname"];

                    //Populate the relevant column with data
                    row[columnName] = parsedColumn;
                }

                //Now that all of the columns have been populated for the given new row, add this row to the results table
                parsedData.Rows.Add(row);
            }

            //Now that all rows have been parsed properly into a data table, return this table
            return parsedData;
        }
    }
}
