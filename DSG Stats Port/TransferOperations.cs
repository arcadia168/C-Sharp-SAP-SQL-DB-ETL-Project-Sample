using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Stats_Engine
{
    //Class used to invoke the various transfer operations carried out in the DSG stats port
    class TransferOperations
    {

        public static void FindTable()
        {

        }

        public static void FullTransfer()
        {

        }

        //The various SAP queries that get run to retrieve data from the SAP engine
        public static void GetBilling()
        {

            //Parse the parameters into a query object
            Query queryObject = new Query("VBRP")
            {
                Fields = new string[13]
                {
                    "VBELN",
                    "NETWR",
                    "POSNR",
                    "FKIMG",
                    "VRKME",
                    "PRSDT",
                    "PRCTR",
                    "AUBEL",
                    "AUPOS",
                    "VGBEL",
                    "VGPOS",
                    "MATNR",
                    "ERDAT"
                }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetBoMHeaders()
        {
            //Set up the connection to SAP Server 
            if (!SapConnectionManager.destinationIsInitialised)
            {
                SapConnectionManager.SetUpSapConnection();
            }

            //Parse the parameters into a query object
            Query queryObject = new Query("MAST")
            {
                Delimiter = '~'
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

            //Write the results into a file
            string filePath = Environment.CurrentDirectory + queryObject.QueryTable + "TableParsedSAPData.xml";
            testTable.WriteXml(filePath);
        }

        public static void GetBoMItems()
        {
            //Set up the connection to SAP Server 
            SapConnectionManager.SetUpSapConnection();

            //Parse the parameters into a query object
            Query queryObject = new Query("STPO")
            {
                Delimiter = '~',
                Fields = new string[5] { "STLNR", "IDNRK", "POSNR", "LKENZ", "POTX1" }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

            //Write the results into a file
            string filePath = Environment.CurrentDirectory + queryObject.QueryTable + "TableParsedSAPData.xml";
            testTable.WriteXml(filePath);
        }

        public static void GetBundleHeaders()
        {
            //Set up the connection to SAP Server 
            SapConnectionManager.SetUpSapConnection();

            //Parse the parameters into a query object
            Query queryObject = new Query("MAST")
            {
                Fields = new string[2] { "MATNR", "STLNR" }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

            //Write the results into a file
            //            string filePath = Environment.CurrentDirectory + queryObject.QueryTable + "TableParsedSAPData.xml";
            //            testTable.WriteXml(filePath);
        }

        public static void GetConditions()
        {

            Query queryObject = new Query("KONP")
            {
                Fields = new string[4]
                {
                    "KONWA",
                    "KBETR",
                    "KNUMH",
                    "KSCHL"
                },
                RowCount = 1000
            };


            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);


        }

        public static void GetContractLines()
        {

            Query queryObject = new Query("LIKP")
            {
                Fields = new string[1]
                {
                    "VBELN",
                }
            };


            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        //todo: this one is broken
        public static void GetCurrencyDetails()
        {

            Query queryObject = new Query("TCURR")
            {
                Fields = new string[1]
                {
                    "VBELN"      
                }
            };


            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);


        }

        public static void GetCustomers()
        {
            Query queryObject = new Query("KNA1")
            {
                Fields = new string[7] { "KUNNR", "NAME1", "LAND1", "STRAS", "ORT01", "REGIO", "PSTLZ" }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetCustomersAddresses()
        {

            Query queryObject = new Query("ADRC")
            {
                Fields = new string[2] { "NAME1", "CITY1" },
                RowCount = 1000
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetCustomerAddresses2()
        {

            Query queryObject = new Query("KNA1")
            {
                Fields = new string[6] { "KUNNR", "PSTLZ", "REGIO", "STRAS", "ORT01", "LAND1" },
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        //todo: the fields for this query are marked as being invalid
        public static void GetCustomersByCoCode()
        {

            Query queryObject = new Query("KNB1")
            {
                Fields = new string[2] 
                { 
                    "KUNNR", 
                    "MATNR"
                }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);


        }

        public static void GetDeliveries()
        {

            Query queryObject = new Query("LIPS")
            {
                Fields = new string[7] { "VBELN", "POSNR", "MATNR", "WERKS", "LFIMG", "VGBEL", "VGPOS" },
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetEina()
        {
            Query queryObject = new Query("EINA");

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetEine()
        {
            Query queryObject = new Query("EINE");

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetHr()
        {

            Query queryObject = new Query("PA0001");

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        //todo: this is another SAP query that doesn't work - because of params after KUNNR
        public static void GetLeadPartner()
        {

            Query queryObject = new Query("ZTNCPRQS")
            {
                //fields that have been commented out are returned as not being valid
                Fields = new string[2]
                {
                    "KUNNR",
                    //"BUKRS",
                    "ZQLVL"
                    //"ZTLTD"                   
                }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);


        }

        public static void GetMaterial()
        {

            Query queryObject = new Query("MARA")
            {
                Fields = new String[9]
                {
                    "MATNR", 
                    "MATKL", 
                    "MTPOS_MARA",
                    "NORMT",
                    "SPART",
                    "MSTAV",
                    "MSTAE",
                    "MTART",
                    "PRDHA"
                }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);


        }

        public static void GetMaterialData()
        {

            Query queryObject = new Query("MARC")
            {
                Fields = new String[3] { "MATNR", "WERKS", "PRCTR" }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);



        }

        public static void GetMaterialText()
        {

            Query queryObject = new Query("MAKT")
            {
                Fields = new String[2] { "MATNR", "MAKTX" }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetNotifications()
        {



        }

        //TODO: test this one more thoroughly as returs many records  
        public static void GetOpex()
        {

            Query queryObject = new Query("COSP")
            {
                Fields = new String[26]
                {
                    "TWAER",
                    "GJAHR",
                    "WKF001",
                    "WOG001",
                    "OBJNR",
                    "VERSN",
                    "VRGNG",
                    "KSTAR",
                    "WKF002",
                    "WOG002",
                    "WKF003",
                    "WOG003",
                    "WKF004",
                    "WKF005",
                    "WKF006",
                    "WOG006",
                    "WKF007",
                    "WOG007",
                    "WKF008",
                    "WKF009",
                    "WOG009",
                    "WKF010",
                    "WKF011",
                    "WOG011",
                    "WKF012",
                    "WOG012"
                },
                Options = "GJAHR > '2009' and (OBJNR like '%SD%' or  OBJNR  like '%DS%')",
                RowCount = 1000
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        //todo: doesn't work
        public static void GetPartners()
        {
            //Create query object to feed to SAP connector
            Query queryObject = new Query("EQUI")
            {
                // Fields = new string[3] { "KNREF", "KUNNR", "MAN" }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetPersonResForSales()
        {

        }

        public static void GetPos()
        {

            //Parse the parameters into a query object
            Query queryObject = new Query("EKKO")
            {
                Fields = new string[8] { "EBELN", "BUKRS", "BSART", "AEDAT", "ERNAM", "LIFNR", "EKORG", "WAERS" }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

            //Write the results into a file
            //            string filePath = Environment.CurrentDirectory + queryObject.QueryTable + "TableParsedSAPData.xml";
            //            testTable.WriteXml(filePath);
        }

        public static void GetRRec()
        {
            //Set up the connection to SAP Server 
            if (!SapConnectionManager.destinationIsInitialised)
            {
                SapConnectionManager.SetUpSapConnection();
            }

            //Parse the parameters into a query object
            Query queryObject = new Query("S600")
            {
                Fields = new string[7] { "VBELN", "POSNV", "MATNR", "KUNNR", "SPTAG", "ZZAMARR", "WAERK" }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetSalesItemTypes()
        {
            //Set up the connection to SAP Server 
            if (!SapConnectionManager.destinationIsInitialised)
            {
                SapConnectionManager.SetUpSapConnection();
            }

            //Parse the parameters into a query object
            Query queryObject = new Query("TVAPT")
            {
                Fields = new string[3] { "PSTYV", "VTEXT", "SPRAS" }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

            //Write the results into a file
            //todo: replace these writing to files with saving to staging area.
            //            string filePath = Environment.CurrentDirectory + queryObject.QueryTable + "TableParsedSAPData.xml";
            //            testTable.WriteXml(filePath);
        }

        public static void GetSalesLines()
        {

        }

        public static void GetSalesNos()
        {
            Query queryObject = new Query("VBAK")
            {
                Fields = new string[6] { "VBELN", "AUART", "KUNNR", "SPART", "VKBUR", "VKGRP" },
                RowCount = 1000
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetSalesSeatsLines()
        {

        }

        public static void GetSapColumns()
        {

        }

        public static void GetSapColumnsNames()
        {

        }

        public static void GetSapTables()
        {

        }

        public static void GetSerialCustomer()
        {

            Query queryObject = new Query("EQUI")
            {
                Fields = new string[3] 
                {
                    "SERNR", 
                    "KUNDE", 
                    "MATNR"
                },
                Options = "KUNDE like '0000203329'"
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);
        }

        public static void GetStaff()
        {
            Query queryObject = new Query("PA0001")
            {
                Fields = new string[13] 
                {
                    "KOSTL", 
                    "PERNR", 
                    "ZLINEMAN", 
                    "STELL",
                    "ZZDIV",
                    "ZZDEPT",
                    "AEDTM", 
                    "BEGDA", 
                    "ENDDA",
                    "PERSK",
                    "BUKRS", 
                    "BTRTL", 
                    "PLANS"
                }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetStaff2()
        {
            Query queryObject = new Query("PA0002")
            {
                Fields = new string[7] 
                {
                    "NACHN", 
                    "VORNA", 
                    "PERNR", 
                    "BEGDA",
                    "ENDDA",
                    "CNAME",
                    "RUFNM", 
                }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetStaffHierarchy()
        {
            Query queryObject = new Query("HRP1001")
            {
                Fields = new string[6] { "BEGDA", "ENDDA", "SUBTY", "AEDTM", "SOBID", "OBJID" },
                Options = "SUBTY like 'A002%'",
                RowCount = 1000
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetStaffJobs()
        {
            Query queryObject = new Query("T503T")
            {
                Fields = new string[3] { "PERSK", "PTEXT", "SPRSL" },
                RowCount = 1000
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void GetTableData()
        {

        }

        public static void GetTest()
        {

        }

        public static void InvestigateTable()
        {

        }

        public static void JobTitles()
        {

        }

        public static void OpenDatabaseX()
        {

        }

        public static void RevRec1()
        {

            Query queryObject = new Query("ZRR_CONTROL")
            {
                Fields = new string[7]
                {
                    "VBELN",
                    "MAIN",
                    "STATUS",
                    "RECALCULATE",
                    "ERROR",
                    "CHANGED_ON",
                    "OLD_ENGINE"
                }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void RevRec2()
        {

            Query queryObject = new Query("ZRR_FORECAST")
            {
                Fields = new string[7]
                {
                    "VBELN",
                    "POSNR",
                    "CURRENCY",
                    "CHANGED_ON",
                    "YEARPD",
                    "VERSION",
                    "AMOUNT"
                }
            };

            //Attempt to run the query
            DataTable testTable = SapConnectionManager.RetrieveSapDataSet("PRO", queryObject);

        }

        public static void ShowConnection()
        {

        }

        public static void Test()
        {

        }

    }
}
