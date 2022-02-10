using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

    ParticleSystem tmpPtclObj;

    // Use this for initialization
    void Start () {
        //파티클 시스템 컴포넌트 가져오기
        tmpPtclObj = (ParticleSystem)gameObject.GetComponent("ParticleSystem");
    }
	
	// Update is called once per frame
	void Update () {
	// 플레이가 끝났을때, Destroy 
	if (tmpPtclObj.isStopped)
		Destroy(gameObject);		
	}
}
