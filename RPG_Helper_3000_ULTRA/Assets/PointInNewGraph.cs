using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInNewGraph : MonoBehaviour {

    private GraphCreator graphCreator;

    private Vector3 positionWhenCreated;

    private void Start()
    {
        positionWhenCreated = Camera.main.WorldToScreenPoint( gameObject.transform.position ) + (Vector3)CanvasPositionHandler.GetInstance().position;
    }

    public void SetGraphCreator(GraphCreator gc) {
        graphCreator = gc;
    }

    void OnMouseDown() {
        graphCreator.PointWasPressed(this);
    }

    public Vector3 GetPositionWhenCreated() {
        return positionWhenCreated;
    }

}
