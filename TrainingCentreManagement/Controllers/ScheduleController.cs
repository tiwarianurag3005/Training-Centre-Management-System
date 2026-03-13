using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using TrainingCentreManagement.Models;

namespace TrainingCentreManagement.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IConfiguration _configuration;

        public ScheduleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET ALL SCHEDULES
        public IActionResult Index()
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

        // CREATE PAGE
        public IActionResult Create()
        {
            return View();
        }

        // INSERT SCHEDULE
        [HttpPost]
        public IActionResult Create(Schedule schedule)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BatchId", schedule.BatchId);
                cmd.Parameters.AddWithValue("@Date", schedule.Date);
                cmd.Parameters.AddWithValue("@Topic", schedule.Topic);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }

        // EDIT PAGE
        public IActionResult Edit(int id)
        {
            Schedule schedule = new Schedule();

            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_GetScheduleById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ScheduleId", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    schedule.ScheduleId = Convert.ToInt32(dr["ScheduleId"]);
                    schedule.BatchId = Convert.ToInt32(dr["BatchId"]);
                    schedule.Date = Convert.ToDateTime(dr["Date"]);
                    schedule.Topic = dr["Topic"].ToString();
                }

                con.Close();
            }

            return View(schedule);
        }

        // UPDATE
        [HttpPost]
        public IActionResult Edit(Schedule schedule)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ScheduleId", schedule.ScheduleId);
                cmd.Parameters.AddWithValue("@BatchId", schedule.BatchId);
                cmd.Parameters.AddWithValue("@Date", schedule.Date);
                cmd.Parameters.AddWithValue("@Topic", schedule.Topic);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteSchedule", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ScheduleId", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }
    }
}