using System.Collections.Generic;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Models
{
    public class PagerList<T> where T : new()
    {
        public int TotalCount { get; set; }
        public IEnumerable<T> List { get; set; }
    }
}