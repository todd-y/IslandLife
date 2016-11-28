using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 郡县
/// </summary>
public class County : BaseData {
    private Country country;
    private Actor leader;
	private float taxRate;
    private float peopleNum;
    private float loyalty;
    private float remainFood;
    private float armyNum;
    private float corruptionRate;//贪污率
    private float areaFactor;//地区系数

    public float PeopleNum {
        get {
            return peopleNum;
        }
        set {
            peopleNum = value;
            Send.SendMsg(SendType.FoodChange);
        }
    }

    public float Loyalty {
        get {
            return loyalty;
        }
        set {
            loyalty = value;
            Send.SendMsg(SendType.LoyaltyChange);
        }
    }

    public float ArmyNum {
        get {
            return armyNum;
        }
        set {
            armyNum = value;
            Send.SendMsg(SendType.ArmyChange);
        }
    }

    public float CorruptionRate {
        get{
            return corruptionRate;
        }
        set {
            corruptionRate = value;
        }
    }

    public int RemainFood {
        get {
            return (int)remainFood;
        }
        set {
            remainFood = value;
        }
    }

    public int AreaFactor {
        get {
            return (int)areaFactor;
        }
        set {
            areaFactor = value;
        }
    }

    public County( RefCounty _countyData, string _name, Country _country) {
        roleType = RoleType.County;
        roleName = _name;
        country = _country;
        taxRate = 0;
        peopleNum = ToolMgr.Instance.RandomRange(_countyData.PeopleNum);
        loyalty = ToolMgr.Instance.RandomRange(_countyData.Loyalty);
        remainFood = ToolMgr.Instance.RandomRange(_countyData.RemainFood);
        armyNum = ToolMgr.Instance.RandomRange(_countyData.ArmyNum);
        corruptionRate = ToolMgr.Instance.RandomRange(_countyData.CorruptionRate);
        areaFactor = ToolMgr.Instance.RandomRange(_countyData.AreaFactor);
    }

    public void DailyUpdate() {
        remainFood = remainFood - GetPeopleCost() - GetArmyCost();
    }

    public void UpdateDT(float dt) {
        List<BuffObj> list = new List<BuffObj>(buffList);
        for (int index = 0; index < list.Count; index++) {
            list[index].Update(dt);
        }
    }

    private float GetPeopleCost() {
        return peopleNum * GeneralDefine.Instance.peopleFoodCost;
    }

    private float GetArmyCost() {
        return armyNum * GeneralDefine.Instance.stopArmyFoodCost;
    }

    public void HarvestHandle() {
        float curBaseFood = peopleNum * areaFactor;
        float taxFood = curBaseFood * GetTaxRate();
        remainFood = remainFood + curBaseFood - taxFood;
        float createFood = taxFood * (1 - CorruptionRate);
        country.RemainFood += createFood;
        leader.RemainFood += (taxFood - createFood);
    }

    private float GetTaxRate() {
        return country.TaxRate + taxRate;
    }

    public override void AddBuff(ResultType resultType, int value, int durationTime) {
        Debug.LogError("county addbuff");
        switch (resultType) {
            case ResultType.Food:
                RemainFood += value;
                return;//不加入buff
            case ResultType.PeopleNum:
                PeopleNum += value;
                return;//不加入buff
            case ResultType.ArmyNum:
                ArmyNum += value;
                return;//不加入buff
            case ResultType.Loyalty:
                Loyalty += value;
                break;
            case ResultType.CorruptionRate:
                CorruptionRate += value;
                break;
            case ResultType.AreaFactor:
                AreaFactor += value;
                break;
            default:
                Debug.LogError("county resultType is no handle " + resultType);
                return;//不加入buff
        }

        BuffObj buffObj = new BuffObj(this, resultType, value, durationTime);
        buffList.Add(buffObj);
    }

    public override void RemoveBuff(BuffObj buffObj) {
        Debug.LogError("county RemoveBuff  " + buffObj.resultType + "/" + buffObj.value);
        int value = buffObj.value;
        switch (buffObj.resultType) {
            case ResultType.Loyalty:
                Loyalty += value;
                break;
            case ResultType.CorruptionRate:
                CorruptionRate += value;
                break;
            case ResultType.AreaFactor:
                AreaFactor += value;
                break;
            default:
                Debug.LogError("county RemoveBuff is no handle " + buffObj.resultType);
                return;//不加入buff
        }

        buffList.Remove(buffObj);
    }
}
