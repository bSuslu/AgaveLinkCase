namespace AgaveLinkCase.Pool
{
    public interface IPool<T>
    {
        public void ReturnToPool(IPoolable<T> poolable);
    }
}