using UnityEngine;
using System.Collections;

/// <summary>
/// 游戏逻辑模块
/// </summary>
public class Game : Singleton<Game> {

    public void Init() {
        ToolMgr.Instance.Init();
        TimeMgr.Instance.Init();
    }

    public void Clear() {
        ToolMgr.Instance.Clear();
        TimeMgr.Instance.Clear();
    }
}
