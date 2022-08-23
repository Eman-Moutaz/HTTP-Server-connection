using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
     //   static StreamWriter sr = new StreamWriter("log.txt");

        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            //Datetime:
            //message:
            // for each exception write its details associated with datetime 

            if (!File.Exists("log.txt"))
            {
                using (StreamWriter sr = File.CreateText("log.txt"))
                {
                    sr.WriteLine("Date Time is : " + DateTime.Now);
                    sr.WriteLine("Exception : " + ex.Message);
                    sr.Close();
                }
            }
            else
            {
                using (StreamWriter sr = File.AppendText("log.txt"))
                {
                    sr.WriteLine("Date Time is : " + DateTime.Now);
                    sr.WriteLine("Exception : " + ex.Message);
                    sr.Close();
                }
            }
    
        }
    }
}
