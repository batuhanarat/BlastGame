using TMPro;
using UnityEngine;

public class GoalUIItem : MonoBehaviour
{
    [SerializeField] private TextMeshPro _goalCountTextUI;
    [SerializeField] private GameObject _textObject;
    [SerializeField] private GameObject _tick;
    [SerializeField] private SpriteRenderer _spriteRenderer;


    public void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public TextMeshPro GetGoalCountText() {
        return _goalCountTextUI;
    }
    public void UpdateGoalCountUI(int goalCount) {
        if(goalCount == 0) {
            _tick.SetActive(true);
            _goalCountTextUI.text = "";
        } else {
            _goalCountTextUI.text = goalCount.ToString();
        }
    }
    public void SetSprite(Sprite sprite) {
        _spriteRenderer.sprite = sprite;
    }


}