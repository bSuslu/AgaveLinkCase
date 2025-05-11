namespace Pool
{
    public interface IPoolable<T>
    {
        T Get();
        public void SetPool(IPool<T> pool);
    }
}