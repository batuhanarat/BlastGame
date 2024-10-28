using System.Collections;
using System.Collections.Generic;
using _Scripts.Pool;
using DG.Tweening;
using UnityEngine;

public class Tnt : ItemBase, IInteractable
{
    private const int NORMAL_RANGE = 5;
    private const int SUPER_RANGE = 7;
    private int _range = NORMAL_RANGE;
    private Board _board;
    [SerializeField] private List<Sprite> tntParticleSprites;

    public void Prepare(Board board,ItemPool objectPool,ItemFactory itemFactory)
    {
        _board = board;
        base.Prepare(objectPool);
    }
    public override bool TryExplode(ItemType explodeReason, List<Cell> matchGroup = null)
    {
        matchGroup ??= new List<Cell>();
        if (matchGroup.Count >= 2)
        {
            _range = SUPER_RANGE;
            StartCoroutine(ExplodeWithTntMergeAnimation(explodeReason, matchGroup));
            return false;
        } else {

            List<Cell> blastables = _board.GetCellsInRange(_range, _position);

            base.TryExplode(explodeReason, matchGroup);

                foreach (var blastable in blastables)
                {
                    var item = blastable.GetItem();
                    if(item != null) {
                        item.TryExplode(explodeReason);
                    }
                }
            return true;
        }
    }

    private IEnumerator ExplodeWithTntMergeAnimation(ItemType explodeReason, List<Cell> matchGroup)
    {
        Sequence animationSequence = DOTween.Sequence();

        for (int i = 1; i < matchGroup.Count; i++)
        {
            var item = matchGroup[i].GetItem();
            if (item != null)
            {
                animationSequence.Join(item.transform.DOMove(transform.position, 1f).SetEase(Ease.InOutBack));
                animationSequence.Join(item.GetComponent<SpriteRenderer>().DOFade(0,1f).SetEase(Ease.InOutBack));
            }
        }

        animationSequence.Append(transform.DOScale(0.7f, 0.2f).SetEase(Ease.InSine));
        yield return animationSequence.Play().WaitForCompletion();
        List<Cell> blastables = _board.GetCellsInRange(_range, _position);
        base.TryExplode(explodeReason, matchGroup);

        foreach (var blastable in blastables)
        {
            var item =  blastable.GetItem();
                if(item != null) {
                    item.TryExplode(explodeReason);
                }
        }
        _board.AfterExplode();
    }
    public override void PlayParticleEffect()
    {
        var cell =  _board.Grid[_position.x,_position.y];
        cell.PlayParticleEffect(tntParticleSprites);
    }
    public override bool CanExplodeFromCubeBlast()
    {
        return false;
    }
    public override bool CanFalltoEmptyCell()
    {
        return base.CanFalltoEmptyCell();
    }


}
