using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectionStatusCheck : MonoBehaviour
{
    private OscOut _oscOut;
    private Renderer _meshRenderer;
    private Material[] _materials;
    private Color _mat0Color, _mat1Color;

    // Start is called before the first frame update
    void Start()
    {
        _oscOut = GameObject.FindWithTag("OSCHandler").GetComponent<OscOut>();

        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = true;

        _materials = _meshRenderer.materials;
        _mat0Color = _materials[0].color;
        _mat1Color = _materials[1].color;
        _materials[1].SetColor("_Color", Color.clear);

        _meshRenderer.material = _materials[0]; 
    }
    // Update is called once per frame
    void Update()
    {
        if (_oscOut.remoteStatus == OscRemoteStatus.Connected && _meshRenderer.sharedMaterial != _materials[1])
        {
            _materials[0].SetColor("_Color", Color.clear);
            _materials[1].SetColor("_Color", _mat1Color);
            _meshRenderer.material = _materials[1];
        }
        if (_oscOut.remoteStatus == OscRemoteStatus.Disconnected && _meshRenderer.sharedMaterial != _materials[0])
        {
            _materials[1].SetColor("_Color", Color.clear);
            _materials[0].SetColor("_Color", _mat0Color);
            _meshRenderer.material = _materials[0];
        }
    }
}