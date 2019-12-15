using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;

public class SourceObject : MonoBehaviour
{
    int id = 0;
    const string azimuth = "/MultiEncoder/azimuth";
    const string elevation = "/MultiEncoder/elevation";
    const string gain = "/MultiEncoder/gain";
    float radShell;
    ToolTip toolTip;

    Transform trans;
    GameObject handler;
    GameObject Shell;
    OSCOutput output;

    bool obtuseAngle = false;

    Material matDefault;
    Material matMin;
    
    float scaleMinimum = 0.5f;
    float scaleMaximum = 2.5f;
    float initialScale;

    TransformScaleHandler scaleScript;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>().transform;
        handler = GameObject.FindGameObjectWithTag("OSCHandler");
        output = handler.GetComponent<OSCOutput>();
        Shell = GameObject.FindGameObjectWithTag("SourceShell");
        radShell = Shell.GetComponent<Transform>().transform.localScale.x * Shell.GetComponent<SphereCollider>().radius;

        matDefault = Resources.Load("yellow") as Material;
        matMin = Resources.Load("blue") as Material;

        initiateScales();
        setDefaultMat();
        AddToolTip();
}

    public void setAzimuth(float azimuth)
    {
        float radA;
        if (obtuseAngle)
        {
            radA = azimuth * Mathf.Deg2Rad +180;
        }
        else
        {
            radA = azimuth * Mathf.Deg2Rad;
        }
        float radE = this.GetElevation() * Mathf.Deg2Rad;
        float z = radShell * (Mathf.Cos(radE) * Mathf.Cos(radA));
        float x = radShell * (Mathf.Cos(radE) * Mathf.Sin(radA));
        float y = radShell * Mathf.Sin(radE);
        transform.position = new Vector3(x, y, -z);
    }
    public void setElevation(float elevation, float additionalAngleE, bool obtuse)
    {
        float radE = elevation * Mathf.Deg2Rad;
        float radA = this.GetAzimuth() * Mathf.Deg2Rad;
        float radAddE = additionalAngleE * Mathf.Deg2Rad;
        float radAddA = 180 * Mathf.Deg2Rad;
        if (obtuse != obtuseAngle)
        {
            float z = radShell * (Mathf.Cos(radE - radAddE) * Mathf.Cos(radA + radAddA));
            float x = radShell * (Mathf.Cos(radE - radAddE) * Mathf.Sin(radA + radAddA));
            float y = radShell * Mathf.Sin(radE - radAddE);
            transform.position = new Vector3(x, y, -z);
            obtuseAngle = obtuse;
        }
        else
        {
            float z = radShell * (Mathf.Cos(radE - radAddE) * Mathf.Cos(radA));
            float x = radShell * (Mathf.Cos(radE - radAddE) * Mathf.Sin(radA));
            float y = radShell * Mathf.Sin(radE - radAddE);
            transform.position = new Vector3(x, y, -z);
        }
    }
    public void setGain(float gain)
    {
        float newGain = minScale() + ((maxScale() - minScale()) * (gain + 60) / 70);
        transform.localScale = new Vector3(newGain, newGain, newGain);
        if (newGain <= minScale())
        {
            setMinMat();
        }
        else
        {
            setDefaultMat();
        }
    }

    public float GetElevation()
    {
        Vector3 position = transform.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan(position.y / Mathf.Sqrt(position.z * position.z + position.x * position.x));
        return angle;

    }
    public float GetAzimuth()
    {
        Vector3 position = transform.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan(position.x / position.z);
        angle = position.z > 0 ? angle + 180 : angle;
        angle = angle > 180 ? angle - 360 : angle;
        return angle * -1;
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