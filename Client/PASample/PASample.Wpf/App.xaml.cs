using System;
using System.Windows;

namespace PASample.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Random Rand
        {
            get { return _rand; }
        }

        private static readonly Random _rand = new Random();

        private static readonly string[] _keys = { "ABC-123", "XYZ-432", "EFG-234", "HIJ-012", "YUI-765", "ITI-912", "LKL-876", "TRE-432", "LIK-101", "TRE-111" };
        private static readonly string[] _userNames = { "Sue", "Josh", "Fred", "Nathan", "Bill", "Mark", "Gabe", "Joe", "Pat", "Cindy", "Laura", "Emily" };

        private static readonly string[] _departments = { "Executive Management", "Human Resources", "IT Operations", "Sales", "Marketing", "Support", "Finance", "VIP", "Evaluation" };
        private static readonly string _department = _departments[_rand.Next(0, _departments.Length)];
        private static readonly string _userName = _userNames[_rand.Next(0, _userNames.Length)];
        private static readonly string _licenseKey = GetLicenseKey();

        internal static string GetLicenseKey()
        {
            return _keys[_rand.Next(0, _keys.Length - 1)];
        }

        public static string LicenseKey
        {
            get
            {
                return _licenseKey;
            }
        }

        public static string UserName
        {
            get
            {
                return _userName;
            }
        }
        public static string Department
        {
            get { return _department; }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            PAClientFactory.StopApplication(true);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            PAClientFactory.StartApplication(_licenseKey, _department);
        }
    }
}
