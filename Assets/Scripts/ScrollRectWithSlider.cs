using UnityEngine;
using UnityEngine.UI;

public class ScrollRectWithSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private ScrollRect scrollRect;

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        slider.onValueChanged.AddListener(ScrollView);
        scrollRect.onValueChanged.AddListener(UpdateSlider);
    }

    private void Update()
    {
        float contentHeight = scrollRect.content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;
        if (contentHeight > viewportHeight)
        {
            slider.gameObject.SetActive(true);
        }
        else
        {
            slider.gameObject.SetActive(false);
        }
    }

    private void ScrollView(float value)
    {
        scrollRect.verticalNormalizedPosition = 1f - value;
    }

    private void UpdateSlider(Vector2 scrollPos)
    {
        slider.value = 1f - scrollPos.y;
    }
}
