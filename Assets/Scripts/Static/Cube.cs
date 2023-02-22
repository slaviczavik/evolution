using UnityEngine;

public static class Cube
{
    public static GameObject CreateNode(
        Vector3 scale, Vector3 position, Material material)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Set scale and position.
        obj.transform.localScale = scale;
        obj.transform.position = position;

        // Add rigidbody.
        Rigidbody rb = obj.AddComponent<Rigidbody>();

        // Set material.
        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.material = material;

        return obj;
    }

    public static Vector3 CalculatePosition(
        Node node, GameObject parent, Connection connection)
    {
        Vector3 direction = connection.Direction;
        Vector3 placement = connection.Placement;

        // Position of the target.
        Vector3 parentPosition = parent.transform.position;
        // Scale of the target.
        Vector3 parentScale = parent.transform.localScale;
        // Target scale.
        Vector3 targetScale = node.Scale;

        // Calculate corner-to-corner position
        // -----------------------------------

        // Distance between two pivots.
        Vector3 pivotDistance = (parentScale + targetScale) / 2;
        // Relative distance between two pivots with correct direction.
        Vector3 relativePivotDistance = Vector3.Scale(pivotDistance, direction);

        // Calculate correct position relative to the parent
        // -------------------------------------------------

        // Placement with correct direction.
        Vector3 relativePlacement = Vector3.Scale(placement, direction * -1);

        // Shift from the edge.
        Vector3 shift = targetScale * 0.2f;
        // The shift with correct direction.
        Vector3 relativeShift = Vector3.Scale(shift, relativePlacement);

        return parentPosition + relativePivotDistance + relativeShift;
    }

    public static Vector3 SetAnchor(Connection connection)
    {
        // ( 1, y,  1) -> (-0.5, y, -0.5)
        // ( 1, y, -1) -> (-0.5, y,  0.5)
        // (-1, y, -1) -> ( 0.5, y,  0.5)
        // (-1, y,  1) -> ( 0.5, y, -0.5)

        float x = GetAnchor(connection, Axis.x);
        float y = GetAnchor(connection, Axis.y);
        float z = GetAnchor(connection, Axis.z);

        // Works only for corner anchors.
        return new Vector3(x, y, z);
    }

    static float GetAnchor(Connection connection, Axis axis)
    {
        int iaxis = (int)axis;

        Vector3 direction = connection.Direction;
        Vector3 placement = connection.Placement;

        // Anchor positions:
        // x: (0: center, 0.5: right, -0.5: left)
        // y: (0: center, 0.5: up, -0.5: down)
        // z: (0: center, 0.5: forward, -0.5: back)

        if (placement[iaxis] == 0)
        {
            // Move anchor to edge.
            return 0.5f * direction[iaxis] * -1;
        }

        // The node is placed on 20% of its area.
        // Anchor must be in the middle of the area,
        // that is 10% of the area, so anchor is at |0.4|.
        return 0.4f * direction[iaxis] * -1;
    }
}