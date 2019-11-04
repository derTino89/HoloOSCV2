using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SourceObject : MonoBehaviour
{
    private GameObject label;

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

        label = new GameObject("labelID");
        label.transform.SetParent(this.transform);
        label.AddComponent<TextMesh>().text = id.ToString();
        label.transform.localPosition = new Vector3(-0.2f, 1.2f, 0);
        label.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
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
}
