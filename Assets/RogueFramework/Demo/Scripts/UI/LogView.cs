using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogView : MonoBehaviour
{
    [SerializeField] LogEntryView logEntryPrefab = default;

    [SerializeField] ScrollRect scrollRect = default;
    [SerializeField] RectTransform content = default;

    [SerializeField] int entriesLimit = 20;

    private static LogView instance;
    private static LogView Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<LogView>();

            return instance;
        }
    }

    public static void Log(string message)
    {
        var entry = Instance.AddEntry();
        entry.Text = message;

        Instance.content.anchoredPosition = Vector2.zero;
        Instance.scrollRect.velocity = Vector2.zero;
    }

    private LogEntryView AddEntry()
    {
        LogEntryView entry;

        if (content.childCount < entriesLimit)
        {
            entry = Instantiate(logEntryPrefab, content);
        }
        else
        {
            entry = content.GetChild(0).GetComponent<LogEntryView>();
        }

        entry.transform.SetAsLastSibling();

        return entry;
    }

    private void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        content.anchoredPosition =
            (Vector2)scrollRect.transform.InverseTransformPoint(content.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }

    private IEnumerator Start()
    {
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForSeconds(1);

            Log($"Log message #{i}");
        }
    }
}
