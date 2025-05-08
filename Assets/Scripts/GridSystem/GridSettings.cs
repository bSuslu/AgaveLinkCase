using UnityEngine;

namespace AgaveLinkCase.GridSystem
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "Settings/Grid")]
    public class GridSettings : ScriptableObject
    {
        [field: SerializeField] public float CellSize;
        [field: SerializeField] public Vector3 OriginPosition;
        [field: SerializeField] public bool Debug = true;
    }
}
