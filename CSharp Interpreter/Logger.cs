using System;
using System.Collections.Generic;

namespace BitScript
{
    public class Logger
    {
        public Logger(bool isDebug = false, bool waitWhenlog = false)
        { IsDebug = isDebug; WaitWhenLog = waitWhenlog; }
        
        public List<string> Logs { get; } = new List<string>();
        
        public bool IsDebug { get; set; }
        
        public bool WaitWhenLog { get; set; }
        
        public string DebugOutput { get; private set; }
        
        public void Log(string msg)
        {
            Logs.Add(msg);
            if (IsDebug)
                Console.Write(msg);
            if (WaitWhenLog)
                Console.ReadKey(true);
        }
        
        public void LogLine(string msg)
            => Log(msg + "\n");
            
        public void AddOutput(char c)
            => DebugOutput += c;
    }
}