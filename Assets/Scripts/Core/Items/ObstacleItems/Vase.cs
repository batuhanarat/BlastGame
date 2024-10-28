using System;
using System.Collections.Generic;
using _Scripts.Pool;
using UnityEngine;
public class Vase : ItemBase, IGoal
{
    [SerializeField] private Sprite _damagedSprite;
    [SerializeField] private List<Sprite> vaseParticleSprites;
    private Action<GoalType> onDecrementGoal;
    private Board _board;
    private const int NORMAL_STATE_LIFE = 2;
    private const int DAMAGED_STATE_LIFE = 1;
    private int _currentLife = NORMAL_STATE_LIFE;
    public GoalType GoalType => GoalType.VASE;

    public void Prepare(Board board,ItemPool objectPool, Action<GoalType> OnDecrementGoal)
    {
        _board = board;
        onDecrementGoal = OnDecrementGoal;
        base.Prepare(objectPool);
    }

    public override bool TryExplode(ItemType explodeReason, List<Cell> matchGroup = null)
    {
        matchGroup ??= new List<Cell>();
        if(ShouldExplode()) {
            DecrementGoal(GoalType);
            base.TryExplode(explodeReason,matchGroup);
            return true;
        }

        return false;
    }

    private bool ShouldExplode() {
        if(_currentLife == NORMAL_STATE_LIFE) {
            _currentLife--;
            _spriteRenderer.sprite = _damagedSprite;
            return false;
        }
        else if(_currentLife == DAMAGED_STATE_LIFE){
            _currentLife--;
            return true;
        }
        return false;
    }
    public override void PlayParticleEffect()
    {
        var cell =  _board.Grid[_position.x,_position.y];
        cell.PlayParticleEffect(vaseParticleSprites);
    }
    public override bool CanExplodeFromCubeBlast()
    {
        return true;
    }
    public override bool CanFalltoEmptyCell()
    {
        return base.CanFalltoEmptyCell();
    }

    public void DecrementGoal(GoalType goalType)
    {
        onDecrementGoal(goalType);
    }


}