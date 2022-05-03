using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour {
    public GameObject Enemy;
    private Vector3 original;
    public float shakeAmount;
    public float shakeTimer;
    public float shakeCooldown;
    private float cooldown;
    private bool shaking;
    private PlayerController Player;
    public AudioClip breakClip;
    public AudioClip rattle;
    private AudioSource src;
    private bool triggered;

    private void Start() {
        original = transform.position;
        src = GetComponent<AudioSource>();
        cooldown = 0;
        Player = Managers.GetPlayerController();
        triggered = false;
    }

    private void Update() {
        if (gameObject.activeInHierarchy && !shaking && cooldown <= 0) {
            StartCoroutine(Shake(shakeAmount / 100));
        } else if (shakeCooldown > 0) {
            cooldown -= Time.deltaTime;
        }

        if (VectorMath.CalculateVector(transform.position, Player.transform.position).magnitude < src.minDistance && !triggered) {
            src.PlayOneShot(breakClip, Managers.GetDialogueManager().GetSFXSliderValue());
            StopCoroutine(Shake(shakeAmount));
            InstantiateEnemy();
            // gameObject.SetActive(false);
            StartCoroutine(Disable());
            triggered = true;
        }
    }

    public void InstantiateEnemy() {
        Instantiate(Enemy, transform.position, transform.rotation);
    }

    IEnumerator Shake(float shakeAmount) {
        float timer = shakeTimer;
        shaking = true;
        
        if (VectorMath.CalculateVector(transform.position, Player.transform.position).magnitude < src.maxDistance) {
            src.PlayOneShot(rattle, Managers.GetDialogueManager().GetSFXSliderValue());
            // Debug.Log("shake noise");
        }

        while (timer > 0) {
            transform.position = original + 
                new Vector3(
                    Random.Range(-shakeAmount, shakeAmount), 
                    Random.Range(-shakeAmount, shakeAmount));

            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        transform.position = original;
        shaking = false;
        cooldown = Random.Range(shakeCooldown / 2, shakeCooldown * 2);
        yield return null;
    }

    IEnumerator Disable() {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(breakClip.length);
        gameObject.SetActive(false);
        yield return null;
    }
}
