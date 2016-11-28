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

    public int RandomRange(List<int> list) {
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

    public T RandomWithWeight<T>(List<T> list, List<int> weightList) {
        if (list.Count == 0) {
            Debug.LogError("getrandom list is null");
            return default(T);
        }
        if (list.Count == 1) {
            return list[0];
        }
        if (list.Count != weightList.Count) {
            Debug.LogError("list and weight count is unique");
            return list[0];
        }

        T result = default(T);
        int totalWeight = SumList(weightList);
        int randomWeight = Random.Range(0, totalWeight);

        int curTotalWeight = 0;
        for (int index = 0; index < list.Count; index++ ) {
            int curWeight = weightList[index];
            curTotalWeight += curWeight;
            if (curTotalWeight > totalWeight) {
                return list[index];
            }
        }

        return result;
    }

    public int SumList(List<int> list) {
        int result = 0;
        for (int index = 0; index < list.Count; index++ ) {
            result += list[index];
        }
        return result;
    }
}
