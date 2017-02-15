using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheService.Pipe
{
    class ServerSetup
    {
        public void ServerThread(object data)
        {
            while (true)
            {
                PipeSecurity ps = new PipeSecurity();
                ps.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));
                NamedPipeServerStream pipeServer =
                    new NamedPipeServerStream("testpipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.WriteThrough, 4028, 4028, ps);

                int threadId = Thread.CurrentThread.ManagedThreadId;

                // Wait for a client to connect
                pipeServer.WaitForConnection();
                try
                {

                    StringStream ss = new StringStream(pipeServer);
                    

                    ss.WriteString("I am the one true server!");
                    PipeClientStream CS = new PipeClientStream(ss, pipeServer);
                    pipeServer.RunAsClient(CS.Start);  //instead of assigning it with a new thread, this will only take care of the 1 connected client.
                    
                }

                catch (IOException e)
                {
                   
                    goto OUTER;
                }
                OUTER:
                pipeServer.Close();
            }
        }
    }
}
