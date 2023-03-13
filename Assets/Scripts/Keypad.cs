using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    public Transform KeyPad;
    private List<Button> KeyPadButtons = new List<Button>();
    public Button btn_cancel;
    public Button btn_enter;
    public Button btn_close;
    public Text EntryField;
    private Terminal sendingTerminal;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < KeyPad.childCount; i++) {
            Button button = KeyPad.GetChild(i).GetComponent<Button>();
            
            if (button != null)
                KeyPadButtons.Add(button);

        }
    }

    public void OpenCodeEntry(Terminal t) {
        sendingTerminal = t;
        ResetField();
        gameObject.SetActive(true);
        for (int i = 0; i < KeyPadButtons.Count; i++) {
            string num = KeyPadButtons[i].transform.GetChild(0).GetComponent<Text>().text;
            KeyPadButtons[i].onClick.AddListener(() => EnterChar(num));
        }

        btn_cancel.onClick.AddListener(ResetField);
        btn_enter.onClick.AddListener(() => CheckInput(EntryField.text));
        btn_close.onClick.AddListener(CloseCodeEntry);
        Time.timeScale = 0;
    }

    public void CloseCodeEntry() {
        gameObject.SetActive(false);
        for (int i = 0; i < KeyPadButtons.Count; i++) {
            KeyPadButtons[i].onClick.RemoveAllListeners();
        }
        btn_cancel.onClick.RemoveAllListeners();
        btn_enter.onClick.RemoveAllListeners();
        btn_close.onClick.RemoveAllListeners();
    }

    private void EnterChar(string s) {
        if (EntryField.text.Length < 4) {
            EntryField.text += s;
        }
    }

    public void ResetField() {
        EntryField.text = "";
    }

    public void CheckInput(string userInput) {
        if (sendingTerminal.correctCode.Equals(userInput)) {
            sendingTerminal.Fire(true);
            CloseCodeEntry();

        } else {
            ResetField();
        }
    }
}
