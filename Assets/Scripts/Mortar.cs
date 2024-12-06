using UnityEngine;

public class Mortar : MonoBehaviour
{
    int pestleCt = 0; // # of times pestle smashed

    Storage storageScipt;

    ParticleSystem particles;

    private void OnCollisionEnter2D(Collision2D collision)
    {
         // do nothing
    }
}