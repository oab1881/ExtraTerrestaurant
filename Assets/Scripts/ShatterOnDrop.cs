using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterOnDrop : MonoBehaviour
{
    public GameObject[] chunkPrefabs; // Array to hold your chunk prefabs
    public float shatterHeight = 5f;  // Height threshold for shattering
    private bool isFalling = false;   // Track if the ingredient is falling
    private float fallStartHeight;    // Track height when the ingredient starts falling

    void Update()
    {
        // Check if the ingredient is above the shatter height and start tracking its fall
        if (transform.position.y > shatterHeight && !isFalling)
        {
            isFalling = true;
            fallStartHeight = transform.position.y;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the ingredient collided with the floor and if it was dropped from the shatter height
        if (collision.gameObject.CompareTag("Floor") && isFalling)
        {
            if (fallStartHeight - transform.position.y >= shatterHeight)
            {
                Shatter();
            }
            isFalling = false;  // Reset the fall state
        }
    }

    void Shatter()
    {
        // Destroy the original ingredient
        Destroy(gameObject);

        // Instantiate 3-4 chunks at the ingredient's position with random rotations
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, chunkPrefabs.Length);
            GameObject chunk = Instantiate(chunkPrefabs[randomIndex], transform.position, Random.rotation);

            // Optionally add some force to scatter the chunks
            Rigidbody rb = chunk.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(100f, transform.position, 2f);
            }
        }
    }
}
