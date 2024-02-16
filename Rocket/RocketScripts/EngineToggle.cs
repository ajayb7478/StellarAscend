using UnityEngine;
using TMPro;

public class EngineToggle : MonoBehaviour
{ // this is the text box component that you want to toggle
    public TextMeshProUGUI buttonText;
    public bool textState; // this variable stores the state of the text box component

    private void Start()
    {
 // initialize the variable with the initial state of the text box component
        UpdateButtonText();
    }



    public void ToggleTextState()
    {
        textState = !textState;
        UpdateButtonText();
        Debug.Log(textState);
    }



    void UpdateButtonText()
    {
        buttonText.text = textState ? "Engine is Off" : "Engine is On"; // update the button text
    }

}
