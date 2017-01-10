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

        while (creatY < yCount) {
            int creatType = Random.Range(0, 100);
            if (creatType < 5) {
                GridType[][] arrData = GetPlatform(PlatformType.Left);
                AddData(creatY, arrData);
                creatY += (arrData.Length + Random.Range(1, 4));
            }
            else if (creatType < 10) {
                GridType[][] arrData = GetPlatform(PlatformType.Right);
                AddData(creatY, arrData);
                creatY += (arrData.Length + Random.Range(1, 4));
            }
            else if (creatType < 15) {
                GridType[][] arrData = GetRulePlatform(PlatformType.Double);
                AddData(creatY, arrData);
                creatY += (arrData.Length + Random.Range(1, 4));
            }
            else if (creatType < 30) {
                GridType[][] arrData = GetSquare();
                AddData(creatY, arrData);
                creatY += (arrData.Length + Random.Range(1, 3));
            }
            else if (creatType < 70) {
                GridType[][] arrData = GetLine();
                AddData(creatY, arrData);
                creatY += (arrData.Length + Random.Range(1, 3));
            }
            else {
                creatY += Random.Range(1, 5);
            }
        }

        creatY -= yCount;
        waitRoomProxy.SetData(arrCurData);
    }

    private void AddData(int curY, GridType[][] arrData) {
        for (int y = 0; y < arrData.Length; y++ ) {
            GridType[] arrLine = arrData[y];
            if (arrLine == null)
                break;
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

            curY++;
        }
    }

    private GridType[][] GetLine(int xMinStart = 0, int xMaxStart = -1, int numMin = 1, int numMax = 7) {
        if (xMaxStart == -1) {
            xMaxStart = xCount;
        }
        GridType[][] arrLine = new GridType[1][];
        int num = Random.Range(numMin, numMax);
        int startX = Random.Range(xMinStart, xMaxStart + 1 - num);
        arrLine[0] = new GridType[xCount];
        for (int x = 0; x < num; x++) {
            arrLine[0][startX + x] = GridType.Wall;
        }

        return arrLine;
    }

    private GridType[][] GetPlatform(PlatformType type, int minX = 1, int maxX = 8, int minY = 2, int maxY = 6) {
        int numY = Random.Range(minY, maxY + 1);

        GridType[][] arrData = new GridType[numY][];
        int longestY = Random.Range(0, numY);
        int longestX = Random.Range(minX, maxX + 1);

        int[] arrNumX = new int[numY];
        arrNumX[longestY] = longestX;
        int reduceY = longestY - 1;
        int reduceMaxX = longestX;
        while (reduceY >= 0) {
            reduceMaxX = Random.Range(Mathf.Max(1, reduceMaxX - 3), reduceMaxX + 1);
            arrNumX[reduceY] = reduceMaxX;
            reduceY--;
        }

        reduceY = longestY + 1;
        reduceMaxX = longestX;
        while (reduceY < numY) {
            reduceMaxX = Random.Range(Mathf.Max(1, reduceMaxX - 3), reduceMaxX + 1);
            arrNumX[reduceY] = reduceMaxX;
            reduceY++;
        }

        for (int y = 0; y < numY; y++ ) {
            int numX = arrNumX[y];
            arrData[y] = new GridType[xCount];
            for (int x = 0; x < numX; x++) {
                switch(type){
                    case PlatformType.Left:
                        arrData[y][x] = GridType.Wall;
                        break;
                    case PlatformType.Right:
                        arrData[y][xCount - 1 - x] = GridType.Wall;
                        break;
                    case PlatformType.Double:
                        arrData[y][x] = GridType.Wall;
                        arrData[y][xCount - 1 - x] = GridType.Wall;
                        break;
                }
            }
        }

        return arrData;
    }

    private GridType[][] GetRulePlatform(PlatformType type, int minX = 3, int maxX = 5, int minY = 3, int maxY = 6) {
        int numY = Random.Range(minY, maxY + 1);

        GridType[][] arrData = new GridType[numY][];
        int longestY = Random.Range(0, numY);
        int longestX = Random.Range(Mathf.Max(longestY, minY), maxX + 1);

        for (int y = 0; y < numY; y++) {
            int numX = longestX - Mathf.Abs(longestY - y);
            arrData[y] = new GridType[xCount];
            for (int x = 0; x < numX; x++) {
                switch (type) {
                    case PlatformType.Left:
                        arrData[y][x] = GridType.Wall;
                        break;
                    case PlatformType.Right:
                        arrData[y][xCount - 1 - x] = GridType.Wall;
                        break;
                    case PlatformType.Double:
                        arrData[y][x] = GridType.Wall;
                        arrData[y][xCount - 1 - x] = GridType.Wall;
                        break;
                }
            }
        }

        return arrData;
    }

    private GridType[][] GetSquare(int startMinX = 1, int startMaxX = 12, int minY = 2, int maxY = 4, int minX = 3, int maxX = 6) {
        int numY = Random.Range(minY, maxY + 1);

        GridType[][] arrData = new GridType[numY][];
        int longestY = Random.Range(0, numY);
        int longestX = Random.Range(minX, maxX + 1);

        int[] arrNumX = new int[numY];
        arrNumX[longestY] = longestX;
        int reduceY = longestY - 1;
        int reduceMaxX = longestX;
        while (reduceY >= 0) {
            reduceMaxX = Random.Range(Mathf.Max(1, reduceMaxX - 3), reduceMaxX + 1);
            arrNumX[reduceY] = reduceMaxX;
            reduceY--;
        }

        reduceY = longestY + 1;
        reduceMaxX = longestX;
        while (reduceY < numY) {
            reduceMaxX = Random.Range(Mathf.Max(1, reduceMaxX - 3), reduceMaxX + 1);
            arrNumX[reduceY] = reduceMaxX;
            reduceY++;
        }

        int startX = Random.Range(startMinX, startMaxX + 1 - longestX);
        for (int y = 0; y < numY; y++ ) {
            int numX = arrNumX[y];
            arrData[y] = new GridType[xCount];
            int deltaX = longestX - numX;
            int fixX = deltaX % 2 == 0 ? deltaX / 2 : ((int)deltaX / 2 + Random.Range(0, 2));
            for (int x = 0; x < numX; x++ ) {
                arrData[y][startX + fixX + x] = GridType.Wall;
            }
        }

        return arrData;
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

    public enum PlatformType {
        Left,
        Right,
        Double,
    }
}
