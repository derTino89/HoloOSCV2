using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class OSCMessageConverter : MonoBehaviour
{
    /// <summary>
    /// splits incoming OscMessage into StringArray
    /// returns in order: value, adress, sourcenumber, command
    /// </summary>
    public static string[] splitMessage( OscMessage input) {
        string[] results = new string[5];
        float value;
        input.TryGet(0, out value);
        results[0] = value.ToString(); // value
        results[1] = input.address; // adress
        results[2] = Regex.Match(input.address, @"\d+").Value; // sourceNumber
        var output = Regex.Replace(input.address, @"[\d-]", string.Empty);
        results[3] = Regex.Match(output, @"([^/]+$)").Value; //command
        return results;
    }
}
