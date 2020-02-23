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
    
    // Start is called before the first frame update
    void Start()
    {
        connectionStatusCheck = GameObject.FindWithTag("StatusCheck").GetComponent<ConnectionStatusCheck>(); 
        openInput();
    }

    public void openInput()
    {
        Debug.Log("Opening Input, creating Maps");
        oscIn.Open(8001);
        for (int i = 0; i < 63; i++)
        {
            string azimuth = "/MultiEncoder/azimuth" + i.ToString();
            string elevation = "/MultiEncoder/elevation" + i.ToString();
            string gain = "/MultiEncoder/gain" + i.ToString();

            oscIn.Map(azimuth, sh.UpdateThroughReaper);
            oscIn.Map(elevation, sh.UpdateThroughReaper);
            oscIn.Map(gain, sh.UpdateThroughReaper);
        }
        oscIn.MapFloat("/MultiEncoder/mute63", OnConnectionCheck);
        oscIn.Map("/MultiEncoder/inputSetting", sh.UpdateThroughReaper);
    }

    /*void OnEnable()
    {
        // number of Sources should be variable at runtime, there should either be maps for all possible sources (64) 
        // or maps have to be generated dynamically
        for (int i = 0; i < 63; i++)
        {
            string azimuth = "/MultiEncoder/azimuth" + i.ToString();
            string elevation = "/MultiEncoder/elevation" + i.ToString();
            string gain = "/MultiEncoder/gain" + i.ToString();

            oscIn.Map(azimuth, sh.UpdateThroughReaper);
            oscIn.Map(elevation, sh.UpdateThroughReaper);
            oscIn.Map(gain, sh.UpdateThroughReaper);
        }
        oscIn.MapFloat("/MultiEncoder/mute63", OnConnectionCheck);
        oscIn.Map("/MultiEncoder/inputSetting", sh.UpdateThroughReaper);
    }*/

    void OnConnectionCheck(float value)
    {
        if (value == 1)
        {
            connectionStatusCheck.sendMute64(0);
        }
        connectionStatusCheck.setConnectionStatus(true);   
    }
}