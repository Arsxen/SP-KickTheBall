using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Main { get; private set; }

    public BallController Ball { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Main = this;
        Ball = GameObject.FindWithTag("Ball").GetComponent<BallController>();
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Ball.ResetBall();
    //     }
    // }
}
