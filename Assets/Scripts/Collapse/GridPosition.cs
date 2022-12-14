using System;

namespace Collapse
{
    public struct GridPosition : IEquatable<GridPosition>
    {
        public int x;
        public int y;
        
        public GridPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public override bool Equals(object obj)
        {
            return obj is GridPosition other && Equals(other);
        }
        
        public bool Equals(GridPosition other)
        {
            return x == other.x && y == other.y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x * 397) ^ y;
            }
        }

        public static bool operator == (GridPosition leftSide, GridPosition rightSide)
        {
            return leftSide.x == rightSide.x && leftSide.y == rightSide.y;
        }

        public static bool operator != (GridPosition leftSide, GridPosition rightSide)
        {
            return !(leftSide == rightSide);
        }
        
        public static GridPosition operator+(GridPosition a, GridPosition b)
        {
            return new GridPosition(a.x + b.x, a.y + b.y);
        }

        public static GridPosition operator-(GridPosition a, GridPosition b)
        {
            return new GridPosition(a.x - b.x, a.y - b.y);
        }
    }
}