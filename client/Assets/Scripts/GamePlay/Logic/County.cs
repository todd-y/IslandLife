using UnityEngine;
using System.Collections;

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
    }

    public int RemainFood {
        get {
            return (int)remainFood;
        }
    }

    public int AreaFactor {
        get {
            return (int)areaFactor;
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
}
