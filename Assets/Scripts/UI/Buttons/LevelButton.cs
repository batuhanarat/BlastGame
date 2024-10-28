using System;

using DG.Tweening;
using UnityEngine;

public class LevelButton : ButtonWithText
{
    private Action onLevelButtonPressed;

    private float duration = 1f;
    private Vector3 destination = new Vector3(-0.889999986f,-3.19000006f,0);

    void Start() {
        gameObject.transform.DOMove(destination,duration).SetEase(Ease.InOutQuad);
    }

    public void Prepare(Action OnLevelButtonPressed) {
        onLevelButtonPressed = OnLevelButtonPressed ;

    }
    protected override void OnClickAction()
    {
        onLevelButtonPressed();
    }

}