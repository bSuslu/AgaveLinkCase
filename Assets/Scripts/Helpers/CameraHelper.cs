using UnityEngine;

namespace Helpers
{
    internal class CameraHelper : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Vector2 _gridPadding;
        [SerializeField] private Vector3 _offset;
    

        public void HandleGridFrustum(Vector3 frustumMinWorldPos, Vector3 frustumMaxWorldPos)
        {
            var maxPos = frustumMaxWorldPos;
            var minPos = frustumMinWorldPos;
            float width = maxPos.x - minPos.x;
            float height = maxPos.y - minPos.y;

            Vector2 center = (minPos + maxPos) / 2f;

            _camera.transform.position = new Vector3(center.x, center.y, _camera.transform.position.z) + _offset;

            float aspectRatio = _camera.aspect;

            float verticalSize = (height / 2f) / aspectRatio + _gridPadding.y;
            float horizontalSize = (width / 2f) / aspectRatio + _gridPadding.x;

            _camera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
        }
    }
}