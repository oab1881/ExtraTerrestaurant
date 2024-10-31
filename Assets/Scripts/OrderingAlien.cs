using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Linq;

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
    bool hasApproached = false; //Bool tracking if the alien has finished approaching
    private bool movedAway = true;  // Bool to track if the alien is done moving offscreen
    //************************************************************

    //Variables for Dialogue
    public RectTransform dialogueBox;    //Reference to Dialogue box
    public TextMeshProUGUI textComponent;   //Reference to TMPro component
    public string textFileName;  // Name of the text file in Resources folder
    private List<string> lines;  //Lines of text
    public float textSpeed;     //Tracks speed of text
    private int index;
    private bool hasClicked = false; //Tracks if Mouse has been clicked
    //************************************************************

    //Variables for Sticky Note
    public GameObject orderScreen;
    public GameObject kitchenScreen;

    // The order to be made and scored
    public List<List<string>> order = new List<List<string>>();

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

        // Load dialogue lines from the text file
        LoadTextFile(textFileName);

        Invoke("StartDialogue", 6.5f); //Invoke the StartDialogue coroutine (6.5 second delay)
        //StartDialogue();
    }

    void Update()
    {
        if (!hasApproached) //If alien hasn't finished approaching player
        {
            //Debug.Log("I'm being called!");
            Lerp(); //Keep calling LERP
        }
        else
        {
            //Ordering Code will go here****

            //Show text box and dialog
            textComponent.gameObject.SetActive(true);
            dialogueBox.gameObject.SetActive(true);

            if (Input.GetMouseButtonDown(0) && !hasClicked) // If left mouse click pressed and hasn't clicked yet
            {
                //Debugging
                Debug.Log("Mouse Clicked");
                //**********
                hasClicked = true;

                //Create Sticky Note with Order
                CreateOrder();   //Hardcoded Position for now****

                //Clicking will read in next Lines
                //if(textComponent.text == lines[index])
                //{
                //    NextLine(); //Move to the next line
                //}
                //else
                //{

                //Clicking will stop dialog and finish the text
                StopAllCoroutines();
                textComponent.text = lines[index];  //Get current line and fill it out

                // Set the flag to start moving the alien offscreen
                movedAway = false;
                distanceCovered = 0.0f; //Reset DistanceCovered
            }

            //Move Alien Offscreen by calling LerpAway
            if (!movedAway)
            {
                //Debug.Log("I'm being called!");

                //Remove text box
                textComponent.gameObject.SetActive(false);
                dialogueBox.gameObject.SetActive(false);

                //LERP the alien offscreen by calling LErpAway
                LerpAway();
            }
        }
    }

    //Method to load in order files from the resources folder
    void LoadTextFile(string fileName)
    {
        TextAsset textFile = Resources.Load<TextAsset>(fileName);
        if (textFile != null)
        {
            lines = textFile.text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList<string>();
        }
        else
        {
            Debug.LogError("Text file not found in Resources folder.");
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

    //Method that will move the customer offscreen after ordering
    void LerpAway()
    {
        //Reset variables
        approachSpeed = 2.0f;
        float fractionOfJourney = 0.0f;
        Vector3 newStartPoint = endPoint;
        Vector3 newEndPoint = new Vector3(-20, 0, 0);

        // Calculate the journey length for LerpAway
        float journeyLengthAway = Vector3.Distance(newStartPoint, newEndPoint);

        // If the alien hasn't reached the stopping point, continue moving away
        if (distanceCovered < journeyLengthAway)
        {
            distanceCovered += approachSpeed * Time.deltaTime;
            fractionOfJourney = distanceCovered / journeyLengthAway;

            // Lerp the alien's position between newStartPoint and newEndPoint
            alienInstance.transform.position = Vector3.Lerp(newStartPoint, newEndPoint, fractionOfJourney);
        }

        // Check if the alien has reached the new end point
        if (fractionOfJourney >= 1.0f)
        {
            movedAway = true;  // The alien has finished moving away
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

    //Method to move to the next Line (sorta scrapped for now)
    void NextLine()
    {
        //If there is text.. write it
        if(index < lines.Count - 1)
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

    //This method displays an order, taking the capital letters from the NPC's order and displaying them on the translator screen
    void CreateOrder()
    {
        // Instantiate the sticky note at the given position and with no rotation
        //GameObject stickyNoteInstance = Instantiate(stickyNote, position, Quaternion.identity);

        // Get the TextMeshProUGUI component of the orderScreen and set its text to the capital letters from the first line
        TextMeshProUGUI orderText = orderScreen.GetComponentInChildren<TextMeshProUGUI>();  //Displayed in order screen
        TextMeshProUGUI orderText2 = kitchenScreen.GetComponentInChildren<TextMeshProUGUI>();   //Displayed on kitchen screen

        if (orderText != null && orderText2 != null)
        {
            string firstLine = lines[0]; //Read in first line
            string capitalLetters = GetCapitalLetters(firstLine);   //Take only capital letters
            orderText.text = capitalLetters;    //Display order on order screen
            orderText2.text = capitalLetters;   //Display order on kitchen screen

            // Adds to the order list using the second line of the given text file
            // Note: will only work with unprepared food items for now. Need to update
            // it later to work with more than that. - Chris
            List<string> orderComponents = lines[1].Split(",").ToList();
            foreach (string c in orderComponents)
            {
                List<string> com = new List<string>() { c };
                order.Add(com);
            }
        }
        else
        {
            Debug.LogWarning("Order screen does not have a TextMeshProUGUI component.");
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
