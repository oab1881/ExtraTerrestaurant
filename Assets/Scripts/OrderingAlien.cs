using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Script by Owen Beck//
public class OrderingAlien : MonoBehaviour
{
    //Testing Script that will spawn in an alien prefab and have it slowly move toward the camera

    //Variables for LERP
    public GameObject alienPrefab;  // The alien prefab to spawn
    public float startSize = 0.1f;  // Initial size of the alien when far away
    public float endSize = 1.0f;    // Final size of the alien when it reaches the stopping point
    public float approachSpeed = 1.0f;  // Speed at which the alien approaches
    public Vector3 startPoint = new Vector3(0, 0, 10);  // Starting point far away from the camera
    public Vector3 endPoint = new Vector3(0, 0, 0);     // Point where the alien will stop
    private GameObject alienInstance;  // Instance of the alien prefab
    private float journeyLength;
    private float distanceCovered = 0.0f;
    //************************************************************

    //Variables for Dialogue
    public RectTransform dialogueBox;    //Reference to Dialogue box
    public TextMeshProUGUI textComponent;   //Reference to TMPro component
    public string[] lines;  //Lines of text
    public float textSpeed;     //Tracks speed of text
    private int index;
    //************************************************************

    //Variables for Sticky Note
    public GameObject stickyNote;


    bool hasApproached = false; //Bool tracking if the alien has finished approaching

    void Start()
    {
        // Spawn the alien at the starting point and set its initial scale
        alienInstance = Instantiate(alienPrefab, startPoint, Quaternion.identity);
        alienInstance.transform.localScale = Vector3.one * startSize;

        // Calculate the total distance between the start and end points
        journeyLength = Vector3.Distance(startPoint, endPoint);

        //Initialize textComponent
        dialogueBox.gameObject.SetActive(false);
        textComponent.gameObject.SetActive(false); //Set to false until alien has approached you
        textComponent.text = string.Empty;
        Invoke("StartDialogue", 8);
        //StartDialogue();
    }

    void Update()
    {
        if (!hasApproached)
            Lerp();
        else
        {
            //Ordering Code will go here****
            textComponent.gameObject.SetActive(true);
            dialogueBox.gameObject.SetActive(true);
            if (Input.GetMouseButtonDown(0)) //If left mouse click pressed**
            {
                //Debugging
                Debug.Log("Mouse Clicked");
                //**********

                //Create Sticky Note with Order
                CreateStickyNote(new Vector3(1, 1, -1));

                //Clicking will read in next Lines
                if(textComponent.text == lines[index])
                {
                    NextLine(); //Move to the next line
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = lines[index];  //Get current line and fill it out
                }

            }
        }
    }

    //Helper Method that will Lerp the alien towards the player
    void Lerp()
    {
        float fractionOfJourney = 0.0f;

        // If the alien hasn't reached the stopping point, continue approaching
        if (distanceCovered < journeyLength)
        {
            // Move the alien closer to the end point
            distanceCovered += approachSpeed * Time.deltaTime;
            fractionOfJourney = distanceCovered / journeyLength;

            // Lerp the position between startPoint and endPoint
            alienInstance.transform.position = Vector3.Lerp(startPoint, endPoint, fractionOfJourney);

            // Lerp the size of the alien between startSize and endSize
            float currentSize = Mathf.Lerp(startSize, endSize, fractionOfJourney);
            alienInstance.transform.localScale = Vector3.one * currentSize;
        }

        // Check if the alien has reached the end point
        if (fractionOfJourney >= 1.0f)
        {
            hasApproached = true;  // The alien has finished approaching
        }
    }

    //Method to begin dialogue
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    //Coroutine to type each character of the dialogue
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())  //Takes string and breaks down into char array
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    //Method to move to the next Line
    void NextLine()
    {
        //If there is text.. write it
        if(index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        //else if there is no text to write
        else
        {
            gameObject.SetActive(false);    //set textBox inactive
        }
    }

    //This method creates a sticky note taking the capital letters from the NPC's order and displaying them on the note
    void CreateStickyNote(Vector3 position)
    {
        // Instantiate the sticky note at the given position and with no rotation
        GameObject stickyNoteInstance = Instantiate(stickyNote, position, Quaternion.identity);

        // Get the TextMeshProUGUI component of the sticky note and set its text to the capital letters from the first line
        TextMeshProUGUI stickyNoteText = stickyNoteInstance.GetComponentInChildren<TextMeshProUGUI>();

        if (stickyNoteText != null)
        {
            string firstLine = lines[0]; // Get the first line of dialogue
            string capitalLetters = GetCapitalLetters(firstLine);  // Extract only the capital letters

            stickyNoteText.text = capitalLetters;  // Set the text to the filtered capital letters
        }
        else
        {
            Debug.LogWarning("Sticky note does not have a TextMeshProUGUI component.");
        }
    }

    // Helper method to extract only capital letters from a string
    string GetCapitalLetters(string input)
    {
        string result = "";

        foreach (char c in input)
        {
            if (char.IsUpper(c))  // Check if the character is uppercase
            {
                result += c;
            }
        }

        return result;
    }
}
