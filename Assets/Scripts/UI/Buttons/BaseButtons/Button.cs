using UnityEngine;
using DG.Tweening;

public abstract class Button : MonoBehaviour
{
    [SerializeField] private GameObject buttonContent;
    private float _shrinkScale = 0.9f;
    private float _animationDuration = 0.2f;
    private Vector3 _originalScale;
    private bool _isPressed = false;

    private void Awake()
    {
        _originalScale = buttonContent.transform.localScale;
    }

    protected virtual void OnMouseDown()
    {
        _isPressed = true;
        ShrinkButton();
    }

    protected virtual void OnMouseUpAsButton()
    {
        _isPressed = false;
        RestoreButton();
    }

    private void ShrinkButton()
    {
        Vector3 targetScale = _originalScale * _shrinkScale;
        buttonContent.transform.DOScale(targetScale, _animationDuration).SetEase(Ease.OutQuad);
    }

    private void RestoreButton()
    {
        buttonContent.transform.DOScale(_originalScale, _animationDuration).SetEase(Ease.OutQuad).OnComplete(() => {
            gameObject.SetActive(false);
            OnClickAction();
    });
    }

    protected abstract void OnClickAction();

}