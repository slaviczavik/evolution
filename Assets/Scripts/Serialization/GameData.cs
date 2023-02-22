using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public float mutationRate;
    public int lifeTimeLimit;
    public int populationSize;
    public int generationLimit;
    public int animationSpeed;
    public int counter;
    public int generation;
    public string environment;
    public string behaviour;
    public float elitism;
    public float gravity;
    public IndividualData[] population;

    public GameData(Game game)
    {
        mutationRate = game.mutationRate;
        lifeTimeLimit = game.lifeTimeLimit;
        populationSize = game.populationSize;
        generationLimit = game.generationLimit;
        counter = game.counter;
        generation = game.generation;
        environment = game.environment.name;
        behaviour = game.behaviour.name;
        elitism = game.elitism;
        gravity = game.gravity;

        population = SerializePopulation(game.population);
    }

    static IndividualData[] SerializePopulation(Population population)
    {
        List<Genotype> individuals = population.Individuals;
        IndividualData[] result = new IndividualData[individuals.Count];

        for (int i = 0; i < individuals.Count; i++)
        {
            result[i] = new IndividualData(individuals[i]);
        }

        return result;
    }
}
