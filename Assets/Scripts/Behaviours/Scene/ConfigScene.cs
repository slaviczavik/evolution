using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;

public class ConfigScene : MonoBehaviour
{
    public InputField MutationRateInput;
    public InputField LifeTimeLimitInput;
    public InputField PopulationSizeInput;
    public InputField GenerationLimitInput;
    public InputField GravityInput;
    public InputField ElitismInput;
    public Dropdown EnvironmentInput;
    public GameObject StartButton;
    public GameObject ContinueButton;
    Color defaultColor = new Color(1, 1, 1);
    Color errorColor = new Color(0.7f, 0.04f, 0.04f);

    readonly Dictionary<string, int[]> inputRules =
        new Dictionary<string, int[]>()
    {
        { "MutationRate", new int[] { 0, 100 } },
        { "Elitism", new int[] { 0, 50 } },
        { "LifeTimeLimit", new int[] { 10, 60 } },
        { "PopulationSize", new int[] { 10, 500 } },
        { "GenerationLimit", new int[] { 1, 500 } }
    };

    void Start()
    {
        SetEnvironmentOptions();
        SetInputPlaceholders();
        SetInputListeners();
        ApplyGameData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Core.Game != null)
            {
                SceneManager.UnloadSceneAsync("ConfigScene");
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    public void StartGame()
    {
        if (!IsFormValid())
        {
            return;
        }

        float mutationRate = ParseFloat(MutationRateInput) / 100;
        float elitism = ParseFloat(ElitismInput) / 100;
        float gravity = ParseFloat(GravityInput);
        int lifeTimeLimit = ParseInt(LifeTimeLimitInput);
        int populationSize = ParseInt(PopulationSizeInput);
        int generationLimit = ParseInt(GenerationLimitInput);

        Environment environment = GetSelectedEnvironment();

        Core.Game = new Game(mutationRate, lifeTimeLimit, populationSize,
            generationLimit, environment, elitism, gravity);

        SceneManager.LoadScene(environment.scene);
    }

    public void ContinueGame()
    {
        if (!IsFormValid())
        {
            return;
        }

        float mutationRate = ParseFloat(MutationRateInput) / 100;
        float elitism = ParseFloat(ElitismInput) / 100;
        float gravity = ParseFloat(GravityInput);
        int lifeTimeLimit = ParseInt(LifeTimeLimitInput);
        int populationSize = ParseInt(PopulationSizeInput);
        int generationLimit = ParseInt(GenerationLimitInput);

        Environment environment = GetSelectedEnvironment();

        Game game = Core.Game;

        game.mutationRate = mutationRate;
        game.elitism = elitism;
        game.lifeTimeLimit = lifeTimeLimit;
        game.populationSize = populationSize;
        game.generationLimit = generationLimit;
        game.gravity = gravity;

        if (game.environment.name != environment.name)
        {
            game.environment = environment;
            SceneManager.LoadScene(environment.scene);
        }
        else
        {
            SceneManager.UnloadSceneAsync("ConfigScene");
            SceneManager.UnloadSceneAsync("PauseScene");
        }

        game.Resume();
    }

    public void OnEnvironmentChange()
    {
        int index = EnvironmentInput.value;
        string value = EnvironmentInput.options[index].text;
        Environment environment = Core.GetEnvironment(value);

        SetGravity(environment);
    }

    void SetGravity(Environment environment)
    {
        GravityInput.text = environment.gravity.ToString();
    }

    void ApplyGameData()
    {
        Game game = Core.Game;

        if (game != null)
        {
            ShowAppropriateButton(isGameRunning: true);
            SetInputValues(game);
        }
        else
        {
            ShowAppropriateButton(isGameRunning: false);
        }
        
    }

    void ShowAppropriateButton(bool isGameRunning)
    {
        if (isGameRunning)
        {
            ContinueButton.SetActive(true);
            StartButton.SetActive(false);
        }
        else
        {
            ContinueButton.SetActive(false);
            StartButton.SetActive(true);
        }
    }

    bool IsFormValid()
    {
        return ValidateInput(MutationRateInput, inputRules["MutationRate"]) &&
            ValidateInput(ElitismInput, inputRules["Elitism"]) &&
            ValidateInput(LifeTimeLimitInput, inputRules["LifeTimeLimit"]) &&
            ValidateInput(PopulationSizeInput, inputRules["PopulationSize"]) &&
            ValidateInput(GenerationLimitInput, inputRules["GenerationLimit"]);
    }

    void SetInputListeners()
    {
        MutationRateInput.onValueChanged.AddListener(delegate {
            ValidateInput(MutationRateInput, inputRules["MutationRate"]);
        });

        ElitismInput.onValueChanged.AddListener(delegate {
            ValidateInput(ElitismInput, inputRules["Elitism"]);
        });

        LifeTimeLimitInput.onValueChanged.AddListener(delegate {
            ValidateInput(LifeTimeLimitInput, inputRules["LifeTimeLimit"]);
        });

        PopulationSizeInput.onValueChanged.AddListener(delegate {
            ValidateInput(PopulationSizeInput, inputRules["PopulationSize"]);
        });

        GenerationLimitInput.onValueChanged.AddListener(delegate {
            ValidateInput(GenerationLimitInput, inputRules["GenerationLimit"]);
        });
    }

    bool ValidateInput(InputField field, int[] bounds)
    {
        bool isValid = IsInputValid(field, bounds);

        // Set an appropriate color.
        field.textComponent.color = isValid ? defaultColor : errorColor;

        return isValid;
    }

    bool IsInputValid(InputField field, int[] bounds)
    {
        if (field.text.Length == 0)
        {
            return false;
        }

        if (field.contentType == InputField.ContentType.DecimalNumber)
        {
            float value = ParseFloat(field);
            return IsWithinInterval(value, bounds);
        }
        else
        {
            int value = ParseInt(field);
            return IsWithinInterval(value, bounds);
        }
    }

    float ParseFloat(InputField field)
    {
        string input = field.text;
        string str = input.Replace(".", ",");

        CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        culture.NumberFormat.NumberDecimalSeparator = ",";

        float value = float.Parse(str, culture);

        return value;
    }

    int ParseInt(InputField field)
    {
        return int.Parse(field.text);
    }

    bool IsWithinInterval(float value, int[] bounds)
    {
        return value >= bounds[0] && value <= bounds[1];
    }

    bool IsWithinInterval(int value, int[] bounds)
    {
        return value >= bounds[0] && value <= bounds[1];
    }

    Environment GetSelectedEnvironment()
    {
        int value = EnvironmentInput.value;
        string str = EnvironmentInput.options[value].text;

        return Core.GetEnvironment(str);
    }

    void SetInputPlaceholders()
    {
        SetPlaceholder(MutationRateInput,
            string.Join(" – ", inputRules["MutationRate"]));

        SetPlaceholder(ElitismInput,
            string.Join(" – ", inputRules["Elitism"]));

        SetPlaceholder(LifeTimeLimitInput,
            string.Join(" – ", inputRules["LifeTimeLimit"]));

        SetPlaceholder(PopulationSizeInput,
            string.Join(" – ", inputRules["PopulationSize"]));

        SetPlaceholder(GenerationLimitInput,
            string.Join(" – ", inputRules["GenerationLimit"]));
    }

    void SetInputValues(Game game)
    {
        SetInputValue(MutationRateInput, (game.mutationRate * 100).ToString());
        SetInputValue(ElitismInput, (game.elitism * 100).ToString());
        SetInputValue(LifeTimeLimitInput, game.lifeTimeLimit.ToString());
        SetInputValue(PopulationSizeInput, game.populationSize.ToString());
        SetInputValue(GenerationLimitInput, game.generationLimit.ToString());
        SetInputValue(GravityInput, game.gravity.ToString());

        SetDropdownValue(EnvironmentInput, game.environment.name);
    }

    void SetPlaceholder(InputField input, string value)
    {
        input.placeholder.GetComponent<Text>().text = value;
    }

    void SetDropdownValue(Dropdown dropdown, string value)
    {
        dropdown.value = dropdown.options.FindIndex(x => x.text == value);
    }

    void SetInputValue(InputField input, string value)
    {
        input.text = value;
    }

    void SetEnvironmentOptions()
    {
        foreach (Environment env in Core.Environments)
        {
            EnvironmentInput.options.Add(new Dropdown.OptionData(env.name));
        }

        string defaultEnvironment = EnvironmentInput.options[0].text;
        Environment environment = Core.GetEnvironment(defaultEnvironment);

        SetGravity(environment);
    }
}
