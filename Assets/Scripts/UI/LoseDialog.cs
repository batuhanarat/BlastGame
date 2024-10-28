using System;
using DG.Tweening;
using UnityEngine;

public class LoseDialog : MonoBehaviour
{
    [SerializeField] private CloseButton closeButton;
    [SerializeField] private PanelButton tryAgainButton;

    private Vector3 destination = new Vector3(0,0.4f,0);
    private float duration= 1f;

    void Awake()
    {
        gameObject.SetActive(false);
    }
    public void Prepare(Action OnTryAgain) {
        tryAgainButton.Prepare(OnTryAgain);
    }
    public void ShowDialog() {
        gameObject.SetActive(true);
        gameObject.transform.DOMove(destination,duration).SetEase(Ease.InOutQuad);

    }
}