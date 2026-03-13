using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using TrainingCentreManagement.Models;

namespace TrainingCentreManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly IConfiguration _configuration;

        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        // VIEW STUDENTS
        
        public IActionResult Index()
        {
            List<Student> list = new List<Student>();

            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_GetStudents", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Student s = new Student();

                    s.StudentId = Convert.ToInt32(dr["StudentId"]);
                    s.Name = dr["Name"].ToString();
                    s.Email = dr["Email"].ToString();
                    s.Phone = dr["Phone"].ToString();
                    s.CourseId = Convert.ToInt32(dr["CourseId"]);

                    list.Add(s);
                }

                con.Close();
            }

            return View(list);
        }


        // CREATE PAGE
       
        public IActionResult Create()
        {
            return View();
        }

        
        // INSERT STUDENT
       
        [HttpPost]
        public IActionResult Create(Student student)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                cmd.Parameters.AddWithValue("@Phone", student.Phone);
                cmd.Parameters.AddWithValue("@CourseId", student.CourseId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }

       
        // EDIT PAGE
        
        public IActionResult Edit(int id)
        {
            Student student = new Student();

            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_GetStudentById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentId", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    student.StudentId = Convert.ToInt32(dr["StudentId"]);
                    student.Name = dr["Name"].ToString();
                    student.Email = dr["Email"].ToString();
                    student.Phone = dr["Phone"].ToString();
                    student.CourseId = Convert.ToInt32(dr["CourseId"]);
                }

                con.Close();
            }

            return View(student);
        }

        // UPDATE STUDENT
  
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentId", student.StudentId);
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                cmd.Parameters.AddWithValue("@Phone", student.Phone);
                cmd.Parameters.AddWithValue("@CourseId", student.CourseId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }

        // DELETE STUDENT

        public IActionResult Delete(int id)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentId", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }
    }
}