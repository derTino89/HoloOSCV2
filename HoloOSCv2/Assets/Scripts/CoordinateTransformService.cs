using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateTransformService : MonoBehaviour
{
    /// <summary>
    /// transforms sphere coordinates in radians to cartesian
    /// </summary>
    public static Vector3 TransformSphereToCartesian(float r, float theta, float phi)
    {
        //theta = 0° ;
        //phi:
        //0°    -> x = 0, y = 0, z = r
        //90°   -> x = r, y = 0, z= 0
        //-90°  -> x = r, y = 0, z= 0
        //180°  -> x = -r, y = 0, z= -r

        float x = r * Mathf.Cos(theta) * Mathf.Sin(-phi);
        float y = r* Mathf.Sin(theta);
        float z = r * Mathf.Cos(theta)*Mathf.Cos(-phi);
        return new Vector3(x, y, z);
    }

    //TODO: TransformCartesianToSphere
}
