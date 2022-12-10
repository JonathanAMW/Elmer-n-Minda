using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBoxManager : MonoBehaviour
{
    public Image playerImage;
    public TextMeshProUGUI playerName;
    public GameObject playerBoxName;
    public Image npcImage;
    public TextMeshProUGUI npcName;
    public GameObject npcBoxName;
    public TextMeshProUGUI messageText;
    public RectTransform backgroundBox;
    [SerializeField] private AudioSource actorVoice;

    //public AudioSource nextMessageSound;
    private float delay = 0.03f;

    //public List<Message> currentMessages = new List<Message>();
    Message[] currentMessages;
    Actor[] currentActors;
    int activeMessages = 0; //menghitung jumlah dialog

    public static bool isActive = false; //mengecek dialog sedang berjalan atau tidak
    public static bool isLoop = false; //mengecek dialog sedang diwrite/ketik atau tidak agar bisa diskip

    void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero; //agar dialog box menghilang ketika awal
    }

    void Update()
    {   /*
        if(isActive && (Input.GetKeyDown(KeyCode.W))) //berganti dialog
        { //ngebug, game malah nge-next message pertama
            NextMessage();
        }*/
        if(isActive && !isLoop && (Input.GetKeyDown(KeyCode.W))) //berganti dialog
        { //temp fix, kekurangan harus memakai tombol untuk skip dan next message
            NextMessage();
        }
    }

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessages = 0;
        isActive = true;
        
        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.5f);
        
    }

    void DisplayMessage() //menampilkan dialog
    {
        //Message messageToDisplay = currentMessages[activeMessages];
        //messageText.text = messageToDisplay.message;

        //StopCoroutine(WriteText()); //menghentikan coroutine apabila player skip dialog
        StopAllCoroutines(); //menghentikan coroutine apabila player skip dialog
        messageText.text = ""; //membersihkan message sebelumnya    
        StartCoroutine(WriteText()); //memulai writetext

        /*Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;

        AnimateText();*/
    }

    public void NextMessage()
    {
        isLoop = false;
        activeMessages++;
        if(activeMessages < currentMessages.Length)
        {
            DisplayMessage();
            //Time.timeScale = 0;
        } 
        else 
        {
            isActive = false;
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            //Time.timeScale = 1;
        }
    }

    void AnimateText() //menganimasi text
    {
        LeanTween.textAlpha(messageText.rectTransform, 0, 0); //(texttujuan, )
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }

    

    protected IEnumerator WriteText() //overall mengetik teks satu2
    {
        Message messageToDisplay = currentMessages[activeMessages]; //memilih message saat ini
        
        Actor actorToDisplay = currentActors[messageToDisplay.actorId]; //memilih aktor saat ini
        actorVoice.clip = actorToDisplay.nextMessageSound;
        if(messageToDisplay.actorId == 0)
        {
            npcBoxName.SetActive(false);
            npcImage.enabled = false;

            playerName.text = actorToDisplay.name;
            playerImage.sprite = actorToDisplay.image; //menampilkan sprite actor saat ini
            
            playerBoxName.SetActive(true);
            playerImage.enabled = true;
        }
        else
        {
            playerBoxName.SetActive(false);
            playerImage.enabled = false;

            npcName.text = actorToDisplay.name; //menampilkan actor saat ini
            npcImage.sprite = actorToDisplay.image; //menampilkan sprite actor saat ini

            npcBoxName.SetActive(true);
            npcImage.enabled = true;
        }

        AnimateText();

        isLoop = true;
        for(int i = 0; i < messageToDisplay.message.Length; i++)
        {
            if(!isLoop) //mengecek apakah dialog sedang ditulis
            {
                break;
            }
            else
            {
                messageText.text += messageToDisplay.message[i]; 
                actorVoice.Play();
                yield return new WaitForSeconds(delay);
                //yield return new WaitForSecondsRealtime(delay);
            }
        }
    }
}