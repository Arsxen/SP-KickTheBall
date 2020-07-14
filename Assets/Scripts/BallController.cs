using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    [SerializeField] private Vector2 minSpeed = new Vector2(1,1);

    [SerializeField] private Vector2 maxSpeed = new Vector2(10, 10);

    public Rigidbody2D RgBody { get; private set; }
    
    // Start is called before the first frame update
    private void Awake()
    {
        RgBody = GetComponent<Rigidbody2D>();
    }

    public void ShootBall(Vector2 force)
    {
        RgBody.drag = 0;
        RgBody.angularDrag = 0;
        RgBody.AddForce(force);
        RgBody.AddTorque(force.x);
    }


    public void ShootBallRandom()
    {
        //Random Direction
        var direction = new Vector2Int(Random.Range(0, 2), Random.Range(0, 2));
        direction.x = direction.x == 0 ? -1 : 1;
        direction.y = direction.y == 0 ? -1 : 1;
        
        var ballVelocity = new Vector2(
            Random.Range(minSpeed.x, maxSpeed.x),
            Random.Range(minSpeed.y, maxSpeed.y)
        );

        ballVelocity *= direction;
        
        ShootBall(ballVelocity);
    }

    public void SlowBall()
    {
        RgBody.drag = 5;
        RgBody.angularDrag = 3;
    }

    public void StopBall()
    {
        RgBody.Sleep();
        RgBody.MoveRotation(0);
    }

    public void TeleportBall(Vector2 position)
    {
        RgBody.MovePosition(position);
    }

    public void ResetBall()
    {
        StopBall();
        TeleportBall(Vector2.zero);
    }

    private IEnumerator ShootBallAfterOneSecond()
    {
        yield return new WaitForSeconds(0.5f);
        ShootBallRandom();
    }
}
