using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class GraphManager : MonoBehaviour, ISaveable {

    private const string co_graph_parent = "all_graphs";
    private const string co_graph = "graph";
    private const string co_index_in_graph = "index_in_graph";
    private const string co_point_x = "point_x";
    private const string co_point_y = "point_y";
    private const string co_point_z = "point_z";
    private const string co_graph_length = "graph_length";
    private const string co_graph_element = "graph_element";

    public enum GraphTypes {
        wood,
        road
    }

    private Dictionary<GraphTypes, List<Vector3[]>> graphs;

    private static GraphManager instance;

    public static GraphManager GetInstance() {
        return instance;
    }

    public GraphManager() {
        if (instance != null) {
            throw new System.SystemException();
        }
        instance = this;
    }

    // Use this for initialization
    void Start () {

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
                XmlAttribute graphLength = document.CreateAttribute(co_graph_length);
                graphLength.Value = graph.Length.ToString();
                graphNode.Attributes.Append(graphLength);
                for (int i = 0; i < graph.Length; ++i) {
                    XmlNode graphElement = document.CreateElement(co_graph_element);
                    XmlNode pointInGraph = document.CreateElement(co_index_in_graph);
                    pointInGraph.InnerText = i.ToString();
                    graphElement.AppendChild(pointInGraph);
                    XmlNode x = document.CreateElement(co_point_x);
                    x.InnerText = graph[i].x.ToString();
                    graphElement.AppendChild(x);
                    XmlNode y = document.CreateElement(co_point_y);
                    y.InnerText = graph[i].y.ToString();
                    graphElement.AppendChild(y);
                    XmlNode z = document.CreateElement(co_point_z);
                    z.InnerText = graph[i].z.ToString();
                    graphElement.AppendChild(z);
                    graphNode.AppendChild(graphElement);
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

        foreach (var kvp in graphs) {
            kvp.Value.Clear();
        }

        XmlNode node = doc.DocumentElement.SelectSingleNode(typeof(GraphManager).ToString());

        XmlNode parentGraph = node.SelectSingleNode(co_graph_parent);
        foreach (GraphTypes gt in System.Enum.GetValues(typeof(GraphTypes)))
        {
            XmlNode graphType = parentGraph.SelectSingleNode(Enum.Parse(typeof(GraphTypes), gt.ToString()).ToString());
            foreach (XmlNode graph in graphType.ChildNodes)
            {
                Vector3[] graphElements = new Vector3[int.Parse(graph.Attributes[co_graph_length].Value)];
                foreach (XmlNode pointInGraph in graph.ChildNodes)
                {
                    Vector3 vec = new Vector3();
                    vec.x = float.Parse(pointInGraph.SelectSingleNode(co_point_x).InnerText);
                    vec.y = float.Parse(pointInGraph.SelectSingleNode(co_point_y).InnerText);
                    vec.z = float.Parse(pointInGraph.SelectSingleNode(co_point_z).InnerText);
                    graphElements[int.Parse(pointInGraph.SelectSingleNode(co_index_in_graph).InnerText)] = vec;
                }
                graphs[gt].Add(graphElements);
            }
        }
    }
}
