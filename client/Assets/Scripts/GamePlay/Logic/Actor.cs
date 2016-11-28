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
        int randomAge = ToolMgr.Instance.RandomRange(refActor.Age);
        birth = BattleMgr.Instance.CurTime.AddYears(-randomAge).AddDays( UnityEngine.Random.Range(0f,1f) * 365);
        cachet = ToolMgr.Instance.RandomRange(refActor.Cachet);
        loyalty = ToolMgr.Instance.RandomRange(refActor.Loyalty);
        ability = ToolMgr.Instance.RandomRange(refActor.Ability);
        remainFood = ToolMgr.Instance.RandomRange(refActor.RemainFood);
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

    public void UpdateDT(float dt) {
        List<BuffObj> list = new List<BuffObj>(buffList);
        for (int index = 0; index < list.Count; index++) {
            list[index].Update(dt);
        }
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

    public override void AddBuff(ResultType resultType, int value, int durationTime) {
        Debug.LogError("acotr addbuff  " + resultType + "/" + value);
        switch (resultType) {
            case ResultType.Food:
                RemainFood += value;
                return;//不加入buff
            case ResultType.Loyalty:
                loyalty += value;
                break;
            case ResultType.Ability:
                ability += value;
                break;
            case ResultType.Cachet:
                cachet += value;
                break;
            case ResultType.Characteristic:
                break;
            default:
                Debug.LogError("actor resultType is no handle " + resultType);
                return;//不加入buff
        }

        BuffObj buffObj = new BuffObj(this, resultType, value, durationTime);
        buffList.Add(buffObj);
    }

    public override void RemoveBuff(BuffObj buffObj) {
        Debug.LogError("actor removebuff");
        int value = buffObj.value;
        switch (buffObj.resultType) {
            case ResultType.Loyalty:
                loyalty -= value;
                break;
            case ResultType.Ability:
                ability -= value;
                break;
            case ResultType.Cachet:
                cachet -= value;
                break;
            case ResultType.Characteristic:
                break;
            default:
                Debug.LogError("actor RemoveBuff is no handle " + buffObj.resultType);
                break;//不加入buff
        }

        buffList.Remove(buffObj);
    }
}
