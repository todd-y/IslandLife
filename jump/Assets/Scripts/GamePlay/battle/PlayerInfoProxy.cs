using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerInfoProxy : MonoBehaviour {
    private const int skillNum = 4;
    private const int itemNum = 5;
    private const int equipNum = 3;
    private Text txtName;
    private Text txtLv;
    private Slider sldExp;
    private Slider sldHp;
    private Slider sldMp;
    private Text txtExp;
    private Text txtHp;
    private Text txtMp;
    private Text txtAtk;
    private Text txtGold;
    private SkillGrid[] arrSkill;
    private ItemGrid[] arrItem;
    private EquipGrid[] arrEquip;

    private PlayerInfo playerInfo {
        get {
            return BattleMgr.Instance.playerInfo;
        }
    }

	void Start () {
        InitCtrl();
        InitMsg();
        RefreshWindow();
	}

    void Destroy() {
        ClearMsg();
    }

    private void InitCtrl() {
        txtName = gameObject.GetChildControl<Text>("imgNameBg/txtName");
        txtLv = gameObject.GetChildControl<Text>("Info/imgExpBg/txtLv");
        sldExp = gameObject.GetChildControl<Slider>("Info/imgExpBg/SldExp");
        txtExp = gameObject.GetChildControl<Text>("Info/imgExpBg/txtValue");
        sldHp = gameObject.GetChildControl<Slider>("Info/imgHpBg/SldHp");
        txtHp = gameObject.GetChildControl<Text>("Info/imgHpBg/txtValue");
        sldMp = gameObject.GetChildControl<Slider>("Info/imgMpBg/SldMp");
        txtMp = gameObject.GetChildControl<Text>("Info/imgMpBg/txtValue");
        txtAtk = gameObject.GetChildControl<Text>("Attribute/imgAtkBg/txtValue");
        txtGold = gameObject.GetChildControl<Text>("Attribute/imgGoldBg/txtValue");

        arrSkill = new SkillGrid[skillNum];
        for (int i = 1; i <= skillNum; i++ ) {
            SkillGrid skillGrid = gameObject.GetChildControl<Transform>("Skill/ImgSkillBg" + i).gameObject.AddMissingComponent<SkillGrid>();
            arrSkill[i - 1] = skillGrid;
            skillGrid.Init(i - 1);
        }

        arrItem = new ItemGrid[itemNum];
        for (int i = 1; i <= itemNum; i++) {
            ItemGrid itemGrid = gameObject.GetChildControl<Transform>("Item/ImgItemBg" + i).gameObject.AddMissingComponent<ItemGrid>();
            arrItem[i - 1] = itemGrid;
        }

        arrEquip = new EquipGrid[equipNum];
        for (int i = 1; i <= equipNum; i++) {
            EquipGrid equipGrid = gameObject.GetChildControl<Transform>("Equip/imgEquipBg" + i).gameObject.AddMissingComponent<EquipGrid>();
            arrEquip[i - 1] = equipGrid;
        }
    }

    private void InitMsg() {
        Send.RegisterMsg(SendType.LvChange, OnLvChange);
        Send.RegisterMsg(SendType.ExpChange, OnExpChange);
        Send.RegisterMsg(SendType.HpChange, OnHpChange);
        Send.RegisterMsg(SendType.MpChange, OnMpChange);
        Send.RegisterMsg(SendType.AtkChange, OnAtkChange);
        Send.RegisterMsg(SendType.GoldChange, OnGoldChange);
        Send.RegisterMsg(SendType.SkillChange, OnSkillChange);
        Send.RegisterMsg(SendType.ItemChange, OnItemChange);
        Send.RegisterMsg(SendType.EquipChange, OnEquipChange);
    }

    private void ClearMsg() {
        Send.UnregisterMsg(SendType.LvChange, OnLvChange);
        Send.UnregisterMsg(SendType.ExpChange, OnExpChange);
        Send.UnregisterMsg(SendType.HpChange, OnHpChange);
        Send.UnregisterMsg(SendType.MpChange, OnMpChange);
        Send.UnregisterMsg(SendType.AtkChange, OnAtkChange);
        Send.UnregisterMsg(SendType.GoldChange, OnGoldChange);
        Send.UnregisterMsg(SendType.SkillChange, OnSkillChange);
        Send.UnregisterMsg(SendType.ItemChange, OnItemChange);
        Send.UnregisterMsg(SendType.EquipChange, OnEquipChange);
    }

    private void RefreshWindow() {
        OnLvChange();
        OnExpChange();
        OnHpChange();
        OnMpChange();
        OnAtkChange();
        OnGoldChange();
        OnSkillChange();
        OnItemChange();
        OnEquipChange();
    }
    
    private void OnLvChange(params object[] objs){
        txtLv.SetText(playerInfo.Lv);
    }
    private void OnExpChange(params object[] objs) {
        txtExp.SetText(playerInfo.Exp + "/" + playerInfo.MaxExp);
        sldExp.value = (float)playerInfo.Exp / playerInfo.MaxExp;
    }
    private void OnHpChange(params object[] objs) {
        txtHp.SetText(playerInfo.Hp + "/" + playerInfo.MaxHp);
        sldHp.value = (float)playerInfo.Hp / playerInfo.MaxHp;
    }
    private void OnMpChange(params object[] objs) {
        txtMp.SetText(playerInfo.Mp + "/" + playerInfo.MaxMp);
        sldMp.value = (float)playerInfo.Mp / playerInfo.MaxMp;
    }
    private void OnAtkChange(params object[] objs) {
        txtAtk.SetText(playerInfo.Atk);
    }
    private void OnGoldChange(params object[] objs) {
        txtGold.SetText(playerInfo.Gold);
    }
    private void OnSkillChange(params object[] objs) {
        int length = Mathf.Min( playerInfo.skillList.Count, skillNum);
        for (int i = 0; i < length; i++ ) {
            arrSkill[i].SetData(playerInfo.skillList[i]);
        }

        for (int i = length; i < skillNum; i++ ) {
            arrSkill[i].Clear();
        }
    }
    private void OnItemChange(params object[] objs) {
        //int length = Mathf.Min(playerInfo.itemList.Count, itemNum);
        //for (int i = 0; i < length; i++) {
        //    arrItem[i].SetData(playerInfo.itemList[i]);
        //}

        //for (int i = length; i < itemNum; i++) {
        //    arrItem[i].Clear();
        //}
    }
    private void OnEquipChange(params object[] objs) {

    }
}
