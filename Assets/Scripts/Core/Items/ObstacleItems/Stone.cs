using System;
using System.Collections.Generic;
using _Scripts.Pool;
using UnityEngine;

public class Stone : ItemBase, IGoal
{
    public GoalType GoalType => GoalType.STONE;

    [SerializeField] private List<Sprite> stoneParticleSprites;

    private Action<GoalType> onDecrementGoal;
        private Board _board;


    public void Prepare(Board board, ItemPool objectPool, Action<GoalType> OnDecrementGoal)
    {
        _board = board;
        onDecrementGoal = OnDecrementGoal;
        base.Prepare(objectPool);
    }
    public override bool TryExplode(ItemType explodeReason, List<Cell> matchGroup = null)
    {
        matchGroup ??= new List<Cell>();

        if(explodeReason == ItemType.TNT) {
            DecrementGoal(GoalType);
            base.TryExplode(explodeReason,matchGroup);
            return true;
        }
        return false;
    }

    public override void PlayParticleEffect()
    {
        var cell =  _board.Grid[_position.x,_position.y];
        cell.PlayParticleEffect(stoneParticleSprites);
    }

    public override bool CanFalltoEmptyCell()
    {
        return false;
    }

    public override bool CanExplodeFromCubeBlast()
    {
        return false;
    }

    public bool IsGoalAchieved()
    {
        throw new System.NotImplementedException();
    }

    public void DecrementGoal(GoalType goalType)
    {
        onDecrementGoal(goalType);
    }


}