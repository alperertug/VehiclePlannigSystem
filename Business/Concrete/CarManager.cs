using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class CarManager : ICarService
    {
        private ICarDal _carDal;

        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }

        public void Add(Car car)
        {
            _carDal.Add(car);
        }

        public void Delete(int carId)
        {
            //delete için T entity tanımlanmış
            //_carDal.Delete()
        }

        public List<Car> GetAll()
        {
            return _carDal.GetList();
        }

        public void Update(Car car)
        {
            _carDal.Update(car);
        }
    }
}
