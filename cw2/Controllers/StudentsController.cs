using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace cw2.Controllers
{
    [ApiController]
    // jeśli pod ten adres wysylane jest rzadanie to przekaz je co studentsC
    [Route("api/students")]
    
    //adnotacja
    public class StudentsController : ControllerBase
    {
        //Nad metodą dajemy atrybut na jaka metode http bedzie regaowalem ten atrybut 
        /*
         * HttpGet
         * HttpPost
         * HttpPut
         * HttpPatch
         * HttpDelete
         */

            [HttpGet]
            //jeśli samo HttpGet  - rzadanie ktore przyjdzie na glowny adres kontrolera to przejdzie do tej metody, przez to mozna uzyc tylko dla jednej metody
        public string GetStudents()
        {
            return "Kowalski, Malewski, Andrzejweski";
        }
    }
}