using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private int health;
    public int maxHealth;
    public AudioClip destroyed;
    public AudioClip hit;
    public Sprite[] damagedSprites;
    private SpriteRenderer sprite;
    private DialogueManager DialogueManager;
    public Transform nextMarker;
    public bool allowNextMarker;

    // Start is called before the first frame update
    void Start() {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        DialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        health = maxHealth;
    }

    public void DecHealth(int hit, AudioSource src) {
        health -= hit;

        if (health <= 0) { // On Death
            src.PlayOneShot(destroyed, DialogueManager.GetSFXSliderValue());
            gameObject.SetActive(false);

            if (allowNextMarker) {
                src.GetComponent<NavigationController>().SetTarget(nextMarker);
            }
            return;
        } else { // Still Alive
            sprite.sprite = damagedSprites[health - 1];
            src.PlayOneShot(this.hit, DialogueManager.GetSFXSliderValue());
        }
    }

    private int CalculateDamagedState() {
        float healthRatio = health / maxHealth;
        return Mathf.RoundToInt(healthRatio * damagedSprites.Length - 1);
    }
}
