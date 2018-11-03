using System;

public class Transfer
{
    public Transfer()
    {
        //Property for table data

        //Property for the SPROC data

        //Property for the live data
    }

    //Retrieve list of tables to read from data sources
    public static void setTableData() { }

    //Go and retrieve the data from SAP or other databases
    public static void getSAPDataFromSAP { }

    //Import the retrieved SAP data into SQL
    public static void getSAPDataIntoSQL { }

    //Set the stored procedures which need to get run
    public static void setSPROCData { }

    //Run the stored procedures
    public static void runSPROCS { }

    //Collate the data to be put into the live system 
    public static void setLiveData { }

    //Import the data into the live system.
    public static void putLiveDataIntoLive { }

}
