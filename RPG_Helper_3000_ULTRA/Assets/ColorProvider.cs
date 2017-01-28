using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorProvider : MonoBehaviour {

    public Color m_grass;
    public Color m_wood;

    private CanvasPositionHandler m_CanvasPositionHandler;

    void Start()
    {
        m_CanvasPositionHandler = GetComponent<CanvasPositionHandler>();
    }


    public Color getColorForPixel(int x, int y) {
        Vector2 l_mapLocation = GetMapLocationForPixel(x, y);
        
        return GetColorForMapLocation(l_mapLocation);
    }

    private Color GetColorForMapLocation(Vector2 mapLocation)
    {

        bool pointIsInPolygon = false;

        List<Vector3[]> graphs = GraphManager.GetInstance().GetGraphs(GraphManager.GraphTypes.wood);

        for (int i = 0; i < graphs.Count; ++i) {

            int graphElements = graphs[i].Length;

            float[] xCoords = new float[graphElements];
            float[] yCoords = new float[graphElements];

            for (int j = 0; j < graphElements; ++j)
            {
                xCoords[j] = graphs[i][j].x;
                yCoords[j] = graphs[i][j].y;
            }

            if (IsPointInPolygon(graphElements, xCoords, yCoords, mapLocation.x, mapLocation.y))
            {
                pointIsInPolygon = true;
            }

        }
        if (pointIsInPolygon == true) {
            return m_wood;
        } 
        return m_grass;
    }

    private Vector2 GetMapLocationForPixel(int x, int y)
    {
        return m_CanvasPositionHandler.position + new Vector2(x,y);
    }

    private bool IsPointInPolygon(int nvert, float[] vertx, float[] verty, float testx, float testy)
    {
        bool c = false;
        int i, j;
        for (i = 0, j = nvert - 1; i < nvert; j = i++)
        {
            if (((verty[i] > testy) != (verty[j] > testy)) &&
             (testx < (vertx[j] - vertx[i]) * (testy - verty[i]) / (verty[j] - verty[i]) + vertx[i]))
                c = !c;
        }
        return c;
    }
}
