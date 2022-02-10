using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {

    public float speed = 200f;
    public float range = 400;

    public int power = 10;

    public GameObject ExploPtcl;

    private float dist;

	// Use this for initialization
	void Start () {
        //충돌 안해도 시간체크후 삭제
        //Destroy(gameObject, 5.0f);
    }
	
	// Update is called once per frame
	void Update () {

        // 1 GetComponent<Rigidbody>().AddForce(Vector3.forward * speed); // 월드좌표 z축 방향으로 진행.
        // 2 GetComponent<Rigidbody>().AddForce(transform.forward * speed); // 로컬좌표 z축 방향으로 진행.
        // 3 GetComponent<Rigidbody>().AddRelativeForce(transform.forward * speed); // 이상하게 움직임
        // 4 GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);// 로컬좌표 z축 방향으로 진행.
        
        // 리지드바디의 AddForce 해도 되지만 이렇게 Translate 함수로 이동 시켜줘도 된다.
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        // 거리 기록
        dist += Time.deltaTime * speed;

        // 만약 내가 설정한 위치에 도달하면 Destroy
        if (dist >= range)
        {
            Instantiate(ExploPtcl, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        
    }

    // 충돌시 파티클 생성 후 삭제
    /*    void OnTriggerEnter(Collider coll)
        {
            Instantiate(ExploPtcl, transform.position, transform.rotation);
            Destroy(gameObject); 
        }
        */

    // 충돌시 파티클 생성 후 삭제
    void OnCollisionEnter(Collision coll)
    {
        Instantiate(ExploPtcl, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
