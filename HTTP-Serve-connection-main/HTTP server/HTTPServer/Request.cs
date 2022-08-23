using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines= new Dictionary<string, string>();

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        public RequestMethod Method
        {
            get { return method; }
        }
        public HTTPVersion Version
        {
            get { return httpVersion; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
           // throw new NotImplementedException();

            //TODO: parse the receivedRequest using the \r\n delimeter   

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)

            // Parse Request line

            // Validate blank line exists

            // Load header lines into HeaderLines dictionary
            if (ValidateBlankLine_and_spliting() == false)
                return false;
            if (ParseRequestLine() == true && LoadHeaderLines() == true && requestLines[1].ToLower().Contains("host: ") == true)
                return true;
             // request is good
            else
                return false;
            //  request is bad
        }

        private bool ParseRequestLine()
        {
          //  throw new NotImplementedException();
            //MPV[0] contains method
            //MPV[1] contains path
            //MPV[2] contains version
             try
            {
                string[] MPV = requestLines[0].Split(' ');
                
                if (MPV[0] == "GET")
                {
                       method = RequestMethod.GET;
             
                }
                else
                    return false;
                if (!ValidateIsURI(MPV[1]))
                {
                    return false;
                }
                if (MPV[2] == "HTTP/1.0" || MPV[2] == "HTTP/1.1" || MPV[2] == "")
                {
                    switch (MPV[2])
                    {
                        case "HTTP/1.0": httpVersion = HTTPVersion.HTTP10; 
                            break;
                        case "HTTP/1.1": httpVersion = HTTPVersion.HTTP11; 
                            break;
                            ////////////////////
                        case "": httpVersion = HTTPVersion.HTTP09; 
                            break;
                    }

                }
                else
                {
                    return false;
                }
               
                return true; //return true if all is good هنمسحها 
            }
            catch
            {
                return false;
            }

        }

        private bool ValidateIsURI(string uri)
        {
            
           // return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
            if (uri.First() == '/')
            {
                relativeURI = uri;
                return true;
            }
            return false;
            
           
        }

        private bool LoadHeaderLines()
        {
           // throw new NotImplementedException();
            try
            {
                foreach (string line in requestLines)
                {
                    if (line.Contains("GET"))
                    {
                        string[] header = line.Split(new string[] { " " }, StringSplitOptions.None);
                        headerLines.Add(header[0], header[1]);
                        //headerline[0] Like Host:   عايزين نمسحها
                        //headerline[1] Like 127.0.0.1:1000
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        
        }

        private bool ValidateBlankLine_and_spliting()
        {
            // throw new NotImplementedException();
            string[] breaker;
            try
            {
                string[] CRLF = new string[] { "\r\n" };
                string[] BlankLine= new string[] { "\r\n\r\n" };
                breaker = requestString.Split(BlankLine, StringSplitOptions.None);
                requestLines = breaker[0].Split(CRLF, StringSplitOptions.None);
                contentLines = breaker[1].Split(';');
                return true;
            }
            catch
            {
                return false;
            }


        }
    }
}
