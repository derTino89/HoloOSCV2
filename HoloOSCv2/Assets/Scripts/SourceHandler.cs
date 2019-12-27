using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceHandler : MonoBehaviour
{
    public GameObject source;
    public GameObject shell;
    ArrayList sources = new ArrayList();
    float sourceRadius;
    float shellRadius;

    OSCOutput output;
    GameObject handler;

    private float numberOfObjects = 3;

    public void Start() {
       sourceRadius = source.GetComponent<SphereCollider>().radius;
       shellRadius = shell.GetComponent<SphereCollider>().radius;
       handler = GameObject.FindGameObjectWithTag("OSCHandler");
       output = handler.GetComponent<OSCOutput>();
    }

    //Insantiates numberOfObjects Sources 
    public void InstantiateObjects() {  
        if (sources.Count < numberOfObjects) {
            Vector3 spawnPos;
            for (int i = 1; i <= numberOfObjects; i++) {
                // scale has to be equal in all directions or algorithm has to be changed accordingly
                float actualRadius = shellRadius * shell.transform.localScale.x;
                spawnPos = new Vector3(0,0,1) * actualRadius;
                GameObject src = Instantiate(source, spawnPos, Quaternion.identity);
                src.name = "Source" + sources.Count;
                src.transform.parent = this.transform;
                src.GetComponent<SolverHandler>().TransformOverride = src.transform.parent;
                src.GetComponent<RadialView>().MinDistance = actualRadius;
                src.GetComponent<RadialView>().MaxDistance = actualRadius;
                src.GetComponent<SourceObject>().SetID(sources.Count);
                sources.Add(src);
            }
        }
        else {
            Debug.Log("numberOfObjects: " + numberOfObjects);
        }
    }

    public void UpdateThroughReaper(OscMessage oscm) {
        string[] message = OSCMessageConverter.splitMessage(oscm);
        GameObject src;

        switch (message[3]) {
            case "azimuth":
                src = sources[Int32.Parse( message[2])] as GameObject;
                src.GetComponent<SourceObject>().setAzimuth(float.Parse(message[0]));
                break;

            case "elevation":
                src = sources[Int32.Parse(message[2])] as GameObject;
                src.GetComponent<SourceObject>().setElevation(float.Parse(message[0]));
                break;

            case "gain":
                src = sources[Int32.Parse(message[2])] as GameObject;
                src.GetComponent<SourceObject>().setGain(float.Parse(message[0]));
                break;

            case "inputSetting":
                numberOfObjects = float.Parse(message[0]);
                break;
            default:
                Debug.Log("Command could not be recognized.");               
                break;
        }
    }
}
