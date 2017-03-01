using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheService.Logging
{
    class Logger
    {
        public string filename;
        public Logger(string filename)
        {
            this.filename = filename;
        }
        public static string lul = "lul";

        public void logit(string message)
        {
            lock(lul)
            {
                StreamWriter SW = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + filename + ".txt", true);
                SW.WriteLine(DateTime.Now.ToString() + message);
                SW.Flush();
                SW.Close();
            }
        }
    }
}
