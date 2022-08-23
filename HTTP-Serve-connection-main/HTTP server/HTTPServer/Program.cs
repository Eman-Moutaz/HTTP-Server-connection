using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {

        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            //Start server
            // 1) Make server object on port 1000
            // 2) Start Server
            CreateRedirectionRulesFile();
            Server serv = new Server(9000, "redirectionRules.txt");
            serv.StartServer();
        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
            if(!File.Exists("redirectionRules.txt"))
            {
                using(FileStream file_creation = File.Create("redirectionRules.txt"))
                {
                    Byte[] form = Encoding.ASCII.GetBytes("aboutus.html aboutus2.html");
                    file_creation.Write(form, 0, form.Length);
                }
            }
           
        }
         
    }
}
