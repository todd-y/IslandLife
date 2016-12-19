using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DunGen;

public class MiniMapProxy : MonoBehaviour {

    public RectTransform gridPrefab;
    private List<MiniMapGrid> gridList = new List<MiniMapGrid>();

    public void RefreshMap() {
        float roomWidth = GeneralDefine.Instance.roomSizeWidth;
        float roomHeight = GeneralDefine.Instance.roomSizeHeight;
        float gridWidth = gridPrefab.sizeDelta.x;
        float gridHeight = gridPrefab.sizeDelta.y;

        gridList.Clear();
        int length = BattleMgr.Instance.roomList.Count;
        for (int index = 0; index < length; index++) {
            RoomInfo info = BattleMgr.Instance.roomList[index];
            GameObject go = GameObject.Instantiate(gridPrefab.gameObject);
            go.SetActive(true);
            go.transform.SetParent(transform, false);
            MiniMapGrid grid = go.GetComponent<MiniMapGrid>();
            grid.roomInfo = info;
            gridList.Add(grid);
            float posX = info.transform.localPosition.x / roomWidth * gridWidth;
            float posY = info.transform.localPosition.y / roomHeight * gridHeight;
            go.transform.localPosition = new Vector3(posX, posY, 0);
        }
    }

    public void EnterRoom(RoomInfo _roomInfo) {
        List<Doorway> doorwayList = _roomInfo.GetUseDoorwayList();
        for (int index = 0; index < gridList.Count; index++) {
            MiniMapGrid grid = gridList[index];
            if (grid.roomInfo == _roomInfo) {
                grid.SetState(MiniMapGrid.State.InRoom);
                transform.localPosition = new Vector3(-grid.transform.localPosition.x, -grid.transform.localPosition.y, 0);
                continue;
            }

            bool isConnect = false;
            for (int k = 0; k < doorwayList.Count; k++) {
                Doorway doorway = doorwayList[k];
                if (grid.roomInfo == doorway.ConnectedDoorway.Tile.GetComponent<RoomInfo>()) {
                    grid.SetState(MiniMapGrid.State.Show);
                    isConnect = true;
                    break;
                }
            }

            if (isConnect)
                continue;

            grid.SetState(MiniMapGrid.State.Hide);
        }
    }

    public void Clear() {
        for (int index = 0; index < gridList.Count; index++ ) {
            MiniMapGrid grid = gridList[index];
            GameObject.Destroy(grid.gameObject);
        }

        gridList.Clear();
    }
}
