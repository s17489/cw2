using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace cw2.Models
{
    public class StudentDbService : StudentDbInterface
    {
        private string SqlConn = "Data Source=db-mssql;Initial Catalog=s17489;Integrated Security=True";

        public List<Student> getStudentsFromDb()
        {
            List<Student> StudentsList = new List<Student>();
            using (var client = new SqlConnection(SqlConn))
            using (var command = new SqlCommand())
            {
                command.Connection = client;
                command.CommandText = "SELECT * FROM STUDENT";
                client.Open();
                var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                   // st.IdEnrollment = (int)dr["IdEnrollment"];
                    StudentsList.Add(st);

                }
                client.Close();
            }
            return StudentsList;
        }
        // public int getSemestr(string id)
        public Semester getSemester(string id)
        {
            var sem = new Semester();
            using (var client = new SqlConnection(SqlConn))
            using (var command = new SqlCommand())
            {
                command.Connection = client;
                command.CommandText = "SELECT Enrollment.Semester FROM Enrollment JOIN Student ON Enrollment.IdEnrollment = Student.IdEnrollment WHERE Student.IndexNumber = @id;";
                command.Parameters.AddWithValue("@id", id);
                client.Open();
                var dr = command.ExecuteReader(); 
                while (dr.Read())
                {      
                    sem.SemesterNum = (int)dr["Semester"];  
                }
                client.Close();
            }
            return sem;
        }
    }
}