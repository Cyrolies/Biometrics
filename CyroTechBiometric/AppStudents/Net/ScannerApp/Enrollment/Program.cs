using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace StudentEnrollment
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                Application.Run(new StudentEnrollmentForm());
            
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Unhandled Thread Exception");
            // here you can log the exception ...
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
           
            Exception ex = (Exception)e.ExceptionObject;
            //Console.WriteLine("Exception Message : " + e.Message);
            //Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
            var appLog = new EventLog("Application");
            appLog.Source = "DSG Scanner Enrollment";
            appLog.WriteEntry("Exception Message : " + ex.Message + Environment.NewLine + "Stack Trace : " + ex.StackTrace.ToString());
            if (ex.InnerException != null)
            {
                appLog.WriteEntry("Exception Message : " + ex.InnerException.Message + Environment.NewLine + "Stack Trace : " + ex.InnerException.StackTrace.ToString());
            }
            MessageBox.Show(ex.Message +" inner " + ex.InnerException?.Message, "Unhandled UI Exception");
        }
        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            //Console.WriteLine("Exception Message : " + e.Message);
            //Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
            var appLog = new EventLog("Application");
            appLog.Source = "DSG Scanner Enrollment";
            appLog.WriteEntry("Exception Message : " + e.Message + Environment.NewLine + "Stack Trace : " + e.StackTrace.ToString());
            if (e.InnerException != null)
            {
                appLog.WriteEntry("Exception Message : " + e.InnerException.Message + Environment.NewLine + "Stack Trace : " + e.InnerException.StackTrace.ToString());
            }
        }
    }
}