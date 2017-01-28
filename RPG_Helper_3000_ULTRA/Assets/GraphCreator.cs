using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GraphCreator : MonoBehaviour {

    private List<GameObject> pointsInGraph = new List<GameObject>();

    public GameObject pointInGraph;
    public bool draw;
    public GraphManager.GraphTypes typeToDraw;
    public KeyCode hotkeyToggleDrawing;

	
	void Update () {

        if (Input.GetKeyDown(hotkeyToggleDrawing))
        {
            draw = !draw;
        }

    }

    public void PointWasPressed(PointInNewGraph pointInNewGraph)
    {
        if (pointsInGraph.Count <= 2) {
            return; //not enough elements
        }
        
        if (pointsInGraph[0].GetComponent<PointInNewGraph>() == pointInNewGraph) {
            CloseGraph();        
        }
    }

    private void CloseGraph()
    {
        Vector3[] l_graph = new Vector3[pointsInGraph.Count + 1];

        for (int i = 0; i < pointsInGraph.Count; ++i)
        {
            l_graph[i] = pointsInGraph[i].GetComponent<PointInNewGraph>().GetPositionWhenCreated();
        }
        l_graph[l_graph.Length - 1] = pointsInGraph[0].GetComponent<PointInNewGraph>().GetPositionWhenCreated();

        for (int i = 0; i < pointsInGraph.Count; ++i)
        {
            Destroy(pointsInGraph[i]);
        }

        GraphManager.GetInstance().AddNewGraph(l_graph, typeToDraw);

        DrawOnMesh.GetInstance().SetDirty();

        pointsInGraph = new List<GameObject>();
    }

    void OnMouseDown()
    {
        if (draw)
        {
            DrawNewPointInGraph();
        }
    }

    private void DrawNewPointInGraph()
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position = ZOrder.SetZOrder(position, ZOrder.zOrder.GUI);
        GameObject newPoint = Instantiate(pointInGraph, position, Quaternion.Euler(0, 0, 0));

        newPoint.GetComponent<PointInNewGraph>().SetGraphCreator(this);

        pointsInGraph.Add(newPoint);
    }
}
