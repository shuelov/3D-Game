using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Opening : MonoBehaviour
{

    //애셋번들 참조 변수 
    AssetBundle assetBundle;

    //AssetBundle을 내려받을 주소 
    string url = "";

    //AssetBundle의 버전 
    int version = 1;

    //Web 접속하기 위한 변수 
    WWW www;

    /* WWW 클래스는 http://, https://, ftp://, file:///
     * 프로토콜(Protocol). 지원
     */

    void Awake()
    {
        //스크린 절전 모드 안가게 
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // ftp 서버 or 웹 서버 주소 + 애셋번들 파일 위치
        //  url = "http://210.122.7.164/dinoScenes/ScLevel_1.0.6.unity3d";
        url = "file:///C:\\Bundles\\study.study";

    }

    IEnumerator Start()
    {
        //지정된 url주소로 접근하여 해당파일을 내려받음 
        www = WWW.LoadFromCacheOrDownload(url, version);
        yield return www;

        //오류가 있으면 메세지 출력 
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error.ToString());
        }
        else
        {
            //내려받은 AssetBundle을 메모리에 로드 
            assetBundle = www.assetBundle;
        }
    }

    /*  LoadFromCacheOrDownload(url, version) 함수는
     *  url 주소 : 내려받을 AssetBundle의 경로와 이름 지정
     *  version  : Integer 타입으로 만약 동일한 version의
     *  AssetBundle 파일이 Catch메모리에 있을 경우 url 경로를
     *  무시 하고 Catch메모리에 있는 것을 로드한다. 또한 만약 새로
     *  배포하는 앱의 version을 1에서 2로 변경하면 Catch메모리에 
     *  있는 AssetBundle 파일을 무시하고 다시 내려받는다.
     */

    void OnGUI()
    {
        //AssetBundle을 모두 내려받으면 GUI버튼을 생성 
        if (www.isDone && GUI.Button(new Rect(20, 50, 100, 30), "Start Game"))
        {
            LoadScenes();
        }

        //내려받는 진행상태 표시
        GUI.Label(new Rect(20, 20, 200, 30)
                   , "Downloading..." + (www.progress * 100.0f).ToString() + "%");

    }

    /*
        void OnGUI()
        {
            if( GUI.Button( new Rect(50, 50, 200, 50), "Start" ) )
            {
                LoadScenes();
            }
        }
        */

    // 먼저 로드한 scLevel_01 Scene에 scLogic Scene을 병합해서 로드
    void LoadScenes()
    {
        SceneManager.LoadScene("scLevel_01");
        SceneManager.LoadScene("scLogic", LoadSceneMode.Additive);

    }

    // 주의 사항~ 씬 분리/병합처리시 라이트맵은 다시 굽자~ 또한 서로 플랫폼을 맞춰주자~

}
