using UnityEngine;

public class Creature3D : MonoBehaviour
{
    [SerializeField] private Color hoverColor = Color.yellow;

    private Material material;
    private Color normalColor;

    public void ToggleTarget(bool state)
    {
        material.SetFloat("_Draw_Outline", state ? 1f : 0);
    }

    public void ToggleHover(bool state)
    {
        material.SetColor("_Color", state ? hoverColor : normalColor);
    }

    private void Start()
    {
        material = GetComponentInChildren<Renderer>().material;
        normalColor = material.GetColor("_Color");
    }
}
