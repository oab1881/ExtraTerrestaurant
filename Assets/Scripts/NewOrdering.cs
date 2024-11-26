using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Linq;

// Script by Owen Beck
// Handles alien spawning, ordering, dialogue, and cleanup for infinite gameplay
public class NewOrdering : MonoBehaviour
{
    // Alien approach and movement variables
    public GameObject alienPrefab; // Reference to the alien prefab to spawn
    public float startSize = 0.1f; // Initial size of the alien
    public float endSize = 1.0f;   // Final size of the alien when it reaches the player
    public float approachSpeed = 1.0f; // Speed at which the alien approaches the player
    public Vector3 startPoint = new Vector3(0, 0, 10); // Starting point far from the player
    public Vector3 endPoint = new Vector3(0, 0, 0);    // Stopping point near the player

    private GameObject alienInstance; // Current alien instance
    private float journeyLength; // Total distance for alien movement
    private float distanceCovered = 0.0f; // Distance covered by the alien
    private bool hasApproached = false; // Tracks if the alien has finished approaching
    private bool movedAway = true;  // Tracks if the alien has left the screen

    // Dialogue-related variables
    public RectTransform dialogueBox;    // Reference to the dialogue box UI element
    public TextMeshProUGUI textComponent; // Text component for displaying dialogue
    private string textFileName;  // Name of the text file in Resources folder
    private List<string> lines;  // Dialogue lines from the text file
    public float textSpeed; // Speed of typing effect for dialogue
    private int index; // Index of the current dialogue line
    private bool hasClicked = false; // Tracks if the mouse has been clicked during dialogue

    // Order display variables
    public GameObject orderScreen;  // UI element for displaying the order
    public GameObject kitchenScreen; // Kitchen monitor UI for the order
    public List<List<string>> order = new List<List<string>>(); // Parsed customer order

    // Alien pool for infinite spawning
    public List<GameObject> alienPrefabs; // List of alien prefabs for random selection
    public List<string> orderFiles; // List of text files for random orders

    // External References
    [SerializeField]
    ScoreManager scoreManager; // Reference to ScoreManager

    void Start()
    {
        // Start the spawning process with the first alien
        SpawnNewAlien();
    }

    public void Update()
    {
        // Handle alien movement and interaction logic
        if (!hasApproached)
        {
            // If the alien has not yet approached the player, move it closer
            Lerp();
        }
        else
        {
            // Display dialogue and order logic
            textComponent.gameObject.SetActive(true); // Show dialogue text
            dialogueBox.gameObject.SetActive(true);  // Show dialogue box

            if (Input.GetMouseButtonDown(0) && !hasClicked) // If left mouse button is clicked
            {
                hasClicked = true;

                // Display order on screen(UI)
                CreateOrder();

                // Finish current dialogue immediately
                StopAllCoroutines();
                textComponent.text = lines[index];

                // Prepare for the alien to leave
                movedAway = false;
                distanceCovered = 0.0f; // Reset distance covered for departure
            }

            // Move alien offscreen after interaction
            if (!movedAway)
            {
                // Hide dialogue UI
                textComponent.gameObject.SetActive(false);
                dialogueBox.gameObject.SetActive(false);

                // Lerp alien away from the screen
                LerpAway();
            }
        }
    }

    // Spawns a new alien and loads its dialogue and order
    public void SpawnNewAlien()
    {
        // Randomly select an alien prefab and order file
        alienPrefab = alienPrefabs[UnityEngine.Random.Range(0, alienPrefabs.Count)];
        textFileName = orderFiles[UnityEngine.Random.Range(0, orderFiles.Count)];
        Debug.Log(textFileName);

        //^^DEV NOTE: Maybe we should rework this so that the previous order will not be chosen 2x in a row ^^

        // Instantiate the alien and initialize its position and size
        alienInstance = Instantiate(alienPrefab, startPoint, Quaternion.identity);
        alienInstance.transform.localScale = Vector3.one * startSize;

        // Calculate the journey length for approach movement
        journeyLength = Vector3.Distance(startPoint, endPoint);

        // Reset flags and variables
        hasApproached = false;
        movedAway = true;
        distanceCovered = 0.0f;

        //Initialize textComponent
        dialogueBox.gameObject.SetActive(false);
        textComponent.gameObject.SetActive(false); //Set to false until alien has approached you
        textComponent.text = string.Empty;

        // Load dialogue lines from the selected text file
        LoadTextFile(textFileName);

        // Delay before starting the dialogue
        Invoke("StartDialogue", 5.5f);
    }

    // Loads the content of a text file into the dialogue lines
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

    // Moves the alien closer to the player
    void Lerp()
    {
        float fractionOfJourney = 0.0f;

        // Continue moving if the alien hasn't reached the end point
        if (distanceCovered < journeyLength)
        {
            distanceCovered += approachSpeed * Time.deltaTime;
            fractionOfJourney = distanceCovered / journeyLength;

            // Interpolate position and size
            alienInstance.transform.position = Vector3.Lerp(startPoint, endPoint, fractionOfJourney);
            float currentSize = Mathf.Lerp(startSize, endSize, fractionOfJourney);
            alienInstance.transform.localScale = Vector3.one * currentSize;
        }

        // Check if the alien has finished approaching
        if (fractionOfJourney >= 1.0f)
        {
            hasApproached = true;
        }
    }

    // Moves the alien offscreen after the order interaction
    void LerpAway()
    {
        float fractionOfJourney = 0.0f;
        Vector3 newStartPoint = endPoint; // Start from the stopping point
        Vector3 newEndPoint = new Vector3(-20, 0, 0); // Move offscreen

        // Calculate distance for departure movement
        float journeyLengthAway = Vector3.Distance(newStartPoint, newEndPoint);

        if (distanceCovered < journeyLengthAway)
        {
            distanceCovered += approachSpeed * Time.deltaTime;
            fractionOfJourney = distanceCovered / journeyLengthAway;

            // Interpolate position for departure
            alienInstance.transform.position = Vector3.Lerp(newStartPoint, newEndPoint, fractionOfJourney);
        }

        // Once offscreen, destroy the alien 
        if (fractionOfJourney >= 1.0f)
        {
            movedAway = true;
            Destroy(alienInstance);
            //SpawnNewAlien();  (could spawn an alien after destroying it)
        }
    }

    // Starts typing the dialogue line by line
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    // Coroutine for typing dialogue one character at a time
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c; // Add the next character
            yield return new WaitForSeconds(textSpeed); // Wait before adding another character
        }
    }

    // Displays the alien's order on the appropriate UI screens
    void CreateOrder()
    {
        // Get text components from order and kitchen screens
        TextMeshProUGUI orderText = orderScreen.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI orderText2 = kitchenScreen.GetComponentInChildren<TextMeshProUGUI>();

        if (orderText != null && orderText2 != null)
        {
            string firstLine = lines[0]; // Read the first line from the order file
            string capitalLetters = GetCapitalLetters(firstLine); // Extract uppercase letters (order code)

            // Display order on UI screens
            orderText.text = capitalLetters;
            orderText2.text = capitalLetters;

            // Parse the full order from the second line
            List<string> orderComponents = lines[1].Split(",").ToList();
            foreach (string component in orderComponents)
            {
                List<string> com = new List<string>();
                foreach (string c in component.Split(" "))
                {
                    com.Add(c);
                }
                order.Add(com);
            }
        }
        else
        {
            Debug.LogWarning("Order screen does not have a TextMeshProUGUI component.");
        }
    }

    // Extracts uppercase letters from a string
    string GetCapitalLetters(string input)
    {
        string result = "";
        foreach (char c in input)
        {
            if (char.IsUpper(c))
            {
                result += c;
            }
        }
        return result;
    }
}
