using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    [SerializeField] private Vector2 minSpeed = new Vector2(1,1);

    [SerializeField] private Vector2 maxSpeed = new Vector2(10, 10);

    private Rigidbody2D _rgBody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rgBody = GetComponent<Rigidbody2D>();
        ShootBallRandom();
    }

    public void ShootBall(Vector2 force)
    {
        _rgBody.AddForce(force);
        _rgBody.AddTorque(force.x);
    }


    private void ShootBallRandom()
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

    public void StopBall()
    {
        _rgBody.velocity = Vector2.zero;
        _rgBody.angularVelocity = 0;
        _rgBody.MoveRotation(0);
    }

    public void TeleportBall(Vector2 position)
    {
        _rgBody.MovePosition(position);
    }

    public void ResetBall()
    {
        StopBall();
        TeleportBall(Vector2.zero);
        StartCoroutine(ShootBallAfterOneSecond());
    }

    private IEnumerator ShootBallAfterOneSecond()
    {
        yield return new WaitForSeconds(0.5f);
        ShootBallRandom();
    }
}
