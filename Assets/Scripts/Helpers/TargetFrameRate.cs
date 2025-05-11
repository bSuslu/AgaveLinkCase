using UnityEngine;

namespace Helpers
{
    public class ApplicationTargetFrameRate : MonoBehaviour
    {
        void Start()
        {
            Application.targetFrameRate = 60;
        }
    }
}
