using PreEmptive.Analytics.Common;
using PreEmptive.Analytics.NET;

namespace PASample.Wpf
{
    internal class PAClientProvider
    {
        private static PAClient client;

        public static PAClient GetPAClient()
        {
            if (client != null)
            {
                return client;
            }

            var configuration = new Configuration("7d2b02e0-064d-49a0-bc1b-4be4381c62d3", "42AC2020-ABA1-9069-A2BD-98072B33309A")
            {
                ApplicationName = "Quickstart Sample",
                ApplicationType = "Sample",
                CompanyName = "My Company",
                InstanceID = App.GetLicenseKey(),
                ApplicationVersion = "1.0",
                Endpoint = "so-s.info/endpoint",
                UseSSL = false,
                FullData = true,
                SupportOfflineStorage = true
            };

            client = new PAClient(configuration);

            return client;
        }


        public static void StartApplication(string instance, string department)
        {
            var keys = new ExtendedKeys();
            keys.Add("License", instance);
            keys.Add("Department", department);
            GetPAClient().ApplicationStart(keys);
        }

        public static void StopApplication(bool immediate = false)
        {
            GetPAClient().ApplicationStop(immediate: immediate);
        }

        public static void FeatureTick(string name, ExtendedKeys keys = null)
        {
            GetPAClient().FeatureTick(name, keys);
        }

        public static void StartFeature(string featureName, ExtendedKeys keys = null)
        {
            GetPAClient().FeatureStart(featureName, keys);

        }

        public static void StopFeature(string featureName, ExtendedKeys keys = null)
        {
            GetPAClient().FeatureStop(featureName, keys);
        }

    }
}
