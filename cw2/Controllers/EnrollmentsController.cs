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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace cw2.Controllers
{

    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
    
        private IEnrollmentDbServices _enrollmentDbServices;
        public IConfiguration Configuration { get; set; }

        private readonly IRTokenServices _rTokenServices = new RTokenServices();
        
        public EnrollmentsController(IEnrollmentDbServices enrollmentDbServices, IConfiguration configuration)
        {
            _enrollmentDbServices = enrollmentDbServices;
            Configuration = configuration;
            
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
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

        [Authorize(Roles = "employee")]
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
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginRequestDto request)
        {
            var student = _enrollmentDbServices.GetStudentPassword(request.Login);
            if (student.Password.Equals(request.Haslo))
            {
                Console.WriteLine("Hasło poprawne");
            }
            else
            {
                return BadRequest("Niepoprawne hasło");
            }

            var claims = new[]
 {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "student")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken

            (
                issuer: "s17489",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            var rToken = Guid.NewGuid();
            _rTokenServices.SetToken(rToken);

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = rToken
            });
        }
        [HttpPost()]
        [Route("refresh")]
        [AllowAnonymous]
        public IActionResult RefreshToken(RefreshRequest reftoken)
        {
            if (!_rTokenServices.CheckToken(reftoken.refreshToken))
            {
                return BadRequest();
            }

            var claims = new[]
{
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "employee")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken

           (
               issuer: "s17489",
               audience: "Students",
               claims: claims,
               expires: DateTime.Now.AddMinutes(10),
               signingCredentials: creds
           );

            var rToken = Guid.NewGuid();
            _rTokenServices.SetToken(rToken);
            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = rToken
            });
        }
    }

}