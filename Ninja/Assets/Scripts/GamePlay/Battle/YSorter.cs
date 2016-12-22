using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Automatically sorts all SpriteRenderers found in children GOs.
 * Sorting is based on current Y position. 
 * GOs with higher Y are considered behind those with lower Y position.
 */
public class YSorter : MonoBehaviour {
	private float maxY = 10;
    private float minY = -10;
    private float sortingOrderMin = short.MinValue + 200;
	private float sortingOrderMax = 0;

	private Dictionary<SpriteRenderer, int> originalSortingOrder;
	private Transform myTr;

    private Vector3 lastPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		myTr = transform;
		FillOriginalSortingOrder();
	}

    void Update() {
        if (lastPos != transform.position) {
            AutoSort();
            lastPos = transform.position;
        }
    }

	private void FillOriginalSortingOrder() {
		originalSortingOrder = new Dictionary<SpriteRenderer, int>();
		foreach(SpriteRenderer sRen in GetComponentsInChildren<SpriteRenderer>()) {
			originalSortingOrder.Add(sRen, sRen.sortingOrder);
		}
	}

	private void AutoSort() {
		int sortingOffset = GetSortingOffset( myTr.position.y );
		foreach(SpriteRenderer sRen in originalSortingOrder.Keys) {
            sRen.sortingOrder = originalSortingOrder[sRen] + sortingOffset;
		}
	}

	private int GetSortingOffset(float currentY) {
		float clampedY = Mathf.Clamp(currentY, minY, maxY);
		float t = (clampedY - minY) / (maxY - minY);
        float sortingOffset = (t * (sortingOrderMax - sortingOrderMin)) + sortingOrderMin;
		return Mathf.RoundToInt( -sortingOffset );
	}
}
