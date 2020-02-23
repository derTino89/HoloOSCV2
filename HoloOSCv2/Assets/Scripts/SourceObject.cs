using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;

public class SourceObject : MonoBehaviour
{
    
    const string AZIMUTH_ADDRESS = "/MultiEncoder/azimuth";
    const string ELEVATION_ADDRESS = "/MultiEncoder/elevation";
    const string GAIN_ADDRESS = "/MultiEncoder/gain";
    const float GAIN_NORM_MIN = -60;
    const float GAIN_NORM_MAX = 70;

    private int id = 0;
    private float radShell;

    private float phi = 0;
    private float theta = 0;

    float scaleMinimum;
    float scaleMaximum;

    ToolTip toolTip;
    OSCOutput output;
    Material matDefault;
    Material matMin;  
    TransformScaleHandler transformScaleHandler;
    
    void Start()
    {
        scaleMaximum = transform.localScale.x * 7 / 6; 
        scaleMinimum = scaleMaximum * 2/7;
        transformScaleHandler = this.GetComponent<TransformScaleHandler>() as TransformScaleHandler;
        transformScaleHandler.ScaleMinimum = scaleMinimum;
        transformScaleHandler.ScaleMaximum = scaleMaximum;
        output = GameObject.FindGameObjectWithTag("OSCHandler").GetComponent<OSCOutput>();
        toolTip = this.transform.GetChild(0).gameObject.GetComponent<ToolTip>();
        toolTip.ToolTipText = (id + 1).ToString();
        radShell = Mathf.Sqrt(transform.position.x * transform.position.x  + transform.position.y * transform.position.y + transform.position.z * transform.position.z);
        matDefault = Resources.Load("yellow") as Material;
        matMin = Resources.Load("blue") as Material;
        setDefaultMat();
    }

    private void UpdatePosition()
    {
        transform.position = CoordinateTransformService.TransformSphereToCartesian(radShell, theta * Mathf.Deg2Rad, phi * Mathf.Deg2Rad);
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

    public void checkMat() {
        float gain = GetGain();
        if (gain < GAIN_NORM_MIN + 0.5f) {
            setMinMat();
        }
        else {
            setDefaultMat();
        }
    }

    public void setAzimuth(float phi) {
        this.phi = phi;
        UpdatePosition();
    }
    public void setElevation(float theta) {
        this.theta = theta;
        UpdatePosition();
    } 
    public float GetElevation() {
        return Mathf.Rad2Deg * Mathf.Asin(transform.position.y / radShell);
    }
    public float GetAzimuth() {
        return Mathf.Rad2Deg * -Mathf.Atan2(transform.position.x, transform.position.z);
    }
    public float GetGain() {
        float currentScaleValue = (transform.localScale.x - scaleMinimum) / (scaleMaximum - scaleMinimum); // scaleValue normed between 0 and 1
        return GAIN_NORM_MIN + currentScaleValue * GAIN_NORM_MAX;
    }
    public void setGain(float gain) {
        float newGain = scaleMinimum + ((scaleMaximum - scaleMinimum) * (gain - GAIN_NORM_MIN) / GAIN_NORM_MAX);
        transform.localScale = new Vector3(newGain, newGain, newGain);
        if (newGain <= scaleMinimum) {
            setMinMat();
        }
        else {
            setDefaultMat();
        }
    }
    public int GetID() {
        return id;
    }
    public void SetID(int id) {
        this.id = id;
    }    
    public void setDefaultMat() {
        GetComponent<MeshRenderer>().material = matDefault;
    }
    public void setMinMat()
    {
        GetComponent<MeshRenderer>().material = matMin;
    }
}