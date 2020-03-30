using UnityEngine;
using UnityEngine.UI;

public class LogEntryView : MonoBehaviour
{
    [SerializeField] Text messageText = default;

    public string Text
    {
        get => messageText.text;
        set => messageText.text = value;
    }
}
