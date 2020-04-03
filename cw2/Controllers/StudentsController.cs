using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using cw2.Models;


namespace cw2.Controllers
{
    [ApiController]
    // jeśli pod ten adres wysylane jest rzadanie to przekaz je co studentsC
    [Route("api/students")]

    //adnotacja
    public class StudentsController : ControllerBase
    {

        private readonly StudentDbInterface studentsDB = new StudentDbService();

       
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
        //public string GetStudents([FromQuery] string orderBy)
        public IActionResult GetStudents()
        
        //from query nie jest wymagane
        {
            List<Student> list = studentsDB.getStudentsFromDb();
            return Ok(list);
        }
        //dodawanie zasobou 
        //pracuje sie albo na xml albo na json
        [HttpPost]
        public IActionResult AddStudent([FromBody] Models.Student student)
        {
            //.. add to DB
            //... generating index number
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpGet("{id}")]
        // jeśli po /students/ pojawi sie jakas wartosc to zostanie ona tutaj przekazana 
        public IActionResult GetStudentById( int id )
        // [FromRoute] to mozna dodac przed intem ale nie jest wymagane 
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

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id)
        {
           //aktualizaccja BD
            return Ok($"Aktualizacja studenta o id {id} dokończona");
        }


        [HttpDelete("{id}")]
        public IActionResult Deletetudent(int id)
        {
            //usuwanie zasobu z BD
            return Ok($"Usuwanie studenta o id {id} ukończone");
        }

        [HttpGet("getSemester/{id}")]
        public IActionResult GetSemestrByIndex(string id)
            // id to innaczej nr indexu bo w tej bazie nie ma odzielnego idStudent 
        {
            Semester sem = studentsDB.getSemester(id);
            return Ok(sem);
        }
        
    }
}