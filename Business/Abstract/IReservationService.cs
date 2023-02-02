using Entities.Concrete;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IReservationService
    {
        Task<List<ReservationViewModel>> GetAllReservationsNonDeletedAsync();
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<ReservationViewModel> GetReservationByIdAsync(int id);
        void AddReservation(ReservationViewModel reservation);
        void DeleteReservation(int id);
        void UpdateReservation(int id);
    }
}
