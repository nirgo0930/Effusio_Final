using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effusio : MonoBehaviour
{

    public string id = "Empty";
    public float coolTime = 0f;

    public void saveEffusioInfo()
    {
        string path = DataPath.dataPath;

        XmlDocument xmlDoc = new XmlDocument();
        XmlElement EffusioListElement = xmlDoc.CreateElement("EffusioList");
        xmlDoc.AppendChild(EffusioListElement);
        {
            XmlElement EffusioElement = xmlDoc.CreateElement("Info");
            EffusioElement.SetAttribute("id", id);
            EffusioElement.SetAttribute("coolTime", coolTime.ToString());

            EffusioListElement.AppendChild(EffusioElement);
        }
        xmlDoc.Save(path + "EffusioInfo.xml");
    }

    public void loadEffusioInfo(string input)
    {
        string path = DataPath.dataPath;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(DataPath.dataPath + "EffusioInfo.xml");
        XmlElement EffusioListElement = xmlDoc["EffusioList"];

        foreach (XmlElement EffusioElement in EffusioListElement.ChildNodes)
        {
            if (input == EffusioElement.GetAttribute("id"))
            {
                id = EffusioElement.GetAttribute("id");
                coolTime = float.Parse(EffusioElement.GetAttribute("coolTime"));
            }
        }
    }
}