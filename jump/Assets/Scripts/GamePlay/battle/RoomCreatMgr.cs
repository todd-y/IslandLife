using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomCreatMgr : Singleton<RoomCreatMgr> {
    public const int width = 650;
    public const int height = 1050;
    public const int size = 50;
    private GameObject poolGo;
    private int xCount;
    private int yCount;

    private Dictionary<string, GameObject> dicPrefab = new Dictionary<string, GameObject>();
    private Dictionary<string, List<GameObject>> dicPool = new Dictionary<string, List<GameObject>>();

    public void Init() {
        poolGo = Launch.Instance.poolGo;
        xCount = width / size;
        yCount = height / size;
    }

    public void Clear() {

    }

    public void CreatNewRoom(RoomProxy roomProxy) {
        roomProxy.ClearAllGo();
        GridType[][] arrData = GetEmptyData();
        roomProxy.SetData(arrData);
    }

    private GridType[][] GetEmptyData() {
        GridType[][] arrData = new GridType[width / size][];
        for (int x = 0; x < xCount; x++) {
            GridType[] arrLine = new GridType[yCount];
            arrData[x] = arrLine;
            for (int y = 0; y < yCount; y++) {
                arrData[x][y] = GridType.Empty;
            }
        }

        return arrData;
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
