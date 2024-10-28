using TMPro;
using UnityEngine;

public abstract class ButtonWithText : Button
{
    [SerializeField] private TextMeshPro text;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
    }
    public virtual void ChangeText(string newText) {
        if(text == null) return;
        text.text = newText;
    }

}