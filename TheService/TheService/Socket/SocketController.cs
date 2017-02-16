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
    class SocketController
    {
        private const int PORT = 12345;
        IPEndPoint port;
            Thread clientThread;
            ClientCommunication clientCommunication;
            ThreadStart threadDelegate;
            Socket clientSocket;				// socket to client
        public SocketController(){
            port = new IPEndPoint(IPAddress.Any, PORT);
            //start the server once with the initialization of the class..we may change that
            RunServer();
        }

        private void RunServer(){
            //create the socket and place it in a listening state
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(port);
            listener.Listen(10);
            //run the server. we may need to find better ways to handle this..like being able to stop it and start it by command
            while (true)			// Endless loop for Server
            {
                //every console.WriteLine should be replaced to a writeline to a log file, so we can see what happens in the service
                //Console.WriteLine("Waiting for a client...");

                clientSocket = listener.Accept();		// await connection from client
                //Console.WriteLine("Client connected...");

                // Preparation and start of thread which communicates with client
                clientCommunication = new ClientCommunication(clientSocket);
                threadDelegate = new ThreadStart(clientCommunication.HandleClient);
                clientThread = new Thread(threadDelegate);
                clientThread.Start();	// start client thread
            }

        }

            

            

    }
}

