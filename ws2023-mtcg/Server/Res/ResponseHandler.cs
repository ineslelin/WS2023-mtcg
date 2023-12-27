using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Server.Res
{
    internal class ResponseHandler
    {
        public ResponseHandler()
        {
           
        }

        public static void SendResponse(StreamWriter writer, string content, int code)
        {
            string response = $"HTTP/1.1 {code} OK\r\n" +
                          $"Content-Type: text/plain\r\n" +
                          $"Content-Length: {Encoding.UTF8.GetByteCount(content)}\r\n" +
                          "\r\n" +
                          $"{content}";

            writer.Write(response);
        }

        public static void SendErrorResponse(StreamWriter writer, string content, int code)
        {
            string response = $"HTTP/1.1 {code} ERR\r\n" +
                          $"Content-Type: text/plain\r\n" +
                          $"Content-Length: {Encoding.UTF8.GetByteCount(content)}\r\n" +
                          "\r\n" +
                          $"{content}";

            writer.Write(response);
        }
    }
}
