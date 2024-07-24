using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    [Space]
    [Header("Materials")]
    public Material OpaqueTileMat;
    public Material FadeTileMat;

    //private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();
    private List<Renderer> HitTiles = new();

    private void Start()
    {
        EventManager.Instance.levelGenRestart.AddListener(onLevelReset);
        transform.eulerAngles = new Vector3(90, 0, 0);
    }

    void Update()
    {
        /*Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        */

        Vector3 directionToPlayer = target.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, directionToPlayer, distanceToPlayer);

        // Store the current set of hit renderers
        HashSet<Renderer> hitRenderers = new HashSet<Renderer>();

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "Player") continue;
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (renderer.gameObject.CompareTag("Tile"))
                {
                    hitRenderers.Add(renderer);
                    renderer.material = FadeTileMat;
                    HitTiles.Add(renderer);
                }
                /* If this is the first time hitting the renderer, store its original color
                if (!originalColors.ContainsKey(renderer))
                {
                    originalColors[renderer] = renderer.material.color;
                }
                 Change the transparency of the object
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.25f);
                */

            }
        }

        /* Revert the color of any renderers that were not hit this frame
        foreach (var pair in originalColors)
        {
            if (!hitRenderers.Contains(pair.Key))
            {
                try { pair.Key.material.color = pair.Value; }
                catch { Debug.Log("Error in camera follow"); }
            }
        }
        */

        foreach (Renderer tile in HitTiles)
        {
            if (!hitRenderers.Contains(tile))
            {
                tile.material = OpaqueTileMat;
            }
        }
    }

    void onLevelReset()
    {
        HitTiles.Clear();
    }
}
