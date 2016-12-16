using UnityEngine;
using System.Collections;

public class Weapeon : UbhBaseShot {

    public float fireDelay = 0.1f;
    public float betweenDelay = 0.1f;
    public Transform gunGo;
    private float lastFireTime = 0;
    private float angle = 10;
    private bool canShoot = false;

    private Vector3 GunPoint {
        get {
            return gunGo == null ? transform.position : gunGo.position;
        }
    }

    void FixedUpdate() {
        if (Time.time > (lastFireTime + fireDelay)) {
            Vector2 gunPos = gameObject.transform.position;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            angle = Vector2.Angle(Vector2.right, mousePos - gunPos);
            angle = mousePos.y > gunPos.y ? angle : -angle;
            angle -= 90;
            canShoot = true;
        }
    }
    
    public override void Shot() {
        if (canShoot == false)
            return;
        lastFireTime = Time.time;
        canShoot = false;
        ShotHandle();
    }

    protected virtual void ShotHandle() {
        StartCoroutine(ShotCoroutine());
    }

    IEnumerator ShotCoroutine() {
        if (_BulletNum <= 0 || _BulletSpeed <= 0f) {
            Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed is not set.");
            yield break;
        }
        if (_Shooting) {
            yield break;
        }
        _Shooting = true;

        for (int i = 0; i < _BulletNum; i++) {
            if (0 < i && 0f < betweenDelay) {
                yield return StartCoroutine(UbhUtil.WaitForSeconds(betweenDelay));
            }

            var bullet = GetBullet(GunPoint, transform.rotation);
            if (bullet == null) {
                break;
            }

            ShotBullet(bullet, _BulletSpeed, angle);

            AutoReleaseBulletGameObject(bullet.gameObject);
        }

        FinishedShot();
    }
}
