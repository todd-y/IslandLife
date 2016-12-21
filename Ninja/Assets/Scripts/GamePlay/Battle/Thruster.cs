using UnityEngine;
using System.Collections;

public class Thruster : MonoBehaviour {
    //[Tooltip("The current thottle amount")]
    public float Throttle;
    //[Tooltip("The scale of this thruster when throttle is 1")]
    public Vector3 MaxScale = Vector3.one;
    //[Tooltip("How quickly the throttle scales to the desired value")]
    public float Dampening = 10.0f;
    //[Tooltip("The amount of force applied to the rigidbody2D when throttle is 1")]
    public float MaxForce = 10.0f;
    //[Tooltip("The amount the thruster effect can flicker")]
    public float Flicker = 0.1f;
    // The rigidbody this thruster is attached to
    [System.NonSerialized]
    private Rigidbody2D body;
    // The current interpolated throttle value
    [SerializeField]
    private float currentThrottle;

    protected virtual void FixedUpdate() {
        if (body == null) body = GetComponentInParent<Rigidbody2D>();

        if (body != null) {
            body.AddForceAtPosition(transform.up * MaxForce * -Throttle, transform.position, ForceMode2D.Force);
        }
    }

    protected virtual void Update() {
        currentThrottle = ToolMgr.Dampen(currentThrottle, Throttle, Dampening, Time.deltaTime);

        transform.localScale = MaxScale * Random.Range(1.0f - Flicker, 1.0f + Flicker) * currentThrottle;
    }
}
