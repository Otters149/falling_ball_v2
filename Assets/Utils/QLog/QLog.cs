using UnityEngine;
using System.IO;
using System;

namespace utilpackages
{
    namespace qlog
    {
        public enum ELogLevels
        {
            DEBUG,
            INFO,
            WARNING,
            ERROR,
        }

        public class QLog
        {
            private string _logFilePath = Application.dataPath + "/quokka.log";
            static private QLog s_instance;

            static public QLog GetInstance()
            {
                if (s_instance == null)
                {
                    s_instance = new QLog();
                }
                return s_instance;
            }

            private QLog()
            {
                string runTime = "\n==========" + DateTime.Now.ToString() + "==========\n";
                WriteFile(_logFilePath, runTime, false);
            }

            private void WriteFile(string path, string data, bool append=true)
            {
                StreamWriter writer = new StreamWriter(path, append);
                writer.Write(data);
                writer.Close();
            }

            private void WriteLog(string data)
            {
                WriteFile(_logFilePath, DateTime.Now.ToString() + " " + data + Environment.NewLine);
            }

            private void Logging(ELogLevels logLevel, string tag, string msg)
            {
                Color levelColor = GetLogColor(logLevel);
                string finalMsg = "[" + logLevel.ToString() + "]" + "[" + tag + "] " + msg;
                Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(levelColor.r * 255f), (byte)(levelColor.g * 255f), (byte)(levelColor.b * 255f), finalMsg));
                WriteLog(finalMsg);
            }

            private Color GetLogColor(ELogLevels logLevel)
            {
                switch (logLevel)
                {
                    case ELogLevels.DEBUG:
                        return Color.white;
                    case ELogLevels.INFO:
                        return Color.green;
                    case ELogLevels.WARNING:
                        return Color.yellow;
                    case ELogLevels.ERROR:
                        return Color.red;
                    default:
                        return Color.black;
                }
            }

            public void LogDebug(string tag, string msg)
            {
#if _DEBUG
                Logging(ELogLevels.DEBUG, tag, msg);
#endif
            }

            public void LogInfo(string tag, string msg)
            {
#if !_RETAIL
                Logging(ELogLevels.INFO, tag, msg);
#endif
            }

            public void LogWarning(string tag, string msg)
            {
                Logging(ELogLevels.WARNING, tag, msg);
            }

            public void LogError(string tag, string msg)
            {
                Logging(ELogLevels.ERROR, tag, msg);
            }

            public string GetClassName(object cls)
            {
                return cls.GetType().FullName;
            }

            public void ClearLog()
            {
                WriteFile(_logFilePath, "", false);
            }
        }
    }

}