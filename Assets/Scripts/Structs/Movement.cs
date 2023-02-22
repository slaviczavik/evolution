/// <summary>
///     Helper class for storing data about node connection to its parent.
/// </summary>
public struct Movement
{
    public AxisLimit X;
    public AxisLimit Y;
    public AxisLimit Z;

    public Movement(AxisLimit x, AxisLimit y, AxisLimit z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Movement(MovementData data)
    {
        X = new AxisLimit(data.X);
        Y = new AxisLimit(data.Y);
        Z = new AxisLimit(data.Z);
    }
}
