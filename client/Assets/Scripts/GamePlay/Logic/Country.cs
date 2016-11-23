using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 国家
/// </summary>
public class Country : BaseData {
    public Actor king = new Actor();//君主
    public List<County> countyList = new List<County>();//州列表
    public List<Actor> ministerList = new List<Actor>();//臣列表
    public List<Actor> concubineList = new List<Actor>();//妃子列表
    public List<Actor> secretAgent = new List<Actor>();//密探列表

    private float remainFood;   //余粮
    private float taxRate;      //税收
    private float armyRate;     //征兵率

    public float TaxRate {
        get {
            return taxRate;
        }
    }

    public float RemainFood {
        get {
            return remainFood;
        }
        set {
            remainFood = value;
        }
    }

    public float ArmyRate {
        get {
            return armyRate;
        }
    }

    public Country() {
        roleType = RoleType.Country;
        roleName = "Country";
        king.roleType = RoleType.King;
        king.roleName = "King";
    }

	public void Init(){
		
	}
	
	public void Clear(){
		
	}

    /// <summary>
    /// 总人口
    /// </summary>
    public float PeopleNum() {
        float totalNum = 0;
        for (int index = 0; index < countyList.Count; index++) {
            County county = countyList[index];
            totalNum += county.PeopleNum;
        }

        return totalNum;
    }

    /// <summary>
    /// 人民忠诚
    /// </summary>
    public float PeopleLoyalty() {
        float averLoyalty = 0;
        float totalLoyalty = 0;
        float totalNum = 0;

        for (int index = 0; index < countyList.Count; index++) {
            County county = countyList[index];
            totalLoyalty += county.PeopleLoyalty * county.PeopleNum;
            totalNum += county.PeopleLoyalty;
        }

        averLoyalty = totalLoyalty / totalNum;

        return averLoyalty;
    }
    
    /// <summary>
    /// 总军队
    /// </summary>
    public float ArmyNum() {
        float totalNum = 0;
        for (int index = 0; index < countyList.Count; index++) {
            County county = countyList[index];
            totalNum += county.ArmyNum;
        }

        return totalNum;
    }

    /// <summary>
    /// 军队忠诚
    /// </summary>
    public float ArmyLoyalty() {
        // to do
        return 100f;
    }

    /// <summary>
    /// 日常处理
    /// </summary>
    public void DailyUpdate() {
        CountyUpdate();
        MinisterUpdate();
        ConcubineUpdate();
    }

    private void CountyUpdate() {
        for (int index = 0; index < countyList.Count; index++) {
            County county = countyList[index];
            county.DailyUpdate();
        }
    }

    private void MinisterUpdate() {
        for (int index = 0; index < ministerList.Count; index++) {
            Actor minister = ministerList[index];
            minister.DailyUpdate();
        }
    }

    private void ConcubineUpdate() {
        for (int index = 0; index < concubineList.Count; index++) {
            Actor concubine = concubineList[index];
            concubine.DailyUpdate();
        }
    }

    public void HarvestHandle() {
        for (int index = 0; index < countyList.Count; index++) {
            County county = countyList[index];
            county.HarvestHandle();
        }
    }

    public void MonthFoodCost() {
        List<Actor> list = new List<Actor>();
        list.AddRange(ministerList);
        list.AddRange(concubineList);
        for (int index = 0; index < list.Count; index++ ) {

        }
    }
}
