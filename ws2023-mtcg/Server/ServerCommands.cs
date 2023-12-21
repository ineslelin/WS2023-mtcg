using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Server
{
    internal class ServerCommands
    {
        public static int GetContentLength(string headers)
        {
            const string contentLengthHeader = "Content-Length: ";
            int index = headers.IndexOf(contentLengthHeader);

            if (index != -1)
            {
                index += contentLengthHeader.Length;
                int endIndex = headers.IndexOf('\r', index);
                string length = headers.Substring(index, endIndex - index);

                return int.Parse(length);
            }

            return 0;
        }

        public string ReadRequestBody(TcpClient client, int contentLength)
        {
            using var reader = new StreamReader(client.GetStream());

            char[] bodyBuffer = new char[contentLength];
            reader.Read(bodyBuffer, 0, contentLength);

            return new string(bodyBuffer);
        }
    }
}
