using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using TrainingCentreManagement.Models;

namespace TrainingCentreManagement.Controllers
{
    public class TrainerPanelController : Controller
    {
        private readonly IConfiguration _configuration;

        public TrainerPanelController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // DASHBOARD
        public IActionResult Dashboard()
        {
            return View();
        }
        // GET ALL BATCHES

        public IActionResult MyBatche()
        {
            List<Batch> list = new List<Batch>();
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_GetBatches", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Batch b = new Batch();

                    b.BatchId = Convert.ToInt32(dr["BatchId"]);
                    b.BatchName = dr["BatchName"].ToString();
                    b.CourseId = Convert.ToInt32(dr["CourseId"]);
                    b.TrainerId = Convert.ToInt32(dr["TrainerId"]);
                    b.StartDate = Convert.ToDateTime(dr["StartDate"]);
                    b.EndDate = Convert.ToDateTime(dr["EndDate"]);

                    list.Add(b);
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


        // VIEW STUDENTS
        public IActionResult Students()
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

        // ==========================
        // VIEW ATTENDANCE LIST
        // ==========================

        public IActionResult AttendanceList()
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

        // ==========================
        // ADD ATTENDANCE
        // ==========================

        public IActionResult MarkAttendance()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MarkAttendance(Attendance attendance)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentId", attendance.StudentId);
                cmd.Parameters.AddWithValue("@BatchId", attendance.BatchId);
                cmd.Parameters.AddWithValue("@Date", attendance.Date);
                cmd.Parameters.AddWithValue("@Status", attendance.Status);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("AttendanceList");
        }

        // ==========================
        // EDIT ATTENDANCE
        // ==========================

        public IActionResult EditAttendance(int id)
        {
            Attendance attendance = new Attendance();

            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAttendanceById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AttendanceId", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    attendance.AttendanceId = Convert.ToInt32(dr["AttendanceId"]);
                    attendance.StudentId = Convert.ToInt32(dr["StudentId"]);
                    attendance.BatchId = Convert.ToInt32(dr["BatchId"]);
                    attendance.Date = Convert.ToDateTime(dr["Date"]);
                    attendance.Status = dr["Status"].ToString();
                }

                con.Close();
            }

            return View(attendance);
        }

        [HttpPost]
        public IActionResult EditAttendance(Attendance attendance)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AttendanceId", attendance.AttendanceId);
                cmd.Parameters.AddWithValue("@StudentId", attendance.StudentId);
                cmd.Parameters.AddWithValue("@BatchId", attendance.BatchId);
                cmd.Parameters.AddWithValue("@Date", attendance.Date);
                cmd.Parameters.AddWithValue("@Status", attendance.Status);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("AttendanceList");
        }
    }
}