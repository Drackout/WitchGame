using UnityEngine;

public class ElementalStoneDraggable : Draggable
{
    [SerializeField] private GameObject trackerPrefab;

    public override GameObject CreateTracker()
    {
        GameObject tracker = Instantiate(trackerPrefab);

        return tracker;
    }
}
