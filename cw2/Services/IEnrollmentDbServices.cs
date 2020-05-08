using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw2.DTOs.Requests;
using cw2.DTOs.Responses;

namespace cw2.Services
{
    public interface IEnrollmentDbServices
    {
        EnrollStudentResponse EnrollStudent(EnrollStudentRequest request);

        PromoteStudentsResponse PromoteStudents(PromoteStudentRequest request);
    }
}
