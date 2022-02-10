using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObject : MonoBehaviour {

    // 자기 자신의 트렌스폼 연결 레퍼런스
    private Transform myTr;

    //폭발이펙트 파티클 오브젝트 연결 레퍼런스 
    public GameObject exploEffect;

    //히트되는 누적 카운트 변수
    private int hitCount = 0;

    void Start()
    {
        // 레퍼런스 연결 
        myTr = GetComponent<Transform>();

    }

    //CallBack Function
    void OnCollisionEnter(Collision coll)
    {
        //만약 충돌된 오브젝트의 tag 가 Bullet 일 경우
        if (coll.collider.tag == "Bullet")
        {
            //bullet 맞은 횟수가 5회 이상이면 폭발 
            if (++hitCount >= 5)
            {
                // 오브젝트 폭발
                FireObject();
        
            }
        }
    }

    //드럼통이 Ray에 맞았을 때 호출되는 콜백 함수 추가
    void OnCollision(object[] _params)
    {
        // Ray가 Hit된 위치
        Vector3 hItPos = (Vector3)_params[0];
        // 발사 원점
        Vector3 firePos = (Vector3)_params[1];
        //정규화된 입사벡터 만들기 => 정규화된 Ray 맞은 각도는 = (Hit좌표 - 발사 원점).normalized
        Vector3 collVector = (hItPos - firePos).normalized;

        //드럼통의 Ray Hit 위치에 입사벡터의 각도로 힘을 가함
        GetComponent<Rigidbody>().AddForceAtPosition(collVector * 50f, hItPos);

        //Ray 맞은 횟수가 5회 이상이면 폭발 
        if (++hitCount >= 5)
        {
            // 오브젝트 폭발
            FireObject();

        }

    }

    // 오브젝트 폭발시키는 코루틴 함수를 호출
    void FireObject()
    {
        StartCoroutine(this.ExpObject());
    }

    //오브젝트 폭발 함수
    IEnumerator ExpObject()
    {
        //더이상 플레이어가 오브젝트 안쏘게 추가
        GameObject.Find("Player").GetComponent<PlayerCtrl>().barrelFire = false;

        //Debug.Log("Fire~!!");
        //폭발이펙트 파티클 생성
        Instantiate(exploEffect, myTr.position, Quaternion.identity);

        //설정한 폭발 원점을 중심으로 15.0f 반경 내로 들어와 있는 모든 Collider 객체 모두를 추출 함
        Collider[] colls = Physics.OverlapSphere(myTr.position, 15.0f);

        //추출된 Collider 객체들에게 폭발력 전달
        foreach (Collider coll in colls)
        {
            Rigidbody rbody = coll.GetComponent<Rigidbody>();
            //rigidbody 컴포넌트 없으면 무시 
            if (rbody != null)
            {
                //충돌체가 드럼통
                if(rbody.gameObject.tag == "Barrel")
                {
                    //무게 변경 (더 멀리 터지게)
                    rbody.mass = 1.0f;
                    //Rigidbody.AddExplosionForce(폭발력, 원점, 반경, 위로 솟구치는 힘);
                    rbody.AddExplosionForce(1000.0f, myTr.position, 15.0f, 300.0f);

                }
                //충돌체가 Enemy 추가
                else if(rbody.gameObject.tag == "Enemy")
                {
                    //Enemy의 OnCollisionBarrel 함수 호출
                    rbody.gameObject.SendMessage("OnCollisionBarrel", myTr.position
                                                        , SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        //5초 지난 다음 드럼통 제거
        Destroy(gameObject, 5.5f);

        yield return null;
    }

}
