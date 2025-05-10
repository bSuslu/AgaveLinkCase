using UnityEngine;

namespace AgaveLinkCase.Helpers
{
    public class ApplicationTargetFrameRate : MonoBehaviour
    {
        void Start()
        {
            Application.targetFrameRate = 60;
        }
    }
}
