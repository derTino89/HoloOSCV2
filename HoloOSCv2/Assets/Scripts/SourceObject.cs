using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;

public class SourceObject : MonoBehaviour
{
    int id = 0;
    const string azimuth = "/MultiEncoder/azimuth";
    const string elevation = "/MultiEncoder/elevation";
    const string gain = "/MultiEncoder/gain";
    ToolTip toolTip;

    Transform trans;
    GameObject handler;
    OSCOutput output;

    Material matDefault;
    Material matMin;
    
    float scaleMinimum = 0.5f;
    float scaleMaximum = 3.0f;
    float initialScale;

    TransformScaleHandler scaleScript;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>().transform;
        handler = GameObject.FindGameObjectWithTag("OSCHandler");
        output = handler.GetComponent<OSCOutput>();

        matDefault = Resources.Load("yellow") as Material;
        matMin = Resources.Load("blue") as Material;

        initiateScales();
        setDefaultMat();
        AddToolTip();
}

    public float  GetElevation() {
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        float angle = eulerAngles.x;
        angle = angle > 180 ? angle - 360 : angle;
        return angle *= -1;
    }
    public float GetAzimuth() {
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        float angle = eulerAngles.y;
        angle = angle > 180 ? angle - 360 : angle;
        return angle *= -1;
    }
    public float GetGain()
    {
        float currentScale = transform.localScale.x;
        float currentScaleValue = (currentScale - minScale()) / (maxScale() - minScale()); // scaleValue now normed between 0 and 1
        float currentScaleOSC = -60 + currentScaleValue * 70; // scaleValue stretched for encoder to values from -60 to +10
        return currentScaleOSC;
    }

    private void initiateScales() {
        scaleScript = this.GetComponent<TransformScaleHandler>() as TransformScaleHandler;
        scaleScript.ScaleMinimum = scaleMinimum;
        scaleScript.ScaleMaximum = scaleMaximum;
        initialScale = transform.localScale.x;
    }
    private float minScale()
    {
        return scaleMinimum * initialScale;
    }
    private float maxScale()
    {
        return scaleMaximum * initialScale;
    }

    public void sendMessageToOSCHandler() {
        string[] data = new string[2];

        data[0] = azimuth + GetID().ToString();
        data[1] = GetAzimuth().ToString();
        output.SendMessage("SendOSCMessageToClient", data);

        data[0] = elevation + GetID().ToString();
        data[1] = GetElevation().ToString();
        output.SendMessage("SendOSCMessageToClient", data);

        data[0] = gain + GetID().ToString();
        data[1] = GetGain().ToString();
        output.SendMessage("SendOSCMessageToClient", data);
    }
    public int GetID() {
        return id;
    }
    public void SetID(int id) {
        this.id = id;
    }
    
    public void AddToolTip() {
        int channel = id + 1;
        toolTip = this.transform.GetChild(0).gameObject.GetComponent<ToolTip>();
        toolTip.ToolTipText = channel.ToString();
        toolTip.transform.localScale = new Vector3(6.0f, 6.0f, 0.1f);
    }

    public void checkMat() {
        float gain = GetGain();
        if (gain < -59.5)
        {
            setMinMat();
            Debug.Log("Channel " + id+1 + " MUTED");
        }
        else {
            setDefaultMat();
            Debug.Log("Channel " + id + 1 + " UNMUTED");
        }
    }
    public void setDefaultMat() {
        GetComponent<MeshRenderer>().material = matDefault;
    }
    public void setMinMat()
    {
        GetComponent<MeshRenderer>().material = matMin;
    }
}