using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheService.Pipe;

namespace TheService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //-  pipe
      //      SocketController ctrl = new SocketController();
            ServerSetup SS = new ServerSetup();
            Thread PipeThread = new Thread(SS.ServerThread);
            PipeThread.Start();
           


            //  - socket

            
        }

        protected override void OnStop()
        {
        }
    }
}
