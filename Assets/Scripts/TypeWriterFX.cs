using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using TMPro;

public class TypeWriterFX : MonoBehaviour
{
    private char[] stored;
    public float revealSpeed;
    public AudioClip typeSound;
    [HideInInspector] public bool completed;
    public bool running;

    public void Run(Text text) {
        completed = false;
        stored = text.text.ToCharArray();
        Debug.Log("TFX is running: " + stored.ArrayToString());
        text.text = "";

        if (!running) {
            StartCoroutine(
                StartType(text)
            );
        }
    }

    public void Stop() {
        StopCoroutine("StartType");
        running = false;
    }

    IEnumerator StartType(Text txt) {
        int waitIndex;
        
        running = true;
        for (int i = 0; i < stored.Length; i++) {
            if (stored[i] == '\\' ) {
                waitIndex = int.Parse(stored[i + 1].ToString() + stored[i + 2].ToString());
                i += 2;
                for (int j = 0; j < waitIndex; j++) {
                    yield return new WaitForSecondsRealtime(revealSpeed / 100);
                }
                continue;
            }
            if (stored[i] != ' ' || i % 2 == 0) {
                Managers.GetMusic().GetAudioController().PlayClip(typeSound);
            }
            txt.text += stored[i];
            yield return new WaitForSecondsRealtime(revealSpeed / 100);
        }
        completed = true;
        running = false;
        yield return null;
    }
}
