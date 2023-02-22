using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 Scale;
    public List<Connection> Connections;

    public Node()
    {
        Scale = RandomScale();
    }

    /// <summary>
    ///     Use for object duplication.
    /// </summary>
    /// <param name="node">Object to duplicate.</param>
    public Node(Node node)
    {
        Scale = node.Scale;

        if (node.Connections != null)
        {
            Connections = new List<Connection>();
            node.Connections.ForEach(c => Connections.Add(new Connection(c)));
        }
    }

    /// <summary>
    ///     Use for object deserialization.
    /// </summary>
    /// <param name="data"></param>
    public Node(NodeData data)
    {
        float[] scl = data.Scale;
        Scale = new Vector3(scl[0], scl[1], scl[2]);

        if (data.Connections != null)
        {
            Connections = new List<Connection>();

            for (int i = 0; i < data.Connections.Length; i++)
            {
                ConnectionData conn = data.Connections[i];
                Connections.Add(new Connection(conn));
            }
        }
    }

    public void AddConnection(Connection connection)
    {
        if (Connections == null)
        {
            Connections = new List<Connection>();
        }

        Connections.Add(connection);
    }

    public void Mutate(float rate)
    {
        if (Core.Random.NextDouble() < rate)
        {
            Scale = RandomScale();
        }

        if (Connections != null)
        {
            Connections.ForEach(c => c.Mutate(rate));
        }
    }

    // Private static methods.

    /// <summary>
    ///     Will create random scale based on a volume.
    /// </summary>
    /// <returns></returns>
    static Vector3 RandomScale()
    {
        float volume = 0.25f;

        float x = Random.Range(0.1f, 1f);
        float y = Random.Range(0.1f, 1f);
        float z = Random.Range(0.1f, 1f);

        Vector3 scale = new Vector3(x, y, z);

        float m = Mathf.Min(new float[] { x, y, z });

        // Ratio of sides, smallest side has value 1.
        Vector3 n = scale * (1 / m);

        // n.xq * n.yq * n.zq = volume
        // n.x * n.y * n.z * q^3 = volume
        // q^3 = volume / (n.x * n.y * n.z)
        // q = pow(volume / (n.x * n.y * n.z), 1 / 3)
        float f = volume / (n.x * n.y * n.z);
        float q = Mathf.Pow(f, (float)1 / 3);

        return n * q;
    }
}
