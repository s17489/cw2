using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw2.Models
{
   public interface IStudentDbService
    {
        List<Student> getStudentsFromDb();
        Semester getSemester(string id);
        bool trueStudent(string indexNumber);
    }
}
