using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class GraphCreator : MonoBehaviour, IKeyCodeReceiver {

    private List<GameObject> pointsInGraph = new List<GameObject>();

    public GameObject pointPrefab;
    public bool draw;
    public GraphManager.GraphTypes typeToDraw;
    public KeyCode hotkeyToggleDrawing;
    public KeyCode hotkeyAbortDrawing;
    public KeyCode hotkeyCycleStyle;
    public float roadWidth;

    private void Start()
    {
        KeyManager.GetInstance().registerListener(this, hotkeyToggleDrawing, KeyManager.KeyReceivingImportance.low);
    }

    void OnGUI()
    {
        if (draw)
        {
            GUI.Box(new Rect(0, 0, 100, 100), GUIContent.none);
            GUI.Label(new Rect(20, 20, 100, 50), typeToDraw.ToString() );
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
            Vector3 perp = new Vector3(vec.y, -vec.x);
            perp.Normalize();

            l_graph[0] = early + roadWidth * perp;
            l_graph[3] = early - roadWidth * perp;

            vec = early - late;
            //perp = Vector3.Cross(vec, Vector3.forward);
            perp = new Vector3(vec.y, -vec.x);
            perp.Normalize();

            l_graph[2] = late + roadWidth * perp;
            l_graph[1] = late - roadWidth * perp;

            l_graph[l_graph.Length - 1] = l_graph[0];

            GraphManager.GetInstance().AddNewGraph(l_graph, typeToDraw);
            newGraphs.Add(l_graph);
        }

        for (int i = 0, j = 1; i < newGraphs.Count - 1; i = j++) {
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
        KeyManager.GetInstance().unregisterListener(this, hotkeyAbortDrawing, KeyManager.KeyReceivingImportance.standard);
        KeyManager.GetInstance().unregisterListener(this, hotkeyCycleStyle, KeyManager.KeyReceivingImportance.standard);
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
        KeyManager.GetInstance().registerListener(this, hotkeyAbortDrawing, KeyManager.KeyReceivingImportance.standard);
        KeyManager.GetInstance().registerListener(this, hotkeyCycleStyle, KeyManager.KeyReceivingImportance.standard);
    }

    public bool HandlesKeyDown(KeyCode key)
    {
        if (key == hotkeyToggleDrawing)
        {
            draw = !draw;
            return true;
        }

        if (key == hotkeyAbortDrawing)
        {
            RemoveGraphElements();
            return true;
        }

        if (key == hotkeyCycleStyle) {
            selectNextStyle();
            return true;
        }

        return false;
    }

    private void selectNextStyle()
    {
        GraphManager.GraphTypes[] graphTypes = (GraphManager.GraphTypes[])Enum.GetValues(typeof(GraphManager.GraphTypes));
        int i = 0;
        for (; i < graphTypes.Length; ++i) {
            if (graphTypes[i] == typeToDraw) {
                break;
            }
        }
        ++i;
        typeToDraw = graphTypes[i % graphTypes.Length];
    }
}
