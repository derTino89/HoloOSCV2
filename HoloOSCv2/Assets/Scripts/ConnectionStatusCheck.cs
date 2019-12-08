using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectionStatusCheck : MonoBehaviour
{
    private OSCOutput _oscOut;
    private OSCInput _oscIn;
    private Renderer _meshRenderer;
    private Material[] _materials;
    private Color _mat0Color, _mat1Color;
    private Boolean _isConnected;
    protected int _count;
    protected int _connectionFailCounter;

    // Start is called before the first frame update
    void Start()
    {
        _oscOut = GameObject.FindWithTag("OSCHandler").GetComponent<OSCOutput>();
        _oscIn = GameObject.FindWithTag("OSCHandler").GetComponent<OSCInput>();


        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = true;

        _materials = _meshRenderer.materials;
        _mat0Color = _materials[0].color; //Not Connected
        _mat1Color = _materials[1].color; //Connected
        _materials[1].SetColor("_Color", Color.clear);

        _count = 0;
        _connectionFailCounter = 0;
    }

    void Update()
    {
        //Sends "/Multiencoder/mute63 1" oncec every second

        if(_count==600)
        {
            setConnectionStatus(false);
            sendMute64(1);
            _count = 0;
        }
        _changeMaterialDependingOnStatus();
        _count++;

    }

    public void sendMute64(int mute)
    {
        if (mute > 1)
        {
            mute = 1;
        }

        string[] data = new string[2];

        data[0] = "/MultiEncoder/mute63";
        data[1] = mute.ToString();
        _oscOut.SendMessage("SendOSCMessageToClient", data);
    }
    

    public void setConnectionStatus(Boolean status)
    {
        if (status)
            _connectionFailCounter = 0;
        if (!status)
            _connectionFailCounter++;

        _isConnected = status;
    }

    private void _changeMaterialDependingOnStatus()
    {
        if (_isConnected)
        {
            _materials[0].SetColor("_Color", Color.clear);
            _materials[1].SetColor("_Color", _mat1Color);
        }
        if (!_isConnected && _connectionFailCounter>1)
        {
            _materials[1].SetColor("_Color", Color.clear);
            _materials[0].SetColor("_Color", _mat0Color);
        }
    }
}