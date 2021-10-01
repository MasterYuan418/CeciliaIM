using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace CeciliaIM.Server.Backend.Utils
{
    class LogUtil
    {
        private static readonly ILogger _logServices = Log.Logger;
        public static void InitLog()
        {
            string SerilogOutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}]: {Count} {Message:j} {NewLine}";

            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Verbose() // 所有Sink的最小记录级别
              .WriteTo.File(ConfigUtil.DATA_LOGS_PATH + "\\" + Guid.NewGuid().ToString() + ".log",
                            rollingInterval: RollingInterval.Day,
                            outputTemplate: SerilogOutputTemplate
                            //,retainedFileCountLimit: 31
                            )
              .CreateLogger();
            WriteVerbose("Log module init successful.");
        }
        public static void WriteInformation(string text)
        {
            _logServices.Information(text);
        }
        public static void WriteVerbose(string text)
        {
            _logServices.Verbose(text);
        }
        public static void WriteDebug(string text)
        {
            _logServices.Debug(text);
        }
        public static void WriteWarning(string text)
        {
            _logServices.Warning(text);
        }
        public static void WriteError(string text)
        {
            _logServices.Error(text);
        }
        public static void WriteFatal(string text)
        {
            _logServices.Fatal(text);
        }
        public static void Close() => Log.CloseAndFlush();
    }
}
