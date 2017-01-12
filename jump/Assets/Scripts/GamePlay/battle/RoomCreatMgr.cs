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
        xCount = width / size;//13
        yCount = height / size;//22
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

        //创建地形
        while (creatY < yCount) {
            int creatType = ToolMgr.RangeWithMax(0, 100);
            if (creatType < 5) {
                GridType[][] arrData = GetPlatform(PlatformType.Left);
                AddData(creatY, arrData);
                creatY += (arrData.Length + ToolMgr.RangeWithMax(1, 3));
            }
            else if (creatType < 10) {
                GridType[][] arrData = GetPlatform(PlatformType.Right);
                AddData(creatY, arrData);
                creatY += (arrData.Length + ToolMgr.RangeWithMax(1, 3));
            }
            else if (creatType < 15) {
                GridType[][] arrData = GetRulePlatform(PlatformType.Double);
                AddData(creatY, arrData);
                creatY += (arrData.Length + ToolMgr.RangeWithMax(1, 3));
            }
            else if (creatType < 30) {
                GridType[][] arrData = GetSquare();
                AddData(creatY, arrData);
                creatY += (arrData.Length + ToolMgr.RangeWithMax(1, 2));
            }
            else if (creatType < 70) {
                GridType[][] arrData = GetLine();
                AddData(creatY, arrData);
                creatY += (arrData.Length + ToolMgr.RangeWithMax(1, 2));
            }
            else {
                creatY += ToolMgr.RangeWithMax(1, 4);
            }
        }
        //创建金币
        CreatGold();
        CreatItemAndEnemy();

        creatY -= yCount;
        waitRoomProxy.SetData(arrCurData);
    }

    private void CreatItemAndEnemy(){
        List<GridInfo> wallUpEmptyList = new List<GridInfo>();
        List<GridInfo> otherEmptyList = new List<GridInfo>();
        for (int y = 0; y < yCount; y++) {
            for (int x = 0; x < xCount; x++) {
                if (arrCurData[y][x] == GridType.Empty) {
                    if (y != yCount - 1 && arrCurData[y + 1][x] == GridType.Wall) {
                        wallUpEmptyList.Add(new GridInfo(x, y));
                    }
                    else {
                        otherEmptyList.Add(new GridInfo(x, y));
                    }
                }
            }
        }

        int itemNum = ToolMgr.RangeWithMax(0, 2);
        for (int index = 0; index < itemNum; index++ ) {
            if (wallUpEmptyList.Count == 0) {
                break;
            }
            GridInfo gridInfo = wallUpEmptyList[ToolMgr.Range(0, wallUpEmptyList.Count)];
            wallUpEmptyList.Remove(gridInfo);
            arrCurData[gridInfo.y][gridInfo.x] = GridType.Item;
        }

        int enemyNum = ToolMgr.RangeWithMax(3, 5);
        for (int index = 0; index < enemyNum; index++) {
            if (wallUpEmptyList.Count == 0) {
                break;
            }
            GridInfo gridInfo = wallUpEmptyList[ToolMgr.Range(0, wallUpEmptyList.Count)];
            wallUpEmptyList.Remove(gridInfo);
            arrCurData[gridInfo.y][gridInfo.x] = GridType.Enemy;
        }

        //int flyEnemyNum = ToolMgr.RangeWithMax(1, 3);
        //for (int index = 0; index < flyEnemyNum; index++) {
        //    if (otherEmptyList.Count == 0)
        //        break;
        //    GridInfo gridInfo = otherEmptyList[ToolMgr.Range(0, otherEmptyList.Count)];
        //    otherEmptyList.Remove(gridInfo);
        //    arrCurData[gridInfo.y][gridInfo.x] = GridType.Enemy;
        //}

    }

    private void CreatGold() {
        int goldNum = ToolMgr.RangeWithMax(5, 10);
        List<int> batchList = new List<int>();
        while (goldNum > 0) {
            int onceRandomNum = ToolMgr.RangeWithMax(1, 5);
            if (goldNum > onceRandomNum) {
                batchList.Add(onceRandomNum);
                goldNum -= onceRandomNum;
            }
            else {
                batchList.Add(goldNum);
                goldNum = 0;
            }
        }

        int[] arrBatch = batchList.ToArray();
        for (int index = 0; index < arrBatch.Length; index++ ) {
            int curBatchNum = arrBatch[index];
            int startX = ToolMgr.Range(0, xCount);
            int startY = ToolMgr.Range(0, yCount);
            Face face = (Face)ToolMgr.Range(0, (int)Face.Max);
            for (int k = 0; k < curBatchNum; k++ ) {
                int creatX = startX;
                int creatY = startY;
                switch (face) {
                    case Face.Down:
                        creatY = startY + k;
                        if (creatY >= yCount) {
                            creatY = yCount - 1 - k;
                        }
                        break;
                    case Face.Up:
                        creatY = startY - k;
                        if (creatY <= 0) {
                            creatY = k;
                        }
                        break;
                    case Face.Left:
                        creatX = startX - k;
                        if (creatX <= 0) {
                            creatX = k;
                        }
                        break;
                    case Face.Right:
                        creatX = startX + k;
                        if (creatX >= xCount) {
                            creatX = xCount - 1 - k;
                        }
                        break;
                    default:
                        Debug.LogError("not handle type:" + face);
                        break;
                }

                SetGridData(creatX, creatY, GridType.Gold);
            }
        }
    }

    private void SetGridData(int x, int y, GridType _type) {
        if (arrCurData[y][x] == GridType.Empty) {
            arrCurData[y][x] = _type;
        }
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

    private GridType[][] GetLine(int xMinStart = 0, int xMaxStart = -1, int numMin = 1, int numMax = 6) {
        if (xMaxStart == -1) {
            xMaxStart = xCount;
        }
        GridType[][] arrLine = new GridType[1][];
        int num = ToolMgr.RangeWithMax(numMin, numMax);
        int startX = ToolMgr.RangeWithMax(xMinStart, xMaxStart - num);
        arrLine[0] = new GridType[xCount];
        for (int x = 0; x < num; x++) {
            arrLine[0][startX + x] = GridType.Wall;
        }

        return arrLine;
    }

    private GridType[][] GetPlatform(PlatformType type, int minX = 1, int maxX = 8, int minY = 2, int maxY = 6) {
        int numY = ToolMgr.RangeWithMax(minY, maxY);

        GridType[][] arrData = new GridType[numY][];
        int longestY = ToolMgr.Range(0, numY);
        int longestX = ToolMgr.RangeWithMax(minX, maxX);

        int[] arrNumX = new int[numY];
        arrNumX[longestY] = longestX;
        int reduceY = longestY - 1;
        int reduceMaxX = longestX;
        while (reduceY >= 0) {
            reduceMaxX = ToolMgr.RangeWithMax(Mathf.Max(1, reduceMaxX - 3), reduceMaxX);
            arrNumX[reduceY] = reduceMaxX;
            reduceY--;
        }

        reduceY = longestY + 1;
        reduceMaxX = longestX;
        while (reduceY < numY) {
            reduceMaxX = ToolMgr.RangeWithMax(Mathf.Max(1, reduceMaxX - 3), reduceMaxX);
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
        int numY = ToolMgr.RangeWithMax(minY, maxY);

        GridType[][] arrData = new GridType[numY][];
        int longestY = ToolMgr.Range(0, numY);
        int longestX = ToolMgr.RangeWithMax(Mathf.Max(longestY, minY), maxX);

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
        int numY = ToolMgr.RangeWithMax(minY, maxY);

        GridType[][] arrData = new GridType[numY][];
        int longestY = ToolMgr.Range(0, numY);
        int longestX = ToolMgr.RangeWithMax(minX, maxX);

        int[] arrNumX = new int[numY];
        arrNumX[longestY] = longestX;
        int reduceY = longestY - 1;
        int reduceMaxX = longestX;
        while (reduceY >= 0) {
            reduceMaxX = ToolMgr.RangeWithMax(Mathf.Max(1, reduceMaxX - 3), reduceMaxX);
            arrNumX[reduceY] = reduceMaxX;
            reduceY--;
        }

        reduceY = longestY + 1;
        reduceMaxX = longestX;
        while (reduceY < numY) {
            reduceMaxX = ToolMgr.RangeWithMax(Mathf.Max(1, reduceMaxX - 3), reduceMaxX);
            arrNumX[reduceY] = reduceMaxX;
            reduceY++;
        }

        int startX = ToolMgr.RangeWithMax(startMinX, startMaxX - longestX);
        for (int y = 0; y < numY; y++ ) {
            int numX = arrNumX[y];
            arrData[y] = new GridType[xCount];
            int deltaX = longestX - numX;
            int fixX = deltaX % 2 == 0 ? deltaX / 2 : ((int)deltaX / 2 + ToolMgr.RangeWithMax(0, 1));
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

    public void RemoveGameObject(GameObject go) {
        for (int index = 0; index < BattleWindow.Instance.arrGridArea.Length; index++ ) {
            BattleWindow.Instance.arrGridArea[index].ClearGo(go);
        }
        RoomCreatMgr.Instance.ReleaseGameObject(go);
    }

    public enum PlatformType {
        Left,
        Right,
        Double,
    }

    public enum Face {
        Up = 0,
        Down,
        Left,
        Right,
        Max,
    }

    public struct GridInfo {
        public int x;
        public int y;
        public GridInfo(int _x, int _y) {
            x = _x;
            y = _y;
        }
    }
}
