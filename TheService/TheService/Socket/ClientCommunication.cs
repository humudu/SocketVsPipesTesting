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
        Logging.Logger logfile;
        // Constructor
        public ClientCommunication(Socket client)
        {
            this.client = client;
            logfile = new Logging.Logger("insideClient");
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
            logfile.logit("inside handle client");
            data = "Welcome to the Echo Server. Write a text:";
            streamwriter.WriteLine(data);		// Write greeting to Client
            streamwriter.Flush();

            //while (true)		// while communicating with Client
            //{
                try
                {
                    data = streamreader.ReadLine();	// Read data from Client
                    if (data == "DOIT")
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            string clientdata = streamreader.ReadLine();
                            string stringToSend = "I WILL DO IT" + i;
                            streamwriter.WriteLine(stringToSend);
                            streamwriter.Flush();
                        }
                    }
                }
                catch (IOException)		// The Client has closed the connection
                {
                    logfile.logit("client closed the connection with the server");
                    //break;
                }

                
            //}
            //Console.WriteLine("Connection to client is closing down...\n");
                logfile.logit("server closed the connection with the client");
            streamwriter.Close();
            streamreader.Close();
            networkstream.Close();
        }
    }
}
