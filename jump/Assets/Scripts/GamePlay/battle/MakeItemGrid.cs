using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MakeItemGrid : MonoBehaviour {
    private int index = 0;
    private Image imgLock;
    private Image imgUnLock;
    private Image imgUnknow;
    private Image imgIcon;
    private Text txtNum;

    private void InitCtrl() {
        imgLock = gameObject.GetChildControl<Image>("lockBg");
        imgUnLock = gameObject.GetChildControl<Image>("bg");
        imgUnknow = gameObject.GetChildControl<Image>("bg/imgEmpty");
        imgIcon = gameObject.GetChildControl<Image>("bg/imgIcon");
        txtNum = gameObject.GetChildControl<Text>("bg/imgIcon/txtNum");
    }

    public void Init(int _index) {
        index = _index;
        InitCtrl();
    }

    public void Clear() {
        imgLock.gameObject.SetActive(true);
        imgUnLock.gameObject.SetActive(false);
    }

    public void SetData(MakeData data) {
        if (data == null) {
            Clear();
            return;
        }

        imgLock.gameObject.SetActive(false);
        imgUnLock.gameObject.SetActive(true);

        int curNum = data.itemIdList.Count;
        imgUnknow.gameObject.SetActive(curNum == 0);
        imgIcon.gameObject.SetActive(curNum != 0);
        if (curNum > 0) {
            RefIcon.SetItemSprite(imgIcon, data.itemIdList[0]);
        }
        txtNum.SetText(curNum + "/" + data.needNum);
    }
}

public class MakeData {
    public int needNum = 10;
    public MakeType type;
    public bool unLock;
    public List<int> itemIdList = new List<int>();

    public MakeData(MakeType _type, bool _unLock) {
        type = _type;
        unLock = _unLock;
    }

    public void AddItem(int itemId, int num) {
        if (unLock == false) {
            Debug.LogError("make is lock");
            return;
        }
        int overNum = 0;
        if (remainNum() < num) {
            overNum = num - remainNum();
        }

        for (int index = 0; index < num; index++) {
            itemIdList.Add(itemId);
        }

        if (remainNum() <= 0) {
            ProduceResult();
        }
        if (overNum > 0) {
            AddItem(itemId, overNum);
        }
    }

    public void ProduceResult() {
        //to do result
        itemIdList.Clear();
        Send.SendMsg(SendType.MakeEnough, this);
    }

    public int remainNum() {
        return needNum - itemIdList.Count;
    }
}
