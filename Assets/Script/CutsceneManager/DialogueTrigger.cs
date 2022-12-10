using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    //public List<Message> messages = new List<Message>();
    public Message[] messages;
    public Actor[] actors;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueBoxManager>().OpenDialogue(messages, actors);
    }


    //test
    [Header("NPC")]
    //public DialogueTrigger trigger;
    public bool isPadaNpc = false; //static berguna untuk memerikksa di script lain tanpa mengambil script ini
    public GameObject tandaSeru; //tanda seru
    public Transform tandaSeruTransform; //tanda seru posisi

    [Header("Player")]
    [SerializeField] GameObject player;
    [SerializeField] Transform playerTransform;
    
    void Start()
    {
        tandaSeru = gameObject.transform.Find("tandaSeru").gameObject; //mencari object pada child
        tandaSeruTransform = tandaSeru.transform; 
        tandaSeruTransform.position = gameObject.transform.position + new Vector3(0f, 1.8f, 0f); //supaya tanda seru berada ditengah atas npc
        
        player = GameObject.Find("Player");
        playerTransform = player.GetComponent<Transform>();
        tandaSeru.SetActive(false);
    }

    void Update()
    {
        LookAt();
        if(isPadaNpc && !DialogueBoxManager.isActive && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            StartDialogue();
            isPadaNpc = false; //supaya kondisi ini tidak terpenuhi lagi hingga player keluar-masuk dari radius npc
            tandaSeru.SetActive(false);
        }
    }

    void LookAt() //supaya npc melihat ke pemain
    {
        Vector3 direction = playerTransform.position - transform.position;
        if(direction.x > 0f) transform.rotation = Quaternion.Euler(0, 180, 0);
        else if(direction.x < 0f) transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") == true && gameObject.tag != "Npc") //untuk dialog npc unskipable
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            //collision.enabled = false;
            StartDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //untuk dialog npc skipable
    {
        if(collision.gameObject.CompareTag("Player") == true)
        {
            isPadaNpc = true;
            tandaSeru.SetActive(true);
            tandaSeruTransform.position = gameObject.transform.position + new Vector3(0f, 1.8f, 0f); //supaya tanda seru berada ditengah npc        
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//untuk dialog npc skipable
    {
        if(collision.gameObject.CompareTag("Player") == true)
        {
            isPadaNpc = false;
            tandaSeru.SetActive(false);
        }
    }
    //test bagian akhir
}

[System.Serializable] 
public class Message //informasi dialog
{
    public int actorId; //id dari pembicara yang ingin dipanggil
    public string message; //isi dialog
}

[System.Serializable]
public class Actor //informasi pembicara dialog
{
    public string name; //nama pembicara
    //public Sprite sprite; //sprite pembicara
    public Sprite image;
    public AudioClip nextMessageSound; //suara
}
