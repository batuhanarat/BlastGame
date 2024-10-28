using System;
using DG.Tweening;
using UnityEngine;

public class WinDialog : MonoBehaviour
{
    [SerializeField] private GameObject star;
    [SerializeField] private ParticleSystem starParticleSystem;
    [SerializeField] private Sprite starParticle;
    private float animationDuration = 1f;
    private float particleDelay = 2.5f;

    private Action onLevelWin;

    public void Prepare(Action OnLevelWin) {
        onLevelWin = OnLevelWin;
    }
    void Awake()
    {
        gameObject.SetActive(false);
        ParticleSystem.TextureSheetAnimationModule tsam = starParticleSystem.textureSheetAnimation;
        tsam.mode = ParticleSystemAnimationMode.Sprites;
        starParticleSystem.textureSheetAnimation.SetSprite(0,starParticle);
    }
    public void ShowDialog()
    {
        gameObject.SetActive(true);

        star.transform.localScale = star.transform.localScale * 0.5f;

        star.transform.DOScale(star.transform.localScale, animationDuration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            starParticleSystem.Play();

            DOVirtual.DelayedCall(particleDelay, () =>
            {
                onLevelWin();
            });
        });
    }



}