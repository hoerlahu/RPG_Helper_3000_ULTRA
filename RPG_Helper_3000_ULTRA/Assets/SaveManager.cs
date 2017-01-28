using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml;

public class SaveManager : MonoBehaviour {

    private List<ISaveable> saveableObjects;

    private static SaveManager instance;

    public string filepath;

    public static SaveManager GetInstance() {
        return instance;
    }

    void Start()
    {
        instance = this;
        saveableObjects = new List<ISaveable>();
    }

    public void RegisterSaveableObject(ISaveable saveable) {
        saveableObjects.Add(saveable);
    }

    public void Load()
    {
        XmlDocument xmlDoc = new XmlDocument();

        using (System.IO.StreamReader stream = new System.IO.StreamReader(filepath))
        {
            xmlDoc.Load(stream);
        }

        foreach (ISaveable saveable in saveableObjects)
        {
            saveable.Load(xmlDoc);
        }
    }


    public void Save() {

        XmlDocument xmlDoc = new XmlDocument();
        XmlNode root = xmlDoc.CreateElement("BASE");
        foreach (ISaveable saveable in saveableObjects) {
            root.AppendChild( saveable.GetSaveData(xmlDoc) );
        }

        xmlDoc.AppendChild(root);

        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filepath))
        {
            xmlDoc.Save(sw);
        }

    }
}
