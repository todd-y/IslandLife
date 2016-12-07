// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// *Please enable this define if you want to use the DarkTonic's CoreGameKit pooling system.
// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// #define USING_CORE_GAME_KIT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ubh object pool.
/// </summary>
public class UbhObjectPool : UbhSingletonMonoBehavior<UbhObjectPool> {
    List<int> _PooledKeyList = new List<int>();
    Dictionary<int, List<GameObject>> _PooledGoDic = new Dictionary<int, List<GameObject>>();

    protected override void Awake() {
        base.Awake();
    }

    /// <summary>
    /// Get GameObject from object pool or instantiate.
    /// </summary>
    public GameObject GetGameObject(GameObject prefab, Vector3 position, Quaternion rotation, bool forceInstantiate = false) {
        if (prefab == null) {
            return null;
        }
        int key = prefab.GetInstanceID();

        if (_PooledKeyList.Contains(key) == false && _PooledGoDic.ContainsKey(key) == false) {
            _PooledKeyList.Add(key);
            _PooledGoDic.Add(key, new List<GameObject>());
        }

        List<GameObject> goList = _PooledGoDic[key];
        GameObject go = null;

        if (forceInstantiate == false) {
            for (int i = goList.Count - 1; i >= 0; i--) {
                go = goList[i];
                if (go == null) {
                    goList.Remove(go);
                    continue;
                }
                if (go.activeSelf == false) {
                    // Found free GameObject in object pool.
                    Transform goTransform = go.transform;
                    goTransform.position = position;
                    goTransform.rotation = rotation;
                    go.SetActive(true);
                    BulletHandle(go);
                    return go;
                }
            }
        }

        // Instantiate because there is no free GameObject in object pool.
        go = (GameObject)Instantiate(prefab, position, rotation);
        go.transform.parent = _Transform;
        goList.Add(go);
        BulletHandle(go);
        return go;
    }

    private void BulletHandle(GameObject go) {
        UbhBullet bullet = go.GetComponent<UbhBullet>();
        if (bullet != null) {
            bullet.SetDefault();
        }
    }

    /// <summary>
    /// Releases game object (back to pool or destroy).
    /// </summary>
    public void ReleaseGameObject(GameObject go, bool destroy = false) {
        if (destroy) {
            Destroy(go);
            return;
        }
        go.SetActive(false);
    }

    /// <summary>
    /// Get active bullets count.
    /// </summary>
    public int GetActivePooledObjectCount() {
        int cnt = 0;
        for (int i = 0; i < _PooledKeyList.Count; i++) {
            int key = _PooledKeyList[i];
            var goList = _PooledGoDic[key];
            for (int j = 0; j < goList.Count; j++) {
                var go = goList[j];
                if (go != null && go.activeInHierarchy) {
                    cnt++;
                }
            }
        }
        return cnt;
    }
}