using AgaveLinkCase.GridSystem;

namespace AgaveLinkCase.LinkSystem.Validator
{
    public interface ILinkValidator
    {
        public bool IsLinkExist(Grid2D grid2D);
    }
}