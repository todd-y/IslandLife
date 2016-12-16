using UnityEngine;
using System.Collections;
using DunGen;
using System.Collections.Generic;

public class BattleMgr : Singleton<BattleMgr> {

    public Dungeon curDungeon;
    public RoomInfo curRoom;
    public CameraCtrl curCameraCtrl;

    private Player player;
    private List<RoomInfo> roomList = new List<RoomInfo>();
    private Dictionary<RoomInfo, List<Enemy>> enemyDic = new Dictionary<RoomInfo, List<Enemy>>();
    private Dictionary<string, GameObject> cachePrefabList = new Dictionary<string, GameObject>();

	public void Init(){
		Send.RegisterMsg(SendType.GenerationStateChange, OnGenerationStateChange);
        Send.RegisterMsg(SendType.MonsterDead, OnMonsterDead);
        Send.RegisterMsg(SendType.Transfer, OnTransfer);
	}
	
	public void Clear(){
		Send.UnregisterMsg(SendType.GenerationStateChange, OnGenerationStateChange);
        Send.UnregisterMsg(SendType.MonsterDead, OnMonsterDead);
        Send.UnregisterMsg(SendType.Transfer, OnTransfer);
	}

    public void StartBattle() {
        enemyDic.Clear();
        roomList.Clear();
        cachePrefabList.Clear();

        LocalAssetMgr.Instance.Load_Scene("Battle");
    }

    private void OnGenerationStateChange(object[] objs) {
        GenerationStatus status = (GenerationStatus)objs[0];
        if (status == GenerationStatus.Complete) {
            GenerationComplete();
        }
    }

    private void OnMonsterDead(object[] objs) {
        Enemy enemy = (Enemy)objs[0];
        if (enemy.roomInfo == null) {
            Debug.LogError("enemy roominfo is null");
            return;
        }
        enemyDic[enemy.roomInfo].Remove(enemy);
        CheckRoom(enemy.roomInfo);
    }

    private void OnTransfer(object[] objs) {
        Doorway doorWay = (Doorway)objs[0];
        EnterRoom(doorWay.ConnectedDoorway.Tile.roomInfo, doorWay.ConnectedDoorway.TransferPos.transform.position);
    }

    private void GenerationComplete() {
        if (curDungeon == null) {
            Debug.LogError("curdungeon is null");
        }
        WindowMgr.Instance.OpenWindow<BattleWindow>();
        CreatPlayer();
        CteatEnemy();
        EnterRoom(roomList[0], roomList[0].playerPos.transform.position);
    }

    private void CreatPlayer() {
        Tile firstTile = curDungeon.MainPathTiles[0];
        RoomInfo roomInfo = firstTile.GetComponent<RoomInfo>();
        GameObject playerGo = UbhObjectPool.Instance.GetGameObject(LocalAssetMgr.Instance.Load_Prefab("PlayerMage"), 
                                    roomInfo.playerPos.transform.position, roomInfo.playerPos.transform.rotation);
        player = playerGo.GetComponent<Player>();
    }

    private void CteatEnemy() {
        string prefabName = "EnemySkeleton";
        for (int index = 0; index < curDungeon.AllTiles.Count; index++ ) {
            Tile tile = curDungeon.AllTiles[index];
            RoomInfo roomInfo = tile.GetComponent<RoomInfo>();
            roomList.Add(roomInfo);

            List<Enemy> enemyList = new List<Enemy>();
            enemyDic.Add(roomInfo, enemyList);

            if (tile.Placement.IsOnMainPath && tile.Placement.NormalizedDepth == 0) {
                roomInfo.roomType = RoomType.Start;
            }
            else if (tile.Placement.IsOnMainPath && tile.Placement.NormalizedDepth == 1) {
                roomInfo.roomType = RoomType.End;
            }
            else {
                roomInfo.roomType = RoomType.Other;
            }

            //初始关卡不设怪
            if (index == 0) {
                continue;
            }

            for (int k = 0; k < roomInfo.enemyPosList.Count; k++) {
                Vector3 creatPos = roomInfo.enemyPosList[k].transform.position;
                Quaternion creatRotation = roomInfo.enemyPosList[k].transform.rotation;
                GameObject prefab = null;
                if (cachePrefabList.ContainsKey(prefabName)) {
                    prefab = cachePrefabList[prefabName];
                }
                else {
                    prefab = LocalAssetMgr.Instance.Load_Prefab(prefabName);
                    cachePrefabList.Add(prefabName, prefab);
                }

                GameObject enemyGo = UbhObjectPool.Instance.GetGameObject(prefab, creatPos, creatRotation);
                Enemy enemyProxy = enemyGo.GetComponent<Enemy>();
                if (enemyProxy == null) {
                    Debug.LogError("enemy proxy is null");
                    continue;
                }
                enemyProxy.roomInfo = roomInfo;
                enemyList.Add(enemyProxy);
            }
        }
    }

    public void EnterRoom(RoomInfo roomInfo, Vector3 newPos) {
        CheckRoom(roomInfo);
        curRoom = roomInfo;
        player.transform.position = newPos;

        curCameraCtrl.SetPos(roomInfo.transform.position, () => AwakeMonster(roomInfo));
    }


    public void AwakeMonster(RoomInfo roomInfo) {
        List<Enemy> list = enemyDic[roomInfo];
        for (int index = 0; index < list.Count; index++ ) {
            list[index].ai.IsAwake(true);
        }
    }

    public void CheckRoom(RoomInfo roomInfo) {
        List<Enemy> list = enemyDic[roomInfo];
        if (list.Count == 0) {
            roomInfo.OpenDoor();
            if (roomInfo.roomType == RoomType.End) {
                GameObject endGo = GameObject.Instantiate( LocalAssetMgr.Instance.Load_Prefab("EndGame") );
                endGo.transform.SetParent(roomInfo.transform, false);
            }
        }
        else {
            roomInfo.CloseDoor();
        }
    }
}
