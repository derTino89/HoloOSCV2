using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCInput : MonoBehaviour
{    
    public SourceHandler sh;

    [SerializeField]
    private OscIn oscIn;

    private ConnectionStatusCheck connectionStatusCheck;

    string address1 = "/MultiEncoder/inputSetting";
    const string mute64 = "/MultiEncoder/mute63";

    // Start is called before the first frame update
    void Start()
    {
        connectionStatusCheck = GameObject.FindWithTag("StatusCheck").GetComponent<ConnectionStatusCheck>();
        bool response = oscIn.Open(8001);
        Debug.Log("Opening OSCin: " + response);
    }

    void OnEnable()
    {
        for (int i = 0; i < sh.getCount(); i++)
        {
            string azimuth = "/MultiEncoder/azimuth" + i.ToString();
            string elevation = "/MultiEncoder/elevation" + i.ToString();
            string gain = "/MultiEncoder/gain" + i.ToString();

            oscIn.Map(azimuth, sh.UpdateThroughReaper);
            oscIn.Map(elevation, sh.UpdateThroughReaper);
            oscIn.Map(gain, sh.UpdateThroughReaper);
        }

        oscIn.MapFloat(mute64, OnConnectionCheck);
    }

    void OnConnectionCheck(float value)
    {
        Debug.Log("Huhu, ich komme an! " + value);

        if (value == 1)
        {
            connectionStatusCheck.sendMute64(0);
        }
        connectionStatusCheck.setConnectionStatus(true);   
    }
}