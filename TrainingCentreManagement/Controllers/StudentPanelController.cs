using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using TrainingCentreManagement.Models;

namespace TrainingCentreManagement.Controllers
{
    public class StudentPanelController : Controller
    {
        private readonly IConfiguration _configuration;

        public StudentPanelController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // DASHBOARD
        public IActionResult Dashboard()
        {
            return View();
        }

        // ==========================
        // MY COURSE (SHOW ALL COURSES)
        // ==========================

        public IActionResult MyCourse()
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
                    Course course = new Course();

                    course.CourseId = Convert.ToInt32(dr["CourseId"]);
                    course.CourseName = dr["CourseName"].ToString();
                    course.Duration = dr["Duration"].ToString();
                    course.Fee = Convert.ToDecimal(dr["Fee"]);

                    list.Add(course);
                }

                con.Close();
            }

            return View(list);
        }

        // ==========================
        // MY SCHEDULE
        // ==========================

        public IActionResult MySchedule()
        {
            List<Schedule> list = new List<Schedule>();

            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_GetSchedules", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Schedule s = new Schedule();

                    s.ScheduleId = Convert.ToInt32(dr["ScheduleId"]);
                    s.BatchId = Convert.ToInt32(dr["BatchId"]);
                    s.Date = Convert.ToDateTime(dr["Date"]);
                    s.Topic = dr["Topic"].ToString();

                    list.Add(s);
                }

                con.Close();
            }

            return View(list);
        }

        // ==========================
        // MY ATTENDANCE
        // ==========================

        public IActionResult MyAttendance()
        {
            List<Attendance> list = new List<Attendance>();

            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Attendance a = new Attendance();

                    a.AttendanceId = Convert.ToInt32(dr["AttendanceId"]);
                    a.StudentId = Convert.ToInt32(dr["StudentId"]);
                    a.BatchId = Convert.ToInt32(dr["BatchId"]);
                    a.Date = Convert.ToDateTime(dr["Date"]);
                    a.Status = dr["Status"].ToString();

                    list.Add(a);
                }

                con.Close();
            }

            return View(list);
        }
    }
}