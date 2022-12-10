using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("STAT")]
    CharStat karakterStat;

    [Header("Pengaturan Lompat")]
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2;

    [Header("Pengaturan Dash")]
    private bool canDash = true; //penjaga agar pemain tidak bisa dash berkali2
    private bool isDashing; //penjaga supaya tidak bisa ngapa2in saat dash
    private float dashingPower = 20f; //kekuatan dash
    private float dashingTime = 0.2f; //waktu saat berada di mode dash
    private float dashingCooldown = 1f; //waktu agar canDash = true
    private TrailRenderer tr; //sebagai penanda cooldown

    [Header("Kondisi Player")]
    [SerializeField] bool isDiTuas;
    [SerializeField] bool isPencetAtas, isDiPortal, isBisaMasukPortal;

    [Header("Buat Ngecek Ground")]
    //Buat Cek Ground
    [SerializeField] Transform pengecekanGround;
    [SerializeField] float radiusPengecekanGround;
    [SerializeField] LayerMask layerGround;

    public Rigidbody2D rb2d;
    Animator anim;
    //SpriteRenderer sr;

    [Header("Respawn Point")]
    [SerializeField] GameObject respawnPos;
    [SerializeField] Sprite gantiGambarRespawn;

    [Header("Referensi")]
    [SerializeField] StageManager m_stageManager;
    UIManager m_uiManager;
    [SerializeField] HeartSystem m_heartSystem;
    [SerializeField] Animator transisi;
    [SerializeField] LevelManager levelManager;
    [SerializeField] Transform posisiDash;
    [SerializeField] MainKamera kamera;
    Inventaris inventaris;
    Portal portal;

    //[SerializeField] private GameObject dialogueManager;
    //private DialogueBoxManager dialogueManagerScript;

    void Start()
    {
        //dashing
        tr = GetComponent<TrailRenderer>();
        tr.time = dashingCooldown; //agar sebagai penanda bisa dashing lagi

        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gameObject.tag = "Player";
        m_uiManager = FindObjectOfType<UIManager>();
        //sr = GetComponent<SpriteRenderer>();
        karakterStat = GameObject.Find("StatPemain").GetComponent<CharStat>();
        inventaris = GameObject.Find("Inventaris").GetComponent<Inventaris>();
        karakterStat.darah = karakterStat.darahPenuh;
        karakterStat.zirah = karakterStat.zirahPenuh;

        if(karakterStat.nyawa > 0)
            karakterStat.isPernahMati = false;
    }

    /*void Awake()
    {
        if(dialogueManager == null)
        {
            dialogueManager = GameObject.Find("DialogueManager");
            dialogueManagerScript = dialogueManager.GetComponent<DialogueBoxManager>();
        } 

    }*/

    // Update is called once per frame
    void Update()
    {   
        if(DialogueBoxManager.isActive || DialogueBoxManager.isLoop)
        {
            rb2d.velocity = new Vector2(0, 0);
            anim.SetBool("isJalan", false);
            //anim.SetBool("isGrounded", true);
            return;
        }
        if(isDashing) return; //supaya pemain tidak bisa ngapain2 saat dash
        
        Bergerak();
        CekGround();
        BetterJump();
		if (isDiTuas)
		{
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log("Nambah Balok");
                m_stageManager.tambahBalok++;
                m_stageManager.NambahBalok(m_stageManager.tambahBalok);
            }
        }

        if(gameObject.transform.position.y < -7)
		{
            KenaDamage(karakterStat.darahPenuh);
            //JatuhJurang();
            //transform.position = respawnPos.transform.position;
		}
        //else if (gameObject.transform.position.y < -7 && karakterStat.nyawa <= 0)
        //{
        //    Kalah();
        //}

        if (isBisaMasukPortal && Input.GetKeyDown(KeyCode.W) || 
            isBisaMasukPortal && Input.GetKeyDown(KeyCode.UpArrow))
		{
            Debug.Log("Didalam Portal");
            transform.position = portal.nextPos.transform.position;
            portal.GantiStage();
            transisi.SetTrigger("Mulai");
        }
    }

    void Bergerak(){
        //Menyimpan nilai x yang di inputkan player
        float x = Input.GetAxis("Horizontal");
        //Meng-kalikan input player tadi dengan kecepatan player
        float gerak = x * karakterStat.kecepatanGerak;
        //membuat jalan
        rb2d.velocity = new Vector2(gerak, rb2d.velocity.y);

        //buat player lompat
        anim.SetBool("isGrounded", karakterStat.isGrounded);

        if ((Input.GetKeyDown(KeyCode.Z)||Input.GetKeyDown(KeyCode.Space)) && karakterStat.isGrounded)
		{
            rb2d.velocity = new Vector2(rb2d.velocity.x, karakterStat.kekuatanLompat);
        }

        /*
        if(rb2d.velocity.x > 0)
		{
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
                Debug.Log("DASH!");
                transform.position = posisiDash.position;
			}
		}*/
        
        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash) //dash
        {
            StartCoroutine(Dash());
        }
		//if (Input.GetKeyDown(KeyCode.LeftShift))
		//{
  //          if (karakterStat.isTdkHadapKanan)
  //              rb2d.velocity = new Vector2(-karakterStat.kecepatanGerak * 10, rb2d.velocity.y);
  //          else
  //              rb2d.velocity = new Vector2(karakterStat.kecepatanGerak * 10, rb2d.velocity.y);
		//}

        //jika kecepatan player > || < 0
        if (rb2d.velocity.x > 0 || rb2d.velocity.x < 0)
		{
            karakterStat.isJalan = true;
        }else if (rb2d.velocity.x == 0)
            karakterStat.isJalan = false;

        //jika mengadap kanan maka flip (kiri) jika menghadap kiri flip (kanan)
        if (x < 0 && !karakterStat.isTdkHadapKanan)
            Menghadap();
        else if (x > 0 && karakterStat.isTdkHadapKanan)
            Menghadap();

        anim.SetBool("isJalan", karakterStat.isJalan); //mainkan animasi jalan
    }

    void CekGround()
	{
        //Di game object 'pengecekanGround' akan membuat sebuah lingkaran yg mana itu nanti buat deteksi ground;
        Collider2D collider = Physics2D.OverlapCircle(pengecekanGround.position, radiusPengecekanGround, layerGround);
        if (collider != null)
            karakterStat.isGrounded = true;
        else
            karakterStat.isGrounded = false;
	}

    void Menghadap()
	{
        karakterStat.isTdkHadapKanan = !karakterStat.isTdkHadapKanan;
        transform.Rotate(0f, 180f, 0);
	}

    void BetterJump()
    {
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb2d.velocity.y > 0 && (!Input.GetKeyDown(KeyCode.Z)||!Input.GetKeyDown(KeyCode.Space)))
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public void Penyembuhan(int tambah)
    {
        karakterStat.darah += tambah;
    }

    public void KenaDamage(int kurang)
    {
        levelManager.kondisiTantangan.isTdkDMG = false;

        if (karakterStat.zirah > 0)
            karakterStat.zirah -= 1;
        else if(karakterStat.zirah <= 0)
            karakterStat.darah -= kurang;

        if (karakterStat.darah <= 0 && karakterStat.nyawa > 0)
        {
            karakterStat.darah = 0;
            karakterStat.nyawa -= 1;
            gameObject.transform.position = respawnPos.transform.position;
            Kalah();
            karakterStat.isPernahMati = true;
        }
        else if (karakterStat.darah <= 0 && karakterStat.nyawa <= 0)
            Kalah();
    }

    void Kalah()
	{
        //Debug.Log("Harusnya player mati");
        if(karakterStat.nyawa <= 0)
		{
            karakterStat.nyawa = 0;
            levelManager.GantiScene(0);
		}else if(karakterStat.nyawa > 0)
		{
            karakterStat.darah = karakterStat.darahPenuh;
            karakterStat.zirah = karakterStat.zirahPenuh;
		}
	}

    void JatuhJurang()
    {
        karakterStat.nyawa -= 1;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        anim.SetBool("isGrounded", false);
        float originalGravity = rb2d.gravityScale;
        rb2d.gravityScale = 0f; //agar gravitasi tidak mengefek saat dash
        if(!karakterStat.isTdkHadapKanan) rb2d.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        else if(karakterStat.isTdkHadapKanan) rb2d.velocity = new Vector2(transform.localScale.x * -dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime); //lamanya dash
        tr.emitting = false;
        rb2d.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown); //lamanya cooldown
        canDash = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (pengecekanGround == null)
            return;

        Gizmos.DrawWireSphere(pengecekanGround.position, radiusPengecekanGround);
    }

    private void OnTriggerStay2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Lever")
            isDiTuas = true;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "Coin")
		{
            Destroy(collision.gameObject);
            inventaris.koin+=15;
		}

        if(collision.gameObject.layer == 8)
		{
            portal = collision.gameObject.GetComponent<Portal>();
            isBisaMasukPortal = true;
		}

        if (collision.gameObject.layer == 7)
		{
            respawnPos = collision.GetComponentInChildren<Transform>().transform.gameObject;
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = gantiGambarRespawn;
            //Debug.Log("RP Set");
		}

        if (collision.gameObject.layer == 9)
		{
            UIManager.TampilBossUI(m_uiManager.uiBoss, true, m_uiManager.tembokBoss1, m_uiManager.tembokBoss2, true);
            kamera.tujuan = kamera.tujuanB;
		}

  //      if (collision.gameObject.layer == 10)
		//{
  //          //UIManager.TampilBossUI(m_uiManager.uiBoss, false, m_uiManager.tembokBoss1, m_uiManager.tembokBoss2, false);
  //          kamera.tujuan = gameObject;
		//}

        if (collision.gameObject.layer == 13)
            levelManager.LevelSelesai();

    }


	private void OnCollisionEnter2D(Collision2D collision)
	{
        //if (collision.gameObject.tag == "Musuh")
        //    KenaDamage(collision.gameObject.GetComponent<CharStat>().hitKarakter);

        if (collision.gameObject.tag == "Penyembuhan")
            Penyembuhan(1);
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "Lever")
            isDiTuas = false;

        if (collision.gameObject.layer == 8)
		{
            isBisaMasukPortal = false;
            transisi.SetTrigger("Akhir");
		}
    }
}
