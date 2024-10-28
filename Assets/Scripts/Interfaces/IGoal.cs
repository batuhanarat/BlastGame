using System;
using _Scripts.Pool;
using UnityEngine;

public interface IGoal{
    public GoalType GoalType { get; }
    void Prepare(Board board,ItemPool pool , Action<GoalType> OnDecrementGoal);
    void DecrementGoal(GoalType goalType);

}