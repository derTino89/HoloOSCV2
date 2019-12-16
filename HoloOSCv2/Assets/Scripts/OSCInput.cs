using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCInput : MonoBehaviour
{
    public SourceHandler sh;

    [SerializeField]
    private OscIn oscIn;

    string address1 = "/MultiEncoder/inputSetting";

    // Start is called before the first frame update
    void Start()
    {
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
    }
}