using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DragableItem: MonoBehaviour
{

    private Vector2 initialMousePositionScreen;
    private Vector2 initialMousePositionWorld;
    private List<IDragable> listeners = new List<IDragable>();

    public void AddListener(IDragable listener)
    {
        listeners.Add(listener);
    }
    public void RemoveListener(IDragable listener)
    {
        listeners.Remove(listener);
    }

    protected void OnMouseDown()
    {
        UpdateInitialPositions();
    }

    private void UpdateInitialPositions()
    {
        initialMousePositionScreen = Input.mousePosition;
        initialMousePositionWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void UpdateInitialPositions(Vector2 currentWorldPos)
    {
        initialMousePositionScreen = Input.mousePosition;
        initialMousePositionWorld = currentWorldPos;
    }

    protected void OnMouseDrag()
    {

        Vector2 currentWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 draggedDistanceWorld = new Vector2(currentWorldPosition.x - initialMousePositionWorld.x, currentWorldPosition.y - initialMousePositionWorld.y);
        Vector2 draggedDistanceScreen = new Vector2(Input.mousePosition.x - initialMousePositionScreen.x, Input.mousePosition.y - initialMousePositionScreen.y);

        foreach (IDragable listener in listeners) {
            listener.OnDrag(draggedDistanceScreen, draggedDistanceWorld);
        }

        UpdateInitialPositions(currentWorldPosition);
    }
}

