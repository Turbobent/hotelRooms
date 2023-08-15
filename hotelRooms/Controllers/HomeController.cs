
using hotelRooms.Models;
using Microsoft.AspNetCore.Mvc;
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
                                roomToUpdate.Price = (int)reader[2];
                                roomToUpdate.occipied = (bool)reader[3];
                                roomToUpdate.maxPersoncount = (int)reader[4];
                                roomToUpdate.startdate = (DateOnly)reader[5];
                                roomToUpdate.slutdate = (DateOnly)reader[6];

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
            using (SqlConnection con = new SqlConnection("Data Source=PCVDATALAP100\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    con.Open();
                    string query = "UPDATE hoteltest.dbo.Rooms SET Type = @Type, Price = @Price, occipied = @Occipied, maxPersoncount = @MaxPersoncount, startdate = @StartDate, slutdate = @EndDate WHERE ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", updatedRoom.ID);
                        cmd.Parameters.AddWithValue("@Type", updatedRoom.Type);
                        cmd.Parameters.AddWithValue("@Price", updatedRoom.Price);
                        cmd.Parameters.AddWithValue("@Occipied", updatedRoom.occipied);
                        cmd.Parameters.AddWithValue("@MaxPersoncount", updatedRoom.maxPersoncount);
                        cmd.Parameters.AddWithValue("@StartDate", updatedRoom.startdate);
                        cmd.Parameters.AddWithValue("@EndDate", updatedRoom.slutdate);

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
                                tempList.Price = (int)reader[2];
                                tempList.occipied = (bool)reader[3];
                                tempList.maxPersoncount = (int)reader[4];

                                tempList.temp = (int)reader[7];
                                if (!reader.IsDBNull(5))
                                {
                                    DateTime startDateTime = reader.GetDateTime(5);
                                    tempList.startdate = new System.DateOnly(startDateTime.Year, startDateTime.Month, startDateTime.Day);
                                }

                                if (!reader.IsDBNull(6))
                                {
                                    DateTime slutDateTime = reader.GetDateTime(6);
                                    tempList.slutdate = new System.DateOnly(slutDateTime.Year, slutDateTime.Month, slutDateTime.Day);
                                }

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