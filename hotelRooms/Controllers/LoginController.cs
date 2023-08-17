using hotelRooms.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace hotelRooms.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Login()
        {
            List<Login> loginList = ReadLogins();
            return View(loginList);
        }

        [HttpPost]
        public IActionResult Login(string AdminUsername, string AdminPassword)
        {
            List<Login> loginList = ReadLogins();

            var user = loginList.FirstOrDefault(login =>
                login.AdminUsername == AdminUsername && login.AdminPassword == AdminPassword);

            if (user != null)
            {
            
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View(); 
            }
        }

        public List<Login> ReadLogins()
        {
            List<Login> loginList = new List<Login>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM hoteltest.dbo.AdminLogin";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Login logins = new Login();

                            logins.AdminLoginId = (int)reader[0];
                            logins.AdminUsername = (string)reader[1];
                            logins.AdminPassword = (string)reader[2];
                            loginList.Add(logins);
                        }
                    }
                }
            } 

            return loginList;
        }
    }
}
