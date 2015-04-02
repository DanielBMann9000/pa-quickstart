using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PASample.Wpf
{
    internal static class InstrumentationHelpers
    {
        internal static IReadOnlyDictionary<string, Action> TabStart
        {
            get
            {
                return new ReadOnlyDictionary<string, Action>(tabStartActions);
            }
        }

        internal static IReadOnlyDictionary<string, Action> TabStop
        {
            get
            {
                return new ReadOnlyDictionary<string, Action>(tabStopActions);
            }
        }

        private static readonly Dictionary<string, Action> tabStartActions = new Dictionary<string, Action>
        {
            {"About", StartAboutTiming},
            {"View Profile", StartShowIdentityTiming },
            {"Feedback Request", StartCaptureFeedbackTiming},
            {"View User Task", StartMeasurePerformanceTiming },
            {"Incident Alert", StartMonitorExceptionsTiming },
            {"Expense Request Page", StartExpenseRequestTiming }
        };

        private static readonly Dictionary<string, Action> tabStopActions = new Dictionary<string, Action>
        {
            {"About", StopAboutTiming},
            {"View Profile", StopShowIdentityTiming },
            {"Feedback Request", StopCaptureFeedbackTiming},
            {"View User Task", StopMeasurePerformanceTiming },
            {"Incident Alert", StopMonitorExceptionsTiming },
            {"Expense Request Page", StopExpenseRequestTiming }
        };
        private static void StartAboutTiming() { }
        private static void StartShowIdentityTiming() { }
        private static void StartCaptureFeedbackTiming() { }
        private static void StartMeasurePerformanceTiming() { }
        private static void StartMonitorExceptionsTiming() { }
        private static void StartExpenseRequestTiming() { }
        private static void StopAboutTiming() { }
        private static void StopShowIdentityTiming() { }
        private static void StopCaptureFeedbackTiming() { }
        private static void StopMeasurePerformanceTiming() { }
        private static void StopMonitorExceptionsTiming() { }
        private static void StopExpenseRequestTiming() { }
    }
}
