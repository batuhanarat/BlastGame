using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TopPanel : MonoBehaviour
{
    [SerializeField] private TextMeshPro _moveCountTextUI;
    [SerializeField] private GameObject _goalTransformHolder;
    private Vector3 destination = new Vector3(0,3.56999993f,0);
    [SerializeField] private Dictionary<GoalType,GoalUIItem> _goalUIItemDictionary = new Dictionary<GoalType, GoalUIItem>();
    private GoalUIData _goalUIDataHelper;
    [SerializeField] private GameObject _oneGoalPrefab;
    [SerializeField] private GameObject _twoGoalPrefab;
    [SerializeField] private GameObject _threeGoalPrefab;
    [SerializeField] private GameObject _fourGoalPrefab;


    void Awake()
    {
        _goalUIDataHelper = Resources.Load<GoalUIData>("ScriptableObjects/GoalUIData");

            if(_goalUIDataHelper == null) {
            Debug.LogWarning("goal UI data is null");
            }
        gameObject.transform.DOMove(destination,1f ).SetEase(Ease.InOutQuad);
    }

    public GameObject GetGoalPanelPrefab(int goalCount){
            switch (goalCount)
            {
                case 1:
                    return _oneGoalPrefab;
                case 2:
                    return _twoGoalPrefab;
                case 3:
                    return _threeGoalPrefab;
                case 4:
                    return _fourGoalPrefab;
                default:
                    Debug.LogError("Invalid goal count. Must be between 1 and 4.");
                    break;
            }
        return null;
    }

    public TextMeshPro GetMoveCountText() {
        return _moveCountTextUI;
    }

    public void SetMoveCountUI(string moveCount) {
        _moveCountTextUI.text = moveCount;
    }
    public void SetGoalCountUI(GoalType goalType,int goalCount) {
        var goalUIItem =_goalUIItemDictionary[goalType];
        goalUIItem.UpdateGoalCountUI(goalCount);
    }

    public void InitializeGoalUI(Dictionary<GoalType, Goal> goals) {
        var goalKeys = new List<GoalType>(goals.Keys);
        var count = goalKeys.Count;

        _goalUIDataHelper = Resources.Load<GoalUIData>("ScriptableObjects/GoalUIData");

        GameObject panelPrefab = GetGoalPanelPrefab(count);

        GameObject goalUI = Instantiate(panelPrefab,_goalTransformHolder.transform);



        Transform parentTransform = goalUI.transform;

        int i = 0;
        foreach (Transform child in parentTransform)
        {
            if (i >= goalKeys.Count) break;

            var goalType = goalKeys[i];


            var goalUIItemobject = child.GetChild(0);

            GoalUIItem goalUIItem = goalUIItemobject.GetComponent<GoalUIItem>();
            _goalUIItemDictionary.Add(goalType,goalUIItem);

            // Set the sprite for the UI item based on the GoalType
            Sprite sprite = _goalUIDataHelper.GetSprite(goalType);
            goalUIItem.SetSprite(sprite);

            // Get the goal entity and update the UI with the current goal count
            Goal goalEntity = goals[goalType];
            goalUIItem.UpdateGoalCountUI(goalEntity.GetGoalCount());

            i++;
        }

    }



}