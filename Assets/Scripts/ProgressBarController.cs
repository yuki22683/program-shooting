using UnityEngine;

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] private int totalLessons = 1;
    [SerializeField] private int completedLessons = 0;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateProgressBar();
    }

    private void OnValidate()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        UpdateProgressBar();
    }

    public void UpdateProgressBar()
    {
        if (rectTransform == null) return;
        if (totalLessons <= 0) return;

        float completionRate = (float)completedLessons / totalLessons;
        completionRate = Mathf.Clamp01(completionRate);

        float width = 500f * completionRate;
        float xPosition = 300f - (250f - (width / 2f));

        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.x = width;
        rectTransform.sizeDelta = sizeDelta;

        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        anchoredPosition.x = xPosition;
        rectTransform.anchoredPosition = anchoredPosition;
    }

    public void SetProgress(int completed, int total)
    {
        completedLessons = completed;
        totalLessons = total;
        UpdateProgressBar();
    }
}
