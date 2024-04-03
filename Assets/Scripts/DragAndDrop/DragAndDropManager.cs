using System.Collections.Generic;
using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    private IDictionary<string, IList<DropArea>> areaGroups;

    public void Register(Draggable component, string group)
    {
        component.OnDragStart += HandleDragStart;
        component.OnDragEnd += HandleDragEnd;
    }

    public void Register(DropArea component, string group)
    {
        if (!areaGroups.ContainsKey(group))
        {
            areaGroups.Add(group, new List<DropArea>());
        }

        areaGroups[group].Add(component);

        component.OnDropItem += HandleDrop;
    }

    public void Deregister(Draggable component, string group)
    {
        component.OnDragStart -= HandleDragStart;
        component.OnDragEnd -= HandleDragEnd;
    }

    public void Deregister(DropArea component, string group)
    {
        areaGroups[group].Remove(component);
    }

    private void Awake()
    {
        areaGroups = new Dictionary<string, IList<DropArea>>();
    }

    private void HandleDragStart(Draggable draggable)
    {
        string group = draggable.Group;

        Debug.Log($"DragAndDropManager: started drag; <{group}>");

        if (!areaGroups.ContainsKey(group))
        {
            return;
        }

        IList<DropArea> areas = areaGroups[group];
        foreach (DropArea a in areas)
        {
            a.Toggle(true);
        }
    }

    private void HandleDragEnd(Draggable draggable)
    {
        Debug.Log($"DragAndDropManager: drag end; <{draggable.gameObject.name}>");

        DisableAllAreas();
    }

    private void HandleDrop(GameObject obj)
    {
        DisableAllAreas();
    }

    private void DisableAllAreas()
    {
        foreach (string group in areaGroups.Keys)
        {
            foreach (DropArea a in areaGroups[group])
            {
                a.Toggle(false);
            }
        }
    }
}