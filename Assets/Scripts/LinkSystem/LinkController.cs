using System;
using System.Collections.Generic;
using System.Linq;
using EventSystem;
using GridSystem;
using LinkSystem.Conditions;
using ServiceLocatorSystem;
using Settings;
using UnityEngine;

namespace LinkSystem
{
    public class LinkController : MonoBehaviour
    {
        [SerializeField] private GridInputSystem _gridInputSystem;
        private LinkCondition[] _linkCreateConditions;
        public event Action<List<ILinkable>> OnLinkSuccess;
        public event Action OnLinkReset;
        public event Action<List<ILinkable>> OnLinkUpdated;

        private readonly List<ILinkable> _linkables = new List<ILinkable>();
        public List<ILinkable> ActiveLinkables;

        private LinkSettings _linkSettings;

        private void Awake()
        {
            _gridInputSystem.OnNewCellTouched += OnNewCellTouched;
            _gridInputSystem.OnRelease += OnRelease;

            _linkSettings = ServiceLocator.Global.Get<SettingsProvider>().LinkSettings;
            _linkCreateConditions = _linkSettings.LinkConditions;
        }

        private void OnDestroy()
        {
            _gridInputSystem.OnNewCellTouched -= OnNewCellTouched;
            _gridInputSystem.OnRelease -= OnRelease;
        }

        private void OnNewCellTouched(Cell newCell)
        {
            if (newCell.IsLocked || !newCell.IsOccupied)
                return;

            if (newCell.ChipEntity is not ILinkable newLinkable)
                return;

            if (_linkables.Count.Equals(0))
            {
                LinkCell(newCell);
                return;
            }

            if (_linkables.Count > 1 && newLinkable == _linkables[^2])
            {
                UnlinkLastCell();
                return;
            }

            if (_linkables.Contains(newLinkable))
                return;

            if (!AreConditionsMet(newCell))
                return;

            LinkCell(newCell);
        }

        private bool AreConditionsMet(Cell newCell)
        {
            var lastLinkable = _linkables.Last();
            foreach (var linkCondition in _linkCreateConditions)
            {
                if (!linkCondition.AreMet(lastLinkable, newCell.ChipEntity))
                    return false;
            }

            return true;
        }

        private void UnlinkLastCell()
        {
            _linkables.RemoveAt(_linkables.Count - 1);
            OnLinkUpdated?.Invoke(_linkables);
        }

        private void OnRelease()
        {
            if (IsLinkConditionsMet())
            {
                ActiveLinkables = new List<ILinkable>(_linkables);
                OnLinkSuccess?.Invoke(ActiveLinkables);
                EventBus<LinkSuccessEvent>.Publish(new LinkSuccessEvent(ActiveLinkables));
            }

            _linkables.Clear();
            OnLinkReset?.Invoke();
        }

        private bool IsLinkConditionsMet()
        {
            return _linkables.Count >= _linkSettings.MinLinkLength;
        }

        private void LinkCell(Cell cell)
        {
            _linkables.Add(cell.ChipEntity);
            OnLinkUpdated?.Invoke(_linkables);
        }
    }
}