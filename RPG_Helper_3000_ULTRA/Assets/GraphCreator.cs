using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GraphCreator : MonoBehaviour {

    private List<GameObject> pointsInGraph = new List<GameObject>();

    public GameObject pointPrefab;
    public bool draw;
    public GraphManager.GraphTypes typeToDraw;
    public KeyCode hotkeyToggleDrawing;
    public KeyCode hotkeyAbortDrawing;
    public float roadWidth;

    void Update () {

        if (Input.GetKeyDown(hotkeyToggleDrawing))
        {
            draw = !draw;
        }

        if (Input.GetKeyDown(hotkeyAbortDrawing))
        {
            RemoveGraphElements();
        }

    }

    public void PointWasPressed(PointInNewGraph pointInNewGraph)
    {
        if (typeToDraw == GraphManager.GraphTypes.road)
        {
            if (pointsInGraph.Count <= 1)
            {
                return; //not enough elements
            }
            if (pointsInGraph[0].GetComponent<PointInNewGraph>() == pointInNewGraph)
            {
                createRoad();
            }
        }
        else
        {
            if (pointsInGraph.Count <= 2)
            {
                return; //not enough elements
            }
            if (pointsInGraph[0].GetComponent<PointInNewGraph>() == pointInNewGraph)
            {
                CloseGraph();
            }
        }
        
    }

    private void createRoad()
    {
        List<Vector3[]> newGraphs = new List<Vector3[]>();
        for (int i = 1, j = 0; i < pointsInGraph.Count; j = i++) {
            Vector3[] l_graph = new Vector3[5];
            Vector3 early = pointsInGraph[j].GetComponent<PointInNewGraph>().GetPositionWhenCreated();
            Vector3 late = pointsInGraph[i].GetComponent<PointInNewGraph>().GetPositionWhenCreated();

            Vector3 vec = late - early;
            Vector3 perp = Vector3.Cross(vec, Vector3.forward);

            l_graph[0] = early + roadWidth * perp;
            l_graph[3] = early - roadWidth * perp;

            vec = early - late;
            perp = Vector3.Cross(vec, Vector3.forward);

            l_graph[2] = late + roadWidth * perp;
            l_graph[1] = late - roadWidth * perp;

            l_graph[l_graph.Length - 1] = l_graph[0];

            GraphManager.GetInstance().AddNewGraph(l_graph, typeToDraw);
            newGraphs.Add(l_graph);
        }

        for (int i = 0, j = 1; i < newGraphs.Count - 1; j = i++) {
            Vector3[] l_graph = new Vector3[5];

            l_graph[0] = newGraphs[i][1];
            l_graph[1] = newGraphs[j][0];
            l_graph[2] = newGraphs[j][3];
            l_graph[3] = newGraphs[i][2];
            l_graph[4] = l_graph[0];

            GraphManager.GetInstance().AddNewGraph(l_graph, typeToDraw);

        }

        RemoveGraphElements();
        DrawOnMesh.GetInstance().SetDirty();
    }

    private void CloseGraph()
    {
        Vector3[] l_graph = new Vector3[pointsInGraph.Count + 1];

        for (int i = 0; i < pointsInGraph.Count; ++i)
        {
            l_graph[i] = pointsInGraph[i].GetComponent<PointInNewGraph>().GetPositionWhenCreated();
        }
        l_graph[l_graph.Length - 1] = pointsInGraph[0].GetComponent<PointInNewGraph>().GetPositionWhenCreated();

        RemoveGraphElements();

        GraphManager.GetInstance().AddNewGraph(l_graph, typeToDraw);

        DrawOnMesh.GetInstance().SetDirty();
        
    }

    private void RemoveGraphElements()
    {
        for (int i = 0; i < pointsInGraph.Count; ++i)
        {
            Destroy(pointsInGraph[i]);
        }
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
        GameObject newPoint = Instantiate(pointPrefab, position, Quaternion.Euler(0, 0, 0));

        newPoint.GetComponent<PointInNewGraph>().SetGraphCreator(this);

        pointsInGraph.Add(newPoint);

        if (pointsInGraph.Count == 1) {
            newPoint.GetComponent<PointInNewGraph>().Highlight();
        }
    }
}
