﻿using System;
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
         * HttpGet -> Pobierz z BD
         * HttpPost - > Dodaj zasób do BD
         * HttpPut ->  Zakutalizuj zasób
         * HttpPatch -> załataj (częściowa aktualizacja)
         * HttpDelete -> usuń zasób
         */

            [HttpGet]
            //jeśli samo HttpGet  - rzadanie ktore przyjdzie na glowny adres kontrolera to przejdzie do tej metody, przez to mozna uzyc tylko dla jednej metody
        public string GetStudents()
        {
            return "Kowalski, Malewski, Andrzejweski";
        }

        [HttpGet("{id}")]
        // jeśli po /students/ pojawi sie jakas wartosc to zostanie ona tutaj przekazana 
        public IActionResult GetStudentById(int id )
        {
            if (id == 1)
            {
                return Ok("Kowalski");
            }
            else if (id == 2)
            {
                return Ok("Malewski");
            }
            return NotFound($"Nie znalezeiono studenta od id {id}");
        }
    }
}