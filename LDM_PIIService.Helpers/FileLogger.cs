using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LDM_PIIService.Helpers
{
    public class FileLogger
    {
        public static event Func<string> GetLogFilePath_Event;
        private static readonly string _executingAssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
        private readonly string _subFolderLogName;
        private static readonly Dictionary<string, FileLogger> _loggerDictionary = new();

        private FileLogger(string subFolderLogName)
        {
            _subFolderLogName = subFolderLogName;
        }

        public static FileLogger GetInstance(string? subFolderLogName = null)
        {
            if (string.IsNullOrWhiteSpace(subFolderLogName))
                subFolderLogName = "General";

            lock (_loggerDictionary)
            {
                if (!_loggerDictionary.TryGetValue(subFolderLogName.ToLower(), out var logger))
                {
                    logger = new FileLogger(subFolderLogName);
                    _loggerDictionary[subFolderLogName.ToLower()] = logger;
                }
                return logger;
            }
        }

        public string GetLogFilePath()
        {
            return GetLogFilePath_Event();
        }

        public void WriteToLogFile(Exception exception, string exceptionDetail, bool newLine = true)
        {
            WriteToLogFile(ActionTypeEnum.Exception, exceptionDetail + Environment.NewLine + exception.ToString(), newLine);
        }

        public void WriteToLogFile(ActionTypeEnum logAction, string message, bool newLine = true,
                                  [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
        {
            try
            {
                string baseLogPath = GetLogFilePath();

                string directoryPath = Path.Combine(baseLogPath, DateTime.Now.ToString("yyyy-MM-dd"), _subFolderLogName);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string filePath = Path.Combine(directoryPath,
                    DateTime.Now.ToString("yyyy-MM-dd HH") +
                    (logAction == ActionTypeEnum.Exception ? "_Exception" : string.Empty) +
                    $"_V{_executingAssemblyVersion}.txt");

                BackupIfSizeExceeded(filePath);

                using (StreamWriter streamWriter = new StreamWriter(filePath, true))
                {
                    string[] str = new string[8];
                    str[0] = DateTime.Now.ToString("HH:mm:ss.fff");
                    str[1] = " || ";
                    str[2] = methodName.PadRight(33);
                    str[3] = " || ";
                    str[4] = logAction.ToString().PadRight(11);
                    str[5] = " || ";
                    str[6] = message;
                    if (logAction == ActionTypeEnum.Exception && newLine)
                        str[7] = Environment.NewLine + "==============================================" + Environment.NewLine;

                    streamWriter.WriteLine(string.Concat(str));
                }
            }
            catch (Exception)
            {
                
            }
        }
        private static void BackupIfSizeExceeded(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return;

                if (new FileInfo(filePath).Length > 3 * 1024 * 1024)
                {
                    string backupName = Path.Combine(
                        Path.GetDirectoryName(filePath),
                        Path.GetFileNameWithoutExtension(filePath) + "_" + DateTime.Now.ToString("HH_mm_ss") + ".txt"
                    );
                    File.Move(filePath, backupName);
                }
            }
            catch 
            { 

            }
        }
    }

    public enum ActionTypeEnum
    {
        Information = 1,
        Action = 2,
        Exception = 3
    }
}
