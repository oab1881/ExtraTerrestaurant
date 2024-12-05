using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Handles how the button acts when it is clicked.
 */

public class ConveyorButton : MonoBehaviour
{
    public Sprite pressedButton;
    public Sprite unpressedButton;
    public SpriteRenderer spriteRenderer;

    //tracks currentScore for player progression
    private static int currentScore = 0;

    [SerializeField]
    public ScoreManager scoring;

    // Conveyor position: X:15.3, Y:-3.5
    [SerializeField]
    GameObject tray;
    [SerializeField]
    GameObject newTray;

    private bool isScoring = false;
    private float conveyorSpeed = 4.0f;
    private float time = 4.0f;

    // Public property with getter and setter for score, can be used for progression and new customer spawning
    public static int CurrentScore
    {
        get { return currentScore; }
        set
        {
            if (value < 0)
            {
                Debug.LogWarning("Score cannot be negative!");
                return;
            }

            currentScore = value;
            //Debug.Log("Score updated! Current Score: " + currentScore);
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Accomplishes the same as the old "Update" function did while it was here, only as a coroutine.
    private IEnumerator HandleAll()
    {
        Vector3 startingPos = tray.transform.position;
        Vector3 finalPos = tray.transform.position + (-transform.up * conveyorSpeed);

        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            tray.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bool isPerfect = scoring.DisplayScore();
        if (isPerfect)
        {
            IncreaseScore();    //if plate is correct, increase score ticker
        }

        CleanAll();
        ChangeSprite(unpressedButton);
    }

    private void OnMouseDown()
    {
        if (isScoring)
        {
            Debug.Log("Button still inactive! Wait a bit, will ya?");

        }
        else
        {
            isScoring = true;
            tray.transform.position = new Vector3(15.3f, -3.5f, 0.0f);
            ChangeSprite(pressedButton);
            StartCoroutine(HandleAll());
        }
    }

    // Resets everything after the scoring button is pressed
    private void CleanAll()
    {
        Debug.Log("Cleaning...");
        Destroy(tray);
        tray = Instantiate(newTray);
        tray.transform.position = new Vector3(15.3f, -3.5f, 0.0f);
        scoring.CleanPlate(tray);
    }

    private void ChangeSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }

    public void EnableSwitch()
    {
        isScoring = false;
    }
    public void IncreaseScore()
    {
        CurrentScore++; // Use the setter to increase the score
        Debug.Log("Score updated! Current Score: " + currentScore);
    }
}
