using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Handles how the button acts when it is clicked.
 * 
 * FOR NOW: This function is the scoring function. In future sprints, the scoring will be
 * handled by a class that is not this.
 */

public class ConveyorButton : MonoBehaviour
{
    public Sprite pressedButton;
    public Sprite unpressedButton;
    public SpriteRenderer spriteRenderer;

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
                scoring.DisplayScore();
                Destroy(tray);
                tray = Instantiate(newTray);
                tray.transform.position = new Vector3(24.42f,1.14f, 0.0f);
                scoring.CleanPlate(tray);
                rb2D = tray.GetComponent<Rigidbody2D>();
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
}
