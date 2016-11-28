using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 国家
/// </summary>
public class Country : BaseData {
    public Actor king;//君主
    public List<County> countyList = new List<County>();//州列表
    public List<Actor> ministerList = new List<Actor>();//臣列表
    public List<Actor> wifeList = new List<Actor>();//妃子列表
    public List<Actor> secretAgentList = new List<Actor>();//密探列表

    private RefCountry refCountry;
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
            Send.SendMsg(SendType.FoodChange);
        }
    }

    public float ArmyRate {
        get {
            return armyRate;
        }
    }

    public Country() {
        roleType = RoleType.Country;
        roleName = RefName.GetRandomName(NameType.Country);
    }

	public void Init(int countryID){
        refCountry = RefCountry.GetRef(countryID);
        //info
        remainFood = refCountry.RemainFood;
        taxRate = refCountry.TaxRate;
        armyRate = refCountry.ArmyRate;

        king = new Actor(RefActor.GetRef(ToolMgr.Instance.RandomRange(refCountry.King)), RefName.GetActorRandomName());
        king.Init(OfficialType.King, RoleType.King);
		//create county
        List<int> countyIDList = new List<int>(RefCounty.cacheMap.Keys);
        List<string> countyNameList = RefName.GetTypeList(NameType.County);
        for (int index = 0; index < refCountry.CountyNum; index++ ) {
            int countyID = ToolMgr.Instance.RandomAndRemove(countyIDList);
            if (countyID == 0)
                break;
            string countyName = ToolMgr.Instance.RandomAndRemove(countyNameList);
            RefCounty refCounty = RefCounty.GetRef(countyID);
            if (refCounty == null)
                continue;
            County county = new County(refCounty, countyName, this);
            countyList.Add(county);
        }

        //create actor
        List<int> actorIDList = new List<int>(RefActor.cacheMap.Keys);
        for (int index = 0; index < refCountry.MinisterNum; index++ ) {
            int ministerID = ToolMgr.Instance.RandomAndRemove(actorIDList);
            if (ministerID == 0)
                break;
            RefActor refActor = RefActor.GetRef(ministerID);
            if (refActor == null)
                continue;
            Actor minister = new Actor(refActor, RefName.GetActorRandomName());
            ministerList.Add(minister);
        }
        ministerList.Sort(SortCachet);

        for (int index = 0; index < ministerList.Count; index++ ) {
            Actor actor = ministerList[index];
            OfficialType official = OfficialType.LPM + index;
            if(official > OfficialType.Officer){
                official = OfficialType.Officer;
            }
            actor.Init(official, RoleType.Minister);
        }

        for (int index = 0; index < refCountry.WifeNum; index++) {
            int wifeID = ToolMgr.Instance.RandomAndRemove(actorIDList);
            if (wifeID == 0)
                break;
            RefActor refActor = RefActor.GetRef(wifeID);
            if (refActor == null)
                continue;
            Actor wife = new Actor(refActor, RefName.GetActorRandomName());
            wifeList.Add(wife);
        }
        wifeList.Sort(SortCachet);
        for (int index = 0; index < wifeList.Count; index++ ) {
            Actor actor = wifeList[index];
            OfficialType official = OfficialType.Queen + index;
            if (official > OfficialType.Hetaera) {
                official = OfficialType.Hetaera;
            }

            actor.Init(official, RoleType.Wife);
        }

        for (int index = 0; index < refCountry.SecretAgentNum; index++) {
            int secretID = ToolMgr.Instance.RandomAndRemove(actorIDList);
            if (secretID == 0)
                break;
            RefActor refActor = RefActor.GetRef(secretID);
            if (refActor == null)
                continue;
            Actor secret = new Actor(refActor, RefName.GetActorRandomName());
            secretAgentList.Add(secret);
        }
        secretAgentList.Sort(SortCachet);
        for (int index = 0; index < secretAgentList.Count; index++ ) {
            Actor actor = secretAgentList[index];
            OfficialType official = OfficialType.Manager + index;
            if (official > OfficialType.Secret) {
                official = OfficialType.Secret;
            }

            actor.Init(official, RoleType.SecretAgent);
        }
	}

    private int SortCachet(Actor a, Actor b) {
        int res = b.cachet.CompareTo(a.cachet);
        return res;
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
    public int PeopleLoyalty() {
        float averLoyalty = 0;
        float totalLoyalty = 0;
        float totalNum = 0;

        for (int index = 0; index < countyList.Count; index++) {
            County county = countyList[index];
            totalLoyalty += county.Loyalty * county.PeopleNum;
            totalNum += county.PeopleNum;
        }

        averLoyalty = totalLoyalty / totalNum;

        return (int)averLoyalty;
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
        ReduceDailyCost();
        CountyUpdate();
        MinisterUpdate();
        ConcubineUpdate();
    }

    public void UpdateDT(float dt) {
        for (int index = 0; index < countyList.Count; index++) {
            County county = countyList[index];
            county.UpdateDT(dt);
        }

        List<Actor> list = new List<Actor>();
        list.AddRange(ministerList);
        list.AddRange(secretAgentList);
        list.AddRange(wifeList);
        for (int index = 0; index < list.Count; index++) {
            Actor actor = list[index];
            actor.UpdateDT(dt);
        }
    }

    private void ReduceDailyCost() {
        RemainFood -= 100;
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
        for (int index = 0; index < wifeList.Count; index++) {
            Actor concubine = wifeList[index];
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
        list.AddRange(wifeList);
        list.AddRange(secretAgentList);
        for (int index = 0; index < list.Count; index++ ) {

        }
    }

    public override void AddBuff(ResultType resultType, int value, int durationTime) {
        switch(resultType){
            case ResultType.Food:
                RemainFood += value;
                break;
            case ResultType.ChooseMinister:
                Debug.LogError("to do choose minister");
                break;
            case ResultType.ChooseWife:
                Debug.LogError("to do choose wife");
                break;
            case ResultType.Meeting:
                Debug.LogError("to do meeting");
                break;
            default:
                Debug.LogError("country resultType is no handle " + resultType);
                break;
        }
    }
}
