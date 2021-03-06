﻿using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;
    public Image enemyHealth;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(.5f, .5f, .5f, 0.1f);
    Color enemyColor;
    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
        
        currentHealth = startingHealth;
        //enemyColor = capsuleCollider.material.color;
    }


    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }

    }


    public void TakeDamage (int amount, Vector3 hitPoint, Vector3 hitDirection)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;
        float maxHealth = (float)startingHealth;
        enemyHealth.fillAmount = currentHealth/ maxHealth;
   
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        

        if(currentHealth <= 0)
        {
            gameObject.transform.position += 2.0f * hitDirection;
            Death ();
        }
        else
        {
           // gameObject.renderer.material.color = Color.Lerp(flashColor, enemyColor, flashSpeed * Time.deltaTime);
        }
    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;
        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
        
    }


    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }
}
