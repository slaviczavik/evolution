using System;
using System.Collections.Generic;

public struct Environment
{
    public string name;
    public string scene;
    public float gravity;
    public List<String> behaviours;

    public Environment(string name, string scene, float gravity, List<String> behaviours)
    {
        this.name = name;
        this.scene = scene;
        this.gravity = gravity;
        this.behaviours = behaviours;
    }
}
