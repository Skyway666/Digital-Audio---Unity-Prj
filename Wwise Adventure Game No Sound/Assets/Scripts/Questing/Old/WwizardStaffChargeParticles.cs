////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class WwizardStaffChargeParticles : MonoBehaviour
{
    [Header("Line Renderer Settings")]
    public LineRenderer lineRenderer;

    public float refreshSpeed = 0.1f;

    public AnimationCurve yOffsets;
    public int extraSteps = 5;
    public float animationStrength = 1f;
    public float animationSpeed = 5f;

    public GameObject wwizard;



    [Header("Source")]
    public AudioSource SFX_Player;

    [Header("Clips")]
    public AudioClip charging;
    public AudioClip uncharging;

    [Header("End Point Settings")]
    public GameObject endPoint;
    public GameObject wwizard;

    public GameObject chargeDoneParticles;

    #region private variables
    private IEnumerator chargeRoutine;
    #endregion



    void OnEnable()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        if (endPoint != null)
        {
            SFX_Player.clip = charge;
            SFX_Player.Play();
            // HINT: Wizard staff charge particles appear, you may want to play the appropiate sound effect here
            chargeRoutine = AnimatePoints();
            StartCoroutine(chargeRoutine);

            lineRenderer.enabled = true;
            ParticleSystem[] p = endPoint.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < p.Length; i++)
            {
                p[i].Play();
            }
        }
    }

    IEnumerator AnimatePoints()
    {
        while (true)
        {
            lineRenderer.useWorldSpace = true;
            lineRenderer.positionCount = extraSteps + 2;
            Vector3[] lrIn = new Vector3[lineRenderer.positionCount];
            lrIn[0] = transform.position;
            for (int i = 1; i < lineRenderer.positionCount - 1; i++)
            {
                float t = (float)i / (float)(lineRenderer.positionCount + 2f);
                lrIn[i] = Vector3.Lerp(transform.position, endPoint.transform.position, t);
                lrIn[i].y += yOffsets.Evaluate(t);
                lrIn[i] += new Vector3(Mathf.Sin(Time.time * animationSpeed + i) * animationStrength, -Mathf.Sin(Time.time * animationSpeed + i) * animationStrength, Mathf.Cos(Time.time * animationSpeed + i) * animationStrength);
            }
            lrIn[lineRenderer.positionCount - 1] = endPoint.transform.position;

            lineRenderer.SetPositions(lrIn);
            yield return new WaitForSeconds(refreshSpeed);
        }
    }

    void OnDisable()
    {
        // HINT: Wizard staff charge particles disappear, you may want to play the appropiate sound effect here
        SFX_Player.clip = uncharge;
        SFX_Player.Play();

        StopCoroutine(chargeRoutine);
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }

        if (endPoint != null)
        {
            ParticleSystem[] p = endPoint.GetComponentsInChildren<ParticleSystem>();
            if (p != null)
            {
                for (int i = 0; i < p.Length; i++)
                {
                    p[i].Stop();
                }
            }

            if (chargeDoneParticles != null && endPoint != null)
            {
                GameObject fx = Instantiate(chargeDoneParticles, endPoint.transform.position, Quaternion.identity, SceneStructure.Instance.TemporaryInstantiations.transform);
                Destroy(fx, 5f);
            }
        }
    }
}
