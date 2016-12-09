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
    [HideInInspector]
    public Tile tile;

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

    private void GetDoorList() {
        if (doorList.Count == 0) {
            for (int index = 0; index < tile.Placement.UsedDoorways.Count; index++) {
                Doorway doorway = tile.Placement.UsedDoorways[index];
                GameObject doorGo = doorway.gameObject.GetChildControl<Transform>("Door/Door").gameObject;
                doorList.Add(doorGo);
            }
        }
    }

    public void OpenDoor() {
        GetDoorList();
        Debug.LogError("open");
        for (int index = 0; index < doorList.Count; index++) {
            Debug.LogError(doorList[index].name);
            doorList[index].SetActive(false);
        }
    }

    public void CloseDoor() {
        GetDoorList();
        Debug.LogError("close");
        for (int index = 0; index < doorList.Count; index++) {
            doorList[index].SetActive(true);
        }
    }
}
