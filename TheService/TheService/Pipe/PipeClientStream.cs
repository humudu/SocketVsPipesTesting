using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheService.Logging;

namespace TheService.Pipe
{
    class PipeClientStream
    {
        private NamedPipeServerStream pipeServer;
        private StringStream ss;

        private Logger logger;
        public PipeClientStream(StringStream ss, NamedPipeServerStream pipeServer)
        {
            this.ss = ss;
            this.pipeServer = pipeServer;
            this.logger = new Logger("pipelog");
        }

        public void Start()
        {
            logger.logit("started start thread");
            string connecteduser = pipeServer.GetImpersonationUserName();

            while (true)      //THIS IS THE MAIN MENU
            {
                try
                {

                    string receivedString = "";
                    try
                    {
                        //waiting for value from connected user..

                        logger.logit("waiting for value from connected user...");
                        receivedString = ss.ReadString();
                        logger.logit("received: " + receivedString);
                    }
                    
                    catch
                    {
                        Thread.Sleep(2000);
                        continue;
                    }

                    if (receivedString == "DOIT")
                    {
                        for(int u = 0; u<50; u++)
                        {
                            string msgBack = ss.ReadString();
                        string stringToSend = "I WILL DO IT" + u;
                        ss.WriteString(stringToSend);
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.logit("got an exception: " + e.ToString());

                    logger.logit("returning from start method :(");
                    return;
                }
            }
        }

        public void DOIT()
        {

        }
    }
}
