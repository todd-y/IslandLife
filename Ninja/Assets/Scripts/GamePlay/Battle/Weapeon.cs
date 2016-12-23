using UnityEngine;
using System.Collections;

public class Weapeon : UbhBaseShot {

    public float fireDelay = 0.1f;
    public float betweenDelay = 0.1f;
    public Transform gunGo;
    public float mpCost = 5;
    private float lastFireTime = 0;
    private bool canShoot = false;

    private Vector3 GunPoint {
        get {
            return gunGo == null ? transform.position : gunGo.position;
        }
    }

    void FixedUpdate() {
        if (Time.time > (lastFireTime + fireDelay)) {
            canShoot = true;
        }
    }

    public bool TryShot(float curMp) {
        if (curMp < mpCost)
            return false;

        if (canShoot == false)
            return false;

        Shot();
        return true;
    }
    
    public override void Shot() {
        if (canShoot == false)
            return;
        lastFireTime = Time.time;
        canShoot = false;
        ShotHandle();
    }

    protected virtual void ShotHandle() {
        SoundManager.Instance.PlaySound("shot");
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

            ShotBullet(bullet, _BulletSpeed, transform.rotation.eulerAngles.z);

            AutoReleaseBulletGameObject(bullet.gameObject);
        }

        FinishedShot();
    }
}
