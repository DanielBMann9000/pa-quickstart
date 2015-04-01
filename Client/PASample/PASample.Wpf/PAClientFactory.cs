using PreEmptive.Analytics.Common;
using PreEmptive.Analytics.NET;

namespace PASample.Wpf
{
    internal class PAClientFactory
    {
        private static PAClient client;

        public static PAClient GetPAClient(string instance = null, string username = null)
        {
            if (client != null)
            {
                return client;
            }
            if (instance == null)
            {
                instance = App.GetLicenseKey();
            }

            var configuration = new Configuration("7d2b02e0-064d-49a0-bc1b-4be4381c62d3", "42AC2020-ABA1-9069-A2BD-98072B33309A")
            {
                ApplicationName = "Quickstart Sample",
                ApplicationType = "Sample",
                CompanyName = "My Company",
                InstanceID = instance,
                ApplicationVersion = "1.0",
                Endpoint = "so-s.info/endpoint",
                UseSSL = false,
                FullData = true
            };

            configuration.StopBehavior.SessionExtensionWindow = 15000;
            configuration.SupportOfflineStorage = true;

            client = new PAClient(configuration);

            return client;
        }

        public static void StartApplication(string instance, string department)
        {
            var keys = new ExtendedKeys();
            keys.Add("License", instance);
            keys.Add("Department", department);
            GetPAClient(instance).ApplicationStart(keys);
        }

        public static void StopApplication(bool immediate = false)
        {
            GetPAClient().ApplicationStop(immediate: immediate);
        }

        public static void FeatureTick(string name, ExtendedKeys keys = null)
        {
            GetPAClient().FeatureTick(name, keys);
        }

    }
}
