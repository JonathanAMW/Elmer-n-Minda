using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCollision : MonoBehaviour
{
    [Header("Resources")]
    public Sprite openedChest;
    public Sprite leverActive;
    public List<GameObject> teleportFrom = new List<GameObject>();
    public List<GameObject> teleportTo = new List<GameObject>();
    
    Inventaris inventaris;
    PuzzleManager puzzleManager;
    LevelManager levelManager;

    private void Start() 
    {
        inventaris = GameObject.Find("Inventaris").GetComponent<Inventaris>();
        puzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
        levelManager = FindObjectOfType<LevelManager>();
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Chest" && !puzzleManager.puzzleIsRunning)
        {
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = openedChest; //ganti sprite
            collision.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
            collision.enabled = false;
            int Koin = Random.Range(50, 200);
            inventaris.koin += Koin;
        }

        if(collision.gameObject.tag == "Lever" && !puzzleManager.puzzleIsRunning)
        {
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = leverActive;
            collision.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
            collision.enabled = false;
            puzzleManager.maxRunCommands += 1;
        }

        if(collision.gameObject.tag == "PortalTeleport" && !puzzleManager.puzzleIsRunning)
        {
            puzzleManager.maxRunCommands += 1;
            int index = teleportFrom.IndexOf(collision.gameObject);
            if(index < teleportFrom.Count) //supaya keliatan apakah jumlah from-to sama
            {
                //Transform nextPortal = teleportTo[index].GetComponent<Transform>();
                //Transform playerTransform = GameObject.Find("Player").GetComponent<Transform>();
                //playerTransform.transform.position = nextPortal.transform.position;
                GameObject.Find("Player").GetComponent<Transform>().position = teleportTo[index].GetComponent<Transform>().position;
            }
        }

        if (collision.gameObject.layer == 13 && !puzzleManager.puzzleIsRunning)
            levelManager.LevelSelesai();
    }
}
