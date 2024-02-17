using UnityEngine;
using TMPro;
using System.Collections;
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
        //Debug.Log(textState);
    }



    void UpdateButtonText()
    {
        StartCoroutine(UpdateButtonTextWithDelay());
    }

    IEnumerator UpdateButtonTextWithDelay()
    {
        yield return new WaitForSeconds(1.2f);
        buttonText.text = textState ? "Cut-Off" : "Run Ignition"; // update the button text
    }


}
