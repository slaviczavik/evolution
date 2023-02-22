using System.Collections.Generic;
using UnityEngine;

public class Genotype
{
    public List<Node> Nodes;
    public float Fitness;

    public Genotype()
    {
        Nodes = new List<Node>();

        CreateGenotype();
    }

    public Genotype(List<Node> nodes)
    {
        Nodes = nodes;
    }

    public Genotype(IndividualData data)
    {
        Fitness = data.Fitness;
        Nodes = new List<Node>();

        for (int i = 0; i < data.Nodes.Length; i++)
        {
            Node node = new Node(data.Nodes[i]);
            Nodes.Add(node);
        }
    }

    /// <summary>
    ///     Use for object duplication.
    /// </summary>
    /// <param name="genotype">Object to duplicate.</param>
    public Genotype(Genotype genotype)
    {
        List<Node> nodes = new List<Node>();

        genotype.Nodes.ForEach(n => nodes.Add(new Node(n)));

        Nodes = nodes;
    }

    // Public instance methods.

    public List<Genotype> Crossover(Genotype otherParent)
    {
        // Create copies of the parents.
        Genotype individualA = new Genotype(this);
        Genotype individualB = new Genotype(otherParent);

        switch(Core.Random.Next(0, 3))
        {
            case 0:
                individualA.OnePointCrossover(individualB);
                break;
            case 1:
                individualA.TwoPointCrossover(individualB);
                break;
            case 2:
                individualA.UniformCrossover(individualB);
                break;
        }

        return new List<Genotype>() { individualA, individualB };
    }

    public void Mutate(float rate)
    {
        // Mutate the genotype.
        foreach(Node node in Nodes)
        {
            node.Mutate(rate);
        }

        if (Core.Random.NextDouble() < rate)
        {
            if (Nodes.Count > 12)
            {
                RemoveNode();
            }
            else
            {
                AddNode();
            }
        }
    }

    // Private instance methods.

    void OnePointCrossover(Genotype otherParent)
    {
        int countA = Nodes.Count;
        int countB = otherParent.Nodes.Count;

        int maximum = Mathf.Min(countA, countB);
        int point = Core.Random.Next(1, maximum); // <1, maximum)

        for (int i = point; i < maximum; i++)
        {
            SwapGene(Nodes, otherParent.Nodes, i);
        }
    }

    void TwoPointCrossover(Genotype otherParent)
    {
        int countA = Nodes.Count;
        int countB = otherParent.Nodes.Count;

        int maximum = Mathf.Min(countA, countB);
        int pointA = Core.Random.Next(1, maximum); // <1, maximum)
        int pointB = Core.Random.Next(1, maximum); // <1, maximum)

        int min = Mathf.Min(pointA, pointB);
        int max = Mathf.Max(pointA, pointB);

        for (int i = min; i <= max; i++)
        {
            SwapGene(Nodes, otherParent.Nodes, i);
        }
    }

    void UniformCrossover(Genotype otherParent)
    {
        int countA = Nodes.Count;
        int countB = otherParent.Nodes.Count;

        int maximum = Mathf.Min(countA, countB);

        for (int i = 0; i < maximum; i++)
        {
            // Fifty percent chance.
            if (Core.Random.Next(0, 10) < 5) continue;

            SwapGene(Nodes, otherParent.Nodes, i);
        }
    }

    void SwapGene(List<Node> A, List<Node> B, int position)
    {
        Node nodeA = A[position];
        Node nodeB = B[position];
        Node temp = nodeA;

        nodeA.Scale = nodeB.Scale;
        nodeB.Scale = temp.Scale;

        SwapConnections(nodeA, nodeB);
    }

    void SwapConnections(Node A, Node B)
    {
        if (A.Connections == null || B.Connections == null)
        {
            return;
        }

        int max = Mathf.Min(A.Connections.Count, B.Connections.Count);

        for (int i = 0; i < max; i++)
        {
            Connection connA = A.Connections[i];
            Connection connB = B.Connections[i];
            Connection temp = connA;

            connA.Movement = connB.Movement;
            connB.Movement = temp.Movement;
        }
    }

    void CreateGenotype()
    {
        Node rootNode = new Node();
        Nodes.Add(rootNode);

        // Number (<2, 4>) of children of the root node.
        int degree = Core.Random.Next(2, 5);

        for (int i = 0; i < degree; i++)
        {
            // Fifty percent chance.
            bool isParent = degree > 2
                ? Core.Random.Next(0, 10) < 5
                : true;

            CreateNode(rootNode, isParent);
        }
    }

    void CreateNode(Node parent, bool isParent = false)
    {
        Node node = new Node();
        Nodes.Add(node);

        Vector3 direction = FindDirectionPoint(parent);

        Connection connection = new Connection(direction, Nodes.Count - 1);

        parent.AddConnection(connection);

        if (isParent) CreateNode(node);
    }

    Vector3 FindDirectionPoint(Node parent)
    {
        if (Nodes.IndexOf(parent) == 0)
        {
            return Position.RandomPoint(parent);
        }

        // Check where the parent is connected and use the same direction.
        Connection parentConnection = FindParentConnection(parent);

        if (parentConnection == null)
        {
            // If the parent isn't the root node, then it has to be connected!
            // If it isn't, then there is an issue!
            return Position.RandomPoint(Nodes[0]);
        }

        return parentConnection.Direction;
    }

    Node FindParent(Node target)
    {
        int index = Nodes.IndexOf(target);

        foreach (Node node in Nodes)
        {
            if (node.Connections == null) continue;

            foreach (Connection conn in node.Connections)
            {
                if (conn.Node == index) return node;
            }
        }

        return null;
    }

    Connection FindParentConnection(Node target)
    {
        int index = Nodes.IndexOf(target);
        Node parent = FindParent(target);

        if (parent == null || parent.Connections == null) return null;

        foreach (Connection conn in parent.Connections)
        {
            if (conn.Node == index) return conn;
        }

        return null;
    }

    void AddNode()
    {
        // Find a parent.
        Node parent = Core.Random.Next(0, 10) < 5
            ? Nodes[0] // Parent is the root node.
            : FindLeaf();

        CreateNode(parent);
    }

    void RemoveNode()
    {
        Node node = FindLeaf();
        Nodes.Remove(node);
    }

    Node FindLeaf()
    {
        foreach (Node node in Nodes)
        {
            if (node.Connections == null) return node;
        }

        return Nodes[Nodes.Count - 1];
    }
}
