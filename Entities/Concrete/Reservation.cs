using Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Reservation : EntityBase
    {
        [Column("UserId", TypeName ="nvarchar(36)")]
        public string UserId { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime DeliveryTime { get; set; }
        public int ReasonForReservationId { get; set; }
        public ReasonForReservation? ReasonForReservation { get; set; }
    }
}
