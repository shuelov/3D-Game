using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCtrl : MonoBehaviour {

    //애니메이터 연결 레퍼런스
    public Animator myAnim;

    // Use this for initialization
    void Start () {
        // 애니메이터 연결
        myAnim = GetComponent<Animator>();
    }

    //만약 문 앞에 콜라이더와 충돌이 발생하면...이 함수가 호출
    void OnTriggerEnter(Collider col)
    {
        // 충돌 게임오브젝트의 테그 비교
        if (col.gameObject.tag == "Player")
        {
            // 문 열어주자
            myAnim.SetTrigger("Open");
        }
    }

    //만약 문 콜라이더를 빠져나오면...이 함수가 호출
    void OnTriggerExit(Collider col)
    {
        // 충돌 게임오브젝트의 테그 비교
        if (col.gameObject.tag == "Player")
        {
            // 문 닫자
            myAnim.SetTrigger("Close");
        }
    }
}
