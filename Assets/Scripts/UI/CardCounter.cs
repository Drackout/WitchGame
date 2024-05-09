using UnityEngine;
using UnityEngine.UI;

public class CardCounter : MonoBehaviour
{
    [SerializeField] private Image[] indicators;

    public void Set(int number)
    {
        for (int i = 0; i < number; i++)
        {
            indicators[i].enabled = false;
        }

        for (int i = number; i < indicators.Length; i++)
        {
            indicators[i].enabled = true;
        }
    }
}
