
using hotelRooms.Models;
using Microsoft.AspNetCore.Mvc;
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
                                //tempList.startdate = (DateOnly)reader[5];
                                //tempList.slutdate = (DateOnly)reader[6];
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