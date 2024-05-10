using System.Collections;
using UnityEngine;
using TMPro;

public class MessageBox : MonoBehaviour
{
    [SerializeField] private GameObject messageBox;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Transform fromTransform;
    [SerializeField] private float enterTime;
    [SerializeField] private AnimationCurve enterCurve;
    [SerializeField] private float timeoutTime;

    private Vector3 toPosition;
    private Coroutine animationRoutine;

    public void Show(string text)
    {
        messageBox.SetActive(true);
        messageText.text = text;
        animationRoutine = StartCoroutine(AnimateShow());
    }

    private IEnumerator AnimateShow()
    {
        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
        }

        float timer = 0f;
        Vector3 fromPosition = fromTransform.position;

        while (timer < enterTime) {
            float pct = timer / enterTime;
            pct = enterCurve.Evaluate(pct);
            transform.position = Vector3.Lerp(fromPosition, toPosition, pct);
            yield return null;
            timer += Time.deltaTime;
        }

        yield return new WaitForSeconds(timeoutTime);
        messageBox.SetActive(false);

        animationRoutine = null;
    }

    private void Start()
    {
        toPosition = transform.position;
        messageBox.SetActive(false);
    }
}
