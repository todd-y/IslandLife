using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemGrid : MonoBehaviour {
    private int index;
    private ItemData itemData;
    private Text txtKey;
    private Text txtNum;
    private Image imgIcon;
    private Image imgUnknow;

    private void InitCtrl() {
        txtKey = gameObject.GetChildControl<Text>("ImgIcon/txtKey");
        txtNum = gameObject.GetChildControl<Text>("ImgIcon/txtValue");
        imgIcon = gameObject.GetChildControl<Image>("ImgIcon");
        imgUnknow = gameObject.GetChildControl<Image>("imgUnknow");
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

    public void SetData(ItemData _itemData) {
        itemData = _itemData;
        if (itemData == null) {
            Clear();
            return;
        }

        imgIcon.gameObject.SetActive(true);
        imgUnknow.gameObject.SetActive(false);

        RefIcon.SetSprite(imgIcon, itemData.data.Icon);
        txtNum.SetText(itemData.Num);
    }
}

public class ItemData {
    public RefItem data;
    private int m_num;
    public int Num {
        get { return m_num; }
        set { m_num = Mathf.Min(value, data.MaxNum); }
    }

    public void Init(int _id, int _num) {
        data = RefItem.GetRef(_id);
        Num = _num;
    }

    public int remainNum() {
        return data.MaxNum - Num;
    }
}
