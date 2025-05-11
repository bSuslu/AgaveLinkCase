using System.Collections.Generic;
using System.Linq;
using Chip;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using LinkSystem.Conditions;
using LinkSystem.Validator;
using ServiceLocatorSystem;
using Settings;
using UnityEngine;
using Utils;

namespace GridSystem.GridProcess.Shuffle
{
    [CreateAssetMenu(fileName = "LinkValidationShuffleGridProcessHandler", menuName = "GridProcessHandlers/LinkValidationShuffle")]
    public class LinkValidationShuffleHandler : BaseGridProcessHandler
    {
        private int _minLinkLength;
        private List<LinkCondition> _conditions;
        private Vector2Int[] _neighborDirectionsToCheck; // this parts decide diagonals or orthogonal or both
        private LinkValidator _linkValidator;
        private List<ChipEntity> _allChips;

        public override void Init(GridController gridController)
        {
            base.Init(gridController);

            SetupFields();
        }

        private void SetupFields()
        {
            var linkSettings = ServiceLocator.Global.Get<SettingsProvider>().LinkSettings;
            _minLinkLength = linkSettings.MinLinkLength;
            _conditions = linkSettings.LinkConditions.ToList();

            var neighbourCondition = _conditions.OfType<LinkNeighbourCondition>().FirstOrDefault();
            _neighborDirectionsToCheck =
                neighbourCondition?.Directions ?? linkSettings.DefaultAnyNeighbourCondition.Directions;
            _linkValidator = new LinkValidator(_conditions, _minLinkLength, _neighborDirectionsToCheck);
        }

        public override async UniTask HandleAsync()
        {
            await UniTask.Yield();
            
            int shuffleCount = 0;
            // TODO add max shuffle count
            while (!_linkValidator.IsLinkExist(_gridController.Grid))
            {
                shuffleCount++;
                Shuffle();
            }

            if (shuffleCount > 0)
            {
                var taskList = new List<UniTask>();
                MoveChipToGridPosition(_allChips, taskList);
                await UniTask.WhenAll(taskList.ToArray());
            }

            await UniTask.Yield();
        }

        private void Shuffle()
        {
            _allChips = new List<ChipEntity>();
            ExtractChips(_allChips);
            CollectionUtils.FisherYatesShuffle(_allChips);
            AssignChipsToCells(_allChips);
        }

        private void MoveChipToGridPosition(List<ChipEntity> allChips, List<UniTask> taskList)
        {
            foreach (var chipEntity in allChips)
            {
                taskList.Add(chipEntity.transform.DOMove(
                    _gridController.Grid.GetWorldPositionCenter(chipEntity.CellPos.x, chipEntity.CellPos.y),
                    _visualSettings.ShuffleDuration).SetEase(_visualSettings.ShuffleEase).ToUniTask());
            }
        }

        private void AssignChipsToCells(List<ChipEntity> allChips)
        {
            for (int x = 0; x < _gridController.Grid.Width; x++)
            {
                for (int y = 0; y < _gridController.Grid.Height; y++)
                {
                    Cell currentCell = _gridController.Grid.GetCell(x, y);
                    currentCell.SetChip(allChips[x * _gridController.Grid.Height + y]);
                }
            }
        }

        private void ExtractChips(List<ChipEntity> allChips)
        {
            for (int x = 0; x < _gridController.Grid.Width; x++)
            {
                for (int y = 0; y < _gridController.Grid.Height; y++)
                {
                    Cell currentCell = _gridController.Grid.GetCell(x, y);
                    if (currentCell.IsOccupied)
                    {
                        ChipEntity currentChipEntity = currentCell.ChipEntity;
                        allChips.Add(currentChipEntity);
                        currentCell.SetChip(null);
                    }
                }
            }
        }
    }
}