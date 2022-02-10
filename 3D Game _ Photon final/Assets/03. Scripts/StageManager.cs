using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour {

    //로그 정보
    public Text TxtLogMsg;
    //몇 명 들어와 있는지
    public Text TxtConnect;
 
    //스폰 장소 
    private Transform[] EnemySpawnPoints;

    //Enemy 프리팹을 위한 레퍼런스
    public GameObject Enemy;

    //게임 끝
    private bool gameEnd;

    // 스테이지 Enemy들을 위한 레퍼런스
    private GameObject[] Enemys;

    // 베이스 스타트를 위한 변수
    public BaseCtrl baseStart;

	//플레이어가 생성되는 지점
	public Transform playerPos;

    //포톤뷰 
    PhotonView pv;

    //채팅메세지
    public InputField Chattxt;

    public Text TxtChatMsg;

	void Awake()
    {
        //포톤뷰 레퍼런스 할당
        pv = GetComponent<PhotonView>();

        //스폰 위치 얻기
        EnemySpawnPoints = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();

        // 몬스터 스폰 코루틴 호출
        StartCoroutine(this.CreateEnemy());

    }

    // Use this for initialization
    IEnumerator Start ()
    {
        yield return new WaitForSeconds(5.0f);

        //베이스 지킴이 시작
        baseStart.StartBase();

		PhotonNetwork.Instantiate("MainPlayer", playerPos.position, playerPos.rotation, 0);
		PhotonNetwork.isMessageQueueRunning = true;

        GetConnectPlayerCount();

        //로그 메시지에 출력할 문자열 생성
        string msg = "\n\t<color=#00ff00>["
            + PhotonNetwork.player.NickName
            + "]Connected</color>";

        //RPC함수 호출 
        pv.RPC("LogMsg", PhotonTargets.AllBuffered,msg);
    }

    [PunRPC]
    void LogMsg(string msg)
    {
        TxtLogMsg.text = TxtLogMsg.text + msg;
    }
    
    //Send 버튼 누르면 호출되게
    public void Chatting()
    {
        //채팅 메세지
        string ChatStory = Chattxt.text;

        if (PhotonNetwork.isMasterClient)
        //로고 메시지에 출력할 문자열 생성
        {
            string msg = "\n\t<color=#00ffff>"
              + PhotonNetwork.player.NickName
              + " : "
              + ChatStory
              + "</color>";
            //RPC함수 호출 
            pv.RPC("ChatMsg", PhotonTargets.AllBuffered, msg);
        }

        else
        {
            string msg = "\n\t<color=#0000ff>"
              + PhotonNetwork.player.NickName
              + " : "
              + ChatStory
              + "</color>";
            //RPC함수 호출 
            pv.RPC("ChatMsg", PhotonTargets.AllBuffered, msg);
        }


    }

    //채팅 글자 출력
    [PunRPC]
    void ChatMsg(string msg)
    {
        TxtChatMsg.text = TxtChatMsg.text + msg;
    }


    // 몬스터 생성 코루틴 함수
    IEnumerator CreateEnemy()
    {
        //게임중 일정 시간마다 계속 호출됨 
        while (!gameEnd)
        {
            //리스폰 타임 5초
            yield return new WaitForSeconds(5.0f);

            // 스테이지 총 몬스터 객수 제한을 위하여 찾자~
            Enemys = GameObject.FindGameObjectsWithTag("Enemy");

            // 스테이지 총 몬스터 객수 제한
            if (Enemys.Length < 20)
            {
                //루트 생성위치는 필요하지 않다.그래서 1 번째 인덱스부터 돌리자
                for (int i = 1; i< EnemySpawnPoints.Length; i++)
                {
                    Instantiate(Enemy, EnemySpawnPoints[i].localPosition, EnemySpawnPoints[i].localRotation);
                }
            }
        }
    }

    //새로운 플레이어가 들어오면 자동 호출해줌
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {

        Debug.Log(newPlayer.ToStringFull());

        GetConnectPlayerCount();

    }
    void OnPhotonPlayerDisconnected(PhotonPlayer outPlyaer)
    {

        Debug.Log(outPlyaer.ToStringFull());

        GetConnectPlayerCount();

    }
    //룸 접속자를 조회하는 함수
    void GetConnectPlayerCount()
    {
        Room currRoom = PhotonNetwork.room;

        TxtConnect.text = currRoom.PlayerCount.ToString()
            + "/"
            + currRoom.MaxPlayers.ToString();
            
     }

    //버튼
    public void OnClickExitRoom()
    {
        //로그메세지에 출력할 문자열 생성
        //문자열에 색상 줄 수 있음
        string msg = "\n\t<color=#ff0000>["
        + PhotonNetwork.player.NickName
        + "]Disconnected</color>";

        //RPC함수 호출 
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);

        Debug.Log(msg);
        
        //들어오면서 만들었던 모든 객체를(rpc포함) 삭제하라는 메시지를 클라이언트들에 다 보냄
        PhotonNetwork.LeaveRoom();
    }

    void OnLeftRoom()
    {
        //모든게 정리되고 로비로 다시 넘어감
        SceneManager.LoadScene("sNetLobby");
    }

}
