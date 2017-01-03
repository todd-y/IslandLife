using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EquipGrid : MonoBehaviour {
    private int index;
    private EquipData equipData;
    private Text txtName;
    private Text txtValue;
    private Image imgIcon;
    private Image imgUnknow;

    private void InitCtrl() {
        txtName = gameObject.GetChildControl<Text>("imgEquip/txtName");
        txtValue = gameObject.GetChildControl<Text>("imgEquip/txtValue");
        imgIcon = gameObject.GetChildControl<Image>("imgEquip");
        imgUnknow = gameObject.GetChildControl<Image>("imgBg");
    }

    public void Init(int _index) {
        index = _index;
        InitCtrl();

        Clear();
    }

    public void Clear() {
        imgIcon.gameObject.SetActive(false);
        imgUnknow.gameObject.SetActive(true);
    }

    public void SetData(EquipData _equipData) {
        equipData = _equipData;
        if (equipData == null) {
            Clear();
            return;
        }

        imgIcon.gameObject.SetActive(true);
        imgUnknow.gameObject.SetActive(false);
        txtName.SetText(equipData.GetName());
        txtValue.SetText(equipData.GetDesc());
        txtName.color = equipData.GetColor();
        txtValue.color = equipData.GetColor();
    }
}

public class EquipData{
    public RefEquip data;

    public EquipData( int id) {
        data = RefEquip.GetRef(id);
    }

    public string GetName() {
        return RefLanguage.GetValue(data.quality.ToString()) + RefLanguage.GetValue(data.type.ToString());
    }

    public Color GetColor() {
        Color color = Color.white;
        switch(data.quality){
            case Quality.White:
                color = new Color(210f / 255f, 210f / 255f, 212f/ 255f);
                break;
            case Quality.Green:
                color = new Color(0, 214f / 255f, 0);
                break;
            case Quality.Blue:
                color = new Color(0, 154f / 255f, 246f / 255f);
                break;
            case Quality.Purple:
                color = new Color(173f / 255f, 0, 234f / 255f);
                break;
            case Quality.Orange:
                color = new Color(255f / 255f, 163f / 255f, 0);
                break;
            default:
                Debug.LogError("not handle:" + data.quality);
                break;
        }

        return color;
    }

    public string GetDesc() {
        string desc = "";
        int length = Mathf.Min(data.attributeList.Count, data.valueList.Count);
        for (int index = 0; index < length; index++ ) {
            Attribute attribute = data.attributeList[index];
            int value = data.valueList[index];
            desc += RefLanguage.GetValue(attribute.ToString()) + "+" + value + " ";
        }

        return desc;
    }
}
