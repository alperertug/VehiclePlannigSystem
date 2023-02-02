using AutoMapper;
using Business.Abstract;
using DataAccess.UnitOfWorks;
using Entities.Concrete;
using Entities.ViewModels;

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

        public void DeleteReservation(int id)
        {
            throw new NotImplementedException();
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

        public async Task<ReservationViewModel> GetReservationByIdAsync(int id)
        {
            var reservation = await _unitOfWork.GetRepository<Reservation>().GetAsync(x => x.Id == id);
            var map = _mapper.Map<ReservationViewModel>(reservation);

            return map;
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
