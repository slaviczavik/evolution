using System;

[Serializable]
public class NodeData
{
    public float[] Scale;
    public ConnectionData[] Connections;

    public NodeData(Node node)
    {
        Scale = new float[3];
        Scale[0] = node.Scale.x;
        Scale[1] = node.Scale.y;
        Scale[2] = node.Scale.z;

        if (node.Connections != null)
        {
            Connections = new ConnectionData[node.Connections.Count];

            for (int i = 0; i < node.Connections.Count; i++)
            {
                Connection conn = node.Connections[i];
                Connections[i] = new ConnectionData(conn);
            }
        }
    }
}