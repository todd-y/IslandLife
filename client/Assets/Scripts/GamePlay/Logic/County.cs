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
    }

    public float PeopleLoyalty {
        get {
            return loyalty;
        }
    }

    public float ArmyNum {
        get {
            return armyNum;
        }
    }

    public float CorruptionRate {
        get{
            return corruptionRate;
        }
    }

    public County() {
        roleType = RoleType.County;
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
