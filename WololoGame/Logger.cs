using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WololoGame
{
    enum LogLevel
    {
        error = 0,
        warning = 1,
        other = 2
    }
    class Logger
    {
        private static Object mLock = new Object();
        private static Logger instance = null;
        private Dictionary<string, LogLevel> logLevels = new Dictionary<string, LogLevel>(); 
        public static Logger Get() {
            if (instance != null)
            {
                return instance;
            }
            lock(mLock)
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }

        public Logger()
        {
        }
        public void SetLogLevel(string area, LogLevel level)
        {
            logLevels[area] = level;
        }
        public bool ShouldLog(string area, LogLevel level)
        {
            return level == LogLevel.error || logLevels.ContainsKey(area) && logLevels[area] >= level; 
        }
        public bool Log(string area, LogLevel level, string message)
        {
            if(ShouldLog(area,level))
            {
                System.Diagnostics.Debug.WriteLine(message);
                return true;
            }
            return false;
        }

    }
}
