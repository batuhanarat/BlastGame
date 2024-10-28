using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseButton : Button
{
    protected override void OnClickAction()
    {
        SceneManager.LoadScene((int)Scenes.MAIN_SCENE);
    }
}