using System;

public struct Behaviour
{
    public string name;
    public Func<Creature, float> fitnessFunc;

    public Behaviour(string name, Func<Creature, float> fitnessFunc)
    {
        this.name = name;
        this.fitnessFunc = fitnessFunc;
    }
}
