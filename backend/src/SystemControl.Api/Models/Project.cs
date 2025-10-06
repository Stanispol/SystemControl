using System;
using System.Collections.Generic;

namespace SystemControl.Api.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<Defect> Defects { get; set; } = new List<Defect>();
    }
}
