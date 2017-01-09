using UnityEngine;
using System.Collections;

/// <summary>
/// 游戏逻辑模块
/// </summary>
public class Game : Singleton<Game> {

    public void Init() {
        WindowMgr.Instance.Init();
        ToolMgr.Instance.Init();
        TimeMgr.Instance.Init();
        BattleMgr.Instance.Init();
        RoomCreatMgr.Instance.Init();
    }

    public void Clear() {
        WindowMgr.Instance.Clear();
        ToolMgr.Instance.Clear();
        TimeMgr.Instance.Clear();
        BattleMgr.Instance.Clear();
        RoomCreatMgr.Instance.Clear();
    }
}
