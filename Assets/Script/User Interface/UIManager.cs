using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[Header("HUD")]
	//public GameObject uiInfo;
	public GameObject uiBoss, finishUI, pauseUI;
	public Image gmbrBintangFinish, gmbrBintangPause;
	public Sprite gmbrDptBintang;
	public GameObject tembokBoss1, tembokBoss2;

	bool isPaused;

	[Header("Koin & Lives")]
	[SerializeField] TextMeshProUGUI teksKoin;
	[SerializeField] TextMeshProUGUI teksLives, teksSerangan;
	public int jmlLives;

	//[Header("Referensi")]
	Inventaris inventaris;
	LevelManager levelManager;
	CharStat karakterStat;

	private void Start()
	{
		inventaris = GameObject.Find("Inventaris").GetComponent<Inventaris>();
		levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		karakterStat = GameObject.Find("StatPemain").GetComponent<CharStat>();
		teksSerangan.text = "x " + karakterStat.serangan.ToString();
		teksLives.text = "x " + karakterStat.nyawa.ToString();
	}

	public static void TampilBossUI(GameObject barSistem, bool tampilBar, GameObject tembok1, GameObject tembok2, bool tampilTembok)
	{
		barSistem.SetActive(tampilBar);
		tembok1.SetActive(tampilTembok);
		tembok2.SetActive(tampilTembok);
	}

	private void Update()
	{
		teksKoin.text =  "x " + inventaris.koin;
		teksLives.text = "x " + karakterStat.nyawa.ToString();

		if(levelManager.isLvlSelesai == false)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				Paused();
		}
	}

	public void Paused()
	{
		if(!levelManager.isLvlSelesai) //menjaga agar tombol pause tidak bocor
		{
			isPaused = !isPaused;
			if (isPaused)
			{
				pauseUI.SetActive(isPaused);
				Time.timeScale = 0;
			}
			else
			{
				pauseUI.SetActive(isPaused);
				Time.timeScale = 1;
			}
		}
	}
}
