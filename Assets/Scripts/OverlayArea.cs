using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OverlayArea : MonoBehaviour
{
    public TilemapRenderer overlay;
    public bool start;
    public List<Collider2D> extraColliders;
    private string oldSortingLayer;

    private void Start() {
        overlay.enabled = false;
        if (extraColliders != null) {
            foreach (var col in extraColliders) {
                col.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (start) {
            if (collision.GetComponent<PlayerController>() != null && overlay != null) {
                overlay.enabled = true;
                SpriteRenderer plrSprite = collision.GetComponent<SpriteRenderer>();

                oldSortingLayer = plrSprite.sortingLayerName;
                plrSprite.sortingLayerName = overlay.sortingLayerName;
            } else if (overlay == null) {
                Debug.Log("No Tilemap Renderer set!");
            }

            if (extraColliders != null) {
                foreach (var col in extraColliders) {
                    col.enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!start) {
            overlay.enabled = false;
            if (collision.GetComponent<PlayerController>() != null) {
                collision.GetComponent<SpriteRenderer>().sortingLayerName = oldSortingLayer;
            }

            if (extraColliders != null) {
                foreach (var col in extraColliders) {
                    col.enabled = false;
                }
            }
        }
    }
}
