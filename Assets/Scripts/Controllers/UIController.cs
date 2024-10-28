using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIPrefabData uiPrefabReferences;
    public int LEVEL;

    private LevelButton levelButton;
    private TopPanel topPanel;
    private LoseDialog loseDialog;
    private WinDialog winDialog;
    private Action loadGameplay;

    public void Start() {
    }
    public void InitializeMainSceneUI() {
        var go = Instantiate(uiPrefabReferences.levelButtonPrefab);
        if(go != null) {
            levelButton =go.GetComponent<LevelButton>();
        }
    }
    public void InitializeLevelSceneUI() {
        var go = Instantiate(uiPrefabReferences.topPanelPrefab);
        if(go != null) {
            topPanel = go.GetComponent<TopPanel>();
        }
    }
    public void PrepareLevelButton(Action LoadGameplay,int level,Action MoveNextLevel) {
        loadGameplay = LoadGameplay;
        SetLevelButtonText(level,MoveNextLevel);
        levelButton.Prepare(loadGameplay);
    }
    public void ShowLoseDialog() {
        var go = Instantiate(uiPrefabReferences.loseDialogPrefab);

        if(go != null) {
            loseDialog = go.GetComponent<LoseDialog>();
            loseDialog.ShowDialog();
        }
    }
    public void ShowWinDialog(Action OnLevelWin) {
        var go = Instantiate(uiPrefabReferences.winDialogPrefab);

        if(go != null) {
            winDialog = go.GetComponent<WinDialog>();
            winDialog.Prepare(OnLevelWin);
            winDialog.ShowDialog();
        }
    }

    public void UpdateMoveCount(string moveCount) {
        if(topPanel == null) {
                Debug.Log("top panel is null atUpdateMoveCount ");
        } else {
        topPanel.SetMoveCountUI(moveCount);

        }
    }

    public void UpdateGoalCount(GoalType goalType,int goalCount) {
        if(topPanel == null) {
                Debug.Log("top panel is null at  UpdateGoalCount");
        } else {
                topPanel.SetGoalCountUI(goalType,goalCount);

        }
    }

    public void SetGoalUI(Dictionary<GoalType, Goal> goals) {
        if(topPanel == null || goals.Count == 0) {
                Debug.Log("top panel is null at SetGoalUI");
        } else {
                topPanel.InitializeGoalUI(goals);
        }
    }

    private void SetLevelButtonText(int level,Action MoveNextLevel ) {

        LEVEL = level;
        if(levelButton == null) {
            return;
        }
        if(level == 0) {
            levelButton.ChangeText("Finished");
            MoveNextLevel();
        }else {
            levelButton.ChangeText("Level "+ level);
        }
    }


}