using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SFB;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class IO
{
    public static void LoadGame()
    {
        string path = GetPath();
        if (path == null) return;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        try
        {
            GameData gameData = formatter.Deserialize(stream) as GameData;

            Core.Game = new Game(gameData);
            Environment environment = Core.GetEnvironment(gameData.environment);

            SceneManager.LoadScene(environment.scene);
        }
        catch (SerializationException)
        {
            // Open an error scene.
            SceneManager.LoadScene("ErrorScene", LoadSceneMode.Additive);
        }
        finally
        {
            stream.Close();
        }
    }

    public static void SaveGame()
    {
        string path = StandaloneFileBrowser.SaveFilePanel(
            "", "", FileName(), "wad");

        if (path == null) return;

        Game game = Core.Game;
        GameData data = new GameData(game);

        SaveAsBinary(path, data);
        // SaveAsText(path, data);
    }

    static string FileName()
    {
        return string.Format(
            "simulation-{0}",
            DateTime.Now.ToString("yyy-MM-dd-HH-mm-ss"));
    }

    static void SaveAsBinary(string path, GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    static void SaveAsText(string path, GameData data)
    {
        string jsonString = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(path, jsonString);
    }

    static string GetPath()
    {
        string[] selected = StandaloneFileBrowser.OpenFilePanel(
               "", "", "wad", false);

        if (selected.Length == 0) return null;

        string path = selected[0];

        if (!File.Exists(path)) return null;

        return path;
    }
}
