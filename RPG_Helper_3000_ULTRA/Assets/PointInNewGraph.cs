using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInNewGraph : MonoBehaviour, IDragable, IDoubleClickReceiver {

    private GraphCreator graphCreator;

    private Vector3 positionWhenCreated;

    private void Start()
    {
        GetComponent<DragableItem>().AddListener(this);
        GetComponent<DoubleClickReceiver>().AddListener(this);
        positionWhenCreated = Camera.main.WorldToScreenPoint( gameObject.transform.position ) + (Vector3)CanvasPositionHandler.GetInstance().position;
    }

    public void SetGraphCreator(GraphCreator gc) {
        graphCreator = gc;
    }

    public Vector3 GetPositionWhenCreated()
    {
        return positionWhenCreated;
    }

    public void OnDoubleClick()
    {
        graphCreator.PointWasPressed(this);
    }
    
    public void OnDrag(Vector2 draggedDistanceScreen, Vector2 draggedDistanceWorld)
    {
        transform.position = transform.position + (Vector3)draggedDistanceWorld;
    }
}
