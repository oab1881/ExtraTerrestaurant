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

    // Starting position: X:24.42, Y:1.14
    // Conveyor position: X:15.3, Y:-3.5
    [SerializeField]
    GameObject tray;
    [SerializeField]
    GameObject newTray;

    private Rigidbody2D rb2D;
    private bool trayMoving = false;
    private bool buttonActive = true;
    private float conveyorSpeed = 0.5f;
    private float time = 3.0f;

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
        rb2D = tray.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (trayMoving)
        {
            rb2D.AddForce(-transform.up * conveyorSpeed);
            time -= Time.deltaTime;

            if (time < 0.0f)
            {
                trayMoving = false;

                rb2D.velocity = Vector3.zero;
                bool isPerfect = scoring.DisplayScore();
                if (isPerfect) 
                { 
                    IncreaseScore();    //if plate is correct, increase score ticker
                }
                Destroy(tray);
                tray = Instantiate(newTray);
                tray.transform.position = new Vector3(15.3f, -3.5f, 0.0f);
                rb2D = tray.GetComponent<Rigidbody2D>();
                scoring.CleanPlate(tray);
                ChangeSprite(unpressedButton);

                buttonActive = true;
            }
        }

    }
    private void OnMouseDown()
    {
        if (buttonActive)
        {
            tray.transform.position = new Vector3(15.3f, -3.5f, 0.0f);
            ChangeSprite(pressedButton);
            MoveTray();
        }
    }

    private void ChangeSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }
    private void MoveTray()
    {
        trayMoving = true;
        buttonActive = false;
    }

    public void IncreaseScore()
    {
        CurrentScore++; // Use the setter to increase the score
        Debug.Log("Score updated! Current Score: " + currentScore);
    }
}
