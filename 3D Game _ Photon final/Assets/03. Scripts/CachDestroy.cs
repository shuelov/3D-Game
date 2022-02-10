using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CachDestroy : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Debug.Log(Caching.ClearCache());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
