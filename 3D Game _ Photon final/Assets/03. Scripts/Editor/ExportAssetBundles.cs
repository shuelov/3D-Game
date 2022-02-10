using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// UnityEditor 네임스페이스를 사용해야 사용자 메뉴를 등록 가능
using UnityEditor;


public class ExportAssetBundles : MonoBehaviour
{
    //등록할 대분류 메뉴/중분류 메뉴 선언 
    //[MenuItem("경로")] 로 메뉴에 새로운 탭을 만들거나 
    //이미 존재하는 탭에 옵션이나 하위 메뉴를 만들 수 있다.
    // 뒤에 실행될 함수는 무조건 static 선언을 해야한다.
    [MenuItem("Build/Build AssetBundle #%&d")]
    //메뉴를 클릭하면 실행되는 함수 
    public static void BulidSceneToAssetBundle()
    {
        //AssetBundle로 만들 Scene의 경로와 이름을 배열에 저장 
        string sceneNames = "AssetBundles";

        //AssetBundle로 생성 
        //pc 버전
        BuildPipeline.BuildAssetBundles(sceneNames, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        //안드로이드 버전
        //BuildPipeline.BuildAssetBundles(sceneNames, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    //등록할 대분류 메뉴/중분류 메뉴 선언 
    [MenuItem("Build/Build AssetBundle _PGDOWN")]
    //메뉴를 클릭하면 실행되는 함수 
    public static void BulidSceneToAssetBundle2()
    {
        //AssetBundle로 만들 Scene의 경로와 이름을 배열에 저장 
        string sceneNames = "AssetBundles";

        //AssetBundle로 생성 
        //pc 버전
        BuildPipeline.BuildAssetBundles(sceneNames, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        //안드로이드 버전
        //BuildPipeline.BuildAssetBundles(sceneNames, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    //등록할 대분류 메뉴/중분류 메뉴 선언 
    [MenuItem("Assets/Build AssetBundles _")]
    //메뉴를 클릭하면 실행되는 함수 
    public static void BulidSceneToAssetBundle3()
    {
        //AssetBundle로 만들 Scene의 경로와 이름을 배열에 저장 
        string sceneNames = "AssetBundles";

        //AssetBundle로 생성 
        //pc 버전
        BuildPipeline.BuildAssetBundles(sceneNames, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        //안드로이드 버전
        //BuildPipeline.BuildAssetBundles(sceneNames, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    //MenuItem을 사용하면 메뉴창에 새로운 메뉴 항목을 추가할 수 있다.
    //아래 코드로 Bundles 항목에 하위 항목으로 Build AssetBundles 항목을 추가.
    [MenuItem("Assets/Create/Build AssetBundles _")]
    //메뉴를 클릭하면 실행되는 함수 
    public static void BulidAllAssetBundles()
    {
        /*  BuildPipeline 클래스의 클래스함수인 BuildAssetBundles() 함수는 
            에셋 번들을 만들어 준다.BuildAssetBundles() 함수는 String 매개변수가 
            선언 되어있다. 이 매개변수는 빌드된 에셋 번들을 저장할 경로이다.
            예를 들어 Assets 폴더의 하위 폴더 AssetBundles 폴더에 저장하려면
            "Assets/AssetBundles" 라고 입력한다.
        */

        //AssetBundle로 생성
        //pc 버전
        BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        //안드로이드 버전
        //BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
    }


    //CONTEXT/컴포넌트의이름 쓰면 컴포넌트의 설정에 뜬다.
    //리지드바디 컴포넌트의 우측상위의 톱니모양 클릭시 ...
    //컨텍스트 메뉴(context menu)에 Double Mass 메뉴 추가...
    [MenuItem("CONTEXT/Rigidbody/Double Mass")]
    static void DoubleMass(MenuCommand command)
    {

        Rigidbody body = (Rigidbody)command.context;

        body.mass = body.mass * 2;

        Debug.Log("+ Doubled Rigidbody's Mass to " + body.mass + " from Context Menu.");

    }

    //메인 메뉴 GameObject와 hierarchy에 dropdown과 context menue에 메뉴 추가
    //마지막 인자는 같은 종류의 메뉴 아이템에 대해서 순서를 정해주고 크기가 10 초과 할 경우 선 추가.(그룹 지정 가능)
    [MenuItem("GameObject/MyCategory/Custom Game Object1",false, 1)]
    //[MenuItem("GameObject/MyCategory/Custom Game Object1", false, 10)]
    static void CreateCustomGameObject1(MenuCommand menuCommand)
    {

        // custom game object 생성

        GameObject go = new GameObject("Custom Game Object");

        // Custom Game Object란 빈 게임오브젝트를 생성 후
        // 생성된 계층뷰에(Hierarchy) Custom Game Object를 선택 후 우클릭으로
        // 컨텍스트 메뉴에서 오브젝트를 만들면...차일드 안된다...
        // 하지만 다음 함수 호출로 가능해진다..

        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        // undo system(이전으로 돌아가는 처리)에 등록 (스택에 등록...)(ctrl + z)
        //스택은 LIFO(Last In, First Out, 선입선출 구조)

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);

        //객체생성시 Hierarchy 에서 선택되게 해준다...
        Selection.activeObject = go;



    }


    [MenuItem("GameObject/MyCategory/Custom Game Object2", false, 12)]
    static void CreateCustomGameObjectEx1(MenuCommand menuCommand)
    {

        //큐브 생성
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

    }


    //다음과 같이 메뉴에 확인 작업을 할 수 있도록같은 
    //같은 MenuItem 옵션에 인자를 true로 추가로 전달 하면, 
    //동일한 MenuItem 옵션의 함수(CreateCustomGameObject)를 
    //호출하기 전에 이 함수가 먼저 호출되어 확인 작업을 한다.(메뉴 활성/비활성)
    [MenuItem("GameObject/MyCategory/Custom Game Object2", true)]
    static bool CreateCustomGameObjectCheak()
    {
        //현재 활성화된 게임 오브젝트가 씬 에셋인지 체크(오브젝트 클릭시 활성)
        return Selection.activeObject.GetType() == typeof(SceneAsset);

    }
    

    [MenuItem("GameObject/MyCategory/Custom Game Object3", false, 15)]
    static void CreateCustomGameObjectEx2(MenuCommand menuCommand)
    {

        //큐브 생성
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

    }
}

//메뉴의 경우 핫키를 지정 가능하며, 또한 둘 이상 메뉴에 같은 핫키를
//지정하면 우선 순위가 높은 함수 하나만 적용된다.

/* 단축키 지정 키워드
 * # Shift
 * % Ctrl 또는 CMD
 * & Alt
 * _ 없음
 *  +,-, LEFT/RIGHT/UP/DOWN(방향키), F1.....F12
 *  HOME/END/PGUP/PGDN, 문자
 */



