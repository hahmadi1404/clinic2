using System;
using System.Collections.Generic;

namespace QCS_Config.Models
{
    public partial class Service
    {
        public int ClinikId { get; set; }
        public int ServiceId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ServiceImage { get; set; }
    }
}
