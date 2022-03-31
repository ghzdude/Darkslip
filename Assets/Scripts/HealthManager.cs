using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int health;
    public AudioClip destroyed;
    public AudioClip hit;
    public Sprite[] damagedSprites;
    private SpriteRenderer sprite;
    public int markerIndex;
    public bool allowMarkerIndex;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecHealth(int hit, AudioController audio)
    {
        health -= hit;
        switch (health)
        {
            case 3:
                sprite.sprite = damagedSprites[0];
                break;
            case 2:
                sprite.sprite = damagedSprites[1];
                break;
            case 1:
                sprite.sprite = damagedSprites[2];
                break;
            case 0:
                audio.PlayClip(destroyed, 0.66f);
                gameObject.SetActive(false);
                if (allowMarkerIndex)
                {
                    audio.GetComponent<NavigationController>().SetMarkerIndex(markerIndex);
                    foreach (var item in GameObject.FindGameObjectsWithTag("Destructible"))
                    {
                        if (item.GetComponent<HealthManager>().markerIndex == markerIndex)
                        {
                            item.GetComponent<HealthManager>().allowMarkerIndex = false;
                        }
                    }
                }
                break;
            default:
                break;
        }
        audio.PlayClip(this.hit, 0.66f);
        
    }
}
