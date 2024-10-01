using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score;
    private float timer;
    private Dictionary<string, List<string>> ingredients;

    private void Start()
    {
        // Create the Dictionary of ingredients
        // Hardcoded ingredient values
        List<string> tentacle = new List<string>() { "A",};
        List<string> leaf = new List<string>() { "B",};
        List<string> mineral = new List<string>() { "C",};
        List<string> gooball = new List<string>() { "D",};
        List<string> sheet = new List<string>() { "E",};

        // Add ingredient values to ingredients dictionary
        ingredients.Add("tentacle", tentacle);
        ingredients.Add("leaf", leaf);
        ingredients.Add("mineral", mineral);
        ingredients.Add("gooball", gooball);
        ingredients.Add("sheet", sheet);

    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

    }
}
