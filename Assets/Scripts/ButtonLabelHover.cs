using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonLabelHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private List<TextMeshProUGUI> labels = new List<TextMeshProUGUI>();
    [SerializeField] private Color hoverColor = Color.white;

    private List<Color> normalColors = new List<Color>();

    private void Awake()
    {
        if (labels.Count == 0)
        {
            var foundLabels = GetComponentsInChildren<TextMeshProUGUI>();
            labels.AddRange(foundLabels);
        }

        normalColors.Clear();
        foreach (var label in labels)
        {
            if (label != null)
            {
                normalColors.Add(label.color);
            }
        }
    }

    private void OnEnable()
    {
        // Reset to unhovered state when panel becomes visible
        ResetToNormalColors();
    }

    /// <summary>
    /// Resets all labels to their normal (unhovered) colors
    /// </summary>
    public void ResetToNormalColors()
    {
        for (int i = 0; i < labels.Count; i++)
        {
            if (labels[i] != null && i < normalColors.Count)
            {
                labels[i].color = normalColors[i];
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var label in labels)
        {
            if (label != null)
            {
                label.color = hoverColor;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < labels.Count; i++)
        {
            if (labels[i] != null && i < normalColors.Count)
            {
                labels[i].color = normalColors[i];
            }
        }
    }
}
