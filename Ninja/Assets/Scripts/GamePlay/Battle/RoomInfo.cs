using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DunGen;

public class RoomInfo : MonoBehaviour {
    public GoalType goalType = GoalType.Collect;
    public RoomState roomState = RoomState.Wait;
    
    [HideInInspector]
    public List<GameObject> enemyPosList = new List<GameObject>();
    [HideInInspector]
    public RoomType roomType;
    private Tile tile;
    private Draw draw;
    private List<GameObject> doorList = new List<GameObject>();

    private float m_progress = 0;
    public float Progress {
        get {
            return m_progress;
        }
        set {
            m_progress = Mathf.Clamp(value, 0f, 1f);
            if (m_progress == 1f) {
                roomState = RoomState.Complete;
            }
            Send.SendMsg(SendType.RoomProgressChange, m_progress);
        }
    }

    void Awake() {
        GameObject enemyGo = gameObject.GetChildControl<Transform>("EnemyPos").gameObject;
        enemyPosList.Clear();
        Transform[] enemyTransform = enemyGo.GetComponentsInChildren<Transform>();
        for (int index = 0; index < enemyTransform.Length; index++ ) {
            if (enemyGo.transform == enemyTransform[index])
                continue;

            enemyPosList.Add(enemyTransform[index].gameObject);
        }
    }

    void Start() {
        tile = gameObject.GetComponent<Tile>();
        tile.roomInfo = this;

        draw = gameObject.GetChildControl<Draw>("Draw");
        CheckGoalType();
    }

    public void CheckGoalType() {
        switch(goalType){
            case GoalType.Collect:
            case GoalType.Time:
                Progress = 0;
                break;
            default:
                Progress = 1;
                break;
        }
    }

    public void SetGoalType(GoalType _goalType) {
        goalType = _goalType;
        CheckGoalType();
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

    public void Draw(Vector3 pos, string name, bool randomPos = true) {
        pos = pos - transform.position;
        if (randomPos) {
            pos = pos + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
        }
        draw.DrawTexture(pos, name);
    }

    public int CreatAirNum() {
        return Random.Range(1, 4);
    }

    public float CreatAirDelay() {
        return Random.Range(2f, 4f);
    }
}
