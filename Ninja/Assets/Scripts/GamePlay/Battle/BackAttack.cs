using UnityEngine;
using System.Collections;

public class BackAttack : BaseAttack {
    public GameObject prefab;
    private SpriteRenderer spriteRenderer;

    protected override void HitCheck(Transform colTrans) {
        if (colTrans.gameObject.layer == GeneralDefine.EnemyBulletLayer) {
            UbhBullet ubhBullet = colTrans.GetComponent<UbhBullet>();
            if (ubhBullet == null) {
                ubhBullet = colTrans.parent.GetComponent<UbhBullet>();
            }
            if (ubhBullet == null) {
                Debug.LogError("ubhbullet is null:" + colTrans.name);
                return;
            }
            Quaternion newBulletRotation = Quaternion.Euler( transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            UbhObjectPool.Instance.GetGameObject(prefab, colTrans.position, newBulletRotation );
            UbhObjectPool.Instance.ReleaseGameObject(colTrans.gameObject);
        }
    }
    public void SetColor(Color color) {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        }
        if (spriteRenderer == null) {
            Debug.Log("spriterenderer is null");
            return;
        }
        spriteRenderer.color = color;
        prefab.GetComponent<UbhSimpleBullet>().SetColor(spriteRenderer.color);
    }

    protected void AutoReleaseBulletGameObject(GameObject goBullet) {
        UbhCoroutine.StartIE(AutoReleaseBulletGameObjectCoroutine(goBullet));
    }

    IEnumerator AutoReleaseBulletGameObjectCoroutine(GameObject goBullet) {
        float countUpTime = 0f;

        while (true) {
            if (goBullet == null || goBullet.activeInHierarchy == false) {
                yield break;
            }

            if (10f <= countUpTime) {
                break;
            }

            yield return 0;

            countUpTime += UbhTimer.Instance.DeltaTime;
        }

        UbhObjectPool.Instance.ReleaseGameObject(goBullet);
    }
}
