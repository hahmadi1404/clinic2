using System;
using System.Collections.Generic;

namespace QCS_Config.Models
{
    public partial class Idea
    {
        public int IdeaId { get; set; }
        public int ClinicId { get; set; }
        public string Mobile { get; set; } = null!;
        public long NationalCode { get; set; }
        public int? SubjectId { get; set; }
        public string? IdeaDescription { get; set; }
        public string? VoiceFile { get; set; }
        public string? PicFile { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Status { get; set; }
    }
}
