using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameCanvas : MonoBehaviour
{
    public Text timeTextUI; // Elapsed time UI text component.
    public Text distanceTextUI; // Distance UI text component.
    public Text creatureTextUI; // Creature ID text component.
    public Text generationTextUI; // Generation ID text component.
    public Text mutationRateTextUI;
    public Text fitnessTextUI;
    public Text elitismTextUI;
    public Text gravityTextUI;
    public Dropdown speedDropdownUI;
    public Text pauseTextUI;
    public GameObject LoadingPanel;
    Game game;

    /// <summary>
    ///     Start is called before the first frame update
    /// </summary>
    void Start()
    {
        game = Core.Game;

        if (game != null)
        {
            game.SetTimeScale(game.animationSpeed);
            StartCoroutine(PauseHandler());
        }
    }

    /// <summary>
    ///     Update is called once per frame.
    /// </summary>
    void Update()
    {
        if (game.isRunning)
        {
            RenderUIOutput();
        }

        if (game.isLoading && !LoadingPanel.activeSelf)
        {
            LoadingPanel.SetActive(true);

            // Disable everything else.
            return;
        }
        else if (LoadingPanel.activeSelf && !game.isLoading)
        {
            LoadingPanel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (game.isPaused)
            {
                game.Resume();
                pauseTextUI.gameObject.SetActive(false);
            }
            else
            {
                game.Pause();
                pauseTextUI.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator PauseHandler()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!game.isPaused)
                {
                    game.Pause();
                }

                pauseTextUI.gameObject.SetActive(false);
                SceneManager.LoadScene("PauseScene", LoadSceneMode.Additive);
            }

            yield return null;
        }
    }

    /// <summary>
    ///     Render the simaltion data into UI components.
    /// </summary>
    void RenderUIOutput()
    {
        string timerFormat = "Time: {0} / {1} sec.";
        string distanceFormat = "Distance: {0} m";
        string creatureFormat = "Creature: {0}/{1}";
        string generationFormat = "Generation: {0}/{1}";
        string mutationRateFormat = "Mutation Rate: {0}%";
        string fitnessFormat = "Fitness: {0}";
        string elitismFormat = "Elitism: {0}%";
        string gravityFormat = "Gravity: {0}";

        float traveledDistance = game.creature.CurrentDistance;
        float currentFitness = game.CalculateFitness();

        timeTextUI.text = String.Format(
            timerFormat,
            Math.Round(game.creatureLifeTime), game.lifeTimeLimit);

        distanceTextUI.text = String.Format(
            distanceFormat,
            Math.Round(Mathf.Abs(traveledDistance), 2));

        int min = Mathf.Min(game.populationSize, game.population.Individuals.Count);

        creatureTextUI.text = String.Format(
            creatureFormat,
            game.counter + 1, min);

        generationTextUI.text = String.Format(
            generationFormat, game.generation, game.generationLimit);

        mutationRateTextUI.text = String.Format(
            mutationRateFormat, game.mutationRate * 100);

        elitismTextUI.text = String.Format(
            elitismFormat, game.elitism * 100);

        fitnessTextUI.text = String.Format(
            fitnessFormat,
            Math.Round(currentFitness, 2));

        gravityTextUI.text = String.Format(
            gravityFormat,
            Math.Round(game.gravity, 2));
    }

    public void OnSpeedChange()
    {
        int index = speedDropdownUI.value;
        string text = speedDropdownUI.options[index].text;
        string str = Regex.Match(text, @"\d+").Value;
        int speed = int.Parse(str);

        // Important for pause!
        game.animationSpeed = speed;

        if (!game.isPaused)
        {
            game.SetTimeScale(speed);
        }
        else
        {
            game.Resume();
            pauseTextUI.gameObject.SetActive(false);
        }
    }
}
