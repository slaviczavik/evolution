using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Position
{
    public static readonly List<Vector3> Points = new List<Vector3>()
    {
        new Vector3( 1,  1,  1), // Right-Top-Forward
        new Vector3( 1,  1,  0), // Right-Top-Center
        new Vector3( 1,  1, -1), // Right-Top-Backward

        new Vector3( 1,  0,  1), // Right-Center-Forward
        new Vector3( 1,  0,  0), // Right-Center-Center
        new Vector3( 1,  0, -1), // Right-Center-Backward

        new Vector3( 1, -1,  1), // Right-Bottom-Forward
        new Vector3( 1, -1,  0), // Right-Bottom-Center
        new Vector3( 1, -1, -1), // Right-Bottom-Backward

        new Vector3( 0,  1,  1), // Center-Top-Forward
        new Vector3( 0,  1,  0), // Center-Top-Center
        new Vector3( 0,  1, -1), // Center-Top-Backward

        new Vector3( 0,  0,  1), // Center-Center-Forward
        new Vector3( 0,  0, -1), // Center-Center-Backward

        new Vector3( 0, -1,  1), // Center-Bottom-Forward
        new Vector3( 0, -1,  0), // Center-Bottom-Center
        new Vector3( 0, -1, -1), // Center-Bottom-Backward

        new Vector3(-1,  1,  1), // Left-Top-Forward
        new Vector3(-1,  1,  0), // Left-Top-Center
        new Vector3(-1,  1, -1), // Left-Top-Backward

        new Vector3(-1,  0,  1), // Left-Center-Forward
        new Vector3(-1,  0,  0), // Left-Center-Center
        new Vector3(-1,  0, -1), // Left-Center-Backward

        new Vector3(-1, -1,  1), // Left-Bottom-Forward
        new Vector3(-1, -1,  0), // Left-Bottom-Center
        new Vector3(-1, -1, -1)  // Left-Bottom-Backward 
    };

    // Move to Node?
    public static List<Vector3> AvailablePoints(Node node)
    {
        List<Vector3> points = new List<Vector3>(Points);

        if (node.Connections == null) return points;

        foreach (Connection connection in node.Connections)
        {
            Vector3 direction = connection.Direction;

            points.Remove(direction);
            NeighborPoints(direction).ForEach(p => points.Remove(p));
        }

        return points;
    }

    public static Vector3 RandomPoint(Node node)
    {
        List<Vector3> points = AvailablePoints(node);

        return points.Count > 0
            ? points[Core.Random.Next(points.Count)]
            : Vector3.zero;
    }

    /// <summary>
    ///     Returns all neighbor points of the point.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public static List<Vector3> NeighborPoints(Vector3 point)
    {
        List<Vector3> points = new List<Vector3>();

        if (point.x == 0)
        {
            points.Add(new Vector3(1, point.y, point.z));
            points.Add(new Vector3(-1, point.y, point.z));
        }
        else
        {
            points.Add(new Vector3(0, point.y, point.z));
        }

        if (point.y == 0)
        {
            points.Add(new Vector3(point.x, 1, point.z));
            points.Add(new Vector3(point.x, -1, point.z));
        }
        else
        {
            points.Add(new Vector3(point.x, 0, point.z));
        }

        if (point.z == 0)
        {
            points.Add(new Vector3(point.x, point.y, 1));
            points.Add(new Vector3(point.x, point.y, -1));
        }
        else
        {
            points.Add(new Vector3(point.x, point.y, 0));
        }

        return points;
    }
}
