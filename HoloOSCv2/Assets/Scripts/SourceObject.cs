using UnityEngine;
using TMPro;

public class SourceObject : MonoBehaviour
{
    private GameObject label;
    private GameObject toolTip;
    int id = 0;
    const string azimuth = "/MultiEncoder/azimuth";
    const string elevation = "/MultiEncoder/elevation";

    Transform trans;
    GameObject handler;
    OSCOutput output;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>().transform;
        handler = GameObject.FindGameObjectWithTag("OSCHandler");
        output = handler.GetComponent<OSCOutput>();

        AddToolTip();
        AddLabel();
}
    public float  GetElevation() {
        float radius = GetComponent<SphereCollider>().radius;
        float angle = Mathf.Asin(trans.localPosition.y / radius)*Mathf.Rad2Deg % 360;
        return angle > 180 ? angle - 360 : angle;
    }
    public float GetAzimuth() {
        float angle = Mathf.Atan2(trans.localPosition.x, trans.localPosition.z) * Mathf.Rad2Deg % 360;
        return angle > 180 ? angle - 360 : angle;
    }
    public void sendMessageToOSCHandler() {
        string[] data = new string[2];

        data[0] = azimuth + GetID().ToString();
        data[1] = GetAzimuth().ToString();
        output.SendMessage("SendOSCMessageToClient", data);

        data[0] = elevation + GetID().ToString();
        data[1] = GetElevation().ToString();
        output.SendMessage("SendOSCMessageToClient", data);
    }
    public int GetID() {
        return id;
    }
    public void SetID(int id) {
        this.id = id;
    }

    public void AddLabel() {
        label = new GameObject("ChannelNr");
        label.transform.SetParent(this.transform);
        int channel = id + 1;
        label.AddComponent<TextMesh>().text = channel.ToString();
        label.transform.localPosition = new Vector3(-0.2f, 1.2f, 0);
        label.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    public void AddToolTip() {
        int channel = id + 1;
        toolTip = this.transform.GetChild(0).gameObject;
        toolTip.transform.localScale = new Vector3(6.0f, 6.0f, 0.1f);
        GameObject ToolTipLabel = toolTip.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        //Destroy(ToolTipLabel.GetComponent<TextMeshPro>());
        TextMeshPro labelText = ToolTipLabel.GetComponent<TextMeshPro>();
        labelText.text = "uhgzg";
        labelText.ForceMeshUpdate(true);
    }
}
