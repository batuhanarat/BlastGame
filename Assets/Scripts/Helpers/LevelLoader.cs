using System;
using UnityEngine;

public class LevelLoader
{
    private static LevelLoader _instance;
    public static LevelLoader Instance {
        get {
            if (_instance == null) {
                return _instance = new LevelLoader();
            }
            return _instance;
        }
    }
    private const string LEVEL_KEY = "level";
    private const int TOTAL_LEVEL_COUNT = 10;
    public LevelData LevelData { get; set; }
    public int CurrentLevel { get; private set; }

    private LevelLoader() {
        SetCurrentLevel();
    }


    private void SetCurrentLevel()
    {
        if(!PlayerPrefs.HasKey(LEVEL_KEY)) {
            CurrentLevel = 1;
            PlayerPrefs.SetInt(LEVEL_KEY, CurrentLevel);
            PlayerPrefs.Save();
        } else {
            CurrentLevel = PlayerPrefs.GetInt(LEVEL_KEY);
        }
    }
    private void PersistLevel()
    {
        PlayerPrefs.SetInt(LEVEL_KEY, CurrentLevel);
        PlayerPrefs.Save();
    }
    public void PrepareNextLevel()
    {
        CurrentLevel = (CurrentLevel + 1) % (TOTAL_LEVEL_COUNT+1);
        PersistLevel();
    }
    public LevelData LoadLevelJson()
    {
        string levelName = "level_"+CurrentLevel.ToString("D2");

        TextAsset levelFile = Resources.Load<TextAsset>($"Levels/{levelName}");

        if (levelFile != null)
        {
            return JsonUtility.FromJson<LevelData>(levelFile.text);
        } else {
            Debug.Log("Level file cant be found");
        }
        return default;
    }

}