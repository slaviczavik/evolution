using UnityEngine;

public class Game
{
    
    public float mutationRate = 0.01f; // The mutation rate.
    public int lifeTimeLimit = 10; // The life time of the creature.
    public int populationSize = 12; // The population size.
    public int generationLimit = 250; // Where the evolution will stop.
    public float elitism = 0.01f;
    public Behaviour behaviour;
    public Environment environment;
    public int animationSpeed = 1; // Speed of animation.
    public int counter = 0; // Index of current skeleton in the population.
    public int generation = 0; // Generation counter:
    public bool isRunning = false; // Is game running?
    public bool isPaused = false; // For showing the Pause Menu.
    public bool isLoading = false;
    public Population population; // List of creatures for a current population.
    public Creature creature; // A current creature.
    public float creatureLifeTime; // A creature life timer.
    public float gravity = 9.81f; // World gravity

    public Game(float mutationRate, int lifeTimeLimit, int populationSize,
        int generationLimit, Environment environment, float elitism,
        float gravity)
    {
        this.mutationRate = mutationRate;
        this.lifeTimeLimit = lifeTimeLimit;
        this.populationSize = populationSize;
        this.generationLimit = generationLimit;
        this.environment = environment;
        this.elitism = elitism;
        this.gravity = gravity;

        this.behaviour = Core.GetBehaviour("Walking");

        this.population = new Population(populationSize);

        SetGravity(gravity);
    }

    public Game(GameData data)
    {
        LoadData(data);
        SetGravity(gravity);
    }

    /// <summary>
    ///     Sets the animation speed.
    /// </summary>
    public void SetTimeScale(int animationSpeed)
    {
        // https://docs.unity3d.com/ScriptReference/Time-timeScale.html
        Time.timeScale = animationSpeed;
    }

    public void Pause()
    {
        SetTimeScale(0);
        isPaused = true;
    }

    public void Resume()
    {
        SetTimeScale(animationSpeed);
        SetGravity(gravity);
        isPaused = false;
    }

    public void CreateNextGeneration()
    {
        Population newGeneration = population.CreateNextGeneration();

        population = newGeneration;
        counter = 0;
        generation++;
    }

    public float CalculateFitness()
    {
        return behaviour.fitnessFunc(creature);
    }

    public void EvaluateCreature()
    {
        creature.Skeleton.Fitness = CalculateFitness();
    }

    void LoadData(GameData data)
    {
        mutationRate = data.mutationRate;
        lifeTimeLimit = data.lifeTimeLimit;
        populationSize = data.populationSize;
        generationLimit = data.generationLimit;
        counter = data.counter;
        generation = data.generation;
        elitism = data.elitism;
        gravity = data.gravity;

        behaviour = Core.GetBehaviour(data.behaviour);
        environment = Core.GetEnvironment(data.environment);

        population = new Population(data.population);
    }

    static void SetGravity(float gravity)
    {
        Physics.gravity = new Vector3(0, gravity * -1, 0);
    }
}
