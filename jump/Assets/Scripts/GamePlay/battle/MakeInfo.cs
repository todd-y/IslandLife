using UnityEngine;
using System.Collections;

public class MakeInfo {
    public int alchemyNum = 3;
    public MakeData[] arrAlchemy;

    public void SetData() {
        arrAlchemy = new MakeData[alchemyNum];
        arrAlchemy[0] = new MakeData(MakeType.Low, true);
        arrAlchemy[1] = new MakeData(MakeType.Mid, true);
        arrAlchemy[2] = new MakeData(MakeType.High, true);
    }

    public void AddItem(int _itemId, int _num) {
        RefItem refItem = RefItem.GetRef(_itemId);
        if (refItem == null)
            return;
        switch(refItem.Type){
            case ItemType.LowMaterial:
                arrAlchemy[0].AddItem(_itemId, _num);
                break;
            case ItemType.MidMaterial:
                arrAlchemy[1].AddItem(_itemId, _num);
                break;
            case ItemType.HighMaterial:
                arrAlchemy[2].AddItem(_itemId, _num);
                break;
            default:
                Debug.LogError("not handle :" + refItem.Type);
                break;
        }

        Send.SendMsg(SendType.MakeChange, arrAlchemy);
    }
}
