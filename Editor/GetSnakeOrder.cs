using UnityEngine;

namespace Ophura
{
    internal static partial class Projection
    {
        private static Vector3 GetSnakeOrder(int Index)
        {
            // int Column = Index / 4;
            int Column = Index >> 0B10;

            // int Row = Index % 4;
            int Row = Index & 0B11;

            // if (Column % 2 is 1)
            if ((Column & 0B1) is not 0)
            {
                Row = 3 - Row;
            }

            return new(Column * 230, Row * 100, 0F);
        }
    }
}
