using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            //TODO: initialize this.serverSocket
            LoadRedirectionRules(redirectionMatrixPath);
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint hostEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            serverSocket.Bind(hostEndPoint);
           
            
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(100);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket clientSocket = serverSocket.Accept();
              //  Console.WriteLine("New client accepted ", clientSocket.RemoteEndPoint);
                Thread newthread = new Thread(new ParameterizedThreadStart (HandleConnection));
                newthread.Start(clientSocket);
                        
              //  ThreadPool.QueueUserWorkItem(HandleConnection, clientSocket);

            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            Socket clientSock = (Socket)obj;
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            clientSock.ReceiveTimeout = 0;
            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    byte[] request = new byte[2048]; ;
                   // clientSock.Send(request);
                    int receivedLength = clientSock.Receive(request);
                    // TODO: break the while loop if receivedLen==0
                    if (receivedLength == 0)
                    {
                       // Console.WriteLine("Client: {0} ended the connection", clientSock.RemoteEndPoint);
                        break;
                    }
                    ///////////////////////////////////////////////////
                    Console.WriteLine(Encoding.ASCII.GetString(request));
                    // TODO: Create a Request object using received request string
                    Request req = new Request(Encoding.ASCII.GetString(request));
                    // TODO: Call HandleRequest Method that returns the response
                    Response res = HandleRequest(req);
                    // TODO: Send Response back to client
                    Console.WriteLine(res.ResponseString);
                    byte[] all_response = Encoding.ASCII.GetBytes(res.ResponseString);
                    clientSock.Send(all_response);
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                    break;
                }
            }

            // TODO: close client socket
            clientSock.Close();
        }

        Response HandleRequest(Request request)
        {
           // throw new NotImplementedException();
         
                //TODO: check for bad request
                //TODO: map the relativeURI in request to get the physical path of the resource.
                //TODO: check for redirect
                //TODO: check file exists
                //TODO: read the physical file
                // Create OK response
            string content = "Html";
         
            try
            {
                //good request
                if (request.ParseRequest() == true) 
                {
                    string Redirect = GetRedirectionPagePathIFExist(request.relativeURI);
                    // Found Redirection 
                    if (Redirect != string.Empty)
                    {
                        string readfile = File.ReadAllText(Configuration.RootPath + Configuration.RedirectionDefaultPageName);
                        Response response_status = new Response(StatusCode.Redirect, content, readfile, Redirect);
                        return response_status;
                    }

                    // No Redirection
                         string path = Configuration.RootPath + request.relativeURI;
                         //MAIN PAGE
                         if (request.relativeURI == "/")
                         {
                             string readfile = File.ReadAllText(path + "/main.html");
                             Response response_status = new Response(StatusCode.OK, content, readfile, "");
                             return response_status;
                         }
                             //PAGE FOUND
                          if (File.Exists(path))
                         {
                             string readfile = File.ReadAllText(path);
                             Response response_status = new Response(StatusCode.OK, content, readfile, "");
                             return response_status;
                         }
                             //NOT FOUND ( FILE Not Exist)
                         else
                         {
                             string readfile = File.ReadAllText(Configuration.RootPath + Configuration.NotFoundDefaultPageName);
                             Response response_status = new Response(StatusCode.NotFound, content, readfile, "");
                             return response_status;
                         } 
                    }

                //bad request ( parserequest == false)
                else 
                {
                    string readfile = File.ReadAllText(Configuration.RootPath + Configuration.BadRequestDefaultPageName);
                    Response response_status = new Response(StatusCode.BadRequest, content, readfile, "");
                    return response_status;

                }

            }
                  
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
                string readfile = File.ReadAllText(Configuration.RootPath + Configuration.InternalErrorDefaultPageName);
                Response response_status = new Response(StatusCode.InternalServerError, content, readfile, "");
                return response_status;

            }
        }
    

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            foreach (KeyValuePair<string, string> Dic in Configuration.RedirectionRules)
            {
                if (relativePath == ("/" + Dic.Key) )
                {
                    string NewPath = Dic.Value;
                    return NewPath;

                }
            }
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
           string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            
            // else read file and return its content
            return string.Empty;
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                Configuration.RedirectionRules = new Dictionary<string, string>();
                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var fileReader = new StreamReader(stream, Encoding.UTF8, true, 128);
                string line;
                while ((line = fileReader.ReadLine()) != null)
                {
                // then fill Configuration.RedirectionRules dictionary 
                    string[] redirection_sites = line.Split(' ');
                    Configuration.RedirectionRules.Add(redirection_sites[0] , redirection_sites[1]);
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Console.WriteLine("ERROR WHEN READ OR FILL REDIRECTION RULES");
              //  Environment.Exit(1);
            }
        }
    }
}
