using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour {

    Transform myTr;

    float time;
    public float count;

	// Use this for initialization
	void Start () {
        //자기 자신의 Transform 연결
        myTr = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
		
        if(Time.time > time)
        {
            count += 0.5f;
            //회전
            //myTr.rotation = Quaternion.Euler(0,count,0);
            //이동
            //myTr.localPosition = new Vector3(count, 0, 0);

            
            time = Time.time + 0.1f;
        }


        //myTr.Rotate(Vector3.up * 10.0f);
        myTr.Translate(Vector3.right * 10.0f);
    }
}
