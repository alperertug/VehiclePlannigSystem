using AutoMapper;
using Business.Abstract;
using DataAccess.Identity.Data;
using DataAccess.UnitOfWorks;
using Entities.Concrete;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Business.Concrete
{
    public class ReservationManager : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReservationManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddReservation(ReservationViewModel reservationViewModel)
        {
            var reservation = new Reservation()
            {
                UserId = reservationViewModel.UserId,
                CarId = reservationViewModel.CarId,
                ReservationDate = reservationViewModel.ReservationDate,
                DeliveryTime = reservationViewModel.DeliveryTime,
                ReasonForReservationId = reservationViewModel.ReasonForReservationId,
                CreatedDate = reservationViewModel.CreatedDate
            };
            //await _unitOfWork.GetRepository<Reservation>.AddAsync(reservation);
            //await _unitOfWork.SaveAsync();
        }

        public List<string> BusyDates(int carId)
        {
            var reservationsOfVehicle = _unitOfWork.
                GetDbContext().Reservations.
                Where(x => x.CarId == carId).
                Where(d => d.DeliveryTime > DateTime.Today).
                OrderBy(x => x.ReservationDate);
            
            if (reservationsOfVehicle != null)
            {
                var dateList = new List<string>();
                foreach (var item in reservationsOfVehicle)
                {
                    dateList.Add(item.ReservationDate.ToLongDateString() + "-" + item.DeliveryTime.ToLongDateString());
                }
                return dateList;
            }
            return new List<string>();
            
        }

        public bool CheckReservation(Reservation reservation)
        {
            var reservationsInDB = _unitOfWork.GetDbContext().Reservations.Where(i => i.IsDeleted == false).Where(x => x.CarId == reservation.CarId).ToList();
            if (reservationsInDB != null)
            {
                foreach (var reservationInDb in reservationsInDB)
                {
                    if (reservation.ReservationDate < reservationInDb.DeliveryTime && reservationInDb.ReservationDate <= reservation.DeliveryTime)
                    {
                        return false;
                    }
                }
            }            
            return true;
        }
        //reservation.ReservationDate <= reserv.DeliveryTime && reserv.ReservationDate<reservation.DeliveryTime
        public void DeleteReservation(int id)
        {
            throw new NotImplementedException();
        }

        public void EarlyDelivery(int id)
        {
            var reservationInDB = _unitOfWork.GetDbContext().Reservations.FirstOrDefault(i => i.Id == id);
            reservationInDB.DeliveryTime = DateTime.Today;
            reservationInDB.ModifiedDate = DateTime.Now;
            _unitOfWork.Save();
        }

        public List<Reservation> GetAllByUserId(string id)
        {
            return _unitOfWork.GetDbContext().
                Reservations.
                Where(x => x.UserId == id).
                Where(i => i.IsDeleted == false).
                Where(d => d.DeliveryTime > DateTime.Today).
                ToList();
        }

        public List<Reservation> GetAllCustom()
        {
            return _unitOfWork.GetDbContext().Reservations.Where(x => x.IsDeleted == false).Where(d => d.DeliveryTime > DateTime.Today).ToList();
        }

        public async Task<List<Reservation>> GetAllReservationsAsync()
        {
            return await _unitOfWork.GetRepository<Reservation>().GetAllAsync(x => !x.IsDeleted);
        }

        public async Task<List<ReservationViewModel>> GetAllReservationsNonDeletedAsync()
        {
            var reservations = await _unitOfWork.GetRepository<Reservation>().GetAllAsync(x => !x.IsDeleted);
            var map = _mapper.Map<List<ReservationViewModel>>(reservations);
            return map;
        }

        public List<Reservation> GetMyReservations(string userId)
        {            
            return _unitOfWork.
                GetDbContext().Reservations.
                Where(r => r.UserId == userId).
                Where(i => i.IsDeleted == false).
                OrderBy(d => d.ReservationDate).
                ToList();
        }

        public async Task<ReservationViewModel> GetReservationByIdAsync(int id)
        {
            var reservation = await _unitOfWork.GetRepository<Reservation>().GetAsync(x => x.Id == id);
            var map = _mapper.Map<ReservationViewModel>(reservation);

            return map;
        }

        public List<Reservation> GetReservationsByVehicleId(int vehicleId)
        {
            return _unitOfWork.GetDbContext().Reservations.Where(x => x.CarId == vehicleId).Where(d => d.DeliveryTime > DateTime.Today).ToList();
        }

        public void SoftDelete(int id)
        {
            var reservation = _unitOfWork.GetDbContext().Reservations.FirstOrDefault(x => x.Id == id);
            reservation.IsDeleted = true;
            _unitOfWork.Save();
        }

        public void UpdateReservation(int id)
        {
            throw new NotImplementedException();
        }

        void IReservationService.AddReservation(ReservationViewModel reservation)
        {
            throw new NotImplementedException();
        }
    }
}
