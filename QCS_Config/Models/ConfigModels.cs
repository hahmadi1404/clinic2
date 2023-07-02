using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCS_Config.Models
{
    public class ConfigModels
    {
        public string sqlConnectionString { get; set; }
        public LogModel Log { get; set; }
        public  string TransferValidatorApiAddress { get; set; }
        // public  string CheckConnectionExportAddress { get; set; }
        public RabbitmqConfig RabbitmqConfig { get; set; }
        public CheckConnection CheckConnection { get; set; }

    }
    public class CheckConnection
    {
        public Int64 GetFiles { get; set; }
        public Int64 GetFilteredFiles { get; set; }
        public Int64 GetFolders { get; set; }
        public Int64 SortItem { get; set; }
        public Int64 SortOrder { get; set; }
        public Int64 RequestType { get; set; }
        public Int64 FileServerConfigTimeout { get; set; }
        public Int64 PingRetries { get; set; }
        public Int64 PingTimeout { get; set; }
    }
    public class RabbitmqConfig
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LogModel
    {
        public string AgentName { get; set; }
        public FileLogModel FileLog { get; set; }
        public ElasticLogModel ElasticLog { get; set; }
        public ConsoleLogModel ConsoleLog { get; set; }
    }

    public class ElasticLogModel
    {
        public bool Active { get; set; }
        public string Uri { get; set; }
        public string IndexFormat { get; set; }
        public string BufferBaseFilename { get; set; }
        public int? BufferFileCountLimit { get; set; }
        //public long? BufferFileSizeLimitBytes { get; set; }
        public long BufferLogShippingInterval { get; set; }

    }

    public class FileLogModel
    {
        public bool Active { get; set; }
        public string LogTemplate { get; set; }
        public string Address { get; set; }
    }

    public class ConsoleLogModel
    {
        public bool Active { get; set; }
    }

}
