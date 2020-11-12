using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public float heartbeatHealthThreshold = 50f;

    Animator anim;
    AudioSource playerAudio;
    AudioSource heartbeatAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
  

    bool isDead;
    bool damaged;
    int startDelay;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        AudioSource[] clips = GetComponents<AudioSource>();
        playerAudio = clips[0];
        heartbeatAudio = clips[1];
        heartbeatAudio.Stop();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }


    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

        healthSlider.value = currentHealth;

        if (currentHealth < heartbeatHealthThreshold * startingHealth / 100f)
        {
            heartbeatAudio.Play();
        }
        else
        {
            heartbeatAudio.Stop();
        }
        Camera.main.SendMessage("DoShake");
        playerAudio.Play ();

        if (currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;
        startDelay = 5;//animation delay
        playerShooting.DisableEffects ();
        new WaitForSeconds(startDelay);
        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();
        heartbeatAudio.Stop();
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }


    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }
}
