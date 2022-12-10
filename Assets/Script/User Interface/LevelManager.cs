using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	[Header("Pengaturan Level")]
	public bool isLvlSelesai;
	[SerializeField] float waktuLevel;
	public enum tantanganLevel
	{
		None,TanpaMati, TanpaDMG, BossKalah
	}

	[Header("Untuk Tantangan")]
	public tantanganLevel tipeTantangan;
	public KondisiTantangan kondisiTantangan;

	[Header("Referensi")]
	[SerializeField] UIManager m_uimanager;
	[SerializeField] TextMeshProUGUI teksMisiFinish, teksMisiPause;
	SistemTantangan m_sistemTantangan;
	Inventaris inventaris;
	CharStat karakterStat;

	[Header("Info level")]
	public int levelSekarang;
	public int duniaSekarang;
	
	private void Start()
	{
		m_sistemTantangan = GameObject.Find("Data Tantangan").GetComponent<SistemTantangan>();
		inventaris = GameObject.Find("Inventaris").GetComponent<Inventaris>();
		teksMisiFinish.text = m_sistemTantangan.dataTantangan[duniaSekarang - 1].listTantanganDunia[levelSekarang - 1].deksTantangan;
		teksMisiPause.text = m_sistemTantangan.dataTantangan[duniaSekarang - 1].listTantanganDunia[levelSekarang - 1].deksTantangan;
		karakterStat = GameObject.Find("StatPemain").GetComponent<CharStat>();
	}

	public void LevelSelesai()
	{
		if (karakterStat.isPernahMati)
			kondisiTantangan.isTdkMati = false;
		else
			kondisiTantangan.isTdkMati = true;

		if(levelSekarang < m_sistemTantangan.dataTantangan[duniaSekarang - 1].listTantanganDunia.Count)
			m_sistemTantangan.dataTantangan[duniaSekarang - 1].listTantanganDunia[levelSekarang].isLevelTerbuka = true;

		isLvlSelesai = true;
		m_uimanager.finishUI.SetActive(true);

		if (isLvlSelesai)
		{
			if(tipeTantangan == tantanganLevel.TanpaMati && kondisiTantangan.isTdkMati ||
				tipeTantangan == tantanganLevel.TanpaDMG && kondisiTantangan.isTdkDMG ||
				tipeTantangan == tantanganLevel.BossKalah && kondisiTantangan.isBossKalah)
			{
				m_uimanager.gmbrBintangFinish.sprite = m_uimanager.gmbrDptBintang;
				m_uimanager.gmbrBintangPause.sprite = m_uimanager.gmbrDptBintang;
				if (m_sistemTantangan.dataTantangan[duniaSekarang - 1].listTantanganDunia[levelSekarang - 1].isSelesai != true)
				{
					inventaris.bintang += 1;
				}
			}
		}
	}

	public void GantiScene(int scene)
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(scene);
		Simpan();
	}

	void Simpan()
	{
		if (isLvlSelesai)
		{
			if (tipeTantangan == tantanganLevel.TanpaMati && kondisiTantangan.isTdkMati ||
				tipeTantangan == tantanganLevel.TanpaDMG && kondisiTantangan.isTdkDMG ||
				tipeTantangan == tantanganLevel.BossKalah && kondisiTantangan.isBossKalah)
				m_sistemTantangan.dataTantangan[duniaSekarang - 1].listTantanganDunia[levelSekarang - 1].isSelesai = true;
		}
	}

	public void ResetLevel()
	{
		Time.timeScale = 1;
		int scene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(scene);
	}

	public void NextLevel()
	{
		int scene = SceneManager.GetActiveScene().buildIndex + 1;
		SceneManager.LoadScene(scene);
	}
}

[System.Serializable]
public class KondisiTantangan
{
	public bool isTdkMati, isTdkDMG, isBossKalah;
}
