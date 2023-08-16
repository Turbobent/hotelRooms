
using hotelRooms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace HotelH2.Controllers
{
    public class HomeController : Controller
    {
        public List<Rooms> RoomsList = new List<Rooms>();

        public IActionResult Index()
        {
            ReadRoom(RoomsList);

            return View(RoomsList);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Create()
        {


            List<SelectListItem> roomTypes = new List<SelectListItem>
    { 
        new SelectListItem { Text = "Standard", Value = "Standard" },
        new SelectListItem { Text = "Premium", Value = "Premium" },
        new SelectListItem { Text = "Delux", Value = "Delux" }
    };

            ViewData["RoomTypes"] = roomTypes;

            return View();
        }
        [HttpPost]
        public IActionResult CreateRooms(Rooms newRoom)
        {
            using (SqlConnection con = new SqlConnection("Data Source=PCVDATALAP100\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO hoteltest.dbo.Rooms (Type, price, maxPersoncount) VALUES (@Type, @price, @MaxPersoncount)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Type", newRoom.Type);
                        cmd.Parameters.AddWithValue("@price", newRoom.price);
                        cmd.Parameters.AddWithValue("@MaxPersoncount", newRoom.maxPersoncount);
                        cmd.Parameters.AddWithValue("@occipied", newRoom.occipied = false);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            // Handle insert failure here
                        }
                    }
                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the room. Please try again.");
                    return View("Create", newRoom);
                }
            }
        }
        public IActionResult Update(int id)
        {

            using (SqlConnection con = new SqlConnection("Data Source=PCVDATALAP100\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM hoteltest.dbo.Rooms WHERE ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Rooms roomToUpdate = new Rooms();
                                roomToUpdate.ID = (int)reader[0];
                                roomToUpdate.Type = (string)reader[1];
                                roomToUpdate.price = (int)reader[2];
                                roomToUpdate.occipied = (bool)reader[3];
                                roomToUpdate.maxPersoncount = (int)reader[4];


                                return View(roomToUpdate);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Handle exceptions here
                    return View("Error"); // Redirect to an error page or show an error view
                }
            }
        }

        [HttpPost]
        public bool UpdateRoomDetails(Rooms updatedRoom)
        {
            // Determine whether the checkbox is checked or not
            bool isOccipied = !string.IsNullOrEmpty(Request.Form["occipied"]);

            string query = "UPDATE hoteltest.dbo.Rooms SET Type = @Type, price = @price, occipied = @Occipied, maxPersoncount = @MaxPersoncount, temp = @temp WHERE ID = @ID";

            using (SqlConnection con = new SqlConnection("Data Source=PCVDATALAP100\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", updatedRoom.ID);
                        cmd.Parameters.AddWithValue("@Type", updatedRoom.Type);
                        cmd.Parameters.AddWithValue("@price", updatedRoom.price);
                        cmd.Parameters.AddWithValue("@Occipied", isOccipied);
                        cmd.Parameters.AddWithValue("@MaxPersoncount", updatedRoom.maxPersoncount);
                        cmd.Parameters.AddWithValue("@temp", updatedRoom.temp);
                        //cmd.Parameters.AddWithValue("@StartDate", updatedRoom.startdate);
                        //cmd.Parameters.AddWithValue("@EndDate", updatedRoom.slutdate);

                        int rowsAffected = cmd.ExecuteNonQuery();


                        return rowsAffected > 0;
                    }
                }
                catch (SqlException ex)
                {
                    // Handle exceptions here
                    return false;
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<Rooms> ReadRoom(List<Rooms> RoomsList)
        {
            string ConnectionString = "Data Source=PCVDATALAP100\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                try
                {
                    string query = "SELECT * FROM hoteltest.dbo.Rooms";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Rooms tempList = new Rooms();

                                tempList.ID = (int)reader[0];
                                tempList.Type = (string)reader[1];
                                tempList.price = (int)reader[2];
                                tempList.occipied = (bool)reader[3];
                                tempList.maxPersoncount = (int)reader[4];

                                //tempList.temp = (int)reader[7];
                                //if (!reader.IsDBNull(5))
                                //{
                                //    DateTime startDateTime = reader.GetDateTime(5);
                                //    tempList.startdate = new System.DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day);
                                //}

                                //if (!reader.IsDBNull(6))
                                //{
                                //    DateTime slutDateTime = reader.GetDateTime(6);
                                //    tempList.slutdate = new System.DateTime(slutDateTime.Year, slutDateTime.Month, slutDateTime.Day);
                                //}

                                tempList.temp = (int)reader[7];

                                RoomsList.Add(tempList);

                            }
                        }
                    }
                    connection.Close();
                }
                catch (SqlException e)
                {

                    throw e;
                }
            }
            return RoomsList;
        }

    }
}