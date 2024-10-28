using System;
using System.Collections.Generic;


public class GoalManager
{
    private Dictionary<GoalType,Goal> _goals = new Dictionary<GoalType,Goal>();
    private Action _onAllGoalsCompleted;
    private Action<GoalType,int> _onUpdateGoalUIItem;
    private Action<Dictionary<GoalType, Goal>> _onAllGoalsInitialized;


    public GoalManager( Action onAllGoalsCompleted,Action<GoalType,int> onUpdateGoalUIItem, Action<Dictionary<GoalType, Goal>> onAllGoalsInitialized) {
        _onAllGoalsCompleted = onAllGoalsCompleted;
        _onUpdateGoalUIItem = onUpdateGoalUIItem;
        _onAllGoalsInitialized = onAllGoalsInitialized;
    }
    public bool CheckAllGoalsCompleted() {
        if(_goals.Count>0 ) return false;

        if(_goals.Count == 0) {
            _onAllGoalsCompleted();
            return true;
        }
        return false;
    }
    public bool CheckGoalInitialized(GoalType goalType) {
        return _goals.ContainsKey(goalType);
    }

    public void AddGoal(Goal goal) {
        goal.IncrementCount();
        _goals.Add(goal.GetGoalType(),goal);
    }

    public void IncrementGoal(GoalType goalType) {
        Goal goal = _goals[goalType];
        if (goal != null) {
            goal.IncrementCount();
        }
    }

    public void DecrementGoal(GoalType goalType) {
        Goal goal = _goals[goalType];
        if (goal != null) {
            goal.DecrementGoal();
            bool accomplished = goal.CheckGoalCompleted();
            _onUpdateGoalUIItem(goalType,goal.GetGoalCount());
            if(accomplished) {
                RemoveGoal(goalType);
            }
        }
    }

    public void RemoveGoal(GoalType goalType) {
        _goals.Remove(goalType);
        CheckAllGoalsCompleted();
    }


    public void OnAllGoalsInitalized() {
        _onAllGoalsInitialized(_goals);
    }








}