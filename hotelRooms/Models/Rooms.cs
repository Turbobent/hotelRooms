using System.ComponentModel.DataAnnotations.Schema;

namespace hotelRooms.Models
{
    public class Rooms
    {
        public int ID { get; set; }
        public string Type { get; set; }

        public int price{ get; set; }
        public bool occipied { get; set; }
        public int maxPersoncount { get; set; }
        //public DateOnly startdate { get; set; }
        //public DateOnly slutdate { get; set; }
        public int temp { get; set; }
    }
}
