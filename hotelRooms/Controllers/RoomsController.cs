using hotelRooms.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
namespace HotelH2.Controllers
{
    public class RoomsController : Controller
    {
		public List<Rooms> roomList = new List<Rooms>();
		public IActionResult Index()
        {

	     List<Rooms> rooms = new List<Rooms>();
		string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
			

			using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM [hoteltest].[dbo].[Rooms]";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Rooms rom = new Rooms();
                            rooms.Add(new Rooms
                            {
                                ID = (int)reader["ID"],
                                Type = reader["Type"].ToString(),
                                Price = (int)reader["Price"],
                                occipied = (bool)reader["Occipied"],
                                maxPersoncount = (int)reader["MaxPeople"],
                                startdate = (DateOnly)reader["Startdate"],
                                slutdate = (DateOnly)reader["Slutdate"],
                                temp = (int)reader["Temp"]
                            });
                        }
                    }
                }
            }
            return View(roomList);
        }
    }
}
