using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw2.Models;

namespace cw2.DTOs.Responses
{
    public class EnrollStudentResponse
    {
        public string LastName { get; set; }
        public int Semester { get; set; }
        public string StartDate { get; set; }
        public string Studies { get; set; }
    }
}
