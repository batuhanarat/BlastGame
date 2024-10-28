using UnityEngine;

[CreateAssetMenu(order = 0,menuName = "DreamGamesCase/GoalUIData")]
public class GoalUIData : ScriptableObject
{
    [Header("Sprites")]
    [SerializeField] private Sprite VaseSprite;
    [SerializeField] private Sprite BoxSprite;
    [SerializeField] private Sprite StoneSprite;

    [Header("Goal panel prefabs")]
    [SerializeField] private GameObject _oneGoalPrefab;
    [SerializeField] private GameObject _twoGoalPrefab;
    [SerializeField] private GameObject _threeGoalPrefab;
    [SerializeField] private GameObject _fourGoalPrefab;


    public GameObject GetGoalPanelPrefab(int goalCount){
            switch (goalCount)
            {
                case 1:
                    return _oneGoalPrefab;
                case 2:
                    return _oneGoalPrefab;
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

    public Sprite GetSprite(GoalType goalType) {
        switch (goalType)
        {
            case GoalType.VASE:
                return VaseSprite;
            case GoalType.BOX:
                return BoxSprite;
            case GoalType.STONE:
                return StoneSprite;
            default:
                Debug.LogWarning("İnvalid goaltype");
                return VaseSprite;
        }

    }

}