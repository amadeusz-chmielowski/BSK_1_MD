using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BSK_1_MD
{
    class Logger
    {
        private List<string> loggerList;
        private static Mutex mutex = new Mutex();
        public Logger()
        {
            loggerList = new List<string>();
        }

        public void addToLogger(string text)
        {
            mutex.WaitOne();
            loggerList.Add("[" + DateTime.Now.ToString() + "]" + " " + text);
            // Release the Mutex.
            mutex.ReleaseMutex();
        }

        public string popOfLogger()
        {
            string returnText = "";
            if(loggerList.Count > 0 && loggerList != null)
            {
                returnText = loggerList.First();
                loggerList.RemoveAt(0);
            }
            return returnText;        
        }

    }
}
