using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemProxy : MonoBehaviour {
    public RefItem data;
    private Image imgIcon;

    public void Init() {
        imgIcon = gameObject.GetComponent<Image>();
        data = RefItem.GetRandomItem();

        RefIcon.SetSprite(imgIcon, data.Icon);
    }

    public void GetItem() {
        BattleMgr.Instance.makeInfo.AddItem(data.Id);
        RoomCreatMgr.Instance.RemoveGameObject(gameObject);
    }
}
