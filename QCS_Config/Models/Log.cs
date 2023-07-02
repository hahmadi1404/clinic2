using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace QCS_Config.Models
{
    public static class Log
    {
        public static Logger Logger { get; private set; }

        private static LoggerConfiguration _loggerConfiguration;

        public static void Initialize(string agentName, string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u4}] {Message}{NewLine}{Exception}", bool elasticLogActive = false, string elasticSearchUri = null, string indexFormat = "SgiLog", bool FileLogActive = false, string logFileAddress = null, bool ConsoleLogActive = true, LogEventLevel minimumLogType = LogEventLevel.Information, bool addMachineName = false)
        {

            _loggerConfiguration = new LoggerConfiguration().Enrich.WithExceptionDetails().Enrich.WithProperty("AgentName", agentName);

            //if (addMachineName)
            //    _loggerConfiguration.Enrich.WithMachineName();

            _loggerConfiguration.MinimumLevel.Is(minimumLogType);

            if (ConsoleLogActive) _loggerConfiguration.WriteTo.Console();

            if (FileLogActive)
            {
                if (string.IsNullOrWhiteSpace(logFileAddress)) logFileAddress = "d:\\" + agentName + "Log";
                
                Directory.CreateDirectory(logFileAddress);
                string logFile = logFileAddress+ "\\Log.txt";
                _loggerConfiguration.WriteTo.RollingFile(pathFormat: logFile, outputTemplate: outputTemplate, buffered: false, shared: true);
            }

            if (elasticLogActive)
            {
                elasticSearchUri = string.IsNullOrWhiteSpace(elasticSearchUri) ? "localhost:9200" : elasticSearchUri;
                indexFormat = indexFormat.ToLower();
                _loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUri)) { IndexFormat = indexFormat,
                    BufferBaseFilename = Config.All.Log.ElasticLog.BufferBaseFilename,
                    BufferFileCountLimit = Config.All.Log.ElasticLog.BufferFileCountLimit == 0 ? 31 : Config.All.Log.ElasticLog.BufferFileCountLimit,
                    BufferLogShippingInterval = TimeSpan.FromSeconds(Config.All.Log.ElasticLog.BufferLogShippingInterval)
                });
            }

            Logger = _loggerConfiguration.CreateLogger();

        }
    }
}
