using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 테스트는 NavMeshAgent 비활성
public class PlayerMoveCtrl : MonoBehaviour
{
    // CharacterController 컴포넌트를 위한 레퍼런스
    CharacterController controller;

    // 중력 
    public float gravity = 20.0f;

    // 케릭터 이동속도
    public float movSpeed = 5.0f;
    // 케릭터 회전속도
    public float rotSpeed = 120.0f;
    //케릭터 점프 속도
    public float jumpSpeed = 10.0f;

    // 케릭터 이동 방향
    Vector3 moveDirection;

    void Start()
    {
        // 레퍼런스 연결
        controller = GetComponent<CharacterController>();

    }

    void Update()
    {
        // 만약 콜라이더가 땅에 있을 경우 
        if (controller.isGrounded)
        {
            //디바이스마다 일정한 회전 속도
            float amtRot = rotSpeed * Time.deltaTime;

            //키보드 값을 얻어온다.
            float ver = Input.GetAxis("Vertical");
            float ang = Input.GetAxis("Horizontal");

            //오브젝트를 회전
            transform.Rotate(Vector3.up * ang * amtRot);

            //방향 벡터를 생성 (현재는 z축만...)
            moveDirection = new Vector3(0, 0, ver * movSpeed);

            // transform.TransformDirection 함수는 인자로 전달된 벡터를 
            // 월드좌표계 기준으로 변환하여 변환된 벡터를 반환해 준다.
            //즉, 로컬좌표계 기준의 방향벡터를 > 월드좌표계 기준의 방향벡터로 변환

            moveDirection = transform.TransformDirection(moveDirection);

            //키보드가 점프 입력일 경우
            if (Input.GetButton("Jump"))
            {
                // jumpSpeed 만큼 케릭을 이동
                moveDirection.y = jumpSpeed;
            }
        }

        // 디바이스마다 일정 속도로 케릭에 중력 적용
        moveDirection.y -= gravity * Time.deltaTime;
        // CharacterController의 Move 함수에 방향과 크기의 벡터값을 적용(디바이스마다 일정)
        controller.Move(moveDirection * Time.deltaTime);
    }
}

/* 만약 CharacterController를 안쓴다면...

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerCtrl_1 : MonoBehaviour {

    // 이동 관련 변수
    private float ang = 0.0f;
    private float ver = 0.0f;
    
    // 자기 자신의 트렌스폼을 가리킴 
    private Transform myTr;
    //이동 속도 변수 
    public float moveSpeed = 10.0f;
    
    //회전 속도 변수
    public float rotSpeed = 100.0f;
         

    
    void Awake () {
        
        //자기 자신의 Transform 컴포넌트 할당
        myTr = GetComponent<Transform>();

    }
    
    void Update () {
        ang = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
        
        //Debug.Log("ang = " + h.ToString());
        //Debug.Log("ver = " + v.ToString());
        
        //전후좌우 이동 방향 벡터 계산
        Vector3 moveDir = (Vector3.forward * ver) + (Vector3.right * ang);
        
        //Translate(이동 방향 * Time.deltaTime * 변위값 * 속도, 기준좌표)
        tr.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);
        
        //Vector3.up 축을 기준으로 rotSpeed만큼의 속도로 회전 (마우스를 이용한 회전)
        //자동 게임에선 Input.GetAxis("Mouse X") 이거 대신 적 케릭터 위치값 넣으면 된다.
        tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));
        
    }
    

 
}
 */
