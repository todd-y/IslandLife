using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 工具类
/// </summary>
public class ToolMgr : Singleton<ToolMgr> {
	public void Init(){
		
	}
	
	public void Clear(){
		
	}

    public int GetRandom(List<int> list) {
        if (list.Count == 0) {
            Debug.LogError("getrandom list is null");
            return 0;
        }
        else if(list.Count == 1){
            return list[0];
        }
        else {
            return Random.Range(list[0], list[1] + 1);
        }
    }

    public T RandomAndRemove<T>(List<T> list) {
        if (list.Count == 0) {
            Debug.LogError("getrandom list is null");
            return default(T);
        }
        int index = Random.Range(0, list.Count);
        T result = list[index];
        list.RemoveAt(index);
        return result;
    }
}
