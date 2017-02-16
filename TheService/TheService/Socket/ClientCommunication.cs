using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TheService
{
    class ClientCommunication
    {
        private Socket client;		// socket to client

        // Constructor
        public ClientCommunication(Socket client)
        {
            this.client = client;
        }

        // Thread method, which communicates with a client
        public void HandleClient()
        {
            string data;
            NetworkStream networkstream;
            StreamReader streamreader;
            StreamWriter streamwriter;

            networkstream = new NetworkStream(this.client);
            streamreader = new StreamReader(networkstream);
            streamwriter = new StreamWriter(networkstream);

            data = "Welcome to the Echo Server. Write a text:";
            streamwriter.WriteLine(data);		// Write greeting to Client
            streamwriter.Flush();

            while (true)		// while communicating with Client
            {
                try
                {
                    data = streamreader.ReadLine();	// Read data from Client
                }
                catch (IOException)		// The Client has closed the connection
                {
                    break;
                }

                //Console.WriteLine(data);
                if (data == "DOIT")
                {
                    string stringToSend = "I WILL DO IT";
                    streamwriter.WriteLine(stringToSend);
                }
                streamwriter.Flush();
            }
            //Console.WriteLine("Connection to client is closing down...\n");
            streamwriter.Close();
            streamreader.Close();
            networkstream.Close();
        }
    }
}
