using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIController _uiController;
    private LevelLoader  _levelLoader;

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        _levelLoader = LevelLoader.Instance;
        _uiController = Instantiate(_uiController).GetComponent<UIController>();
        _uiController.InitializeMainSceneUI();
        _uiController.PrepareLevelButton(LoadNextLevel,_levelLoader.CurrentLevel,_levelLoader.PrepareNextLevel);
    }

    private void LoadNextLevel() {
        SceneManager.LoadScene((int)Scenes.LEVEL_SCENE);
    }
}