using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour {

    //몬스터 라이프
    private int life = 100;
    //자신의 트렌스폼
    private Transform myTr;
    //혈흔 효과 프리팹
    public GameObject enemyBloodEffect;
    //혈흔데칼 효과 프리팹
    public Transform enemyBloodDecal;

    //EnemyCtrl 연결 레퍼런스
    public EnemyCtrl enemy;
    //생명력 바 연결 레퍼런스 (특정 컴포넌트 아니면 Renderer 로 연결가능)
    public MeshRenderer lifeBar;



    void Awake()
    {
        //레퍼런스 할당
        myTr = GetComponent<Transform>();
    }


    //포탑에 의해서 Enemy가 총알과 맞았을 때 호출되는 콜백 함수
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Bullet")
        {
            //총알이 맞은 위치를 위한 ContactPoint 구조체 선언 및 할당
            ContactPoint contact = coll.contacts[0];

            //혈흔 효과 함수를 호출
            CreateBlood(contact.point);

            //맞은 총알의 파워를 가져와 Enemy의 life를 감소
            life -= coll.gameObject.GetComponent<BulletCtrl>().power;
            //로컬적인 개념으로 머트리얼 셋팅
            lifeBar.material.SetFloat("_Progress", life/100.0f);


            // 생명력이 바닥이면 죽이자
            if (life <= 0)
            {
                enemy.EnemyDie();
            }

            //몬스터 타격 루틴을 위한 호출
            enemy.HitEnemy();
        }
    }

    //Enemy가 Ray에 맞았을 때 호출되는 콜백 함수
    void OnCollision(object[] _params)
    {
        Debug.Log(string.Format("info {0} : {1}", _params[0], _params[1]));

        //혈흔 효과 함수를 호출
        CreateBlood((Vector3)_params[0]);

        //맞은 총알의 파워를 가져와 Enemy의 life를 감소
        life -= (int)_params[1];
        //로컬적인 개념으로 머트리얼 셋팅
        lifeBar.material.SetFloat("_Progress", life / 100.0f);

        // 생명력이 바닥이면 죽이자
        if (life <= 0)
        {
            enemy.EnemyDie();
        }

    }

    // 드럼통 폭발 몬스터 사망 처리
    public void OnCollisionBarrel(Vector3 firePos)
    {
        //혈흔 효과 함수를 호출
        CreateBlood(firePos);

        //Enemy의 life를 0 으로
        life = 0;
        //로컬적인 개념으로 머트리얼 셋팅
        lifeBar.material.SetFloat("_Progress", life / 100.0f);

        enemy.EnemyBarrelDie(firePos);
    }

    // 블러드 연출을 시작해주는 코루틴함수 호출
    void CreateBlood(Vector3 pos)
    {
        //혈흔 효과를 위한 코루틴 함수 호출
        StartCoroutine(this.CreateBloodEffects(pos));
    }

    IEnumerator CreateBloodEffects(Vector3 pos)
    {
        //blood effect 생성
        GameObject enemyblood1 = Instantiate(enemyBloodEffect, pos, Quaternion.identity) as GameObject;
        //만약 블러드 이펙트에 오브젝트 삭제 컴포넌트가 없을시...
        //Destroy(enemyblood1, 1.5f);

        //만약 혈흔 프리팹에 차일드 오브젝트를(혈흔) up 방향으로 미리 올려놨다면...
        //혈흔데칼의 생성되는 위치는 바닥에서 조금 올린 위치로 만들어야 바닥에 묻히지 않는다
        //Vector3 decalPos = myTr.position + (Vector3.up * 0.1f);

        //혈흔데칼의 회전을 Y 축으로 랜덤으로 설정
        Quaternion decalRot = Quaternion.Euler(0, Random.Range(0, 360), 0);
        //혈흔데칼의 크기를 랜덤으로 설정
        float scale = Random.Range(1.0f, 2.5f);

        //혈흔데칼 프리팹 생성
        //Transform enemyblood2 = Instantiate(enemyBloodDecal, decalPos, decalRot) as Transform;
        //만약 혈흔 프리팹에 차일드 오브젝트를(혈흔) up 방향으로 미리 올려놨다면...
        Transform enemyblood2 = Instantiate(enemyBloodDecal, myTr.position, decalRot) as Transform;

        //혈흔데칼의 크기를 랜덤으로 설정
        enemyblood2.localScale = Vector3.one * scale;

        yield return null;
    }

}
