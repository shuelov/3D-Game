using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 컴포넌트는 AudioSource 컴포넌트가 필요함
[RequireComponent(typeof(AudioSource))]

public class BaseCtrl : MonoBehaviour {

    // public 멤버 인스펙터에 노출을 막는 어트리뷰트
    // 인스펙터에 노출은 막고 외부 노출은 원하는 경우 사용
    [HideInInspector]
    //죽었는지 상태변수 
    public bool isDie;

    //적과의 거리를 위한 변수
    public float dist1;
    public float dist2;

    //Enemy를 찾기 위한 배열 
    private GameObject[] Enemys;
    private Transform EnemyTarget;

    //자신의 Transform 참조 변수  
    private Transform myTr;

    // 회전의 중심축
    public Transform targetTr;

    // 터렛 발사 변수
    private bool shot;
    // 적을 봐라보는 회전 속도
    private float enemyLookTime;
    //적을 봐라보는 회전각
    private Quaternion enemyLookRotation;

    //Ch7
    //테스트용 변수
    public Texture test;

    //총탄 프리팹을 위한 레퍼런스
    public GameObject bullet;
    //총탄의 발사 시작 좌표 연결 변수 
    public Transform firePos;
    //총알 발사 주기
    private float bulletSpeed;
    //AudioSource 컴포넌트 저장할 레퍼런스 
    private AudioSource source = null;
    //MuzzleFlash GameObject를 연결 할 레퍼런스 
    public GameObject muzzleFlash;
    //총탄의 발사 사운드 
    public AudioClip fireSfx;

    //Ray 정보 저장 구조체 
    Ray ray;
    // Ray에 맞은 오브젝트 정보를 저장 할 구조체
    RaycastHit hitInfo;
    //Ray 센서를 위한 변수
    bool check;

    //레이저 발사를 위한 컴포넌트
    public LineRenderer rayLine;

    //레이저 도트 타겟을 위한 변수
    public Transform rayDot;


    void Awake()
    {
        //Ch10/////////////////////////////////
        /* Resources.Load(string path), Resources.Load(string path, Type systemTypeInstance) 함수는 Assets 루트디렉터리 안에 예약된 폴더인 Resources에서 리소스를 동적 사용 가능하다
         * 함수는 위와 같이 오버로딩 되어 있으며, 제너릭 타입의 문법도 사용 가능
         * path 인자는 Resources에 하위 폴더가 있을 경우 "폴더 경로/리소스이름"을 전달 한다.
         * EX) (GameObject) Resources.Load( "Base/Bullet" , typeof(GameObject));
         */

        //Bullet prefab을 타입이 Gameobject인지 확인 후 Resources 폴더에서 불러와 변수에 할당 
        bullet = (GameObject)Resources.Load("Base/Bullet", typeof(GameObject));
        test  = (Texture)Resources.Load("Base/Bullet", typeof(Texture));

        //위와 같은 결과
        //bullet = Resources.Load("Base/Bullet", typeof(GameObject)) as GameObject;

        //위와 같은 결과
        //bullet = (GameObject)Resources.Load( "Base/Bullet" );//검색 조건이 없으니 불안
        //test = (Texture)Resources.Load( "Base/Bullet" ); //검색 조건이 없으니 Error

        //제너릭 타입 문법 -> 같은 결과( 형 변환이 따로 필요 없다 : 속도 Up, 검색 조건도 OK 따라서 이걸 사용!!!! )
        //bullet = Resources.Load<GameObject>( "Base/Bullet" );
        //test = Resources.Load<Texture>("Base/Bullet");

        //케논 발사 사운드 파일을 Resources 폴더에서 불러와 레퍼런스에 할당 ()
        fireSfx = Resources.Load<AudioClip>("Base/bazooka");

        //테스트 후 해당 변수들을 private로 바꾸자...

        ////////////////////////////////////////////////////////////////////////////////////

        //자기 자신의 Transform 연결
        myTr = GetComponent<Transform>();

        //AudioSource 컴포넌트를 해당 변수에 할당
        source = GetComponent<AudioSource>();
        //처음에 MuzzleFlash 를 비활성화  
        muzzleFlash.SetActive(false);
    }


	// Use this for initialization
	public void StartBase () {

        // 일정 간격으로 주변의 가장 가까운 Enemy를 찾는 코루틴 
        StartCoroutine(this.TargtSetting());

        // 가장 가까운 적을 찾아 발사...
        StartCoroutine(this.ShotSetting());

    }
	
	// Update is called once per frame
	void Update () {


        //ray 정보 업데이트
        ray.origin = firePos.position;
        // firePos local space(앞 방향)를 world space로 변환 
        ray.direction = firePos.TransformDirection(Vector3.forward);

        //Scene 뷰에만 시각적으로 표현함
        Debug.DrawRay(ray.origin, ray.direction * 30.0f, Color.green);


        //위에서 미리 생성한 ray를 인자로 전달, out(메서드 안에서 메서드 밖으로 데이타를 전달 할때 사용)hit, ray 거리
        if (Physics.Raycast(ray, out hitInfo, 30.0f))
        {

             // hitInfo.point 는 월드좌표이다 따라서 로컬 좌표로 변환
             Vector3 posValue = firePos.InverseTransformPoint(hitInfo.point);
             //타겟 거리체크 레이저 생성
             rayLine.SetPosition(0, posValue);
             //타겟에 레이저 Dot 생성 
             rayDot.localPosition = posValue;

            if (shot && hitInfo.collider.tag == "Enemy")
            {
                //발사를 위한 변수 true
                check = true;
            }


        }
        else
        {
            //기본 거리체크 레이저 생성
            rayLine.SetPosition(0, new Vector3(0, 0, 30.0f));

            //타겟에 레이저 Dot 초기화 
            rayDot.localPosition = Vector3.zero;

        }



        if (!shot)
        {

            myTr.RotateAround(targetTr.position, Vector3.up, Time.deltaTime * 55.0f);
            //transform.RotateAroundLocal(Vector3.up, Time.deltaTime * 55.0f);

            //발사를 위한 변수 false
            check = false;
        }
        else
        {
            //적을 봐라봄  
            if (shot)
            {
                 if (Time.time > enemyLookTime)
                 {

                    //	enemyLookRotation = Quaternion.LookRotation(-(EnemyTarget.forward)); // - 해줘야 바라봄  
                        enemyLookRotation = Quaternion.LookRotation(EnemyTarget.position - myTr.position); // - 해줘야 바라봄  
                        myTr.rotation = Quaternion.Lerp(myTr.rotation, enemyLookRotation, Time.deltaTime * 2.0f);
                        enemyLookTime = Time.time + 0.01f;
                 }
             }
        }



        //만약 발사가 true 이면....
        if (shot && check)
        {
            if(Time.time > bulletSpeed)
            {
                //일정 주기로 발사
                ShotStart();
                bulletSpeed = Time.time + 0.3f;
            }
        }


    }

    // 테스트 용
  /*  void FixedUpdate()
    {
        // TransformDirection 함수는 방향(벡터)을 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다
        Vector3 rayForward = transform.TransformDirection(Vector3.forward);

        if(Physics.Raycast(firePos.position, rayForward, 30f))
        {
            Debug.DrawRay(transform.position, rayForward * 30.0f, Color.green);
        }
    }*/



    // 자신과 가장 가까운 적을 찾음
    IEnumerator TargtSetting()
    {

        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);

            // 자신과 가장 가까운 플레이어 찾음
            Enemys = GameObject.FindGameObjectsWithTag("EnemyBody");
            Transform EnemyTargets = Enemys[0].transform;
            float dist = (EnemyTargets.position - myTr.position).sqrMagnitude;
            foreach (GameObject _Enemy in Enemys)
            {
                if ((_Enemy.transform.position - myTr.position).sqrMagnitude < dist)
                {
                    EnemyTargets = _Enemy.transform;
                    dist = (EnemyTargets.position - myTr.position).sqrMagnitude;
                }
            }


            EnemyTarget = EnemyTargets;

        }

    }

    // 가장 가까운 적을 찾아 발사...
    IEnumerator ShotSetting()
    {

        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);

            dist1 = (EnemyTarget.position - myTr.position).sqrMagnitude;
            dist2 = Vector3.Distance(myTr.position, EnemyTarget.position);


            // 체크후 발사 지정... 코루틴으로 처리가 더 효율
            if (dist2 < 37.0f)
            {
                shot = true;
            }
            else
            {
                shot = false;
               
            }

        
        }

    }


    //터렛 발사
    private void  ShotStart()
    {
        //잠시 기다리는 로직처리를 위해 코루틴 함수로 호출
        StartCoroutine(this.FireStart());
    }

    // 총탄 발사 코루틴 함수
    IEnumerator FireStart()
    {
        //Debug.Log("Fire");
        //Bullet 프리팹을 동적 생성
        Instantiate(bullet, firePos.position, firePos.rotation);

        //총탄 사운드 발생 
        source.PlayOneShot(fireSfx, fireSfx.length + 0.2f);

        //MuzzleFlash 스케일을 불규칙하게 하자 
        float scale = Random.Range(1.0f, 2.5f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        //MuzzleFlash를 Z축으로 불규칙하게 회전시키자 
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;

        //활성화 시킴
        muzzleFlash.SetActive(true);

        //랜덤 시간 동안 Delay한 다음 MeshRenderer를 비활성화
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));

        //비활성
        muzzleFlash.SetActive(false);

    }





    //인스펙터에 스크립트 우 클릭시 컨텍스트 메뉴에서 함수호출 가능
    [ContextMenu("FireStart")]
    void Fire()
    {
        shot = true;
    }

}
