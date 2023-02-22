using System;

[Serializable]
public class ConnectionData
{
    public float[] direction;
    public float[] placement;
    public int node;
    public MovementData movement;

    public ConnectionData(Connection connection)
    {
        direction = new float[3];
        direction[0] = connection.Direction.x;
        direction[1] = connection.Direction.y;
        direction[2] = connection.Direction.z;

        placement = new float[3];
        placement[0] = connection.Placement.x;
        placement[1] = connection.Placement.y;
        placement[2] = connection.Placement.z;

        node = connection.Node;
        movement = new MovementData(connection.Movement);
    }
}