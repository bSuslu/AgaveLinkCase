using System.Collections.Generic;
using System.Linq;
using AgaveLinkCase.Chip;
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

        public ShuffleHandler(Grid2D grid, VisualSettings visualSettings, List<Vector2Int> coords)
        {
            _grid = grid;
            _visualSettings = visualSettings;
            _minLinkLength = ServiceLocator.Global.Get<SettingsProvider>().LinkSettings.MinLinkLength;
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
            var conditions = ServiceLocator.Global.Get<SettingsProvider>().LinkSettings.LinkConditions.ToList();
            int width = _grid.Width;
            int height = _grid.Height;
            var visited = new HashSet<Vector2Int>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pos = new Vector2Int(x, y);

                    if (visited.Contains(pos))
                        continue;

                    var cell = _grid.GetCell(x, y);
                    if (!cell.IsOccupied || cell.ChipEntity is not ILinkable start)
                        continue;

                    var chain = new HashSet<Vector2Int>();
                    DFS(_grid, pos, null, visited, chain, conditions);

                    if (chain.Count >= _minLinkLength)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void DFS(Grid2D grid, Vector2Int pos, ILinkable? prev, HashSet<Vector2Int> visited,
            HashSet<Vector2Int> chain, List<LinkNeighbourCondition> conditions)
        {
            if (!grid.IsValid(pos.x, pos.y) || visited.Contains(pos))
                return;

            var cell = grid.GetCell(pos.x, pos.y);
            if (!cell.IsOccupied || cell.ChipEntity is not ILinkable current)
                return;
            
            if (prev != null && !conditions.All(c => c.AreMet(prev, current)))
                return;

            visited.Add(pos);
            chain.Add(pos);

            Vector2Int[] dirs = { new(0, 1), new(1, 0), new(0, -1), new(-1, 0), new(1, 1), new(1, -1), new(-1, 1), new(-1, -1) };

            foreach (var dir in dirs)
            {
                var nextPos = pos + dir;
                DFS(grid, nextPos, current, visited, chain, conditions);
            }
        }
        
        private async UniTask Shuffle()
        {
            do
            {
                var list = new List<ChipEntity>();
                for (int x = 0; x < _grid.Width; x++)
                {
                    for (int y = 0; y < _grid.Height; y++)
                    {
                        var cell = _grid.GetCell(x, y);
                        list.Add(cell.ChipEntity);
                        cell.SetChip(null);
                    }
                }

                ListShuffle(list);
                await AssignShuffledChips(list);
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
        private async UniTask AssignShuffledChips(List<ChipEntity> list)
        {
            var taskList = new List<UniTask>();
            int i = 0;
            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    var cell = _grid.GetCell(x, y);
                    var chip = list[i++];
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