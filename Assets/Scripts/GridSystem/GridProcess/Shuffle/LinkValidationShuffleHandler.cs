using System.Collections.Generic;
using System.Linq;
using AgaveLinkCase.Chip;
using AgaveLinkCase.LinkSystem;
using AgaveLinkCase.LinkSystem.Conditions;
using AgaveLinkCase.ServiceLocatorSystem;
using AgaveLinkCase.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace AgaveLinkCase.GridSystem.GridProcess.Shuffle
{
    [CreateAssetMenu(fileName = "LinkValidationShuffleGridProcessHandler", menuName = "GridProcessHandlers/LinkValidationShuffle")]
    public class LinkValidationShuffleHandler : BaseGridProcessHandler
    {
        private int _minLinkLength;
        private List<LinkCondition> _conditions;
        protected Vector2Int[] _neighborDirectionsToCheck; // this parts decide diagonals or orthogonal or both
        private LinkValidator _linkValidator;

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
            while (!_linkValidator.IsLinkExist(_gridController.Grid))
            {
                await UniTask.Delay(_visualSettings.ShuffleIntervalMS);
                await Shuffle();
            }
        }

        private async UniTask Shuffle()
        {
            var taskList = new List<UniTask>();
            List<ChipEntity> allChips = new List<ChipEntity>();

            ExtractChips(allChips);
            FisherYatesShuffle(allChips);
            AssignChipsToCells(allChips);
            MoveChipToGridPosition(allChips, taskList);
            
            await UniTask.WhenAll(taskList.ToArray());
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

        // TODO : Add Fisher-Yates shuffle to utils
        private void FisherYatesShuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}