using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using TrainingCentreManagement.Models;

namespace TrainingCentreManagement.Controllers
{
    public class TrainerController : Controller
    {
        private readonly IConfiguration _configuration;

        public TrainerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ======================
        // INDEX
        // ======================
        public IActionResult Index()
        {
            List<Trainer> list = new List<Trainer>();
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_GetTrainers", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Trainer
                    {
                        TrainerId = Convert.ToInt32(dr["TrainerId"]),
                        TrainerName = dr["TrainerName"].ToString(),
                        Email = dr["Email"].ToString(),
                        Phone = dr["Phone"].ToString(),
                        Expertise = dr["Expertise"].ToString()
                    });
                }
            }

            return View(list);
        }

        // ======================
        // CREATE
        // ======================

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Trainer trainer)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertTrainer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TrainerName", trainer.TrainerName);
                cmd.Parameters.AddWithValue("@Email", trainer.Email);
                cmd.Parameters.AddWithValue("@Phone", trainer.Phone);
                cmd.Parameters.AddWithValue("@Expertise", trainer.Expertise);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // ======================
        // EDIT (GET)
        // ======================

        public IActionResult Edit(int id)
        {
            Trainer trainer = new Trainer();

            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_GetTrainerById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TrainerId", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    trainer.TrainerId = Convert.ToInt32(dr["TrainerId"]);
                    trainer.TrainerName = dr["TrainerName"].ToString();
                    trainer.Email = dr["Email"].ToString();
                    trainer.Phone = dr["Phone"].ToString();
                    trainer.Expertise = dr["Expertise"].ToString();
                }
            }

            return View(trainer);
        }

        // ======================
        // EDIT (POST)
        // ======================

        [HttpPost]
        public IActionResult Edit(Trainer trainer)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateTrainer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TrainerId", trainer.TrainerId);
                cmd.Parameters.AddWithValue("@TrainerName", trainer.TrainerName);
                cmd.Parameters.AddWithValue("@Email", trainer.Email);
                cmd.Parameters.AddWithValue("@Phone", trainer.Phone);
                cmd.Parameters.AddWithValue("@Expertise", trainer.Expertise);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // ======================
        // DELETE
        // ======================

        public IActionResult Delete(int id)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteTrainer", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TrainerId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}