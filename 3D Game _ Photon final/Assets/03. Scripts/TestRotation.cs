using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : MonoBehaviour {

    public Transform targetTr;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(targetTr.position, Vector3.up, Time.deltaTime * 55.0f);
        //transform.RotateAroundLocal(Vector3.up, Time.deltaTime * 55.0f);
    }
}
