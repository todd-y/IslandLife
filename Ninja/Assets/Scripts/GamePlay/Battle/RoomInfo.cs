using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomInfo : MonoBehaviour {
    public GameObject playerPos;
    public GameObject enemyPos;
    public GameObject door;
    [HideInInspector]
    public List<GameObject> enemyPosList = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> doorList = new List<GameObject>();

    void Awake() {
        enemyPosList.Clear();
        doorList.Clear();
        Transform [] enemyTransform = enemyPos.GetComponentsInChildren<Transform>();
        for (int index = 0; index < enemyTransform.Length; index++ ) {
            enemyPosList.Add(enemyTransform[index].gameObject);
        }

        if (door == null)
            return;

        Transform[] doorTransform = door.GetComponentsInChildren<Transform>();
        for (int index = 0; index < doorTransform.Length; index++) {
            doorList.Add(doorTransform[index].gameObject);
        }
    }
}
