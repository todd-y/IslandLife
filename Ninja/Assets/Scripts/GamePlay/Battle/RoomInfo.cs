using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DunGen;

public class RoomInfo : MonoBehaviour {
    public GameObject playerPos;
    public GameObject enemyPos;
    [HideInInspector]
    public List<GameObject> enemyPosList = new List<GameObject>();
    [HideInInspector]
    public RoomType roomType;
    private Tile tile;

    private List<GameObject> doorList = new List<GameObject>();

    void Awake() {
        enemyPosList.Clear();
        Transform [] enemyTransform = enemyPos.GetComponentsInChildren<Transform>();
        for (int index = 0; index < enemyTransform.Length; index++ ) {
            if (enemyPos.transform == enemyTransform[index])
                continue;

            enemyPosList.Add(enemyTransform[index].gameObject);
        }
    }

    void Start() {
        tile = gameObject.GetComponent<Tile>();
        tile.roomInfo = this;
    }

    private void GetDoorList() {
        if (doorList.Count == 0) {
            for (int index = 0; index < tile.Placement.UsedDoorways.Count; index++) {
                Doorway doorway = tile.Placement.UsedDoorways[index];
                GameObject doorGo = doorway.gameObject.GetChildControl<Transform>("GO/Door").gameObject;
                doorList.Add(doorGo);
            }
        }
    }

    public void OpenDoor() {
        GetDoorList();
        for (int index = 0; index < doorList.Count; index++) {
            doorList[index].SetActive(false);
        }
    }

    public void CloseDoor() {
        GetDoorList();
        for (int index = 0; index < doorList.Count; index++) {
            doorList[index].SetActive(true);
        }
    }

    public List<Doorway> GetUseDoorwayList() {
        return tile.Placement.UsedDoorways;
    }
}
