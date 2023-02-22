using System;
using System.Collections.Generic;

public static class Core
{
    public static Game Game;
    public static Random Random = new Random();

    public static readonly List<Environment> Environments = new List<Environment>()
    {
        new Environment("Earth", "EarthScene", 9.81f,
            new List<string>() { "Walking" }),

        new Environment("Mars", "MarsScene", 3.71f,
            new List<string>() { "Walking" }),

        new Environment("Moon", "MoonScene", 1.62f,
            new List<string>() { "Walking" })
    };

    static readonly List<Behaviour> Behaviours = new List<Behaviour>()
    {
        new Behaviour("Walking", (creature) => {
            // Fraction of time spent on the ground <0, 1>.
            // float t = creature.TimeOnGround / Game.creatureLifeTime;
            // float d = creature.MaxDistance;
            float d = creature.CurrentDistance;

            // float fitness = t * Math.Abs(d);
            float fitness = Math.Abs(d);

            return fitness > 0 ? fitness : 0;
        })
    };

    public static Environment GetEnvironment(string name)
    {
        return Environments.Find(x => x.name == name);
    }

    public static Behaviour GetBehaviour(string name)
    {
        return Behaviours.Find(x => x.name == name);
    }
}