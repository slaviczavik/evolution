public struct AxisLimit
{
    public bool enabled;
    public float angle;
    public float speed;
    public float force;

    public AxisLimit(bool enabled, float limit, float speed, float force)
    {
        this.angle = limit;
        this.enabled = enabled;
        this.speed = speed;
        this.force = force;
    }

    public AxisLimit(AxisLimitData axisLimit)
    {
        angle = axisLimit.angle;
        enabled = axisLimit.enabled;
        speed = axisLimit.speed;
        force = axisLimit.force;
    }
}