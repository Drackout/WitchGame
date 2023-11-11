using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Slider bar;
    [SerializeField] private TMP_Text currentValueText;
    [SerializeField] private TMP_Text maxValueText;

    public void Set(int current, int max)
    {
        maxValueText.text = max.ToString();
        currentValueText.text = current.ToString();
        bar.value = (float)current / max;
    }
}
