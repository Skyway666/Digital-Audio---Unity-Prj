////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilHeadAI : Creature
{
    [Header("Evil Head Specifics")]
    public GameObject SmokeFX;
    public GameObject deathFX;
    public GameObject keepOnDeath;

    [Header("Wwise")]
    public float MovementRTPC;

    [Header("Clips")]
    public AudioClip hover_sound;

    #region private variables
    private Vector3 targetLocation = Vector3.zero;
    private IEnumerator chargeRoutine;
    private AudioSource source;

    //Cached Animator hashes
    private readonly int spawnHash = Animator.StringToHash("Spawn");
    private readonly int deathHash = Animator.StringToHash("Death");
    #endregion

    private void SetMovementSpeed(float speed) {
        MovementRTPC = speed;
    }

    private void Awake()
    {
        if(anim == null)
        {
            anim = GetComponent<Animator>();
        }
        source = GetComponent<AudioSource>();
    }

    public override void Start(){
		base.Start();
        // HINT: Hover sound start here
        source.clip = hover_sound;
        source.Play();
	}

    public override void OnSpotting()
    {
        base.OnSpotting();
        anim.SetTrigger(spawnHash);
		SmokeFX.SetActive(true);
    }

    public override void Move(Vector3 yourPosition, Vector3 targetPosition)
    {
        base.Move(yourPosition, targetPosition);
        SetMovementSpeed(20f);
    }

    public override void Anim_MeleeAttack()
    {
        
        base.Anim_MeleeAttack();
        SetMovementSpeed(100f);
    }

    /// <summary>
    /// Called from Animation Event. This happens when the Evil Head telegraphs its attack!
    /// </summary>
    public void DisableMovement()
    {
        thisNavMeshAgent.destination = transform.position;
        targetLocation = targetOfNPC.transform.position + Vector3.up;
        StartCoroutine(RotateTowardsTarget(targetLocation, 1f));
        // HINT: The head is sending a telegraph attack, this might need a sound effect
    }


    public void ReenableMovement()
    {
        SetMovementSpeed(0f);
        thisNavMeshAgent.SetDestination(transform.position);
        //thisNavMeshAgent.SetDestination(targetOfNPC == null ? transform.position : targetOfNPC.transform.position);
    }

    /// <summary>
    /// Called from Animation Event. Initiates the charging towards the player!
    /// </summary>
    public void Charge()
    {
        if (chargeRoutine != null)
        {
            StopCoroutine(chargeRoutine);
        }
        chargeRoutine = ChargeTowardsPlayer(0.5f);
        StartCoroutine(chargeRoutine);
    }

    IEnumerator RotateTowardsTarget(Vector3 targetLocation, float seconds)
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(targetLocation - transform.position);

        for (float t = 0; t < 1; t += Time.deltaTime / seconds)
        {
            float s = Curves.Instance.Overshoot.Evaluate(t);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, s);
            yield return null;
        }
    }

    IEnumerator ChargeTowardsPlayer(float seconds)
    {
        //print(Time.realtimeSinceStartup + ": ChargeTowardsPlayer");
        // HINT: Charge started, a telegrpah sound could be useful here

        Vector3 currentPosition = transform.position;
        Vector3 destination = targetLocation + ((targetLocation) - currentPosition).normalized * 2f;

        for (float t = 0; t < 1; t += Time.deltaTime / seconds)
        {
            float s = Curves.Instance.SmoothOut.Evaluate(t);
            Vector3 nextPosition = Vector3.Lerp(currentPosition, destination, s);
            transform.position = nextPosition;
            yield return null;
        }
        ReenableMovement();
    }

    public override void OnDeathAnimation()
    {
        base.OnDeathAnimation();
        isAlive = false;

        if (chargeRoutine != null)
        {
            StopCoroutine(chargeRoutine);
        }

        thisNavMeshAgent.nextPosition = transform.position;

        anim.SetTrigger(deathHash);
    }

    /// <summary>
    /// Called from the EvilHeadMethodRerouter class when an object is hit during charge.
    /// </summary>
    public void StopCharge()
    {
        if (chargeRoutine != null)
        {
            StopCoroutine(chargeRoutine);
        }
        ReenableMovement();
    }

    public void Explode()
    {
        SetMovementSpeed(0f);
        //print(Time.realtimeSinceStartup + ": Explode");
        // HiNT: We should stop hover sound at this point

        GameObject fx = (GameObject)Instantiate(deathFX, transform.position, Quaternion.identity);
        Destroy(fx, 5f);

        //This following section makes sure that the particles of the Evil Head doesn't just suddenly disappear in a flash. Rather, they stop emitting and are removed after some time 
        if (keepOnDeath != null)
        {
            keepOnDeath.transform.parent = null;
            foreach (ParticleSystem p in keepOnDeath.GetComponentsInChildren<ParticleSystem>())
            {
                p.Stop();
            }
            Destroy(keepOnDeath, 5f);
        }

        PlayCreatureDeathSound();
        Destroy(gameObject);
    }

    /// <summary>
    /// Called from Animation Event. Initiates the charging towards the player!
    /// </summary>
    public void PlayBiteSound()
    {
        // HINT: Looks like a good place to play the bite sound
    }
}
