using DataAccess.Identity.Data;
using Entities.Concrete;
using Entities.ViewModels;

namespace Business.Abstract
{
    public interface IReservationService
    {
        Task<List<ReservationViewModel>> GetAllReservationsNonDeletedAsync();
        List<Reservation> GetAllCustom();
        List<Reservation> GetAllByUserId(string id);
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<ReservationViewModel> GetReservationByIdAsync(int id);
        void AddReservation(ReservationViewModel reservation);
        void DeleteReservation(int id);
        void SoftDelete(int id);
        void EarlyDelivery(int id);
        void UpdateReservation(int id);
        bool CheckReservation(Reservation reservation);
        List<string> BusyDates(int carId);
        List<Reservation> GetReservationsByVehicleId(int vehicleId);
        List<Reservation> GetMyReservations(string user);
    }
}
