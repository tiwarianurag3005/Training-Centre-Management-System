using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using TrainingCentreManagement.Models;

namespace TrainingCentreManagement.Controllers
{
    public class CourseController : Controller
    {
        private readonly IConfiguration _configuration;

        public CourseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // SHOW COURSES
        public IActionResult Index()
        {
            List<Course> list = new List<Course>();

            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_GetCourses", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Course c = new Course();

                    c.CourseId = Convert.ToInt32(dr["CourseId"]);
                    c.CourseName = dr["CourseName"].ToString();
                    c.Duration = dr["Duration"].ToString();
                    c.Fee = Convert.ToDecimal(dr["Fee"]);

                    list.Add(c);
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

        // INSERT COURSE
        [HttpPost]
        public IActionResult Create(Course course)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertCourse", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CourseName", course.CourseName);
                cmd.Parameters.AddWithValue("@Duration", course.Duration);
                cmd.Parameters.AddWithValue("@Fee", course.Fee);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }

        // EDIT PAGE
        public IActionResult Edit(int id)
        {
            Course course = new Course();

            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_GetCourseById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CourseId", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    course.CourseId = Convert.ToInt32(dr["CourseId"]);
                    course.CourseName = dr["CourseName"].ToString();
                    course.Duration = dr["Duration"].ToString();
                    course.Fee = Convert.ToDecimal(dr["Fee"]);
                }

                con.Close();
            }

            return View(course);
        }

        // UPDATE COURSE
        [HttpPost]
        public IActionResult Edit(Course course)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateCourse", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CourseId", course.CourseId);
                cmd.Parameters.AddWithValue("@CourseName", course.CourseName);
                cmd.Parameters.AddWithValue("@Duration", course.Duration);
                cmd.Parameters.AddWithValue("@Fee", course.Fee);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }

        // DELETE COURSE
        public IActionResult Delete(int id)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteCourse", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CourseId", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }
    }
}