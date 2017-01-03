using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillGrid : MonoBehaviour {
    private int index;
    private int skillId;
    private Text txtKey;
    private Text txtCost;
    private Image imgIcon;
    private Image imgUnknow;

    private void InitCtrl() {
        txtKey = gameObject.GetChildControl<Text>("ImgIcon/txtKey");
        txtCost = gameObject.GetChildControl<Text>("ImgIcon/txtCost");
        imgIcon = gameObject.GetChildControl<Image>("ImgIcon");
        imgUnknow = gameObject.GetChildControl<Image>("imgUnknow");
    }

	public void Init(int _index){
        index = _index;
        InitCtrl();
        //to do key set

        Clear();
    }

    public void Clear() {
        imgIcon.gameObject.SetActive(false);
        imgUnknow.gameObject.SetActive(true);
    }

    public void SetData(int _skillId) {
        skillId = _skillId;
        if (skillId == 0) {
            Clear();
            return;
        }

        imgIcon.gameObject.SetActive(true);
        imgUnknow.gameObject.SetActive(false);

        RefSkill refSkill = RefSkill.GetRef(skillId);
        RefIcon.SetSprite(imgIcon, refSkill.Icon);
        txtCost.SetText(refSkill.Cost);
    }
}
