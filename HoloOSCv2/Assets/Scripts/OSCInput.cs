using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCInput : MonoBehaviour
{
    public SourceHandler sh;

    [SerializeField]
    private OscIn oscIn;

    // Start is called before the first frame update
    void Start()
    {
        bool response = oscIn.Open(8001);
        Debug.Log("Opening OSCin: " + response);
    }

    void OnEnable()
    {
        // number of Sources should be variable at runtime, there should either be maps for all possible sources (64) 
        // or maps have to be generated dynamically
        for (int i = 0; i < 5; i++)
        {
            string azimuth = "/MultiEncoder/azimuth" + i.ToString();
            string elevation = "/MultiEncoder/elevation" + i.ToString();
            string gain = "/MultiEncoder/gain" + i.ToString();

            oscIn.Map(azimuth, sh.UpdateThroughReaper);
            oscIn.Map(elevation, sh.UpdateThroughReaper);
            oscIn.Map(gain, sh.UpdateThroughReaper);
        }
        oscIn.Map("/MultiEncoder/inputSetting", sh.UpdateThroughReaper);
    }
}