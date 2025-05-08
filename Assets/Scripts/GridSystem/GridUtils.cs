using UnityEngine;

namespace AgaveLinkCase.GridSystem
{
    public static class GridUtils
    {
        public static bool IsOrthogonalNeighbor(int x1, int y1, int x2, int y2)
        {
            int dx = Mathf.Abs(x1 - x2);
            int dy = Mathf.Abs(y1 - y2);
            return dx + dy == 1; // Yalnızca yatay veya dikey komşu
        }

        public static bool IsDiagonalNeighbor(int x1, int y1, int x2, int y2)
        {
            int dx = Mathf.Abs(x1 - x2);
            int dy = Mathf.Abs(y1 - y2);
            return dx == 1 && dy == 1; // Sadece çapraz
        }

        public static bool IsAnyNeighbor(int x1, int y1, int x2, int y2)
        {
            int dx = Mathf.Abs(x1 - x2);
            int dy = Mathf.Abs(y1 - y2);
            return (dx == 1 && dy == 1) || (dx + dy == 1); // Çapraz + yatay/dikey
        }
    }
}