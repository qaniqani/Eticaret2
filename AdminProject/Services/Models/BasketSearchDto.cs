using System;

namespace AdminProject.Services.Models
{
    public class BasketSearchDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}