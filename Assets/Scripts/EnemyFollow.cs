﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {Patrol, Chase };

public class EnemyFollow : MonoBehaviour
{
    private State currentState = State.Patrol;
    private EnemyBehaviour csAI;
    
    private Animator myAnimator;
    private bool bAnimate = true;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        if (myAnimator == null)
        {
            print("Enemy has no animator");
            bAnimate = false;
        }

        csAI = GetComponent<EnemyBehaviour>();
        if (csAI == null)
            print("Enemy has no AI");
    }

    void Update()
    {
        if (currentState == State.Patrol)
            csAI.Patrol();
        else
            csAI.Chase();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Please don't attack walls
        if (other.tag == "Player")
        {
            currentState = State.Chase;
            if (bAnimate)
                myAnimator.SetBool("ChaseMode", true);

            csAI.clTarget = other;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Please don't be calmed down by walls
        if (other.tag == "Player")
        {
            currentState = State.Patrol;
            if (bAnimate)
                myAnimator.SetBool("ChaseMode", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (bAnimate)
                myAnimator.SetTrigger("Attack");
            collision.gameObject.GetComponent<PlayerHealth>().ChangeHP(-1);
        }
        else if (collision.gameObject.tag == "Wall")
            transform.Rotate(0, 180, 0);
    }
}
