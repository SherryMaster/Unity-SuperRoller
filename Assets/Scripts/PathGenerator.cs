using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public GameObject Parent;
    public GameObject referenceObject;
    public float timeOffset = 0.4f;
    public float distanceBetweenTiles = 4.0F;

    [Space]
    [Header("Path Properties")]
    public int startPathNum = 1;
    public int number_of_paths = 3;
    public int path_length = 50;
    public int checkpoint_every_n_path = 15;
    public int max_overlaps = 100;
    public bool is_sub_path = false;

    [Space]
    [Header("Sub Path Properties")]
    public bool checkpoints_allowed_in_subpaths = false;
    public int subpath_reduced_ratio = 2;


    private int path_left_till_checkpoint = 0;
    private int path_left_to_spawn = 0;
    private int overlaps_left = 0;
    private Vector3 previousTilePosition, previousTileRotation;
    private float startTime;
    private Vector3 direction, mainDirection = new Vector3(0, 0, 1);
    GameObject path;
    GameObject prev_path;
    GameObject LastTile;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.levelCompleteEvent.AddListener(onLevelComplete);
        resetStartConfig();
    }

    void resetStartConfig()
    {
        LastTile = referenceObject;
        previousTilePosition = LastTile.transform.position;
        previousTileRotation = LastTile.transform.eulerAngles;
        startTime = Time.time;
        path_left_till_checkpoint = checkpoint_every_n_path;
        path_left_to_spawn = path_length;
        overlaps_left = max_overlaps;

        if (is_sub_path)
        {
            if (!checkpoints_allowed_in_subpaths)
                path_left_till_checkpoint = -1;

            path_length /= subpath_reduced_ratio;
            path_left_to_spawn /= subpath_reduced_ratio;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            EventManager.Instance.LevelGenerarionRestart();
            retryGeneration();
        }
        if(path_left_to_spawn < 0) return;
        if (Time.time - startTime > timeOffset)
        {
            Vector3 spawnPos = previousTilePosition + distanceBetweenTiles * direction;
            startTime = Time.time;
            path = GetAPath(spawnPos);
            bool spawned = SpawnPath(path);
            if (!spawned)
            {
                return;
            }
        }
    }

    bool SpawnPath(GameObject path)
    {
        int dir_rand = Random.Range(0, 4);
        path.transform.SetParent(Parent.transform);

        if (dir_rand == 0) path.transform.Rotate(0, 0, 0);
        else if (dir_rand == 1) path.transform.Rotate(0, 90, 0);
        else if (dir_rand == 2) path.transform.Rotate(0, 180, 0);
        else path.transform.Rotate(0, 270, 0);

        path.transform.eulerAngles += LastTile.transform.eulerAngles;

        if (path.GetComponent<PathScript>().IsPathOverLapping())
        {
            if (!is_sub_path)
            {
                Debug.Log("OverLapping: " + overlaps_left.ToString() + " Left");
                overlaps_left--;
                if (overlaps_left == 0)
                {
                    EventManager.Instance.LevelGenerarionRestart();
                    retryGeneration();
                }
            }
            return false; // didn't spawned due to overlapping
        }
        overlaps_left = max_overlaps;
        path.GetComponent<PathScript>().setAllTilesTag("Tile");
        
        LastTile = path.GetComponent<PathScript>().GetLastTile();
        previousTilePosition = LastTile.transform.position;
        previousTileRotation = LastTile.transform.eulerAngles;

        if (path.GetComponent<PathScript>().allow_subpaths)
        {
            List<GameObject> otherLastTiles = path.GetComponent<PathScript>().getNonMainEndTiles(LastTile);

            foreach (GameObject otherTile in otherLastTiles)
            {
                path.AddComponent<PathGenerator>();

                path.GetComponent<PathGenerator>().Parent = Parent;
                path.GetComponent<PathGenerator>().referenceObject = otherTile;
                path.GetComponent<PathGenerator>().timeOffset = timeOffset;
                path.GetComponent<PathGenerator>().distanceBetweenTiles = distanceBetweenTiles;
                path.GetComponent<PathGenerator>().startPathNum = startPathNum;
                path.GetComponent<PathGenerator>().number_of_paths = number_of_paths;
                path.GetComponent<PathGenerator>().path_length = path_length;
                path.GetComponent<PathGenerator>().checkpoint_every_n_path = checkpoint_every_n_path;
                path.GetComponent<PathGenerator>().is_sub_path = true; // changeable
                path.GetComponent<PathGenerator>().checkpoints_allowed_in_subpaths = checkpoints_allowed_in_subpaths;
                path.GetComponent<PathGenerator>().subpath_reduced_ratio = subpath_reduced_ratio;
            }
        }

        if (path_left_till_checkpoint == 0)
        {
            path_left_till_checkpoint = checkpoint_every_n_path;
        }
        path_left_till_checkpoint--;
        path_left_to_spawn--;
        return true;
    }
    
    GameObject GetRandomNormalPath(Vector3 position)
    {
        int path_num = Random.Range(startPathNum, number_of_paths + 1);
        return Instantiate(Resources.Load("Prefabs/Paths/Path" + path_num.ToString(), typeof(GameObject)), position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }

    GameObject GetAPath(Vector3 spawnPos)
    {
        if (path_left_till_checkpoint == 0)
        {
            return Instantiate(Resources.Load("Prefabs/CheckPointPath", typeof(GameObject)), spawnPos, Quaternion.Euler(0, 0, 0)) as GameObject;
        }
        else if (path_left_to_spawn == 0 && !is_sub_path)
        {
            return Instantiate(Resources.Load("Prefabs/WinPlatform", typeof(GameObject)), spawnPos, Quaternion.Euler(0, 0, 0)) as GameObject;
        }
        else
        {
            return GetRandomNormalPath(spawnPos);
        }
    }

    GameObject getDifferentLastTile(GameObject path)
    {
        return path.GetComponent<PathScript>().GetLastTile();
    }

    void retryGeneration()
    {
        foreach (Transform child in Parent.transform)
        {
            Destroy(child.gameObject);
        }
        resetStartConfig();
    }

    void onLevelComplete()
    {
        EventManager.Instance.LevelGenerarionRestart();
        retryGeneration();
    }
}

