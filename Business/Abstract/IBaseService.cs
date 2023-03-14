namespace Business.Abstract
{
    public interface IBaseService<T>
    {
        void DeleteObject(T obj);
    }
}
