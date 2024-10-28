
public class Goal {
        private GoalType _goalType;
        private int _count;

        public bool IsGoalSucceed { get; set; }

        public Goal(GoalType goalType) {
            _goalType = goalType;
            _count = 0;
        }
        public bool CheckGoalCompleted() {
            if(_count == 0) {
                return true;
            }
            return false;
        }

        public void IncrementCount() {
            _count++;
        }

        public void  DecrementGoal() {
            _count--;
        }

        public GoalType GetGoalType() {
            return _goalType;
        }
        public int GetGoalCount() {
            return _count;
        }



}

