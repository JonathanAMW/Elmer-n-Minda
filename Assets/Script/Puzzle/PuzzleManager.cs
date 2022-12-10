using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PuzzleManager : MonoBehaviour
{
    private Queue<string> commands = new Queue<string>();
    
    [Header("Player")]
    [SerializeField] GameObject player;
    public bool isJalan;
	public bool isHadapKanan, isGrounded;
    private Rigidbody2D rb2d;
    private Animator anim;
    private Collider2D playerCollision;

    [Header("Buat Ngecek Ground")]
    //Buat Cek Ground
    [SerializeField] Transform pengecekanGround;
    [SerializeField] float radiusPengecekanGround;
    [SerializeField] LayerMask layerGround;

    [Header("Queue GameObject")]
    public List<GameObject> QueueBlock = new List<GameObject>();
    //public GameObject[] QueueBlock;
    public List<GameObject> QueueBlockButton = new List<GameObject>();
    //public GameObject[] QueueBlockButton;
    [SerializeField] private TextMeshProUGUI runCommandCount;
    [SerializeField] private TextMeshProUGUI coinCount;

    [Header("Option")]
    [SerializeField] private float delay; //lama per gerakan dieksekusi
    private int MAX; //maksimal ukuran/size queue
    public int maxRunCommands; //maksimal run command
    private float moveDistance = 1; //jarak dari starting point

    public bool puzzleIsRunning;
    Inventaris inventaris;

    void Start()
    {
        //animasi
        anim = player.GetComponent<Animator>();
        rb2d = player.GetComponent<Rigidbody2D>();
        isJalan = false; isGrounded = true;
        anim.SetBool("isJalan", isJalan);
        anim.SetBool("isGrounded", isGrounded);

        //stats
        maxRunCommands = 4;
        MAX = QueueBlock.Count;

        //inventaris 
        inventaris = GameObject.Find("Inventaris").GetComponent<Inventaris>(); 
        TampilInventaris();
    
        //block queue dihilangkan
        for(int i=0; i<QueueBlock.Count; i++)
        {
            //QueueBlock[i].SetActive(false);
            QueueBlock[i].transform.localScale = Vector3.zero; //agar dialog box menghilang ketika awal
        }
        puzzleIsRunning = false;
        
    }

    public void JalanKanan()
    {
        if(maxRunCommands != 0 && !puzzleIsRunning)
        {
            if(commands.Count < MAX) //mulai dari 1, maka dari itu indexnya program dibawah dikurang satu
            {
                commands.Enqueue("jalanKanan");
                //QueueBlock[commands.Count-1].SetActive(true); //program ini dst ^
                QueueBlock[commands.Count-1].LeanScale(Vector3.one, 0.5f);
                QueueBlock[commands.Count-1].GetComponentInChildren<Text>().text = "Kanan";

            }else if(commands.Count > MAX){
                return;
            }
        }
    }

    public void JalanKiri()
    {
        if(maxRunCommands != 0 && !puzzleIsRunning)
        {
            if(commands.Count < MAX)
            {
                commands.Enqueue("jalanKiri");
                //QueueBlock[commands.Count-1].SetActive(true);
                QueueBlock[commands.Count-1].LeanScale(Vector3.one, 0.5f);
                QueueBlock[commands.Count-1].GetComponentInChildren<Text>().text = "Kiri";

            }else if(commands.Count > MAX){
                return;
            }
        }
    }
    
    /*
    public void Lompat()
    {
        if(commands.Count < MAX)
        {
            commands.Enqueue("lompat");
            //QueueBlock[commands.Count-1].SetActive(true);
            QueueBlock[commands.Count-1].LeanScale(Vector3.one, 0.5f);

            QueueBlockText[commands.Count-1].text = "Lompat";

        }else if(commands.Count > MAX){
            return;
        }
    }*/

    public void RunCommand()
    {   if(maxRunCommands != 0 && !puzzleIsRunning && commands.Count != 0)
        {
            maxRunCommands--;
            puzzleIsRunning = true;
            for(int i=0; i<QueueBlockButton.Count; i++) //button dihilaangkan agar tidak ditekan2 sehingga malah mengganggu queue
            {
                QueueBlockButton[i].LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo(); //agar dialog box menghilang ketika awal
                //QueueBlockButton[i].SetActive(false);
            }
            StopAllCoroutines();
            StartCoroutine(CompileCommand());
            rb2d.velocity = new Vector2(0, 0);
        }
    }


    protected IEnumerator CompileCommand()
    {
        for(int i=commands.Count, j=0; i>0; i--, j++)
        {
            float moveDistanceX=0, moveDistanceY=0;
            if(commands.Peek() == "jalanKanan")
            {
                moveDistanceX = moveDistance;
                Menghadap(1);
                isJalan = true;
            }else if(commands.Peek() == "jalanKiri"){
                moveDistanceX = -moveDistance;
                Menghadap(-1);
                isJalan = true;
            /*}else if(commands.Peek() == "lompat"){
                moveDistanceY = moveDistance;
                isGrounded = false;
                isJalan = false;*/
            }else{
                Debug.Log("command gagal");
            }

            commands.Dequeue();
            QueueBlock[j].LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo(); //agar dialog box menghilang ketika awal
            //QueueBlockText[j].text = ""; //teks dikosongkan               
            QueueBlock[j].GetComponentInChildren<Text>().text = "";//teks dikosongkan


            Vector2 startPosition = player.transform.position;
            Vector2 endPosition = new Vector2(startPosition.x+moveDistanceX, startPosition.y+moveDistanceY);
            
            float totalMovementTime = delay; //the amount of time you want the movement to take
            float currentMovementTime = 0f;//The amount of time that has passed
            while (Vector2.Distance(player.transform.localPosition, endPosition) > 0) 
            {   
                if(!isGrounded)
                    break;
                //anim.SetBool("isGrounded", isGrounded);
                anim.SetBool("isJalan", isJalan);
                player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentMovementTime / totalMovementTime);  
                currentMovementTime += Time.deltaTime;
                yield return null;
            }
            
            
            isJalan = false;
            anim.SetBool("isJalan", isJalan);
            //anim.SetBool("isGrounded", isGrounded);
            
            //QueueBlock[j].LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo(); //agar dialog box menghilang ketika awal
            //QueueBlockText[j].text = ""; //teks dikosongkan
            //commands.Dequeue(); 
        }
        
        //setelah didequeue semua, button diaktifkan semua
        for(int i=0; i<QueueBlockButton.Count; i++)
        {
            QueueBlockButton[i].LeanScale(Vector3.one, 0.5f);
            //QueueBlockButton[i].SetActive(true); //dinonaktifkan 
        }

        puzzleIsRunning = false;

    }

    void CekGround()
	{
        //Di game object 'pengecekanGround' akan membuat sebuah lingkaran yg mana itu nanti buat deteksi ground;
        Collider2D collider = Physics2D.OverlapCircle(pengecekanGround.position, radiusPengecekanGround, layerGround);
        if (collider != null)
            isGrounded = true;
        else
            isGrounded = false;  

        anim.SetBool("isGrounded", isGrounded);
	}

    void Menghadap(int rotasi)
	{
        if(rotasi<0 && !isHadapKanan) //jika ke arah kanan tapi currentHadap == kiri
        {
            player.transform.Rotate(0f, 180f, 0);
            isHadapKanan = true;
        }
        else if(rotasi>0 && isHadapKanan) //jika ke arah kiri tapi currentHadap == kanan
        {
            player.transform.Rotate(0f, 180f, 0);
            isHadapKanan = false;
        }
	}


    void Update()
    {
        TampilInventaris();
        CekGround();

        if(maxRunCommands != 0)
        {
            //if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !puzzleIsRunning)
            //    Lompat();
            
            if((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && !puzzleIsRunning)
                JalanKiri();
            
            if((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && !puzzleIsRunning)
                JalanKanan();
            
            if(Input.GetKeyDown(KeyCode.R) && !puzzleIsRunning && commands.Count != 0)
            {
                RunCommand();
                //maxRunCommands--;
                //puzzleIsRunning = true;
            }
        }

    }
    

    void TampilInventaris()
    {
        runCommandCount.SetText(" x " + maxRunCommands.ToString());
        coinCount.SetText(" x " + inventaris.koin.ToString());
    }

    private void cekMenangKalah()
    {
        if(!puzzleIsRunning && maxRunCommands <= 0)
        {
            
        }
    }
    
}
