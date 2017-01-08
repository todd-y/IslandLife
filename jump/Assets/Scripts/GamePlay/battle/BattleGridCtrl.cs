using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleGridCtrl : MonoBehaviour {
    public static int xCount = 8;
    public static int yCount = 9;
    public static float[] arrPosX = { -280, -200, -120, -40, 40, 120, 200, 280 };
    public static float[] arrPosY = { 57.5f, -22.5f, -102.5f, -182.5f, -262.5f, -342.5f, -422.5f, -502.5f, -582.5f };
    public GameObject gridPrefab;
    private BattleGrid[][] arrGrid;
    private List<BattleGrid> usedGridPool;
    private List<BattleGrid> unUsedGridPool;

    public void CreatGrid() {
        usedGridPool = new List<BattleGrid>();
        unUsedGridPool = new List<BattleGrid>();

        arrGrid = new BattleGrid[xCount][];
        for (int xIndex = 0; xIndex < xCount; xIndex++ ) {
            BattleGrid[] arrY = new BattleGrid[yCount];
            arrGrid[xIndex] = arrY;
            for (int yIndex = 0; yIndex < yCount; yIndex++ ) {
                BattleGrid grid = GetOneGrid();
                arrY[yIndex] = grid;
                grid.InitPos( xIndex, yIndex );
            }
        }
    }

    public void RemoveLine(int yIndex = 0) {
        for (int xIndex = 0; xIndex < xCount; xIndex++ ) {
            RemoveGrid(xIndex, yIndex);
        }
    }

    public void RemoveGrid(int xIndex, int yIndex) {
        BattleGrid releaseGrid = arrGrid[xIndex][yIndex];
        if (releaseGrid == null)
            return;

        ShowGrid(xIndex - 1, yIndex);
        ShowGrid(xIndex + 1, yIndex);
        ShowGrid(xIndex, yIndex + 1);
        StartCoroutine(ReleaseHandle(releaseGrid, xIndex, yIndex));
    }

    private void ShowGrid(int xIndex, int yIndex) {
        if (xIndex >= 0 && xIndex < xCount && yIndex >= 0 && yIndex < yCount) {
            if (arrGrid[xIndex][yIndex] != null) {
                arrGrid[xIndex][yIndex].CurState = BattleGrid.State.Show;
            }
        }
    }

    private IEnumerator ReleaseHandle(BattleGrid releaseGrid, int xIndex, int yIndex) {
        if (releaseGrid.CurState == BattleGrid.State.Hide) {
            releaseGrid.CurState = BattleGrid.State.Show;
            yield return new WaitForSeconds(0.5f);
            arrGrid[xIndex][yIndex] = null;
            ReleaseGrid(releaseGrid);
        }
        else {
            arrGrid[xIndex][yIndex] = null;
            ReleaseGrid(releaseGrid);
            yield return new WaitForSeconds(0.5f);
        }

        for (int index = yIndex + 1; index < yCount; index++) {
            BattleGrid grid = arrGrid[xIndex][index];
            if (grid == null)
                continue;

            arrGrid[xIndex][index - 1] = grid;
            arrGrid[xIndex][index] = null;
            grid.SetTargetPos(xIndex, index - 1);
        }

        BattleGrid newGrid = GetOneGrid();
        arrGrid[xIndex][yCount - 1] = newGrid;
        newGrid.InitPos(xIndex, yCount - 1);
    }

    private BattleGrid GetOneGrid() {
        BattleGrid grid;
        if (unUsedGridPool.Count > 0) {
            grid = unUsedGridPool[0];
            grid.gameObject.SetActive(true);
            unUsedGridPool.Remove(grid);
            usedGridPool.Add(grid);
            return grid;
        }
        else {
            GameObject newGridGo = GameObject.Instantiate(gridPrefab) as GameObject;
            newGridGo.transform.SetParent(transform, false);
            grid = newGridGo.GetComponent<BattleGrid>();
            grid.gameObject.SetActive(true);
            usedGridPool.Add(grid);
            return grid;
        }
    }

    private void ReleaseGrid(BattleGrid grid) {
        grid.gameObject.SetActive(false);
        if (usedGridPool.Contains(grid)) {
            usedGridPool.Remove(grid);
        }
        else {
            Debug.LogError("gird is not contain in usedpool:" + grid);
        }

        if (unUsedGridPool.Contains(grid)) {
            Debug.LogError("gird is contain in unUsedGridPool:" + grid);
        }
        else {
            unUsedGridPool.Add(grid);
        }
    }
}
