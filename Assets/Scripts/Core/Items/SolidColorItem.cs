using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Pool;
using DG.Tweening;
using UnityEngine;

public class SolidColorItem : ItemBase, IInteractable
{
    private Sprite _tntHintSprite;
    private Sprite _normalSprite;
    private Sprite _particleSprite;
    private Board _board;
    private ItemFactory _itemFactory;

    public void Prepare(Board board, ItemPool itemPool ,ItemFactory itemFactory)
    {
        _itemFactory =itemFactory;
        _board = board;
        base.Prepare(itemPool);
    }

    public override bool TryExplode(ItemType explodeReason, List<Cell> matchGroup  = null ) {
        matchGroup ??= new List<Cell>();
        if(explodeReason == ItemType.TNT)
        {
            ExplodeFromTnt(explodeReason, matchGroup);
            return true;
        }
        if(matchGroup.Count == 0)
        {
            ExplodeOneSolidCube(explodeReason, matchGroup);
        }
        else if(matchGroup.Count == 1) {
            PlayInvalidMoveAnimation(transform, 0.3f);
            return false;
        }
        if(matchGroup.Count >= 5) {
            StartCoroutine(ExplodeWithTurningToTnt(ItemType.NONE, matchGroup));
            return false;
        }
        foreach(var cell in matchGroup) {
            var item = cell.GetItem();
            if(item != null) {
                item.TryExplode(explodeReason);
            }
        }
        return true;
    }

    private void ExplodeOneSolidCube(ItemType explodeReason, List<Cell> matchGroup)
    {
        TryBlastNeighbors();
        base.TryExplode(explodeReason, matchGroup);
    }

    private void ExplodeFromTnt(ItemType explodeReason, List<Cell> matchGroup)
    {
        base.TryExplode(explodeReason, matchGroup);
    }

    private IEnumerator ExplodeWithTurningToTnt(ItemType explodeReason, List<Cell> matchGroup)
    {
        Sequence animationSequence = DOTween.Sequence();

        for (int i = 1; i < matchGroup.Count; i++)
        {
            var item = matchGroup[i].GetItem();
            if (item != null)
            {
                animationSequence.Join(item.transform.DOMove(transform.position, 0.5f).SetEase(Ease.InOutBack));
                animationSequence.Join(item.GetComponent<SpriteRenderer>().DOFade(0,0.5f).SetEase(Ease.InOutBack));
            }
        }
        animationSequence.Append(transform.DOScale(0.02f, 0.2f).SetEase(Ease.InSine));

        yield return animationSequence.Play().WaitForCompletion();


        foreach(var cell in matchGroup) {
            var item = cell.GetItem();
            if(item != null) {
                item.TryExplode(explodeReason);
            }
        }
            Cell clickedCell = matchGroup[0];
            ChangeToTnt(clickedCell);


        _board.AfterExplode();
    }

    private void ChangeToTnt(Cell clickedCell)
    {
        ItemBase tntItem = _itemFactory.GetItemWithType(ItemType.TNT);
        clickedCell.SetItem(tntItem);
    }
    public override void PlayParticleEffect()
    {
        var cell =  _board.Grid[_position.x,_position.y];
        cell.PlayParticleEffect(_particleSprite);
    }

    public void TryBlastNeighbors()
    {
        List<Cell> neighbors = _board.CheckNeighbours(_position);
        foreach(var neighbor in neighbors) {
            ItemBase item = neighbor.GetItem();
            if(CellVisitTracker.HasVisited(neighbor._pos) || !item.CanExplodeFromCubeBlast()) { continue; }
            CellVisitTracker.AddAsVisited(neighbor._pos,neighbor);
            item.TryExplode(ItemType.SOLID_COLOR);
        }
    }
    public override bool CanExplodeFromCubeBlast()
    {
        return false;
    }
    public override bool CanFalltoEmptyCell()
    {
        return base.CanFalltoEmptyCell();
    }
    public void ChangeSpriteToTnt()
    {
        _spriteRenderer.sprite = _tntHintSprite;
    }
    public void ChangeSpriteToNormal()
    {
        _spriteRenderer.sprite = _normalSprite;
    }
    public virtual void SetSprite(Sprite sprite)
    {
        if (_spriteRenderer != null)
        {
            _normalSprite = sprite;
            _spriteRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogError("SpriteRenderer not initialized in SetSprite!");
        }
    }

    public void SetParticleSprite(Sprite sprite)
    {
        _particleSprite =sprite;
    }
    public void SetTntSprite(Sprite sprite)
    {
        _tntHintSprite = sprite;
    }

}