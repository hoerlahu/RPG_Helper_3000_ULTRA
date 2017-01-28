using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPositionHandler : MonoBehaviour {

    public Vector2 position { get; private set; }

    private Vector2 initialMousePosition;

    private static CanvasPositionHandler instance;

    private DrawOnMesh mDrawer;

    // Use this for initialization
    void Start () {
        mDrawer = GetComponent<DrawOnMesh>();
	}

    private void Awake()
    {
        instance = this;
    }

    public static CanvasPositionHandler GetInstance() {
        return instance;
    }

    private void OnMouseDown()
    {
        initialMousePosition = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        position = position - new Vector2(Input.mousePosition.x - initialMousePosition.x, Input.mousePosition.y - initialMousePosition.y);
        initialMousePosition = Input.mousePosition;

        mDrawer.SetDirty();

        Debug.Log(position);

    }

    private void OnMouseUp()
    {
        initialMousePosition = default(Vector2);
    }
}
