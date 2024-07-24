using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour
{
    public List<GameObject> lastTile;
    public GameObject firstTile;
    public GameObject Tiles;
    public GameObject Coins;
    public bool allow_subpaths = false;
    public int paths_per_subpath = 5;

    int selected_main_last_tile = 0;

    List<int> selected_tiles;

    public GameObject GetLastTile()
    {
        if (lastTile.Count == 0) return null;
        if (lastTile.Count == 1) return lastTile[0];
        else
        {
            selected_tiles = new List<int>();
            selected_main_last_tile = Random.Range(0, lastTile.Count);
            selected_tiles.Add(selected_main_last_tile);
            return lastTile[selected_main_last_tile];
        }
    }

    public List<GameObject> getNonMainEndTiles(GameObject endTile)
    {
        List<GameObject> tiles = lastTile;
        tiles.Remove(endTile);

        return tiles;
    }


    public void SpawnCoins()
    {
        // spawn a coin on every tile in Tiles object
        foreach (Transform tile in Tiles.transform)
        {
            if (tile.gameObject.tag == "Tile")
            {
                GameObject coin = Instantiate(Resources.Load("Prefabs/Coin", typeof(GameObject)), tile.position + new Vector3(0, 1f, 0), Quaternion.Euler(90, 0, 0)) as GameObject;
                coin.transform.SetParent(Coins.transform);
            }
        }
    }

    public void RotateLastTilesTo(int angle)
    {
        foreach (GameObject tile in lastTile)
        {
            tile.transform.Rotate(0, angle, 0);
        }
    }

    bool IsObjectAtPosition(Transform Tile)
    {
        int tiles = 0;
        //Collider[] intersecting = Physics.OverlapSphere(tile.transform.position, 1.5f);
        Collider[] intersecting = Physics.OverlapBox(Tile.position, Tile.localScale/2.2f, Quaternion.Euler(Tile.eulerAngles));
        //GameObject overlap_vis = Instantiate(Resources.Load("Prefabs/Overlapformation", typeof(GameObject)), Tile.position + new Vector3(0, 1, 0), Quaternion.Euler(Tile.eulerAngles)) as GameObject;
        //overlap_vis.transform.localScale = (Tile.localScale/2.2f) * 2;
        //overlap_vis.transform.SetParent(Tile);
        foreach (Collider tile in intersecting)
        {
            if(tile.gameObject.CompareTag("Tile"))
            {
                tiles++;
            }
        }
        return tiles > 0;
    }

    public bool IsPathOverLapping()
    {
        foreach (Transform tile in Tiles.transform)
        {
            if (IsObjectAtPosition(tile)) 
            {
                Destroy(gameObject);
                return true;
            }
        }
        return false;
    }

    public void setAllTilesTag(string tag)
    {
        foreach (Transform tile in Tiles.transform)
        {
            tile.tag = tag;
        }
    }

}
