using System;

[Serializable]
public class AxisLimitData
{
    public bool enabled;
    public float angle;
    public float speed;
    public float force;

    public AxisLimitData(AxisLimit axisLimit)
    {
        enabled = axisLimit.enabled;
        angle = axisLimit.angle;
        speed = axisLimit.speed;
        force = axisLimit.force;
    }
}