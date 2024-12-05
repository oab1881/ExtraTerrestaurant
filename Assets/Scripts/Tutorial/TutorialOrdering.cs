using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine.SceneManagement;

// Script by Owen Beck//
// In Order1.txt, the format for an order should be as follows:
// X,X Y | Where X is a food item and Y is a prep method
// There should be no spaces unless a food item has a prep method attached to it
public class TutorialOrdering : MonoBehaviour
{
    //Testing Script that will spawn in an alien prefab and have it slowly move toward the camera

    //Lists to store all aliens and orders
    public List<GameObject> alienPrefabs; // List of alien prefabs
    public List<string> orderFileNames;   // List of order text file names
    private GameObject currentAlien;    //Currently spawned alien
    //***********************************************************

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

    public RectTransform dialogueBox2;    //Reference to Dialogue box
    public TextMeshProUGUI textComponent2;   //Reference to TMPro component
    public GameObject orderArrow;
                                            //************************************************************

    // Order Management
    public GameObject orderScreen;
    public GameObject kitchenScreen;
    public List<List<string>> order;
    private int tempScore = 0;

    // External References
    public TutorialScore scoring; // Reference to ScoreManager
    [SerializeField]
    TutorialConveyor conveyorButton;

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
        orderArrow.gameObject.SetActive(false);

        // Load dialogue lines from the text file
        LoadTextFile(textFileName);

        Invoke("StartDialogue", 2.5f); //Invoke the StartDialogue coroutine (6.5 second delay)
        //StartDialogue();
    }

    public void Update()
    {
        //Logic for spawning new alien for infinite looping
        if (TutorialConveyor.CurrentScore > tempScore) //if score increases spawn new alien
        {
            Debug.Log("THIS IS BEING CALLED");
            // Swicth scenes with a delay
            tempScore++;    //increase temp score
            StartCoroutine(SwitchScene());
        }
        if (!hasApproached) // If alien hasn't finished approaching player
        {
            Lerp(); // Keep calling LERP
        }
        else
        {
            // Show text box and dialogue
            textComponent.gameObject.SetActive(true);
            dialogueBox.gameObject.SetActive(true);
            dialogueBox2.gameObject.SetActive(true);
            textComponent2.gameObject.SetActive(true);

            if (Input.GetMouseButtonDown(0)) // If left mouse click is pressed
            {
                //Debug.Log("Mouse Clicked");
                hasClicked = true;

                if (textComponent.text == lines[index])
                {
                    // If the current text is fully displayed, move to the next line
                    NextLine();
                }
                else
                {
                    // Finish the current line immediately
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
            }
        }
    }
    private IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(3f); // Delay by 3 seconds
        //Load the Gameplay scene
        SceneManager.LoadScene("Gameplay");
    }

    //Method that handles all alien info
    public void SpawnNewAlien()
    {
        // Reset State
        hasApproached = false;
        movedAway = true;
        distanceCovered = 0.0f;

        // Choose random alien and order file
        int randomIndex = Random.Range(0, alienPrefabs.Count);
        GameObject alienPrefab = alienPrefabs[randomIndex];
        string orderFile = orderFileNames[randomIndex];

        // Spawn alien
        if (currentAlien != null)
        {
            Destroy(currentAlien);
        }
        currentAlien = Instantiate(alienPrefab, startPoint, Quaternion.identity);
        currentAlien.transform.localScale = Vector3.one * startSize;

        // Load order file
        LoadTextFile(orderFile);

        // Set up journey
        journeyLength = Vector3.Distance(startPoint, endPoint);

        // Delay dialogue to match alien's approach
        Invoke(nameof(StartDialogue), 6.5f);
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
        if (index < lines.Count - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else if (index == lines.Count - 1) // Explicitly handle last line
        {
            Debug.Log($"Last line reached: Index = {index}, Lines count = {lines.Count}");
            CreateOrder();
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Unexpected state in NextLine(). Index exceeded lines.Count.");
        }
    }

    //This method displays an order, taking the capital letters from the NPC's order and displaying them on the translator screen
    void CreateOrder()
    {
        // Retrieve the TextMeshProUGUI components from both screens
        TextMeshProUGUI orderText = orderScreen?.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI orderText2 = kitchenScreen?.GetComponentInChildren<TextMeshProUGUI>();

        if (orderText == null)
        {
            Debug.LogWarning("Order screen is missing a TextMeshProUGUI component or orderScreen is not assigned.");
        }
        if (orderText2 == null)
        {
            Debug.LogWarning("Kitchen screen is missing a TextMeshProUGUI component or kitchenScreen is not assigned.");
        }

        //THIS IS CAUSING A BUG WITH SCORING FOR TUTORIAL
        if (orderText != null && orderText2 != null)
        {
            string order2 = "A"; // Hardcoded order for tutorial purposes
            List<List<string>> orderComp = new List<List<string>>();
            List<string> com = new List<string>();

            com.Add(order2);
            orderComp.Add(com);

            // Display the hardcoded order on both screens
            orderText.text = order2;
            orderText2.text = order2;
            order = orderComp;

            Debug.Log("Order successfully displayed on both screens.");
            orderArrow.gameObject.SetActive(true);
            
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
