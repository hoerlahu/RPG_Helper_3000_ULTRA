using System.Xml;

public interface ISaveable {

    XmlNode GetSaveData(XmlDocument document);

    void Load(XmlDocument doc);

}
