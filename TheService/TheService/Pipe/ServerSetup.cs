using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheService.Pipe
{
    class ServerSetup
    {
        ThreadHandler TH;
        public void ServerThread(object maxinstances)
        {
            int MaxServers = (int)maxinstances;

            TH = ThreadHandler.Instance;
                Logging.Logger logger = new Logging.Logger("testtext");
                logger.logit("creating pipe..");

            SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            PipeSecurity ps = new PipeSecurity();
            //   ps.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), PipeAccessRights.ReadWrite | PipeAccessRights.CreateNewInstance, AccessControlType.Allow));
            //    ps.AddAccessRule(new PipeAccessRule("Users", PipeAccessRights.ReadWrite | PipeAccessRights.CreateNewInstance, AccessControlType.Allow));

            ps.AddAccessRule(new PipeAccessRule(everyone, PipeAccessRights.FullControl, AccessControlType.Allow));
            //ps.AddAccessRule(new PipeAccessRule("SYSTEM", PipeAccessRights.FullControl, AccessControlType.Allow));
            //ps.AddAccessRule(new PipeAccessRule("CREATOR OWNER", PipeAccessRights.FullControl, AccessControlType.Allow));
            //ps.AddAccessRule(new PipeAccessRule("USERS", PipeAccessRights.FullControl, AccessControlType.Allow));

           // logger.logit("step4");


            NamedPipeServerStream pipeServer =
                new NamedPipeServerStream("testpipe", PipeDirection.InOut, MaxServers, PipeTransmissionMode.Byte, PipeOptions.WriteThrough, 4028, 4028, ps);

     //       NamedPipeServerStream pipeServer2 =
     //          new NamedPipeServerStream("testpipe", PipeDirection.InOut, 5, PipeTransmissionMode.Byte, PipeOptions.WriteThrough, 4028, 4028, ps);


                   logger.logit("pipe crreated..");
            int threadId = Thread.CurrentThread.ManagedThreadId;



            // Wait for a client to connect
           
                logger.logit("Thread(" + threadId + "): waiting for connection");
            
            pipeServer.WaitForConnection();
            try
            {

                StringStream ss = new StringStream(pipeServer);
                TH.ClientConnected();
                logger.logit("sending first string to client");

                ss.WriteString("I am the one true server!");

                //      logger.logit("sent I am the one true server!");
                PipeClientStream CS = new PipeClientStream(ss, pipeServer);
                
                logger.logit("Thread(" + threadId + "): Client connected, about to RunAsClient");

                pipeServer.RunAsClient(CS.Start);  //instead of assigning it with a new thread, this will only take care of the 1 connected client.

            }

            catch (IOException e)
            {

                   logger.logit("IOException: " + e);

                goto OUTER;
            }
            catch (Exception ex)
            {
                logger.logit("Exception: " + ex);

            }
            OUTER:
            logger.logit("OUTER: closing pipeserver and TH.RemoveThread");

            pipeServer.Close();
            TH.RemoveThread();

        }
    }
}

