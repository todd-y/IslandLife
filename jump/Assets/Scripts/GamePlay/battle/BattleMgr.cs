using UnityEngine;
using System.Collections;

public class BattleMgr : Singleton<BattleMgr> {
    public PlayerInfo playerInfo;

	public void Init(){
		
	}
	
	public void Clear(){
		
	}

    public void StartBattle() {
        playerInfo = new PlayerInfo();
        playerInfo.SetData(RoleType.Warrior);

        WindowMgr.Instance.OpenWindow<BattleWindow>();
    }
}
