using System;

[Serializable]
public class MovementData
{
    public AxisLimitData X;
    public AxisLimitData Y;
    public AxisLimitData Z;

    public MovementData(Movement movement)
    {
        X = new AxisLimitData(movement.X);
        Y = new AxisLimitData(movement.Y);
        Z = new AxisLimitData(movement.Z);
    }
}
