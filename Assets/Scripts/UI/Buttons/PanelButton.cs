using System;
using UnityEngine.SceneManagement;

public class PanelButton : ButtonWithText
{
    private Action onTryAgain;
    public void Prepare(Action OnTryAgain) {
        onTryAgain = OnTryAgain;
    }
    protected override void OnClickAction()
    {
        SceneManager.LoadScene((int)Scenes.LEVEL_SCENE);
    }


}