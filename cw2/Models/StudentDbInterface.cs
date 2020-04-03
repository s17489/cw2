using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw2.Models
{
    interface StudentDbInterface
    {
        List<Student> getStudentsFromDb();
        int getSemestr(string id);
    }
}
