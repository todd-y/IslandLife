using UnityEngine;
using System.Collections;

public class BattleMgr : Singleton<BattleMgr> {
    public PlayerInfo playerInfo;
    public int totalProgress = 100;
    public int[] arrProgress = { 24, 48, 72 };
    private float m_curProgress;
    public float CurProgress {
        get { return m_curProgress; }
        set {
            m_curProgress = value;
            if (m_curProgress == totalProgress) {
                //to do;
            }
            Send.SendMsg(SendType.ProgressChange, CurProgress, totalProgress);
        }
    }

	public void Init(){
        Send.RegisterMsg(SendType.PlayerHit, OnPlayerHit);
	}
	
	public void Clear(){
        Send.UnregisterMsg(SendType.PlayerHit, OnPlayerHit);
	}

    public void StartBattle() {
        playerInfo = new PlayerInfo();
        playerInfo.SetData(RoleType.Warrior);
        CurProgress = 0;

        WindowMgr.Instance.OpenWindow<BattleWindow>();
    }

    private void OnPlayerHit(params object[] objs) {
        CurProgress++;
    }
}
