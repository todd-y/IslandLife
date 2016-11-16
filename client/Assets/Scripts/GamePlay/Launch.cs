using UnityEngine;
using System.Collections;

/// <summary>
/// 游戏启动
/// </summary>
public class Launch : MonoBehaviour {

    public static Launch inst = null;

    // build in 
    void Awake() {
        Debug.Log(" Launch Awake Begin......");
        // 保证只有一个， 重复创建的对象直接删掉
        if (null != inst) {
            GameObject.DestroyImmediate(gameObject);
            Debug.Log(" Launch Already Awaked !!! ");
            return;
        }

        inst = this;

        CoDelegator.Instance = gameObject.AddMissingComponent<CoDelegator>();
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        StartInit();
	}

    public void StartInit(){
        Game.Instance.Init();
    }

    //预留接口
    public void DestoryInit(){
        Game.Instance.Clear();
    }
}
