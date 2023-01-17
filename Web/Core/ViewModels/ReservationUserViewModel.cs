using DataAccess.Identity.Data;
using Entities.Concrete;

namespace AktifVehiclePlanningSystem.Core.ViewModels
{
    public class ReservationUserViewModel
    {
        public ApplicationUser User { get; set; }
        public Reservation Reservation { get; set; }
    }
}
