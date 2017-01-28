using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class GraphManager : MonoBehaviour, ISaveable {

    private const string co_graph_parent = "all_graphs";
    private const string co_graph = "graph";
    private const string co_point_in_graph = "point_in_graph";
    private const string co_point_x = "point_x";
    private const string co_point_y = "point_y";
    private const string co_point_z = "point_z";

    public enum GraphTypes {
        wood,
        road
    }

    private Dictionary<GraphTypes, List<Vector3[]>> graphs;

    private static GraphManager instance;

    public static GraphManager GetInstance() {
        return instance;
    }

	// Use this for initialization
	void Start () {
        instance = GetComponent<GraphManager>();

        graphs = new Dictionary<GraphTypes, List<Vector3[]>>();

        foreach (GraphTypes gt in System.Enum.GetValues(typeof(GraphTypes)))
        {
            graphs[gt] = new List<Vector3[]>();
        }

        SaveManager.GetInstance().RegisterSaveableObject(this);

    }

    public void AddNewGraph(Vector3[] newGraph, GraphTypes graphType) {

        graphs[graphType].Add(newGraph);
        for (int i = 0; i < newGraph.Length; ++i)
        {
            Debug.Log(i + " " + newGraph[i]);
        }

    }

    public List<Vector3[]> GetGraphs(GraphTypes graphType) {
        return graphs[graphType];
    }

    public XmlNode GetSaveData(XmlDocument document)
    {
        XmlNode node = document.CreateElement(typeof(GraphManager).ToString());
        XmlNode graphParentNode = document.CreateElement(co_graph_parent);



        foreach (KeyValuePair<GraphTypes, List<Vector3[]>> kvp in graphs) {
            string enumValueName = Enum.GetName(typeof(GraphTypes), (int)kvp.Key);
            XmlNode graphtype = document.CreateElement(enumValueName);
            foreach (Vector3[] graph in kvp.Value) {
                XmlNode graphNode = document.CreateElement(co_graph);
                for (int i = 0; i < graph.Length; ++i) {
                    XmlNode pointInGraph = document.CreateElement(co_point_in_graph);
                    pointInGraph.InnerText = i.ToString();
                    XmlNode x = document.CreateElement(co_point_x);
                    x.InnerText = graph[i].x.ToString();
                    pointInGraph.AppendChild(x);
                    XmlNode y = document.CreateElement(co_point_y);
                    y.InnerText = graph[i].y.ToString();
                    pointInGraph.AppendChild(y);
                    XmlNode z = document.CreateElement(co_point_z);
                    z.InnerText = graph[i].z.ToString();
                    pointInGraph.AppendChild(z);
                    graphNode.AppendChild(pointInGraph);
                }
                graphtype.AppendChild(graphNode);
            }
            graphParentNode.AppendChild(graphtype);
        }

        node.AppendChild(graphParentNode);
        return node;
    }

    public void Load(XmlDocument doc)
    {
        XmlNode node = doc.DocumentElement.SelectSingleNode(typeof(GraphManager).ToString()); 

        

    }
}
