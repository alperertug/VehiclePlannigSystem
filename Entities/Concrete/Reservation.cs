using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Reservation : EntityBase
    {
        [Column("UserId", TypeName ="nvarchar(36)")]
        public string UserId { get; set; }
        public int CarId { get; set; }
        public Car? Car { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReservationDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime DeliveryTime { get; set; }
        public int ReasonForReservationId { get; set; }
        public ReasonForReservation? ReasonForReservation { get; set; }
    }
}
