using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw2.DTOs;
using cw2.DTOs.Requests;
using cw2.DTOs.Responses;
using cw2.Models;


namespace cw2.Services
{
    public class EnrollmentDbServices : IEnrollmentDbServices
    {
        private string SqlConn = "Data Source=db-mssql;Initial Catalog=s17489;Integrated Security=True";

        public string ConnString { get; private set; }

        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest req)
        {
            EnrollStudentResponse resp = null;
            Enrollment respEnrollment = new Enrollment();
            SqlTransaction  transaction = null;

            using (var con = new SqlConnection(SqlConn))
            using (var com = new SqlCommand())
            {
                com.CommandText = "SELECT * FROM Studies WHERE Name = @StudyName";
                com.Parameters.AddWithValue("StudyName", req.Studies);
                com.Transaction =  transaction;

                com.Connection = con;
                con.Open();
                 transaction = con.BeginTransaction();

                com.Transaction =  transaction;
                SqlDataReader dr = com.ExecuteReader();
                if (!dr.Read())
                {

                    dr.Close();
                    transaction.Rollback();
                    dr.Dispose();
                    throw new ArgumentException("Studies not found");
                }
                int idStudy = (int)dr["IdStudy"]; // needed for 3.
                dr.Close();



                int idEnrollment = 0;

                com.CommandText = "SELECT * FROM Enrollment WHERE Semester = 1 AND IdStudy = @IdStudy";
                com.Parameters.AddWithValue("IdStudy", idStudy);
                com.Transaction = transaction;
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    idEnrollment = Int32.Parse(dr["IdEnrollment"].ToString());
                }
                else
                {
                    dr.Close();

                    com.CommandText = "SELECT MAX(IdEnrollment) AS EnrolmentMaxId FROM Enrollment WHERE Semester = 1";
                    com.Transaction = transaction;
                    dr = com.ExecuteReader();

                    idEnrollment = Int32.Parse(dr["EnrolmentMaxId"].ToString());
                    DateTime todayDate = DateTime.Today;
                    com.CommandText = "INSERT INTO Enrollment (IdEnrollment, IdStudy, Semester, StartDate)values (@EnrolmentId, @IdStudies, 1, @TodayDate);";   
                    com.Parameters.AddWithValue("@TodayDate", todayDate);
                    com.Parameters.AddWithValue("@EnrolmentId", idEnrollment);
                    com.ExecuteNonQuery();
                }
                dr.Close();

                com.CommandText = "SELECT * FROM Student WHERE IndexNumber = @IndexNumber";
                com.Parameters.AddWithValue("IndexNumber", req.IndexNumber);
                com.Transaction = transaction;
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    transaction.Rollback();
                    dr.Dispose();
                    throw new InvalidOperationException("Niewlasciwy numer studenta");
                }
                dr.Close();

                com.CommandText = "INSERT INTO Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES (@IndexNumber, @Firstname, @LastName, convert(datetime, @BirthDate, 104), @IdEnrollment)";
                com.Parameters.AddWithValue("FirstName", req.FirstName);
                com.Parameters.AddWithValue("LastName", req.LastName);
                com.Parameters.AddWithValue("BirthDate", req.BirthDate);
                com.Parameters.AddWithValue("IdEnrollment", idEnrollment);
                com.Transaction = transaction;
                com.ExecuteNonQuery();
                dr.Close();

                com.CommandText = "SELECT * FROM Enrollment WHERE IdEnrollment = @IdEnrollment";
                com.Transaction = transaction;
                dr = com.ExecuteReader();
                dr.Read();

                resp = new EnrollStudentResponse();
                

                resp.LastName = req.LastName;
                resp.Semester = 1;
                resp.Studies = req.Studies;
                resp.StartDate = dr["StartDate"].ToString();

                dr.Dispose();
                 transaction.Commit();
            }
            return resp;
        }


        public PromoteStudentsResponse PromoteStudents(PromoteStudentRequest req)
        {
            Enrollment respEnrollment = new Enrollment();
            var resp = new PromoteStudentsResponse();

            SqlCommand com = new SqlCommand();
            using (SqlConnection con = new SqlConnection(SqlConn))
            {
                con.Open();
                var transaction = con.BeginTransaction();
                com.Connection = con;
                com.Transaction = transaction;
                com.CommandText = "select e.IdEnrollment from dbo.Enrollment e inner join dbo.Studies s on e.IdStudy = s.IdStudy where s.Name = @studyName and e.Semester = @semesterNumber";
                com.Parameters.AddWithValue("@semester", req.Semester);
                com.Parameters.AddWithValue("@studies", req.Studies);

                var dr = com.ExecuteReader();

                if (!dr.Read())
                {
                    dr.Close();
                    return null;
                }
                else
                {
                    dr.Close();
                    using (SqlConnection conn = new SqlConnection(SqlConn))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("Promote", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@studies", req.Studies));
                        cmd.Parameters.Add(new SqlParameter("@semester", req.Semester));
                        cmd.ExecuteReader();
                        conn.Close();
                    }
                    resp.Semester = req.Semester + 1;
                    resp.StartDate = DateTime.Now.ToString("dd.MM.yyyy");
                    resp.StudiesName = req.Studies;
                    transaction.Commit();
                }    
            }
            return resp;
        }
    }
}
