using System;
public class MoveManager
{
    public int MoveCount { get; private set; }
    private bool _locked = false;
    private Action<string> updateMoveCountUI;
    private Func<bool> checkGoalsDepleted;
    private Action onLevelFailed;

    public MoveManager(Action<string> UpdateMoveCountUI, Action OnLevelFailed ) {
        updateMoveCountUI = UpdateMoveCountUI;
        onLevelFailed = OnLevelFailed;
    }

    public void MakeMove() {
        if(CanMakeMove()) {
            MoveCount--;
            UpdateMoveCountText();
            if(MoveCount == 0) {
                _locked = true;
                onLevelFailed();
            }
        }
    }
    public void SetMoveCount(int moveCount) {
        MoveCount = moveCount;
        UpdateMoveCountText();
    }
    public bool CanMakeMove() {
        return !_locked && MoveCount >0;
    }
    public void LockMove() {
        _locked = true;
    }
    public void OpenMove() {
        _locked = false;
    }
    public void UpdateMoveCountText() {
        updateMoveCountUI(MoveCount.ToString());
    }
}