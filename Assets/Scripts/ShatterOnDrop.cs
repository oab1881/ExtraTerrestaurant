using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterOnDrop : MonoBehaviour
{
    [SerializeField]
    GameObject particleSystemCrystal;
    public GameObject[] chunkPrefabs; // Array to hold chunk prefabs
    public float shatterThreshold = 1f;  // Distance the object needs to fall to shatter
    public float startTrackingHeight = 1f; // Height threshold to start tracking the fall
    private bool isFalling = false;   // Track if the object is falling
    private float fallStartHeight;    // Track height when the object starts falling

    void Update()
    {
        // Check if the object is above the tracking height and hasn't started falling
        if (transform.position.y > startTrackingHeight && !isFalling)
        {
            isFalling = true;
            fallStartHeight = transform.position.y;
        }
        // If the object falls below the starting height, reset the falling state
        else if (transform.position.y <= startTrackingHeight && isFalling)
        {
            isFalling = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object collided with the floor and fell from a high enough distance
        if (collision.gameObject.CompareTag("floor") && !isFalling)
        {
            if (fallStartHeight - transform.position.y >= shatterThreshold)
            {
                Shatter();
            }
            isFalling = false;  // Reset the fall state
        }
    }

    void Shatter()
    {
        GameObject particles = Instantiate(particleSystemCrystal);
        particleSystemCrystal.transform.position = gameObject.transform.position;
        Destroy(gameObject);

        /*

        // Instantiate 3-4 chunks at the object's position
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("I should be spawning chunks");
            GameObject chunk = Instantiate(chunkPrefabs[i], transform.position, Quaternion.identity);

            // Optionally add some force to scatter the chunks
            Rigidbody2D rb = chunk.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate a direction vector pointing away from the original object's position
                Vector2 explosionDirection = ((Vector2)chunk.transform.position - (Vector2)transform.position);
                explosionDirection += new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)).normalized; // Add slight randomness

                float explosionForce = Random.Range(50f, 300f); // Adjust force for noticeable explosion
                rb.AddForce(explosionDirection * explosionForce);
            }

            // Destroy each chunk after 3 seconds
            Destroy(chunk, 3f);
        }
        */

        particles.GetComponent<ParticleSystem>().Play();
    }
}
