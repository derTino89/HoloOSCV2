using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCInput : MonoBehaviour
{
    [SerializeField]
    private OscIn oscIn;

    private ConnectionStatusCheck connectionStatusCheck;

    const string mute64 = "/MultiEncoder/mute63";
    // Start is called before the first frame update
    void Start()
    {
        oscIn = GameObject.FindWithTag("OSCHandler").GetComponent<OscIn>();
        connectionStatusCheck = GameObject.FindWithTag("StatusCheck").GetComponent<ConnectionStatusCheck>();
        bool response = oscIn.Open(8001);
        Debug.Log("Opening OSCin: " + response);
    }

    void OnEnable()
    {
        // You can "map" messages to methods in two ways:

        // 1) For messages with a single argument, route the value using the type specific map methods.

        //oscIn.MapFloat(mute64, OnTest);        
        oscIn.MapFloat(mute64, OnConnectionCheck);

        // 2) For messages with multiple arguments, route the message using the Map method.
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