using UnityEngine;
using System.Collections;

public class BattleMgr : Singleton<BattleMgr> {
    public PlayerInfo playerInfo;
    public MakeInfo makeInfo;
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
	}
	
	public void Clear(){
	}

    public void StartBattle() {
        playerInfo = new PlayerInfo();
        playerInfo.SetData(RoleType.Warrior);
        CurProgress = 0;

        makeInfo = new MakeInfo();
        makeInfo.SetData();

        WindowMgr.Instance.OpenWindow<BattleWindow>();
    }
}
