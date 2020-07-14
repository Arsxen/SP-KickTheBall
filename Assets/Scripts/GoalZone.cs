using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZone : MonoBehaviour
{
    public enum Side
    {
        A, B, Both
    }
    
    
    [SerializeField] private Rigidbody2D ballRigidbody2D;
    
    [SerializeField] private Side playerSide = Side.A;

    private WaitUntil _waitUntilBallSleep;

    private void Start()
    {
        _waitUntilBallSleep = new WaitUntil(ballRigidbody2D.IsSleeping);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;
        GameManager.Main.StopTimer();
        GameManager.Main.Ball.SlowBall();
        GameManager.Main.GiveScore(playerSide);
        StartCoroutine(WaitUntilStop());
    }

    private IEnumerator WaitUntilStop()
    {
        yield return _waitUntilBallSleep;
        GameManager.Main.ResetGame();
    }
}
