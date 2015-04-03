using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PreEmptive.Analytics.Common;
using PreEmptive.Analytics.NET;

namespace PASample.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SERVICE_URL = "http://pasample.azurewebsites.net";
        private const string EXPENSE_FEATURE = "Expense Request";

        private readonly string[] _reasons = { "Client Dinner", "Purchase Airfare", "Prepay Hotel", "Register for Conference" };
        private string _lastTab;

        public MainWindow()
        {
            InitializeComponent();

            this.licenseKeylbl.Content = PASample.Wpf.App.LicenseKey;
            this.departmentlbl.Content = PASample.Wpf.App.Department;

            this.favoriteColor.ItemsSource = nameToColor;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            this.expenseReason.ItemsSource = _reasons;
        }


        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            PAClientProvider.ReportException(ExceptionType.Uncaught, e.ExceptionObject as Exception, @"Exception bubbled up to the top of the stack.");
            MessageBox.Show("Unhandled Error simulated");
        }


        private void tabCtrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var source = e.Source as TabControl;
            if (source != null)
            {
                if (!string.IsNullOrEmpty(_lastTab))
                {
                    PAClientProvider.StopFeature(_lastTab);
                }
                _lastTab = ((TabItem)source.SelectedItem).Tag as string;
                PAClientProvider.StartFeature(_lastTab);
            }
        }

        #region Feedback Page

        public List<string> nameToColor = new List<string>
        {
            "Aqua","Black",
            "Blue","Fuschia",
            "Gray","Green",
            "Lime","Maroon",
            "Navy","Olive",
            "Purple","Red",
            "Silver","Teal",
            "White","Yellow"
        };

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (favoriteColor.SelectedIndex >= 0)
            {
                var keys = new ExtendedKeys();
                keys.Add("Happiness", slider.Value);
                keys.Add("Color", favoriteColor.SelectedValue.ToString());
                PAClientProvider.FeatureTick("Feedback Submitted", keys);

                MessageBox.Show("Thank you for submitting your feedback", "Feedback Submitted");
            }
            else
            {
                MessageBox.Show("Please select your favorite color.", "Error");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.Source;

            if (button.Content.Equals("Start"))
            {
                PAClientProvider.StartFeature("Performance Counter");
                button.Content = "Stop";
            }
            else
            {
                PAClientProvider.StopFeature("Performance Counter");
                button.Content = "Start";
            }
        }

        private void handledBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ThrowException();
            }
            catch (Exception de)
            {
                PAClientProvider.ReportException(ExceptionType.Caught, de, null);
                MessageBox.Show("Handled Error simulated and sent to PA");
            }

        }

        private int _zero = 0;
        private decimal ThrowException()
        {
            switch (Wpf.App.Rand.Next(0, 2))
            {

                case 0:
                    return 10 / _zero;
                case 1:
                    object a = null;
                    a.GetType();
                    break;
                case 2:
                    var b = "myString";
                    return b.LastIndexOf('m', 10);

            }
            return 0;
        }

        private void unhandledBtn_Click(object sender, RoutedEventArgs e)
        {
            ThrowException();
            MessageBox.Show("Unhandled Error simulated and sent to PA");
        }

        #endregion

        private void thrownBtn_Click(object sender, RoutedEventArgs e)
        {
            var ex = new System.ArgumentException("Argument is incorrect");
            MessageBox.Show("Thrown Error simulated and sent to PA");
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var strAmt = expenseAmount.Text;
            decimal amt;
            if (!decimal.TryParse(strAmt, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.CurrentCulture, out amt))
            {
                expenseAmount.Text = string.Empty;
                MessageBox.Show("You must enter a valid currency", "Invalid Amount");
                return;
            }

            var button = (Button)sender;
            button.IsEnabled = false;
            await SendExpense(amt, (string)expenseReason.SelectedValue);
            button.IsEnabled = true;

        }

        private static async Task SendExpense(decimal amount, string reason)
        {
            try
            {
                var client = new RestSharp.RestClient();
                var content = new RestSharp.RestRequest(new Uri(SERVICE_URL), RestSharp.Method.POST)
                {
                    RequestFormat = RestSharp.DataFormat.Json
                };

                content.AddJsonBody(new
                {
                    Amount = amount,
                    Reason = reason,
                    LicenseKey = PASample.Wpf.App.LicenseKey,
                    Id = Guid.NewGuid(),
                    Department = PASample.Wpf.App.Department
                });

                content.Resource = "api/Expense/Approve";
                client.BaseUrl = new Uri(SERVICE_URL);
                var uri = client.BuildUri(content);
                var response = await client.ExecuteTaskAsync(content);

                if (response.ResponseStatus != RestSharp.ResponseStatus.Completed)
                {
                    MessageBox.Show("Unknown Error Processing Request", "Error");
                    return;
                }

                var respStream = response.Content;
                var js = new Newtonsoft.Json.JsonSerializer();
                var expResponse = (ExpenseApprovalResponse)js.Deserialize(new Newtonsoft.Json.JsonTextReader(new StringReader(respStream)), typeof(ExpenseApprovalResponse));


                var msg = string.Empty;
                if (expResponse.Exception == null || string.IsNullOrEmpty(expResponse.Exception.Message))
                {
                    msg = "Your request was processed.";
                }
                else
                {
                    msg = "Your request was rejected because - " + expResponse.Exception.Message;
                }
                MessageBox.Show(msg, "Result");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", "Error");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
