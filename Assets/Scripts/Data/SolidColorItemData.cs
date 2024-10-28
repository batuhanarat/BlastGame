using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DreamGamesCase/SolidColorItemData")]
public class SolidColorItemData : ScriptableObject
{
    [Header("Normal Sprites")]
    [SerializeField] private Sprite BlueCubeSprite;
    [SerializeField] private Sprite GreenCubeSprite;
    [SerializeField] private Sprite YellowCubeSprite;
    [SerializeField] private Sprite RedCubeSprite;

    [Header("Tnt Hint Sprites")]
    [SerializeField] private Sprite BlueCubeTntHintSprite;
    [SerializeField] private Sprite GreenCubeTntHintSprite;
    [SerializeField] private Sprite YellowCubeTntHintSprite;
    [SerializeField] private Sprite RedCubeTntHintSprite;

    [Header("Particle Sprites")]

    [SerializeField] private Sprite blueCubeParticle;
    [SerializeField] private Sprite greenCubeParticle;
    [SerializeField] private Sprite redCubeParticle;
    [SerializeField] private Sprite yellowCubeParticle;



    public Sprite GetNormalCubeSprite(ItemColor color) {
        switch (color)
        {
            case ItemColor.BLUE:
                return BlueCubeSprite;
            case ItemColor.GREEN:
                return GreenCubeSprite;
            case ItemColor.YELLOW:
                return YellowCubeSprite;
            case ItemColor.RED:
                return RedCubeSprite;
            default:
            return BlueCubeSprite;
        }
    }
    public Sprite GetTntCubeSprite(ItemColor color) {
        switch (color)
        {
            case ItemColor.BLUE:
                return BlueCubeTntHintSprite;
            case ItemColor.GREEN:
                return GreenCubeTntHintSprite;
            case ItemColor.YELLOW:
                return YellowCubeTntHintSprite;
            case ItemColor.RED:
                return RedCubeTntHintSprite;
            default:
            return BlueCubeTntHintSprite;
        }
    }
    public Sprite GetParticleSprite(ItemColor color) {
        switch (color)
        {
            case ItemColor.BLUE:
                return blueCubeParticle;
            case ItemColor.GREEN:
                return greenCubeParticle;
            case ItemColor.YELLOW:
                return yellowCubeParticle;
            case ItemColor.RED:
                return redCubeParticle;
            default:
            return blueCubeParticle;
        }
    }



}