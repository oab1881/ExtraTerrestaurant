using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject foodItem;

    private void OnMouseDown()
    {
        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPos.z = 0;

        Instantiate(foodItem, spawnPos, transform.rotation);
    }
}
