using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class OSCMessageConverter : MonoBehaviour
{
    private OscMessage oscm;
    private float value;
    private string address;
    private string commandString;
    private string sourceNumberAsString;

    public void extractFromMessage(OscMessage oscm)
    {
        oscm.TryGet(0, out value);
        address = oscm.address;
        sourceNumberAsString = Regex.Match(address, @"\d+").Value;
        this.oscm = oscm;
    }

    public float GetValue() {
        return value;
    }

    public int GetSourceNumber(){
        return int.Parse(sourceNumberAsString);
    }

    public string GetCommandString() {
        int pFrom = address.IndexOf("/MultiEncoder/") + "/MultiEncoder/".Length;
        int pTo = address.LastIndexOf(sourceNumberAsString);
        commandString = address.Substring(pFrom, pTo - pFrom);
        return commandString;
    }





}
