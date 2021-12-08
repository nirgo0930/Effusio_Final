using System.Xml;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public string dataPath;
    public string curStage = "Town";
    public int maxHp = 10;
    public int hp = 10;
    public double attackPoint = 10;
    public float attackRate = 1.0f;
    public float attackDistance = 1.0f;
    public float attackDelay = 0.4f;
    public float hurtDelay = 0.2f;
    public float recoverTime = 1.5f;
    public float movePower = 5f;
    public float jumpPower = 8f;
    public float jumpLimit = 0.1f;
    public int maxJumpCnt = 2;
    public float jumpDelay = 0.2f;
    public string adventId = "Empty";
    public string effusioId_a = "Empty";
    public string effusioId_b = "Empty";
    public string effusioId_c = "Empty";

    public void savePlayerInfo()
    {
        string path = DataPath.dataPath;

        XmlDocument xmlDoc = new XmlDocument();
        XmlElement player = xmlDoc.CreateElement("Player");
        xmlDoc.AppendChild(player);
        {
            XmlElement info = xmlDoc.CreateElement("Info");
            info.SetAttribute("curStage", curStage);
            info.SetAttribute("maxHp", maxHp.ToString());
            info.SetAttribute("hp", hp.ToString());
            info.SetAttribute("attackPoint", attackPoint.ToString());
            info.SetAttribute("attackRate", attackRate.ToString());
            info.SetAttribute("attackDistance", attackDistance.ToString());
            info.SetAttribute("attackDelay", attackDelay.ToString());
            info.SetAttribute("hurtDelay", hurtDelay.ToString());
            info.SetAttribute("recoverTime", recoverTime.ToString());
            info.SetAttribute("movePower", movePower.ToString());
            info.SetAttribute("jumpPower", jumpPower.ToString());
            info.SetAttribute("jumpLimit", jumpLimit.ToString());
            info.SetAttribute("maxJumpCnt", maxJumpCnt.ToString());
            info.SetAttribute("jumpDelay", jumpDelay.ToString());
            info.SetAttribute("adventId", adventId);
            info.SetAttribute("effusioId_a", effusioId_a);
            info.SetAttribute("effusioId_b", effusioId_b);
            info.SetAttribute("effusioId_c", effusioId_c);
            player.AppendChild(info);
        }
        xmlDoc.Save(path + "PlayerInfo.xml");
        print("save at : " + path + "PlayerInfo.xml");
    }

    public void loadPlayerInfo()
    {
        string path = DataPath.dataPath;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path + "PlayerInfo.xml");
        XmlElement list = xmlDoc["Player"];

        foreach (XmlElement Info in list.ChildNodes)
        {
            curStage = Info.GetAttribute("curStage");
            maxHp = int.Parse(Info.GetAttribute("maxHp"));
            hp = int.Parse(Info.GetAttribute("hp"));
            attackPoint = double.Parse(Info.GetAttribute("attackPoint"));
            attackRate = float.Parse(Info.GetAttribute("attackRate"));
            attackDistance = float.Parse(Info.GetAttribute("attackDistance"));
            attackDelay = float.Parse(Info.GetAttribute("attackDelay"));
            hurtDelay = float.Parse(Info.GetAttribute("hurtDelay"));
            recoverTime = float.Parse(Info.GetAttribute("recoverTime"));
            movePower = float.Parse(Info.GetAttribute("movePower"));
            jumpPower = float.Parse(Info.GetAttribute("jumpPower"));
            jumpLimit = float.Parse(Info.GetAttribute("jumpLimit"));
            maxJumpCnt = int.Parse(Info.GetAttribute("maxJumpCnt"));
            jumpDelay = float.Parse(Info.GetAttribute("jumpDelay"));
            adventId = Info.GetAttribute("adventId");
            effusioId_a = Info.GetAttribute("effusioId_a");
            effusioId_b = Info.GetAttribute("effusioId_b");
            effusioId_c = Info.GetAttribute("effusioId_c");
        }
        print("load at : " + path + "PlayerInfo.xml");
    }
}
