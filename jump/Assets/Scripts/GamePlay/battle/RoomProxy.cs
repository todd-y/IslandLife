﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomProxy : MonoBehaviour {

    private Transform gridParent;

    private GridType[][] arrGridData;

    private List<GameObject> goList = new List<GameObject>();

    public void Init() {
        gridParent = gameObject.GetChildControl<Transform>("CvsGrid");
    }

    public void SetData(GridType[][] _arrGridData) {
        ClearAllGo();
        arrGridData = _arrGridData;
        CreatRoom();
    }

    public void CreatRoom() {
        for (int x = 0; x < arrGridData.Length; x++) {
            GridType[] arrLineData = arrGridData[x];
            for (int y = 0; y < arrLineData.Length; y++ ) {
                GridType gridType = arrLineData[y];
                CreatOneGrid(x, y, gridType);
            }
        }
    }

    private void CreatOneGrid(int x, int y, GridType gridType) {
        if (gridType == GridType.Empty)
            return;

        GameObject gridGo = RoomCreatMgr.Instance.GetGameObject(gridType.ToString());
        if (gridGo == null)
            return;

        int size = RoomCreatMgr.size;
        int halfWidth = RoomCreatMgr.width / 2;
        int halfHeight = RoomCreatMgr.height / 2;
        gridGo.transform.SetParent(gridParent, false);
        gridGo.transform.localPosition = new Vector3(-halfWidth + x * size + size / 2, -halfHeight + y * size + size / 2, 0);
        goList.Add(gridGo);
    }

    public void ClearAllGo() {
        for (int index = 0; index < goList.Count; index++ ) {
            RoomCreatMgr.Instance.ReleaseGameObject(goList[index]);
        }

        goList.Clear();
    }

}
