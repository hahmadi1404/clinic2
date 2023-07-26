using System;
using System.Collections.Generic;

namespace QCS_Config.Models
{
    public partial class Agreement
    {
        public int Id { get; set; }
        public int ClinicId { get; set; }
        public string? Text { get; set; }
    }
}
