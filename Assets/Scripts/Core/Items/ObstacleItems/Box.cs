using System;
using System.Collections.Generic;
using _Scripts.Pool;
using UnityEngine;
public class Box : ItemBase, IGoal
{
    public GoalType GoalType => GoalType.BOX;
    private Action<GoalType> onDecrementGoal;
    [SerializeField] private List<Sprite> boxParticleSprites;
    private Board _board;


    public void Prepare(Board board ,ItemPool objectPool, Action<GoalType> OnDecrementGoal)
    {
        onDecrementGoal = OnDecrementGoal;
        _board = board;
        base.Prepare(objectPool);
    }
    public override bool TryExplode(ItemType explodeReason,List<Cell> matchGroup = null)
    {
        matchGroup ??= new List<Cell>();

        if(explodeReason == ItemType.SOLID_COLOR || explodeReason== ItemType.TNT) {
            DecrementGoal(GoalType);
            base.TryExplode(explodeReason,matchGroup);
            return true;
        }
        return true;
    }
    public override void PlayParticleEffect()
    {
        var cell =  _board.Grid[_position.x,_position.y];
        cell.PlayParticleEffect(boxParticleSprites);
    }
    public override bool CanFalltoEmptyCell()
    {
        return false;
    }
    public override bool CanExplodeFromCubeBlast()
    {
        return true;
    }
    public void DecrementGoal(GoalType goalType)
    {
        onDecrementGoal(goalType);
    }


}