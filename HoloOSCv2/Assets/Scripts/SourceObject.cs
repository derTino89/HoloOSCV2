using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;

public class SourceObject : MonoBehaviour
{
    int id = 0;
    const string AZIMUTH_ADDRESS = "/MultiEncoder/azimuth";
    const string ELEVATION_ADDRESS = "/MultiEncoder/elevation";
    const string GAIN_ADDRESS = "/MultiEncoder/gain";
    [SerializeField]
    float radShell;
    ToolTip toolTip;

    private float phi = 0;
    private float theta = 0;

    OSCOutput output;

    Material matDefault;
    Material matMin;
    
    float scaleMinimum = 0.5f;
    float scaleMaximum = 2.5f;
    float initialScale;

    TransformScaleHandler scaleScript;

    // Start is called before the first frame update
    void Start()
    {
        output = GameObject.FindGameObjectWithTag("OSCHandler").GetComponent<OSCOutput>();
        radShell = Mathf.Sqrt(transform.position.x * transform.position.x 
            + transform.position.y * transform.position.y 
            + transform.position.z * transform.position.z);
        matDefault = Resources.Load("yellow") as Material;
        matMin = Resources.Load("blue") as Material;

        initiateScales();
        setDefaultMat();
        AddToolTip();
}

    private void UpdatePosition()
    {
        transform.position = CoordinateTransformService.TransformSphereToCartesian(radShell, theta * Mathf.Deg2Rad, phi * Mathf.Deg2Rad);
    }

    public void setAzimuth(float phi)
    {
        this.phi = phi;
        UpdatePosition();
    }

     public void setElevation(float theta)
    {
        this.theta = theta;
        UpdatePosition();
    }
    /*old setElevation
    public void setElevation(float theta, float additionalAngleE, bool obtuse)
    {
        float radE = theta * Mathf.Deg2Rad;
        float radA = this.GetAzimuth() * Mathf.Deg2Rad;
        float radAddE = additionalAngleE * Mathf.Deg2Rad;
        float radAddA = 180 * Mathf.Deg2Rad;
        if (obtuse != obtuseAngle)
        {
            float z = radShell * (Mathf.Cos(radE - radAddE) * Mathf.Cos(radA + radAddA));
            float x = radShell * (Mathf.Cos(radE - radAddE) * Mathf.Sin(radA + radAddA));
            float y = radShell * Mathf.Sin(radE - radAddE);
            transform.position = new Vector3(x, y, z);
            obtuseAngle = obtuse;
        }
        else
        {
            float z = radShell * (Mathf.Cos(radE - radAddE) * Mathf.Cos(radA));
            float x = radShell * (Mathf.Cos(radE - radAddE) * Mathf.Sin(radA));
            float y = radShell * Mathf.Sin(radE - radAddE);
            transform.position = new Vector3(x, y, z);
        }
    }*/
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
        return Mathf.Rad2Deg * Mathf.Asin(transform.position.y / radShell);
    }
    public float GetAzimuth()
    {
        return Mathf.Rad2Deg * - Mathf.Atan2(transform.position.x, transform.position.z);
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
        //TODO: Refactoring -> OSC-Handler should handle adresses, SourceObject should only tell OSCHandler its values
        string[] data = new string[2];

        data[0] = AZIMUTH_ADDRESS + GetID().ToString();
        data[1] = GetAzimuth().ToString();
        output.SendMessage("SendOSCMessageToClient", data);

        data[0] = ELEVATION_ADDRESS + GetID().ToString();
        data[1] = GetElevation().ToString();
        output.SendMessage("SendOSCMessageToClient", data);

        data[0] = GAIN_ADDRESS + GetID().ToString();
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
        }
        else {
            setDefaultMat();
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