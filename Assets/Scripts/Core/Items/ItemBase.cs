using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Pool;
using DG.Tweening;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    protected SpriteRenderer _spriteRenderer;
    public ItemType Type { get; private set; }
    public ItemColor Color { get; private set; }
    public Vector2Int _position;
    protected ItemPool _objectPool;
    public abstract void PlayParticleEffect();

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public virtual void Prepare(ItemPool objectPool)
    {
        _objectPool = objectPool;
    }
    public virtual bool TryExplode(ItemType explodeReason, List<Cell> matchGroup  = null )
    {
        matchGroup ??= new List<Cell>();
        if(explodeReason != ItemType.NONE){
            PlayParticleEffect();
        }
        _objectPool.ReturnToPool(Type, this);
        _position = new Vector2Int(-1,-1);
        return true;
    }
    public void UpdatePosition(Vector2Int pos)
    {
        _position = pos;
    }
    public void PlaceInCell(Cell cell, Vector2Int position)
    {
        _position = position;
        transform.position = cell.transform.position;
        transform.localScale = Vector3.one;
        transform.localScale = cell.transform.localScale;
        SetAlphaToNormal();
    }
    public void PlayInvalidMoveAnimation(Transform transform, float duration)
    {
        StartCoroutine(InvalidMoveAnimation(transform,duration));
    }
    public void SetItemData(ItemType itemType,ItemColor itemColor)
    {
        Type = itemType;
        Color = itemColor;
    }
    private void SetAlphaToNormal() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (Mathf.Abs(spriteRenderer.color.a - 1f) < 0.01f) return;

        Color newColor = spriteRenderer.color;
        newColor.a = 1f;
        spriteRenderer.color = newColor;
    }
    public bool CanBeMatched(ItemType itemType, ItemColor itemColor = ItemColor.NONE) =>  itemType == Type && itemColor == Color ;
    public virtual bool CanExplodeFromCubeBlast() => true;
    public virtual bool CanFalltoEmptyCell() => true;
    public static IEnumerator InvalidMoveAnimation(Transform transform, float duration)
    {
        transform.DOKill();
        transform.eulerAngles = Vector3.zero;
        transform.DOPunchRotation(new Vector3(0,0,20), duration, 25, 0.5f);
        yield break;
    }




}