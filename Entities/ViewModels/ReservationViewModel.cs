using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels
{
    public class ReservationViewModel
    {
        //public int Id { get; set; }
        public string UserId { get; set; }
        public int CarId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime DeliveryTime { get; set; }
        public int ReasonForReservationId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
