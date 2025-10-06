using System;

namespace SystemControl.Api.DTOs
{
    public class DefectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Priority { get; set; }
        public string Status { get; set; } = string.Empty;
        public string AssignedUserName { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}
