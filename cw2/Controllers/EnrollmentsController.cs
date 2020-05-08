using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cw2.Models;
using cw2.DTOs.Requests;
using cw2.DTOs.Responses;
using cw2.Services;

namespace cw2.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
      //  private string SqlConn = " Data Source=db-mssql;Initial Catalog=s17489;Integrated Security=True";
        private IEnrollmentDbServices _enrollmentDbServices;

        public EnrollmentsController(IEnrollmentDbServices enrollmentDbServices)
        {
            _enrollmentDbServices = enrollmentDbServices;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            try
            {
                var response = _enrollmentDbServices.EnrollStudent(request);
                return CreatedAtAction("EnrollStudent", response);
            }
            catch (ArgumentException)
            {
                return NotFound("Studies with given name not found");
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Student with given index number already exists");
            }
        }

        [Route("promotions")]
        [HttpPost]
        [ActionName("PromoteStudents")]
        public IActionResult PromoteStudents(PromoteStudentRequest request)
        {
            try
            {
                var response = _enrollmentDbServices.PromoteStudents(request);
                return CreatedAtAction("PromoteStudents", response);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Invalid request");
            }
            catch (SqlException)
            {
                return NotFound("Studies with given name not found");
            }
            catch (ArgumentException)
            {
                return BadRequest("No values for response found");
            }
        }
    }

}