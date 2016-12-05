using UnityEngine;
using System.Collections;

/// <summary>
/// 游戏启动
/// </summary>
public class Launch : MonoBehaviour {

    public static Launch Instance = null;

    // build in 
    void Awake() {
        Debug.Log(" Launch Awake Begin......");
        // 保证只有一个， 重复创建的对象直接删掉
        if (null != Instance) {
            GameObject.DestroyImmediate(gameObject);
            Debug.Log(" Launch Already Awaked !!! ");
            return;
        }

        Instance = this;

        CoDelegator.Instance = gameObject.AddMissingComponent<CoDelegator>();
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        CoDelegator.Coroutine(InitClient());
        CoDelegator.Coroutine(InitAsset());
	}

    void FixedUpdate() {
        TimeMgr.Instance.FixUpdate();
    }

    //预留接口
    public void DestoryInit(){
        Game.Instance.Clear();
    }

    IEnumerator InitClient() {
        Debug.Log("Launch InitClient Begin......");
        //Debug.Log(" Create Debug Start");
        //CreateDebug();
        
        // 初始化配置表
        RefDataMgr.Instance.InitBasic();

        Debug.Log(" Create UI Start");
        //CreateUI();
        //Debug.Log(CommTime.GetNowTimeString() + " Create Audio Start");
        //CreateAudio();
        //Debug.Log(CommTime.GetNowTimeString() + " Create Sound Start");
        //CreateSound();
        //SoundManager.instance.PlayBgMusic("music_Launch", true);

        Debug.LogWarning("Launch InitClient End......");
        yield break;
    }

    IEnumerator InitAsset() {
        // 配置加载
        Debug.Log(" RefDataMgr Init Start");
        yield return CoDelegator.Coroutine(RefDataMgr.Instance.Init());
        Debug.Log(" RefDataMgr Init End");

        Game.Instance.Init();

        Debug.Log("Launch InitAsset End !!!");

        //WindowMgr.Instance.OpenWindow<LoadingWindow>();
        yield break;
    }
}
