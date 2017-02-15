using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheService.Pipe
{
    class PipeClientStream
    {
        private NamedPipeServerStream pipeServer;
        private StringStream ss;

        public PipeClientStream(StringStream ss, NamedPipeServerStream pipeServer)
        {
            this.ss = ss;
            this.pipeServer = pipeServer;
        }

        public void Start()
        {
            string connecteduser = pipeServer.GetImpersonationUserName();

            while (true)      //THIS IS THE MAIN MENU
            {
                try
                {

                    string receivedString = "";
                    try
                    {
                        //waiting for value from connected user..
                        receivedString = ss.ReadString();
                    }

                    catch
                    {
                        Thread.Sleep(2000);
                        continue;
                    }

                    if (receivedString == "DOIT")
                    {
                        string stringToSend = "I WILL DO IT";
                        ss.WriteString(stringToSend);
                    }
                }
                catch
                {
                    return;
                }
            }
        }
    }
}
