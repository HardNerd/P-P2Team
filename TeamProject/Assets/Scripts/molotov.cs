using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class molotov : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject fireArea;
    [SerializeField] public float angle;

    void Start()
    {
        rb.velocity = CalculateParabolicVel(transform.position, GameManager.instance.player.transform.position, angle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || other.CompareTag("Enemy"))
            return;

        AudioSource source = fireArea.GetComponent<AudioSource>();
        float pitch = source.pitch;
        GameManager.instance.AudioChange(source);
        Instantiate(fireArea, new Vector3(transform.position.x, fireArea.transform.localScale.y, transform.position.z), fireArea.transform.rotation);
        source.pitch = pitch;
        Destroy(gameObject);
    }

    public Vector3 CalculateParabolicVel(Vector3 source, Vector3 target, float angle)
    {
        Vector3 gravity = Physics.gravity;

        if (angle >= 90f || angle <= -90f)
            return Vector3.zero;

        Vector3 direction = target - source;
        Vector3 horizontal = GetHorizontalVector(direction, gravity);
        float horizontalDistance = horizontal.magnitude;
        Vector3 vertical = GetVerticalVector(direction, gravity);
        float verticalDistance = vertical.magnitude * Mathf.Sign(Vector3.Dot(vertical, -gravity));

        float radAngle = angle * Mathf.Deg2Rad;
        float angleX = Mathf.Cos(radAngle);
        float angleY = Mathf.Sin(radAngle);

        float gravityMag = gravity.magnitude;

        if (verticalDistance / horizontalDistance > angleY / angleX)
            return Vector3.zero;

        float destSpeed = (1 / Mathf.Cos(radAngle)) * Mathf.Sqrt((0.5f * gravityMag * horizontalDistance * horizontalDistance) / ((horizontalDistance * Mathf.Tan(radAngle)) - verticalDistance));

        Vector3 launch = ((horizontal.normalized * angleX) - (gravity.normalized * angleY)) * destSpeed;
        return launch;
    }

    public Vector3 GetHorizontalVector(Vector3 direction, Vector3 gravity)
    {
        Vector3 output;
        Vector3 perpendicular = Vector3.Cross(direction, gravity);
        perpendicular = Vector3.Cross(gravity, perpendicular);
        output = Vector3.Project(direction, perpendicular);
        return output;
    }

    public Vector3 GetVerticalVector(Vector3 direction, Vector3 gravity)
    {
        Vector3 output;
        output = Vector3.Project(direction, gravity);
        return output;
    }
}
