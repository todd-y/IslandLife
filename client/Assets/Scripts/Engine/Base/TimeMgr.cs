using UnityEngine;
using System.Collections;

/// <summary>
/// 时间模块
/// </summary>
public class TimeMgr : Singleton<TimeMgr> {
    private float curTime = 0;
	public void Init(){
		
	}
	
	public void Clear(){
		
	}

    public void FixUpdate() {
        curTime += Time.deltaTime;
        Send.SendMsg(SendType.TimeUpdate);
    }
}
