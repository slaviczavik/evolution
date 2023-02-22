using System.Collections.Generic;

public static class Spawn
{
    public static float GeSpawnPosition(Genotype genotype)
    {
        List<Node> nodes = genotype.Nodes;

        Node node = nodes[0];

        float spawnHeight = 0;

        foreach (Connection connection in node.Connections)
        {
            float height = BranchHeight(nodes, connection, node);

            if (connection.Direction.y == -1)
            {
                height += node.Scale.y / 2;
            }

            if (height > spawnHeight)
            {
                spawnHeight = height;
            }
        }

        return spawnHeight;
    }

    static float MaxBranchHeight(List<Node> nodes, Node node)
    {
        float maximum = 0;

        foreach (Connection connection in node.Connections)
        {
            float result = BranchHeight(nodes, connection, node);

            if (result > maximum)
            {
                maximum = result;
            }
        }

        return maximum;
    }

    static float BranchHeight(List<Node> nodes, Connection conn, Node parent)
    {
        Node child = nodes[conn.Node];
        float childHeight = child.Scale.y;
        float parentHeight = parent.Scale.y;
        float direction = conn.Direction.y;
        // -20% of placement if placed verticaly.
        float activeHeight = conn.Placement.y == 0 ? 1 : 0.8f;

        if (child.Connections == null)
        {
            if (direction == 1)
            {
                return 0;
            }

            if (direction == 0)
            {
                if (childHeight <= parentHeight) return 0;

                return childHeight / 2;
            }

            
            return childHeight * activeHeight;
        }

        float result = MaxBranchHeight(nodes, child);

        if (direction == -1)
        {
            return (childHeight * activeHeight) + result;
        }

        if (direction == 0)
        {
            return result > childHeight / 2 ? result : childHeight / 2;
        }

        return result;
    }
}
