using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RequestInfoBox : MonoBehaviour
{
    [SerializeField] private GameObject fireIcon;
    [SerializeField] private GameObject grassIcon;
    [SerializeField] private GameObject waterIcon;
    [SerializeField] private Image[] intensityIcons;
    [SerializeField] private TMP_Text locationText;
    [SerializeField] private TMP_Text storyText;
    [SerializeField] private Image thumbnailImage;

    private Animator animator;

    public void Show(string location, int intensity, IEnumerable<Element> types,
        string story, Sprite thumbnail)
    {
        locationText.text = location;

        SetIntensity(intensity);

        SetTypes(types);

        storyText.text = story;

        thumbnailImage.sprite = thumbnail;

        animator.SetBool("Active", true);
    }

    public void Hide()
    {
        animator.SetBool("Active", false);
    }

    private void SetIntensity(int intensity)
    {
        for (int i = 0; i < intensity; i++)
        {
            intensityIcons[i].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        for (int i = intensity; i < 3; i++)
        {
            intensityIcons[i].color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
    }

    private void SetTypes(IEnumerable<Element> types)
    {
        fireIcon.SetActive(types.Contains(Element.Fire));
        grassIcon.SetActive(types.Contains(Element.Grass));
        waterIcon.SetActive(types.Contains(Element.Water));
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
}
