using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterCounter : MonoBehaviour
{
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject container;

    private IList<EncounterIcon> icons;
    private int total;

    public void SetTotal(int total)
    {
        icons = new List<EncounterIcon>();

        for (int i = 0; i < (total - 1); i++)
        {
            CreateIcon();

            Instantiate(arrowPrefab, container.transform);
        }

        CreateIcon();

        SetCounter(0);
    }

    public void SetCounter(int current)
    {
        for (int i = 0; i < current; i++)
        {
            icons[i].uncleared.SetActive(false);
            icons[i].cleared.SetActive(true);
            icons[i].current.SetActive(false);
        }

        icons[current].uncleared.SetActive(false);
        icons[current].cleared.SetActive(false);
        icons[current].current.SetActive(true);

        for (int i = current + 1; i < total; i++)
        {
            icons[i].uncleared.SetActive(true);
            icons[i].cleared.SetActive(false);
            icons[i].current.SetActive(false);
        }
    }

    private void CreateIcon()
    {
        GameObject obj = Instantiate(iconPrefab, container.transform);

        EncounterIcon icon = new EncounterIcon();
        icon.uncleared = obj.transform.Find("Uncleared").gameObject;
        icon.cleared = obj.transform.Find("Cleared").gameObject;
        icon.current = obj.transform.Find("Current").gameObject;

        icon.uncleared.SetActive(true);
        icon.cleared.SetActive(false);
        icon.current.SetActive(false);

        icons.Add(icon);
    }

    private class EncounterIcon
    {
        public GameObject uncleared;
        public GameObject cleared;
        public GameObject current;
    }
}
