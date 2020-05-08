using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw2.Models;

namespace cw2.DTOs.Responses
{
    public class PromoteStudentsResponse
    {
        public int Semester { get; set; }

        public string StartDate { get; set; }

        public string StudiesName { get; set; }
    }
}
