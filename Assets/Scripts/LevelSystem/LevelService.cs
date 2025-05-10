namespace AgaveLinkCase.LevelSystem
{
    public class LevelService
    {
        public LevelData LevelData { get; private set; }

        public void SetLevelData(LevelData levelData)
        {
            LevelData = levelData;
        }

    }
}