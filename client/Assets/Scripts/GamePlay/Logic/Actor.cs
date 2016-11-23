using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 人物
/// </summary>
public class Actor : BaseData {
    public string name;
    public DateTime birth;
    public float cachet;    //威望
    public float loyalty;   //忠诚度
    private float remainFood;//余粮

    public OfficialType officialType;
    public List<TitleType> titleType = new List<TitleType>();
    public List<Characteristic> CharacteristicList = new List<Characteristic>();

    public float RemainFood {
        get {
            return remainFood;
        }
        set {
            remainFood = value;
        }
    }

    public void Init(){
		
	}
	
	public void Clear(){
		
	}

    public void DailyUpdate() {

    }

    public int GetAge() {
        return BattleMgr.Instance.CurTime.Year - birth.Year;
    }

    public bool BirthDay(int month, int day) {
        if (birth.Month == month && birth.Day == day) {
            return true;
        }

        return false;
    }
}
