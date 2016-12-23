using UnityEngine;
using System.Collections;

public class BackAttack : BaseAttack {
    public GameObject prefab;
    private SpriteRenderer spriteRenderer;
    private Transform follow;
    private float angle = 0;
    public float Angle {
        get {
            return angle;
        }
        set {
            angle = value;
        }
    }

    protected override void HitCheck(Transform colTrans) {
        if (colTrans.gameObject.layer == GeneralDefine.EnemyBulletLayer) {
            SoundManager.Instance.PlaySound("shot");
            Send.SendMsg(SendType.BackBullet);

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
            GameObject newBulletGo = UbhObjectPool.Instance.GetGameObject(prefab, colTrans.position, newBulletRotation );
            UbhObjectPool.Instance.ReleaseGameObject(colTrans.gameObject);

            UbhBullet newBullet = newBulletGo.GetComponent<UbhBullet>();
            newBullet.Shot(10f, transform.rotation.eulerAngles.z);
        }
    }
    public void SetColor(Color color, Transform _follow) {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        }
        if (spriteRenderer == null) {
            Debug.Log("spriterenderer is null");
            return;
        }
        spriteRenderer.color = color;
        prefab.GetComponent<UbhBullet>().SetColor(spriteRenderer.color);

        follow = _follow;
    }

    void Update() {
        if (follow == null)
            return;
        transform.position = follow.position;
        transform.eulerAngles = follow.eulerAngles + new Vector3(0, 0, angle);
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
