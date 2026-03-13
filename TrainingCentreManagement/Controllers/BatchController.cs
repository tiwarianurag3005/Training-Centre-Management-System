using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using TrainingCentreManagement.Models;

namespace TrainingCentreManagement.Controllers
{
    public class BatchController : Controller
    {
        private readonly IConfiguration _configuration;

        public BatchController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

    
        // GET ALL BATCHES

        public IActionResult Index()
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


        // CREATE
 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Batch batch)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertBatch", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BatchName", batch.BatchName);
                cmd.Parameters.AddWithValue("@CourseId", batch.CourseId);
                cmd.Parameters.AddWithValue("@TrainerId", batch.TrainerId);
                cmd.Parameters.AddWithValue("@StartDate", batch.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", batch.EndDate);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }

        // EDIT
        public IActionResult Edit(int id)
        {
            Batch batch = new Batch();

            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_GetBatchById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BatchId", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    batch.BatchId = Convert.ToInt32(dr["BatchId"]);
                    batch.BatchName = dr["BatchName"].ToString();
                    batch.CourseId = Convert.ToInt32(dr["CourseId"]);
                    batch.TrainerId = Convert.ToInt32(dr["TrainerId"]);
                    batch.StartDate = Convert.ToDateTime(dr["StartDate"]);
                    batch.EndDate = Convert.ToDateTime(dr["EndDate"]);
                }

                con.Close();
            }

            return View(batch);
        }

        [HttpPost]
        public IActionResult Edit(Batch batch)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateBatch", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BatchId", batch.BatchId);
                cmd.Parameters.AddWithValue("@BatchName", batch.BatchName);
                cmd.Parameters.AddWithValue("@CourseId", batch.CourseId);
                cmd.Parameters.AddWithValue("@TrainerId", batch.TrainerId);
                cmd.Parameters.AddWithValue("@StartDate", batch.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", batch.EndDate);

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
                SqlCommand cmd = new SqlCommand("sp_DeleteBatch", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BatchId", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("Index");
        }
    }
}