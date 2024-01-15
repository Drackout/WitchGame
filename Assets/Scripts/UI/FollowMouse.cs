using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private void Start()
    {
        transform.position = Input.mousePosition;
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
