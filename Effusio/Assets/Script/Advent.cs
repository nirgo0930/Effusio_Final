using System.Xml;
using UnityEngine;
public class Advent : MonoBehaviour
{
    public string id = "Empty";
    public int point = 0;
    public string skilltype = "Empty";
    public float skillDelay = 200f;
    public float skillCoolTime = 200f;


    public void saveAdventInfo()
    {
        string path = DataPath.dataPath;
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement player = xmlDoc.CreateElement("Advent");
        xmlDoc.AppendChild(player);
        {
            XmlElement info = xmlDoc.CreateElement("Info");
            info.SetAttribute("id", id.ToString());
            info.SetAttribute("point", point.ToString());
            info.SetAttribute("skilltype", skilltype.ToString());
            info.SetAttribute("skillDelay", skillDelay.ToString());
            info.SetAttribute("skillCoolTime", skillCoolTime.ToString());
            player.AppendChild(info);
        }
        xmlDoc.Save(path + "AdventInfo.xml");
    }

    public void loadAdventInfo(string input)
    {
        string path = DataPath.dataPath;

        XmlDocument xmlDoc = new XmlDocument();
        print("Advent : " + path + "AdventInfo.xml");
        xmlDoc.Load(path + "AdventInfo.xml");
        XmlElement list = xmlDoc["Advent"];

        foreach (XmlElement Info in list.ChildNodes)
        {
            if (input == Info.GetAttribute("id"))
            {
                id = Info.GetAttribute("id");
                point = int.Parse(Info.GetAttribute("point"));
                skilltype = Info.GetAttribute("skilltype");
                skillDelay = float.Parse(Info.GetAttribute("skillDelay"));
                skillCoolTime = float.Parse(Info.GetAttribute("skillCoolTime"));

                return;
            }
        }
    }
}