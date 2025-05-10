using UnityEngine;

namespace AgaveLinkCase.GridSystem
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "Settings/Grid")]
    public class GridSettings : ScriptableObject
    {
        // TODO: Currently dependent to chip and cell bg size, do add to other calculations to make it accessible
        public float CellSize = 2.56f;
        [field: SerializeField] public Vector3 OriginPosition { get; private set; }
        [field: SerializeField] public bool Debug { get; private set; } = true;
    }
}