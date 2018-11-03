using System.Configuration;
using SAP.Middleware.Connector;

namespace Data_Stats_Engine
{
    //Implement the interface which returns a configuration object used to make RFC calls to SAP
    public class SapDestinationConfig : IDestinationConfiguration
    {
        public bool ChangeEventsSupported()
        {
            return false;
        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        //Call this before making a call to SAP, sets up the connection details within SAP RFC objects.
        public RfcConfigParameters GetParameters(string destinationName)
        {
            //Popuplate a configuration object using the app.config values and the return to caller
            RfcConfigParameters parms = new RfcConfigParameters();

            parms.Add(RfcConfigParameters.Name, "PRO");
            parms.Add(RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["SAP_APPSERVERHOST"]);
            parms.Add(RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SAP_SYSTEMNUM"]);
            parms.Add(RfcConfigParameters.SystemID, ConfigurationManager.AppSettings["SAP_CLIENT"]);
            parms.Add(RfcConfigParameters.User, ConfigurationManager.AppSettings["SAP_USERNAME"]);
            parms.Add(RfcConfigParameters.Password, ConfigurationManager.AppSettings["SAP_PASSWORD"]);
            parms.Add(RfcConfigParameters.Client, ConfigurationManager.AppSettings["SAP_CLIENT"]);
            parms.Add(RfcConfigParameters.Language, ConfigurationManager.AppSettings["SAP_LANGUAGE"]);
            parms.Add(RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings["SAP_POOLSIZE"]);

            return parms;
        }
    }
}
