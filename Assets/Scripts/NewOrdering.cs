using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

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
    private int lastOrderIndex = -1; // Tracks the last order to avoid repetition

    // External References
    //[SerializeField]
    //ConveyorButton scoring; // Reference to scoring in ConveyorButton

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
        // Get the correct folder based on the player's score to access the appropriate rank of orders
        string rankFolder = GetRankFolder(ConveyorButton.CurrentScore);

        // Load all available order files from the specified folder, converting them to a list of filenames
        List<string> availableOrders = Resources.LoadAll<TextAsset>($"{rankFolder}")
                                          .Select(o => o.name)
                                          .ToList();

        // Check if there are any available orders in the folder
        if (availableOrders.Count > 0)
        {
            int orderIndex;
            do
            {
                // Randomly select an order index from the available orders
                orderIndex = Random.Range(0, availableOrders.Count);
            }
            while (orderIndex == lastOrderIndex); // Ensure the selected order is not the same as the last one

            // Update the last order index to avoid repetition in the next spawn
            lastOrderIndex = orderIndex;

            // Set the selected order's file name for loading dialogue later
            textFileName = availableOrders[orderIndex];

            // Randomly select an alien prefab from the list of available prefabs
            alienPrefab = alienPrefabs[Random.Range(0, alienPrefabs.Count)];

            // Instantiate the selected alien at the starting point with an initial rotation
            alienInstance = Instantiate(alienPrefab, startPoint, Quaternion.identity);

            // Set the initial scale of the alien to the defined start size
            alienInstance.transform.localScale = Vector3.one * startSize;

            // Calculate the total distance the alien needs to travel to reach the player
            journeyLength = Vector3.Distance(startPoint, endPoint);

            // Reset flags and variables for alien movement and interaction
            hasApproached = false;    // Alien hasn't reached the player yet
            movedAway = true;         // Alien is ready to move offscreen after interaction
            distanceCovered = 0.0f;   // Reset the distance traveled

            // Load the dialogue file for the selected order from the specified folder
            LoadTextFile($"{rankFolder}/{textFileName}");

            // Start the dialogue interaction after a delay of 5.5 seconds
            Invoke(nameof(StartDialogue), 5.5f);
        }
        else
        {
            // Log an error message if no orders are found in the specified rank folder
            Debug.LogError($"No available orders found in rank folder: {rankFolder}");
        }
    }

    //Method to grab the right Rank folder of orders based on player's currentScore
    string GetRankFolder(int score)
    {
        if (score < 3) return "Rank1";
        if (score >= 3 && score < 6) return "Rank2";
        return "Rank3";
    }

    // Loads the content of a text file into the dialogue lines
    void LoadTextFile(string fileName)
    {
        // Log the path being used to load the file for debugging purposes
        Debug.Log($"Trying to load file from: Resources/{fileName}");

        // Load the text file from the Resources folder using the specified path
        TextAsset textFile = Resources.Load<TextAsset>(fileName);

        // Check if the file was successfully loaded
        if (textFile != null)
        {
            Debug.Log($"Successfully loaded file: {fileName}");

            // Split the text into lines and store them in the 'lines' list
            lines = textFile.text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
        }
        else
        {
            // Log an error if the file was not found
            Debug.LogError($"Text file not found at: Resources/{fileName}");
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
