using System.Collections.Generic;
using UnityEngine;

public class Population
{
    public List<Genotype> Individuals;

    // Creates the first random population.
    public Population(int size)
    {
        List<Genotype> individuals = new List<Genotype>();

        for (int i = 0; i < size; i++)
        {
            Genotype individual = new Genotype();
            individuals.Add(individual);
        }

        Individuals = individuals;
    }

    public Population(List<Genotype> individuals)
    {
        Individuals = individuals;
    }

    public Population (IndividualData[] data)
    {
        Individuals = new List<Genotype>();

        for (int i = 0; i < data.Length; i++)
        {
            Individuals.Add(new Genotype(data[i]));
        }
    }

    public Population CreateNextGeneration()
    {
        // Sort creatures from best to worst.
        Individuals.Sort(CompareFitness);

        // Create next generation.
        List<Genotype> offsprings = CreateOffsprings();

        return new Population(offsprings);
    }

    // Private instance methods.

    List<Genotype> CreateOffsprings()
    {
        float mutationRate = Core.Game.mutationRate;
        List<Genotype> offsprings = new List<Genotype>();

        float generationScore = CalculateGenerationFitness();

        // Select few best individuals and add them to the next generation
        // without any modification.
        List<Genotype> elite = BestIndividuals();

        offsprings.AddRange(elite);

        int populationSize = Core.Game.populationSize;

        while (offsprings.Count < populationSize)
        {
            Genotype parentA = SelectParent(generationScore);
            Genotype parentB = SelectParent(generationScore);

            List<Genotype> children = parentA.Crossover(parentB);
            children.ForEach(child => child.Mutate(mutationRate));

            offsprings.AddRange(children);
        }

        if (offsprings.Count > populationSize)
        {
            return offsprings.GetRange(0, populationSize);
        }

        return offsprings;
    }

    List<Genotype> BestIndividuals()
    {
        int count = Individuals.Count;
        int elitism = Mathf.CeilToInt(count * Core.Game.elitism);

        return Individuals.GetRange(0, elitism);
    }

    Genotype SelectParent(float generationScore)
    {
        float r = Random.Range(0, generationScore);
        float s = 0;

        // Random Pool Selection.
        foreach (Genotype individual in Individuals)
        {
            s += individual.Fitness;

            if (s >= r)
            {
                return individual;
            }
        }

        return Individuals[0];
    }

    float CalculateGenerationFitness()
    {
        float sum = 0;
        Individuals.ForEach(individual => sum += individual.Fitness);

        return sum;
    }

    // Private static methods.

    static int CompareFitness(Genotype a, Genotype b)
    {
        if (a.Fitness > b.Fitness)
        {
            // A comes first.
            return -1;
        }
        else if (a.Fitness < b.Fitness)
        {
            // B comes first.
            return 1;
        }

        return 0;
    }
}
