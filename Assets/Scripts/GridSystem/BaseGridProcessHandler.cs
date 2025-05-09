using Cysharp.Threading.Tasks;

namespace AgaveLinkCase.GridSystem
{
    public abstract class BaseGridProcessHandler
    {
        public abstract UniTask HandleAsync();
    }
}