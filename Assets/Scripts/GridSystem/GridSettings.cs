using System.Collections.Generic;
using GridSystem.GridProcess;
using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "Settings/Grid")]
    public class GridSettings : ScriptableObject
    {
        [HideInInspector] public float CellSize = 2.56f;
        [field: SerializeField] public Vector3 OriginPosition { get; private set; }
        [field: SerializeField] public bool Debug { get; private set; } = true;
        [field: SerializeField] public List<BaseGridProcessHandler> InitialProcessHandlers { get; private set; }
        [field: SerializeField] public List<BaseGridProcessHandler> LinkSuccessHandlers { get; private set; }
        
        
    }
}