using UnityEngine;
using UnityEngine.UI;

public class UISlots : MonoBehaviour
{
    [SerializeField] private Image[] images;

    [Range(0, 1)]
    [SerializeField]
    private float lockedSlotAlpha = 0.2f;

    public int Slots
    {
        set
        {
            for (int i = 0; i < images.Length; i++)
            {
                if (i < value)
                {
                    Color c = images[i].color;
                    c.a = 1;
                    images[i].color = c;
                }
                else
                {
                    Color c = images[i].color;
                    c.a = lockedSlotAlpha;
                    images[i].color = c;
                }
            }
        }
    }
}
