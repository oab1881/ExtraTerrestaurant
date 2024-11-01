/// === overhaul by jack 11/01 ===
/// stores objects, but which?
///  -d&d? ingredients?

using UnityEngine;

public class Storage : MonoBehaviour
{
    //private GameObject[] storedItems;
    // hard coded for each
    [SerializeField]
    public int maxCapacity;
    public int currentCap = 0;
    public bool isTray = false;

    [SerializeField]
    //private GameObject snapZone;
    private SpriteRenderer snapSprite;

    Color ogColor;
    Color hoverColor;
    Color fullColor;
    private SpriteRenderer sprRend;

    // hard coded storage positions
    [SerializeField]
    public Vector3[] positions;
    //public Vector3 position = Vector3.zero;

    void Start()
    {
        if (isTray)
            maxCapacity = 2;
        else maxCapacity = positions.Length;

        sprRend = GetComponent<SpriteRenderer>();
        // sprite color declaration
        ogColor = sprRend.color;
        hoverColor = ogColor;
        hoverColor.a = .1f; // mostly transparent
        fullColor = ogColor;
        fullColor.a = 1f;   // full opaque

        //snapSprite = snapZone.GetComponent<SpriteRenderer>();

        //positions = new Vector3[maxCapacity];
        //Debug.Log(positions[0]);
        // test code
        //positions[0] = transform.position;
    }

    public void HighlightSprite(bool highlight)
    {
        // no highlight
        if (!highlight)
        {
            sprRend.color = ogColor;
            snapSprite.enabled = false;
        }
        else
        {
            if (currentCap == maxCapacity)
                sprRend.color = fullColor;
            else
                sprRend.color = hoverColor;
        }
    }

    // highlight overload for item hover --> sprite renderer
    public void HighlightSprite(SpriteRenderer itemSprite)
    {
        if (currentCap == maxCapacity)
            sprRend.color = fullColor;
        else
        {
            snapSprite.gameObject.transform.localPosition = positions[currentCap-1];
            // get held item sprite
            snapSprite.sprite = itemSprite.sprite;
            snapSprite.enabled = true;
            HighlightSprite(true);
        }
    }
}