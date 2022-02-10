using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor : MonoBehaviour {

    public float deadTime;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, deadTime);
	}
	

}
