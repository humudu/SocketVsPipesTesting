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
            Logging.Logger asd = new Logging.Logger("testtext");

            //-  pipe
            ThreadHandler TH = ThreadHandler.Instance;

            int numThreads = 3;
            asd.logit("service started. Maximum number of threads set to: " + numThreads);

            Thread threadhandlerthread = new Thread(new ParameterizedThreadStart(TH.handleThreads));
            TH.handleThreads(numThreads);

           

            //  - socket

            
        }

        protected override void OnStop()
        {
        }
    }
}
