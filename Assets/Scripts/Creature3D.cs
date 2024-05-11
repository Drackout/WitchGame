using UnityEngine;

public class Creature3D : MonoBehaviour
{
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Renderer spriteRenderer;
    [SerializeField] private float minThickness = 10f;
    [SerializeField] private float maxThickness = 20f;
    [SerializeField] private float animationSpeed = 1.0f;

    private Material material;
    private Color normalColor;
    private Animator animator;
    private bool outlineActive;
    private float outlineAnimationTimer;

    public void ToggleTarget(bool state)
    {
        material.SetFloat("_Draw_Outline", state ? 1f : 0);
        outlineAnimationTimer = 0;
        outlineActive = state;
    }

    public void ToggleHover(bool state)
    {
        material.SetColor("_Color", state ? hoverColor : normalColor);
    }

    public void PlayAnimation(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    private void Start()
    {
        material = spriteRenderer.material;
        normalColor = material.GetColor("_Color");

        animator = GetComponent<Animator>();
    }

    private void AnimateOutline()
    {
        if (outlineActive)
        {
            float pct = Mathf.Sin(outlineAnimationTimer * animationSpeed) / 2 + 0.5f;
            float thickness = Mathf.Lerp(minThickness, maxThickness, pct);
            material.SetFloat("_Thickness", thickness);
            outlineAnimationTimer += Time.deltaTime;
        }
    }

    private void Update()
    {
    }
}
