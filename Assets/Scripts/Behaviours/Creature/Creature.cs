using System.Collections.Generic;
using UnityEngine;

// This component class is responsible for rendering the creature.
public class Creature : MonoBehaviour
{
    public Genotype Skeleton;
    public List<Material> Materials;
    public Vector3 Position;
    public float MaxDistance;
    public float CurrentDistance;
    public float TimeOnGround;
    public float AverageHeight;

    void Update()
    {
        CalculateTraveledDistance();
        // CalculateTimeOnGround();
        // CalculateAverageHeight();
    }

    public void Init()
    {
        float spawn = Spawn.GeSpawnPosition(Skeleton);
        Position.y += spawn;

        Node genotype = GetBody();
        GameObject phenotype = RenderBody();

        CreateBranches(genotype, phenotype);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    // Private instance methods.
    void CalculateTimeOnGround()
    {
        float sum = 0;

        foreach (Transform child in transform)
        {
            Sensor sensor = child.GetComponent<Sensor>();

            sum += sensor.TimeOnGround;
        }

        TimeOnGround = sum / transform.childCount;
    }

    void CalculateAverageHeight()
    {
        float maximumHeight = 0;

        foreach (Transform child in transform)
        {
            Sensor sensor = child.GetComponent<Sensor>();

            maximumHeight += sensor.MaximumHeight;
        }

        AverageHeight = maximumHeight / transform.childCount;
    }

    void CalculateTraveledDistance()
    {
        Transform root = transform.GetChild(0);
        float traveledDistance = Mathf.Abs(root.position.z);

        if (traveledDistance > MaxDistance)
        {
            MaxDistance = traveledDistance;
        }

        CurrentDistance = traveledDistance;
    }

    void CreateBranches(Node node, GameObject go)
    {
        if (node.Connections == null) return;

        foreach (Connection connection in node.Connections)
        {
            CreateBranch(connection, go);
        }
    }

    void CreateBranch(Connection connection, GameObject go)
    {
        Node child = Skeleton.Nodes[connection.Node];

        CreateNode(child, go, connection);
    }

    void CreateNode(Node node, GameObject go, Connection connection)
    {
        Material material = Materials[1];
        Movement movement = connection.Movement;

        Vector3 position = Cube.CalculatePosition(node, go, connection);
        Vector3 anchor = Cube.SetAnchor(connection);

        GameObject obj = RenderNode(node.Scale, position, material);
        Muscle muscle = obj.AddComponent<Muscle>();
        muscle.SetJoint(go, anchor, movement);

        CreateBranches(node, obj);
    }

    Node GetBody()
    {
        return Skeleton.Nodes[0];
    }

    GameObject RenderBody()
    {
        Material material = Materials[0];
        Node body = GetBody();

        return RenderNode(body.Scale, Position, material);
    }

    GameObject RenderNode(Vector3 scale, Vector3 position, Material material)
    {
        GameObject obj = Cube.CreateNode(scale, position, material);
        obj.transform.SetParent(transform);
        obj.AddComponent<Sensor>();

        return obj;
    }
}
