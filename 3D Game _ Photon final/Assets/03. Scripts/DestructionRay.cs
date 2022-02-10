// 전처리기는 최 상단에...
#define CBT_MODE
#define RELEASE_MODE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.iOS;


public class DestructionRay : MonoBehaviour
{

    // 폭발 이펙트
    public GameObject fireEffect;

    //Ray 정보 저장 구조체 
    Ray ray;


    // Ray에 맞은 오브젝트 정보를 저장 할 구조체
    RaycastHit hitInfo;


    // OnDrawGizmosSelected() 로 만든 기즈모는 이 컴포넌트를 가지고 있는 
    // 게임오브젝트를 클릭 시 표현되며 기즈모 클릭이 불가능.
    // 유용한 부분은 폭발 스크립트 작성시 폭발 반경을 보여주는 영역을 그릴 수 있다. 

    // 씬뷰에서 좌 하단으로 부터 100픽셀 떨어진... 
    // 선택된 카메라의 가까운면의 위치에 
    // 노란색 구체를 그린다.
    void OnDrawGizmosSelected()
    {
        //position을 화면 공간(스크린)에서 월드 공간으로 반환해준다.
        //화면 공간은 픽셀로 정의 되며, 화면의 좌하단은(0,0)이고 
        //우상단은(pixelWidth, pixelHeight)이다.
        //z position은 선택 카메라로부터의 거리를 월드 단위로 환산한 값 이다.

        Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(100, 100, Camera.main.nearClipPlane));
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(p, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {

        //Main Camera 에서 마우스 커서(Vector3 타입이지만 Z값 무시한 값 (0~1280,0~800,0) )의 위치로 캐스팅되는 Ray를 생성함
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#if CBT_MODE // #if WSJ,MODE_1,MODE_2 로 바꿔보기
        //Scene 뷰에만 시각적으로 표현함
        Debug.DrawRay(ray.origin, ray.direction * 150.0f, Color.green);
#elif RELEASE_MODE
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.red);
#endif


#if UNITY_EDITOR
        //마우스 왼쪽 버튼을 클릭시 Ray를 캐스팅
        if (Input.GetMouseButtonDown(0))
        {
            //위에서 미리 생성한 ray를 인자로 전달, out(메서드 안에서 메서드 밖으로 데이타를 전달 할때 사용)hit, ray 거리
            if (Physics.Raycast(ray, out hitInfo, 150.0f))
            {
                //ray에 hit된 객체의 태그를 비교함 
                if (hitInfo.collider.tag == "DESTROYOBJECT")
                {
                    //파티클 생성 
                    //.............
                    Instantiate(fireEffect, hitInfo.point, Quaternion.identity );

                    //오브젝트 제거
                    Destroy(hitInfo.collider.gameObject);
                }

                //드럼통에 Hit 되면 MovePlayerCtrlRay에 BarrelFire 함수호출
                else if(hitInfo.collider.tag == "Barrel")
                {
                    GameObject.Find("MainPlayer").GetComponent<PlayerCtrl>().BarrelFire(hitInfo.collider.gameObject.transform);
                }

            }
        }
#endif

#if UNITY_ANDROID
        //스크린에 터치가 이루어진 상태에서 첫 번째 손가락 터치가 시작됐는지 비교
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //Main Camera에서 손가락을 터치한  벡터 위치로 캐스팅하는 Ray를 생성 함
            ray = Camera.main.ScreenPointToRay(Input.touches[0].position);

            //위에서 미리 생성한 ray를 인자로 전달, out(메서드 안에서 메서드 밖으로 데이타를 전달 할때 사용)hit, ray 거리
            if (Physics.Raycast(ray, out hitInfo, 150.0f))
            {
                //ray에 hit된 객체의 태그를 비교함 
                if (hitInfo.collider.tag == "DESTROYOBJECT")
                {
                    //파티클 생성 
                    //.............
                    Instantiate(fireEffect, hitInfo.point, Quaternion.identity );

                    //오브젝트 제거
                    Destroy(hitInfo.collider.gameObject);
                }

                //드럼통에 Hit 되면 MovePlayerCtrlRay에 BarrelFire 함수호출
                else if(hitInfo.collider.tag == "Barrel")
                {
                    GameObject.Find("Player").GetComponent<PlayerCtrl>().BarrelFire(hitInfo.collider.gameObject.transform);
                }

            }

        }
#endif


#if UNITY_IPHONE
        //스크린에 터치가 이루어진 상태에서 첫 번째 손가락 터치가 시작됐는지 비교
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //Main Camera에서 손가락을 터치한  벡터 위치로 캐스팅하는 Ray를 생성 함
            ray = Camera.main.ScreenPointToRay( Input.touches[0].position );

            //위에서 미리 생성한 ray를 인자로 전달, out(메서드 안에서 메서드 밖으로 데이타를 전달 할때 사용)hit, ray 거리
            if (Physics.Raycast(ray, out hitInfo, 150.0f))
            {
                //ray에 hit된 객체의 태그를 비교함 
                if (hitInfo.collider.tag == "DESTROYOBJECT")
                {
                    //파티클 생성 
                    //.............

                    Instantiate(fireEffect, hitInfo.point, Quaternion.identity );
                    //오브젝트 제거
                    Destroy(hitInfo.collider.gameObject);
                }

                //드럼통에 Hit 되면 MovePlayerCtrlRay에 BarrelFire 함수호출
                else if(hitInfo.collider.tag == "Barrel")
                {
                    GameObject.Find("Player").GetComponent<PlayerCtrl>().BarrelFire(hitInfo.collider.gameObject.transform);
                }

            }

        }
#endif




    }
}


/*
    Touch 구조체는 스크린상에 터치한 손가락의 모든 상태와 정보를 담는다.

    position 터치된 좌표 (픽셀 단위)
    fingerId   손가락의 고유한 인덱스(터치별로 고유한 값이 순서대로 설정된다.)
    deltaPosition 마지막의 위치로부터 총 움직인 위치 변위 값 
    deltaTime  마지막 위치 변경 이후부터 총 경과된 시간
    tapCount  탭의 수 (더블클릭 등...)
    phase   터치의 유형(터치의 시작, 터치의 종료, 터치의 취소, 터치의 오래 누르기)

    = TouchPhase 는 터치의 유형을 정의한 열거형 타입 =
    Began  스크린에 터치를 시작
    Moved  스크린에 터치를 한 후 이동하는 상태
    Stationary  터치를 한 후에 이동하지 않고 계속 터치중인 상태 
    Canceled 터치가 취소됐을 경우 
    Ended 터치를 종료했을 경우 

    Input.touches는 한 번에 여러 개의 손가락 터치를 처리하기 위해서 Touch 구조체 배열을 반환한다. Input.touches[0] 는 첫 번째 터치 정보

*/
