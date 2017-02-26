using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheService.Pipe
{
    class ThreadHandler
    {
        private static readonly ThreadHandler instance = new ThreadHandler();

        Logging.Logger logger;
        int MaxThreads;
        int CurrentThreads;
        int CurrentClients;

        ServerSetup SS;

        private ThreadHandler()
        {
            logger = new Logging.Logger("testtext");
            CurrentThreads = 0;
            CurrentClients = 0;
            SS = new ServerSetup();

        }
        public static ThreadHandler Instance
        {
            get
            {
                return instance;
            }
        }

        internal void AddThread()
        {
            if (MaxThreads>CurrentThreads)
            {
                Thread newThread = new Thread(SS.ServerThread);
                newThread.Start();
                CurrentThreads++;
                logger.logit(" New thread started, currentThreads: " + CurrentThreads);
            }
        }

        internal void ClientConnected()
        {
            CurrentClients++;
            logger.logit(" New client connected, currentClients: " + CurrentClients);

            if (CurrentClients == CurrentThreads)
            {
                if(CurrentThreads<MaxThreads)
                {
                    AddThread();
                }
            }
        }
        internal void RemoveThread()
        {
            CurrentThreads--;
            CurrentClients--;
            logger.logit(" Client disconnected, currentThreads: " + CurrentThreads + " currentClients: " + CurrentClients);

            if (CurrentClients==CurrentThreads)
            {
                if (CurrentThreads<MaxThreads)
                {
                    AddThread();
                }
            }
        }

        internal void handleThreads(object numThreads)
        {
        MaxThreads = (int)numThreads;

            AddThread();

        }
    }
}
