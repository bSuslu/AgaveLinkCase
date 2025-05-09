using System.Collections.Generic;
using AgaveLinkCase.Chip;
using UnityEngine;

namespace AgaveLinkCase.LinkSystem
{
    public class LinkLineDrawer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private LinkController _linkController;

        private Vector3[] _positions;
        private void OnEnable()
        {
            _linkController.OnLinkUpdated += UpdateLine;
            _linkController.OnLinkReset += ResetLine;
        }
        
        private void OnDisable()
        {
            _linkController.OnLinkUpdated -= UpdateLine;
            _linkController.OnLinkReset -= ResetLine;
        }

        private void ResetLine()
        {
            _positions = null;
            _lineRenderer.positionCount = 0;
        }
        
        private void UpdateLine(List<ILinkable> linkables)
        {
            _positions = linkables.ConvertAll(linkable => linkable.Transform.position).ToArray();
            _lineRenderer.positionCount = _positions.Length;
            _lineRenderer.SetPositions(_positions);
        }
    }
}