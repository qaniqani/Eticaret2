using System.Collections.Generic;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Models
{
    public class PropertyListDto
    {
        public string Name { get; set; }
        public List<PropertyItem> PropertyItem { get; set; }
    }
}