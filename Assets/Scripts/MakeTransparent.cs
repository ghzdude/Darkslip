using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTransparent : MonoBehaviour {
    private SpriteRenderer render;
    public Color transparent;
    private Color oldColor;
    private bool isTransparent;

    // Start is called before the first frame update
    void Start() {
        render = GetComponent<SpriteRenderer>();
        oldColor = render.color;
    }
    private void Update() {
        if (isTransparent) {
            ChangeColorAlpha(transparent);
        }
        if (!isTransparent) {
            ChangeColorAlpha(oldColor);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision != null && collision.GetComponent<PlayerController>() != null) {
            isTransparent = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision != null && collision.GetComponent<PlayerController>() != null) {
            isTransparent = false;
        }
    }

    public void ChangeColorAlpha(Color color) {
        render.color = Color.Lerp(render.color, color, Time.deltaTime);
    }
}
