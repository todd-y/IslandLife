using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomCreatMgr : Singleton<RoomCreatMgr> {
    public const int width = 650;
    public const int height = 1100;
    public const int size = 50;
    private GameObject poolGo;
    private int xCount;
    private int yCount;
    private GridType[][] arrCurData;
    private GridType[][] arrRemainData;
    private int creatY = 0;
    private RoomProxy waitRoomProxy;

    private Dictionary<string, GameObject> dicPrefab = new Dictionary<string, GameObject>();
    private Dictionary<string, List<GameObject>> dicPool = new Dictionary<string, List<GameObject>>();

    public void Init() {
        poolGo = Launch.Instance.poolGo;
        xCount = width / size;
        yCount = height / size;
        arrCurData = new GridType[0][];
        arrRemainData = new GridType[0][];
    }

    public void Clear() {

    }

    public void CreatNewRoom(RoomProxy roomProxy) {
        waitRoomProxy = roomProxy;
        waitRoomProxy.ClearAllGo();
        arrCurData = GetEmptyData();
        AddData(0, arrRemainData);
        ClearRemainData();
        
        while(creatY < yCount){
            GridType[][] arrData = GetLineRandom();
            AddData(creatY, arrData);
            creatY += arrData.Length;
        }

        creatY -= yCount;
        waitRoomProxy.SetData(arrCurData);
    }

    private void AddData(int curY, GridType[][] arrData) {
        for (int y = 0; y < arrData.Length; y++ ) {
            GridType[] arrLine = arrData[y];
            if (arrLine == null)
                break;
            curY++;
            if(curY >= yCount * 2){
                Debug.LogError("data is error:" + arrData + "/" + arrData.Length);
                break;
            }
            else if (curY >= yCount) {
                arrRemainData[curY - yCount] = arrLine;
            }
            else {
                for (int x = 0; x < arrLine.Length; x++) {
                    if (arrLine[x] != null && arrLine[x] != GridType.Empty) {
                        arrCurData[curY][x] = arrLine[x];
                    }
                }
            }
        }
    }

    private GridType[][] GetLineRandom(int xMin = 0, int xMax = -1, int numMin = 2, int numMax = 4) {
        if (xMax == -1) {
            xMax = xCount;
        }
        GridType[][] arrLine = new GridType[1][];
        int num = Random.Range(numMin, numMax);
        int startX = Random.Range(xMin, xMax - num + 1);
        arrLine[0] = new GridType[yCount];
        for (int x = 0; x < num; x++) {
            arrLine[0][startX + x] = GridType.Wall;
        }

        return arrLine;
    }

    private GridType[][] GetEmptyData() {
        GridType[][] arrData = new GridType[yCount][];
        for (int y = 0; y < yCount; y++) {
            GridType[] arrLine = new GridType[xCount];
            arrData[y] = arrLine;
            for (int x = 0; x < xCount; x++) {
                arrData[y][x] = GridType.Empty;
            }
        }

        return arrData;
    }

    private void ClearRemainData(){
        GridType[][] arrData = new GridType[yCount][];
        for (int y = 0; y < yCount; y++) {
            arrData[y] = null;
        }

        arrRemainData = arrData;
    }

    public GameObject GetGameObject(string prefabName) {
        List<GameObject> goList;
        GameObject go = null;
        if (dicPool.ContainsKey(prefabName)) {
            goList = dicPool[prefabName];
        }
        else {
            goList = new List<GameObject>();
            dicPool.Add(prefabName, goList);
        }

        for (int index = 0; index < goList.Count; index++) {
            if (goList[index].activeSelf == true) {
                continue;
            }
            go = goList[index];
            go.SetActive(true);
            break;
        }

        if (go == null) {
            GameObject prefab = GetPrefab(prefabName);
            if (prefab == null)
                return go;

            go = GameObject.Instantiate(prefab);
            go.transform.SetParent(poolGo.transform, false);
            goList.Add(go);
        }

        return go;
    }

    private GameObject GetPrefab(string prefabName) {
        GameObject prefab;
        if (dicPrefab.ContainsKey(prefabName)) {
            prefab = dicPrefab[prefabName];
        }
        else {
            prefab = LocalAssetMgr.Instance.Load_Prefab(prefabName);
        }
        if (prefab == null) {
            Debug.LogError("prefab is null : " + prefabName);
            return null;
        }

        return prefab;
    }

    public void ReleaseGameObject(GameObject go) {
        if (go == null) {
            Debug.LogError("release is null");
            return;
        }
        go.SetActive(false);
        go.transform.SetParent(poolGo.transform, false);
    }
}
