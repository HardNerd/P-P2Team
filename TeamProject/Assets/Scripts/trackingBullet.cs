using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackingBullet : bullet
{
    void Start()
    {
        //Rigidbody player_rb = GameManager.instance.player.GetComponent<Rigidbody>();

        Vector2 player2dPos = new Vector2(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.z);
        Vector2 bullet2dPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 player2dVel = GameManager.instance.playerController.player2DVelocity;

        Vector3 playerDirection = (GameManager.instance.player.transform.position - transform.position).normalized;

        if (InterceptionDirection(player2dPos, bullet2dPos, player2dVel, speed, out Vector2 predictDirection))
            rb.velocity = new Vector3(predictDirection.x, playerDirection.y, predictDirection.y) * speed;
        else
            rb.velocity = playerDirection * speed;

        Destroy(gameObject, destroyBulletTime);
    }

    public bool InterceptionDirection(Vector2 a, Vector2 b, Vector2 vA, float sB, out Vector2 result)
    {
        var aToB = b - a;
        var dC = aToB.magnitude;
        var alpha = Vector2.Angle(aToB, vA) * Mathf.Deg2Rad;
        var sA = vA.magnitude;
        var r = sA / sB;

        if (SolveQuadratic(1 - r * r, 2 * r * dC * Mathf.Cos(alpha), -(dC * dC), out var root1, out var root2) == 0)
        {
            result = Vector2.zero;
            return false;
        }
        var dA = Mathf.Max(root1, root2);
        var t = dA / sB;
        var c = a + vA * t;
        result = (c - b).normalized;
        return true;
    }

    int SolveQuadratic(float a, float b, float c, out float root1, out float root2)
    {
        var discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            root1 = Mathf.Infinity;
            root2 = -root1;
            return 0;
        }

        root1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
        root2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);
        return discriminant > 0 ? 2 : 1;
    }
}
