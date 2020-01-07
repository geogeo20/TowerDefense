using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    /// <summary>
    /// Script to help me generate a bezier curve that enemies would follow
    /// </summary>
    [HideInInspector] public Path path;

    public void CreatePath()
    {
        path = new Path(transform.position);
    }
}
