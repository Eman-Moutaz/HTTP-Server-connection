using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString,Content_Type,Content_Length,RedirectionPath,Date,Content;
        StatusCode Code;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
       // List<string> headerLines = new List<string>();

      /*    public Response(StatusCode code, string contentType, string content, string redirectionpath)
          {
              // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
              this.Code = code;
              this.Content_Type = "Content Type : " + contentType +"\r\n";
              this.Content_Length = "Content Length :  " + content.Length + "\r\n";
              this.RedirectionPath= "New Location : " + redirectionpath+"\r\n";
              this.Date = "Date : " + DateTime.Now+"\r\n";
              this.Content=content;

              // TODO: Create the request string
              if (redirectionpath != "")
              {
                  this.responseString = GetStatusLine(Code) + Content_Type + Content_Length + RedirectionPath + Date + "\r\n" + Content;
              }
              else{
                  this.responseString = GetStatusLine(Code) + Content_Type + Content_Length + Date + "\r\n" + Content;
              }
          }
       */
        
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            this.Code = code;
            this.Content_Type = "Contetnt-Type : " + contentType + "\r\n";
            this.RedirectionPath = "Location : " + redirectoinPath + "\r\n";
            this.Content_Length = "Content-Length : " + content.Length + "\r\n";
            this.Date = "Date : " + DateTime.Now+"\r\n";
            // TODO: Create the request string
            if (redirectoinPath != "")
                this.responseString = GetStatusLine(code) + Content_Type + Content_Length + RedirectionPath + Date + "\r\n" + content;
            else
                this.responseString = GetStatusLine(code) + Content_Type + Content_Length + Date + "\r\n" + content;
        }
      
      

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Empty;
            if (code == StatusCode.OK)
                statusLine = "HTTP/1.1 200 OK\r\n";
            else if (code == StatusCode.InternalServerError)
                statusLine = "HTTP/1.1 500 Internal Server Error\r\n";
            else if (code == StatusCode.NotFound)
                statusLine = "HTTP/1.1 404 Not Found\r\n";
            else if (code == StatusCode.BadRequest)
                statusLine = "HTTP/1.1 400 Bad Request\r\n";
            else if (code == StatusCode.Redirect)
                statusLine = "HTTP/1.1 301 Moved Permanently\r\n";
            return statusLine;
        }
    }
}
