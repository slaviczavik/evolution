using System;
using System.Collections.Generic;

[Serializable]
public class IndividualData
{
    public NodeData[] Nodes;
    public float Fitness;

    public IndividualData(Genotype individual)
    {
        Fitness = individual.Fitness;
        Nodes = SerializeNodes(individual.Nodes);
    }

    NodeData[] SerializeNodes(List<Node> nodes)
    {
        NodeData[] result = new NodeData[nodes.Count];

        for (int i = 0; i < nodes.Count; i++)
        {
            result[i] = new NodeData(nodes[i]);
        }

        return result;
    }
}