using UnityEngine;

public class Connection
{
    public Vector3 Direction;
    public Vector3 Placement;
    public Movement Movement;
    public int Node;

    public Connection(Vector3 direction, int nodeIndex)
    {
        Direction = direction;
        Node = nodeIndex;
        Placement = CreatePlacement(Direction);
        Movement = CreateMovement();
    }

    /// <summary>
    ///     Use for object duplication.
    /// </summary>
    /// <param name="connection">Object to duplicate</param>
    public Connection(Connection connection)
    {
        Direction = connection.Direction;
        Placement = connection.Placement;
        Movement = connection.Movement;
        Node = connection.Node;
    }

    /// <summary>
    ///     Use for deserialization.
    /// </summary>
    /// <param name="data">Serialized data</param>
    public Connection(ConnectionData data)
    {
        float[] dir = data.direction;
        float[] plc = data.placement;

        Direction = new Vector3(dir[0], dir[1], dir[2]);
        Placement = new Vector3(plc[0], plc[1], plc[2]);
        Movement = new Movement(data.movement);
        Node = data.node;
    }

    public void Mutate(float rate)
    {
        if (Core.Random.NextDouble() < rate)
        {
            Movement = CreateMovement();
        }
    }

    // Private static methods.

    /// <summary>
    ///     Create a placement relative to its parent where the node
    ///     will be placed.
    /// </summary>
    /// <returns></returns>
    static Vector3 CreatePlacement(Vector3 direction)
    {
        float x = direction.x;
        float y = direction.y;
        float z = direction.z;

        // Bottom and top center.
        if (x == 0 && z == 0) return Vector3.zero;

        Vector3 placement = new Vector3(1, 1, 1);

        // Corners.
        if (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1 && Mathf.Abs(z) == 1)
        {
            // Node cannot be placed on X and Z axis at same time.
            if (Core.Random.NextDouble() < 0.5) placement.x = 0;
            else placement.z = 0;

            return placement;
        }

        if (y == 0)
        {
            placement.y = 0;

            // Vertical center of edges.
            if (Mathf.Abs(x) == 1 && Mathf.Abs(z) == 1)
            {
                if (Core.Random.NextDouble() < 0.5) placement.x = 0;
                else placement.z = 0;

                return placement;
            }

            // The center of sides.
            return Vector3.zero;
        }

        // Center of edges at top and bottom.
        if (x == 0)
        {
            placement.x = 0;

            if (Core.Random.NextDouble() < 0.5) placement.y = 0;
            else placement.z = 0;
        }
        else if (z == 0)
        {
            placement.z = 0;

            if (Core.Random.NextDouble() < 0.5) placement.x = 0;
            else placement.y = 0;
        }

        return placement;
    }

    static Movement CreateMovement()
    {
        AxisLimit x = CreateAxisLimit();
        AxisLimit y = CreateAxisLimit();
        AxisLimit z = CreateAxisLimit();

        return new Movement(x, y, z);
    }

    static AxisLimit CreateAxisLimit()
    {
        bool enabled = Core.Random.NextDouble() < 0.5;
        float angle = enabled ? Core.Random.Next(10, 71) : 0;
        float speed = enabled ? Core.Random.Next(1, 3) : 0;
        float force = enabled ? Core.Random.Next(0, 101) : 0;

        return new AxisLimit(enabled, angle, speed, force);
    }
}
