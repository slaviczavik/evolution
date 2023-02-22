using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Initial class responsible for creating the game.
public class Factory : MonoBehaviour
{
    // The main camera.
    public GameObject MainCamera;

    // A materials for creatures.
    public List<Material> Materials;

    // The creature's spawn position.
    public Vector3 SpawnPosition;

    /// <summary>
    ///     Game private attributes
    /// </summary>

    Game game;

    // Current camera position.
    Vector3 cameraPosition;

    void Awake()
    {
        game = Core.Game;
        SetGravity();
    }

    /// <summary>
    ///     The MonoBehaviour.Start() function.
    ///     Real simulation is initialized right here.
    /// </summary>
    void Start()
    {
        // Initial position of the main camera.
        cameraPosition = MainCamera.transform.position;

        if (game != null)
        {
            FirstRun();
        }
    }

    /// <summary>
    ///     The MonoBehaviour.Update() function.
    /// </summary>
    void Update()
    {
        if (game != null && game.isRunning)
        {
            WhileRunning();

            if (game.creatureLifeTime > game.lifeTimeLimit)
            {
                NewRun();
            }
        }
    }

    void SetGravity()
    {
        Vector3 gravity = new Vector3(0, game.gravity * -1, 0);
        Physics.gravity = gravity;
    }

    /// <summary>
    ///     Called every frame from the Update method.
    /// </summary>
    void WhileRunning()
    {
        // Increate the creature's life timer.
        game.creatureLifeTime += Time.deltaTime;
    }

    /// <summary>
    ///     Called only once at the start of the simulation.
    /// </summary>
    void FirstRun()
    {
        if (game.counter < game.population.Individuals.Count)
        {
            PrepareNewRun();
        }
    }

    /// <summary>
    ///     Called every time before a new round.
    /// </summary>
    void PrepareNewRun()
    {
        List<Genotype> individuals = game.population.Individuals;
        Genotype individual = individuals[game.counter];

        game.creature = ActivateCreature(individual);

        FollowCreature(game.creature);

        game.isRunning = true;
    }

    /// <summary>
    ///     Called at the end of each round.
    /// </summary>
    void NewRun()
    {
        game.isRunning = false;

        if (game.creature)
        {
            CleanPreviousRun();
        }

        int min = Mathf.Min(game.populationSize, game.population.Individuals.Count);

        if (game.counter < min)
        {
            PrepareNewRun();
        }
        else if (game.generation < game.generationLimit)
        {
            CreateNextGeneration();
        }
        else
        {
            SceneManager.LoadScene("EndScene");
        }
    }

    /// <summary>
    ///     Called every time after creature's lifetime has run out.
    /// </summary>
    void CleanPreviousRun()
    {
        game.creature.Destroy();

        game.EvaluateCreature();

        UnfollowCreature();
        ResetTimer();

        game.counter++;
    }

    void CreateNextGeneration()
    {
        game.isLoading = true;

        game.CreateNextGeneration();

        game.isLoading = false;

        PrepareNewRun();
    }

    /// <summary>
    ///     Create and spawn a new creature.
    /// </summary>
    /// <param name="skeleton">
    ///     A creature genotype.
    /// </param>
    /// <returns>
    ///     The Creature MonoBehaviour script.
    /// </returns>
    Creature ActivateCreature(Genotype skeleton)
    {
        GameObject obj = new GameObject("Creature");
        Creature creature = obj.AddComponent<Creature>();

        creature.Skeleton = skeleton;
        creature.Materials = Materials;
        creature.Position = SpawnPosition;

        creature.Init();

        return creature;
    }

    /// <summary>
    ///     Resets life timer for a new creature.
    /// </summary>
    void ResetTimer()
    {
        // https://docs.unity3d.com/ScriptReference/Time-deltaTime.html
        // Subtracting the ammount is more accurate over time
        // than resetting to zero.
        game.creatureLifeTime -= game.lifeTimeLimit;
    }

    void FollowCreature(Creature creature)
    {
        Transform body = creature.transform.GetChild(0);

        MainCamera.GetComponent<CameraController>().Follow(body);
    }

    void UnfollowCreature()
    {
        MainCamera.GetComponent<CameraController>().Unfollow();
        MainCamera.GetComponent<CameraController>().SetPosition(cameraPosition);
    }
}
