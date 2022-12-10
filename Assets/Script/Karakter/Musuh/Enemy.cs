using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("STAT")]
	CharStat karakterStat;
	[SerializeField] bool isBoss;
	public bool canBossGerak;

	[Header("Buat Ngecek Ground")]
	//Buat Cek Ground
	[SerializeField] Transform pengecekanGround;
	[SerializeField] float radiusPengecekanGround;

	[Header("Utk Gerak Simpel")]
	[SerializeField] Transform pengecekanDepan;
	[SerializeField] float radiusPengecekanDepan;

	[Header("Target Layer")]
	[SerializeField] LayerMask layerGround;

	[Header("Utk Gerak Random")]
	[SerializeField] float accelerationTime = 2f;
	private float timeLeft;
	private int randNumX;

	[Header("Referensi")]
	[SerializeField] LevelManager levelManager;
	[SerializeField] UIManager m_uimanager;
	[SerializeField] MainKamera kamera;

	Inventaris inventaris;

	enum aiMovementType
	{
		Diam,isGerakSimpel, isGerakAcak, isTerbang, isGerakAcakLompat
	}
	[Header("Konfigurasi AI")]
	[SerializeField] aiMovementType pergerakanAI;

	Rigidbody2D rb2d;
	Animator anim;

	private void Start()
	{
		inventaris = GameObject.Find("Inventaris").GetComponent<Inventaris>(); //buat player inventaris

		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		karakterStat = GetComponent<CharStat>();
		if(isBoss) canBossGerak = false;
		Menghadap(); //hadap kiri
	}

	private void Update()
	{
		CekGround();
		if(isBoss && !canBossGerak){
			rb2d.velocity = new Vector2(0, 0);
			anim.SetBool("isJalan", false); 
			//anim.SetBool("isGrounded", true);
			return;
		} 
		if(pergerakanAI == aiMovementType.isGerakSimpel)
			CekDepan();

		if (transform.position.y < -7)
			Destroy(gameObject);
	}

	private void FixedUpdate()
	{
		if(isBoss && !canBossGerak){
			rb2d.velocity = new Vector2(0, 0);
			anim.SetBool("isJalan", false); 
			//anim.SetBool("isGrounded", true);
			return;
		} 
		//Gerak();
		if (pergerakanAI == aiMovementType.isGerakAcak || pergerakanAI == aiMovementType.isGerakAcakLompat)
			GerakAcak();
		else if (pergerakanAI == aiMovementType.isGerakSimpel)
			Gerak();

		AnimasiMusuh();
	}

	void Gerak()
	{
		if(!karakterStat.isTdkHadapKanan)
			rb2d.velocity = new Vector2(karakterStat.kecepatanGerak, rb2d.velocity.y);
		else if(karakterStat.isTdkHadapKanan)
			rb2d.velocity = new Vector2(-karakterStat.kecepatanGerak, rb2d.velocity.y);
	}

	void GerakAcak()
	{
		timeLeft -= Time.deltaTime;
		if(timeLeft <= 0)
		{
			randNumX = Random.Range(-1, 2);
			if(pergerakanAI == aiMovementType.isGerakAcakLompat)
				Lompat();
			timeLeft += accelerationTime;
		}

		rb2d.velocity = new Vector2(randNumX, rb2d.velocity.y);

		if (rb2d.velocity.x < 0 && !karakterStat.isTdkHadapKanan)
			Menghadap();
		else if (rb2d.velocity.x > 0 && karakterStat.isTdkHadapKanan)
			Menghadap();
	}

	void Lompat()
	{
		if(karakterStat.isGrounded)
			rb2d.velocity = new Vector2(rb2d.velocity.x, karakterStat.kekuatanLompat);
	}

	void AnimasiMusuh()
	{
		if (rb2d.velocity.x > 0 || rb2d.velocity.x < 0)
		{
			karakterStat.isJalan = true;
		}
		else if (rb2d.velocity.x == 0)
			karakterStat.isJalan = false;

		anim.SetBool("isGrounded", karakterStat.isGrounded);

		//jika kecepatan player > || < 0
		if (rb2d.velocity.x > 0 || rb2d.velocity.x < 0)
		{
			karakterStat.isJalan = true;
		}
		else if (rb2d.velocity.x == 0)
			karakterStat.isJalan = false;

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

	void CekDepan()
	{
		//Di game object 'pengecekanGround' akan membuat sebuah lingkaran yg mana itu nanti buat deteksi ground;
		Collider2D collider = Physics2D.OverlapCircle(pengecekanDepan.position, radiusPengecekanDepan, layerGround);
		if (collider != null)
			Menghadap();
	}

	void Menghadap()
	{
		karakterStat.isTdkHadapKanan = !karakterStat.isTdkHadapKanan;
		transform.Rotate(0f, 180f, 0);
	}

	public void Penyembuhan(int tambah)
	{
		karakterStat.darah += tambah;
	}

	public void KenaDamage(int kurang)
	{
		if (karakterStat.zirah <= 0)
			karakterStat.darah -= kurang;
		else
			karakterStat.zirah -= 1;

		if (karakterStat.darah <= 0)
		{
			if (isBoss)
			{
				levelManager.kondisiTantangan.isBossKalah = true;
				kamera.tujuan = kamera.tujuanA;
				UIManager.TampilBossUI(m_uimanager.uiBoss, false, m_uimanager.tembokBoss1, m_uimanager.tembokBoss2, false);
			}
			Kalah();
		}
	}

	void Kalah()
	{
		Debug.Log("musuh mati");
		if(isBoss)
		{
			int Koin = Random.Range(250, 300);
			inventaris.koin += Koin; 
		}
		else
		{
			int Koin = Random.Range(5, 15);
			inventaris.koin += Koin; 
		}
		Destroy(gameObject);
	}

	private void OnDrawGizmosSelected()
	{
		if(pengecekanGround != null)
			Gizmos.DrawWireSphere(pengecekanGround.position, radiusPengecekanGround);
		if (pengecekanDepan != null)
			Gizmos.DrawWireSphere(pengecekanDepan.position, radiusPengecekanDepan);
	}
}
