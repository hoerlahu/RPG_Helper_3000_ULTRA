using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPositionHandler : MonoBehaviour, IDragable {

    public Vector2 position { get; private set; }

    private static CanvasPositionHandler instance;

    private DrawOnMesh mDrawer;

    // Use this for initialization
    void Start () {
        GetComponent<DragableItem>().AddListener(this);
        mDrawer = GetComponent<DrawOnMesh>();
	}

    public CanvasPositionHandler() {
        if (instance != null) throw new System.SystemException();
        instance = this;
    }

    public static CanvasPositionHandler GetInstance() {
        return instance;
    }

    public void OnDrag(Vector2 screen, Vector2 world)
    {
        position -= screen;
        mDrawer.SetDirty();
    }
    
}
