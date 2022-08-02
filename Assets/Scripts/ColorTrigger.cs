using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTrigger : MonoBehaviour {
    public Color targetColor;
    public int fadingSteps;
    public SpriteRenderer render;
    private Vector4 colorStep;
    private bool changing;

    // Start is called before the first frame update
    void Start()
    {
        changing = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!changing)
            StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor() {
        // Debug.Log("change color");
        colorStep = (Vector4)targetColor - (Vector4)render.color;

        Debug.Log(
            "Current Color: " + render.color +
            "\nTarget Color: " + targetColor +
            "\nColor Step: " + colorStep
            );
        // colorStep.Normalize();
        // colorStep *= 255;

        changing = true;
        for (int i = 0; i < fadingSteps; i++) {
            render.color += (Color)colorStep / fadingSteps;
            yield return new WaitForSeconds(1f/fadingSteps);
        }
        changing = false;
        yield return null;
    }
}
