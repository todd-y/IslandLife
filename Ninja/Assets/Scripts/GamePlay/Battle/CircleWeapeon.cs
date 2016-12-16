using UnityEngine;
using System.Collections;

public class CircleWeapeon : Weapeon {

    protected override void ShotHandle() {
        if (_BulletNum <= 0 || _BulletSpeed <= 0f) {
            Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed is not set.");
            return;
        }

        float shiftAngle = 360f / (float)_BulletNum;

        for (int i = 0; i < _BulletNum; i++) {
            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null) {
                break;
            }

            float angle = shiftAngle * i;

            ShotBullet(bullet, _BulletSpeed, angle);

            AutoReleaseBulletGameObject(bullet.gameObject);
        }

        FinishedShot();
    }
}
