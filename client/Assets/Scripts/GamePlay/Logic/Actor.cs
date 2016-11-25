using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 人物
/// </summary>
public class Actor : BaseData {
    public string headIconID = "Icon1";
    public string name;
    public DateTime birth;
    public float cachet;    //威望
    public float loyalty;   //忠诚度
    private float remainFood;//余粮
    public float ability;//能力

    public OfficialType officialType;
    public List<TitleType> titleType = new List<TitleType>();
    public List<Characteristic> CharacteristicList = new List<Characteristic>();

    public Actor(RefActor refActor, string _name) {
        name = _name;
        int randomAge = ToolMgr.Instance.GetRandom(refActor.Age);
        birth = BattleMgr.Instance.CurTime.AddYears(-randomAge).AddDays( UnityEngine.Random.Range(0f,1f) * 365);
        cachet = ToolMgr.Instance.GetRandom(refActor.Cachet);
        loyalty = ToolMgr.Instance.GetRandom(refActor.Loyalty);
        ability = ToolMgr.Instance.GetRandom(refActor.Ability);
        remainFood = ToolMgr.Instance.GetRandom(refActor.RemainFood);
    }

    public float RemainFood {
        get {
            return remainFood;
        }
        set {
            remainFood = value;
        }
    }

    public void Init(OfficialType _officialType, RoleType _roleType){
        officialType = _officialType;
        roleType = _roleType;
        roleName = _officialType.ToString();
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
