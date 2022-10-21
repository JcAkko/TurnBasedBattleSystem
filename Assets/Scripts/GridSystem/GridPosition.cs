


using System;


// this is used to store the grid position on x and z only
// use struct instead of class because I want to modify the gridpositon as a copy not a reference
public struct GridPosition : IEqautable<GridPosition>
{
    // the positon on x axis
    public int x;
    // the position on z axis
    public int z;


    // constructor
    public GridPosition(int x_, int z_)
    {
        this.x = x_;
        this.z = z_;
    }


    // auto create by C# *****
    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
            x == position.x &&
            z == position.z;
    }


    // for IEqautable
    public bool Equals(GridPosition other)
    {
        return this == other;
    }

    // auto create by C# *****
    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }


    // overide the to-string function to show the actual coordinate
    public override string ToString()
    {
        return "x: " + x + "; z: " + z;
    }


    // override the equal and not equal function to allow two gridposition to compare
    public static bool operator ==(GridPosition a, GridPosition b)
    {
        // if all equals, return true otherwise return false
        return a.x == b.x && a.z == b.z;
    }

    // override the equal and not equal function to allow two gridposition to compare
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    // allow + and - operation for this class
    public static GridPosition operator + (GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }

    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }

}

public interface IEqautable<T>
{
}