namespace Data_Stats_Engine
{
    //Class to quickly map SAP query parameters to
    public class Query
    {
        public char Delimiter { get; set; }
        public string[] Fields { get; set; }
        public string NoData { get; set; }
        public string Options { get; set; }
        public string QueryTable { get; set; }
        public int RowCount { get; set; }
        public int RowSkips { get; set; }

        public Query(string queryTable)
        {
            //Define any default property values in here
            Delimiter = '~';
            QueryTable = queryTable;
            RowSkips = 0;
            RowCount = 0;
            NoData = "";
            Options = null;
            Fields = null;
        }
    } 
}
