using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using TrainingCentreManagement.Models;

namespace TrainingCentreManagement.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IConfiguration _configuration;

        public AttendanceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // VIEW ATTENDANCE
        public IActionResult Index()
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

        // OPEN MARK ATTENDANCE PAGE
        public IActionResult MarkAttendance()
        {
            return View();
        }

        // SAVE ATTENDANCE
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

            return RedirectToAction("Index");
        }

        // DELETE ATTENDANCE
        public IActionResult Delete(int id)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AttendanceId", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }
    }
}