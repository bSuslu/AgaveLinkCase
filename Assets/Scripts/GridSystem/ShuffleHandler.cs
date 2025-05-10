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

namespace AgaveLinkCase.GridSystem
{
    public class ShuffleHandler : BaseGridProcessHandler
    {
        private readonly Grid2D _grid;
        private readonly VisualSettings _visualSettings;
        private readonly int _minLinkLength;

        private readonly List<LinkCondition> _conditions;
        private readonly Vector2Int[] _directions;

        public ShuffleHandler(Grid2D grid, VisualSettings visualSettings)
        {
            _grid = grid;
            _visualSettings = visualSettings;
            _minLinkLength = ServiceLocator.Global.Get<SettingsProvider>().LinkSettings.MinLinkLength;

            var linkSettings = ServiceLocator.Global.Get<SettingsProvider>().LinkSettings;
            _conditions = linkSettings.LinkConditions.ToList();
            var neighbourCondition = _conditions.OfType<LinkNeighbourCondition>().FirstOrDefault();
            _directions = neighbourCondition?.Directions ?? linkSettings.DefaultAnyNeighbourCondition.Directions;
        }

        public override async UniTask HandleAsync()
        {
            if (!IsLinkExist())
            {
                await Shuffle();
            }
        }

        public bool IsLinkExist()
        {
            int width = _grid.Width;
            int height = _grid.Height;
            var visited = new HashSet<Vector2Int>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var position = new Vector2Int(x, y);

                    if (visited.Contains(position))
                        continue;

                    var cell = _grid.GetCell(x, y);
                    if (!cell.IsOccupied || cell.ChipEntity is not ILinkable start)
                        continue;

                    var chain = new HashSet<Vector2Int>();
                    DepthFirstSearch(_grid, position, null, visited, chain, null);

                    if (chain.Count >= _minLinkLength)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void DepthFirstSearch(Grid2D grid2D, Vector2Int position, ILinkable? previous, HashSet<Vector2Int> visited,
            HashSet<Vector2Int> chain, Vector2Int? incomingDirection = null)
        {
            if (!grid2D.IsValid(position.x, position.y) || visited.Contains(position))
                return;

            var cell = grid2D.GetCell(position.x, position.y);
            if (!cell.IsOccupied || cell.ChipEntity is not ILinkable current)
                return;

            if (previous != null && !_conditions.All(c => c.AreMet(previous, current)))
                return;

            visited.Add(position);
            chain.Add(position);

            foreach (var direction in _directions)
            {
                if (incomingDirection.HasValue && direction == -incomingDirection.Value)
                    continue;

                var nextPosition = position + direction;
                DepthFirstSearch(grid2D, nextPosition, current, visited, chain, direction);
            }
        }

        private async UniTask Shuffle()
        {
            do
            {
                var chipList = new List<ChipEntity>();
                for (int x = 0; x < _grid.Width; x++)
                {
                    for (int y = 0; y < _grid.Height; y++)
                    {
                        var cell = _grid.GetCell(x, y);
                        chipList.Add(cell.ChipEntity);
                        cell.SetChip(null);
                    }
                }

                ListShuffle(chipList);
                await AssignShuffledChips(chipList);
            } while (!IsLinkExist());
        }

        private void ListShuffle<T>(IList<T> list)
        {
            System.Random rng = new System.Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1); // 0 ≤ k ≤ n
                (list[n], list[k]) = (list[k], list[n]); // Swap
            }
        }

        private async UniTask AssignShuffledChips(List<ChipEntity> chipList)
        {
            var taskList = new List<UniTask>();
            int index = 0;
            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    var cell = _grid.GetCell(x, y);
                    var chip = chipList[index++];
                    cell.SetChip(chip);
                    taskList.Add(chip.transform
                        .DOMove(_grid.GetWorldPositionCenter(x, y), _visualSettings.ShuffleDuration)
                        .SetEase(_visualSettings.ShuffleEase)
                        .ToUniTask());
                }
            }

            await UniTask.WhenAll(taskList.ToArray());
        }
    }
}