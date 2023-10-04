using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class molotov : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject fireArea;
    [SerializeField] float angle;

    void Start()
    {
        rb.velocity = CalculateParabolicVel(transform.position, GameManager.instance.player.transform.position, angle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || other.CompareTag("Enemy"))
            return;

        Instantiate(fireArea, new Vector3(transform.position.x, fireArea.transform.localScale.y, transform.position.z), fireArea.transform.rotation);
        Destroy(gameObject);
    }

    Vector3 CalculateParabolicVel(Vector3 source, Vector3 target, float angle)
    {
        Vector3 dir = target - source;

        // Physics equations to calculate angle in radians and distance
        float y = dir.y;
        dir.y = 0;
        float distance = dir.magnitude;
        float radAngle = angle * Mathf.Deg2Rad;
        dir.y = distance * Mathf.Tan(radAngle);
        distance += y / Mathf.Tan(radAngle);

        // Velocity calculation
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * radAngle));
        return velocity * dir.normalized;
    }
}
