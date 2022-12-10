using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[SerializeField] List<DataGambarBintang> dataGambarBintang;
	[SerializeField] List<DataTombol> dataTombol;
	[SerializeField] List<TextMeshProUGUI> teks_misi;
	[SerializeField] Sprite bintangNyala;

	private void Start()
	{
		SistemTantangan m_sistemTantangan = GameObject.Find("Data Tantangan").GetComponent<SistemTantangan>();
		for (int i = 0; i < m_sistemTantangan.dataTantangan.Count; i++)
		{
			if (m_sistemTantangan.dataTantangan[i].isDuniaTerbuka)
				dataTombol[i].tombolDunia.interactable = true;

			for (int j = 0; j < m_sistemTantangan.dataTantangan[i].listTantanganDunia.Count; j++)
			{
				if (m_sistemTantangan.dataTantangan[i].listTantanganDunia[j].isLevelTerbuka)
					dataTombol[i].dataTombolLevel[j].tombolLevel.interactable = true;

				if (m_sistemTantangan.dataTantangan[i].listTantanganDunia[j].isSelesai)
				{
					dataGambarBintang[i].gambarBintang[j].bintangDunia.sprite = bintangNyala;
					dataGambarBintang[i].gambarBintang[j].bintangLevel.sprite = bintangNyala;
				}

				teks_misi[j].text = m_sistemTantangan.dataTantangan[i].listTantanganDunia[j].deksTantangan;
			}
		}
	}

	//menggunakan indeks build
	public void LoadScene(int scene)
	{
		StartCoroutine(LoadSceneAsync(scene));
	}
	private IEnumerator LoadSceneAsync(int scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
		while(!asyncLoad.isDone) //bisa digunakan untuk loading screen
		{
			yield return null;
		}
    }

	//menggunakan nama scene
	public void LoadCutscene(string cutscene)
	{
		StartCoroutine(LoadSceneAsync(cutscene));
	}
	private IEnumerator LoadSceneAsync(string cutscene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(cutscene);
		while(!asyncLoad.isDone)
		{
			yield return null;
		}
    }

	public void KeluarGame()
	{
		Application.Quit();
	}
}

[System.Serializable]
public class DataGambarBintang
{
	public string nama;
	public List<GambarBintang> gambarBintang;
}

[System.Serializable]
public class GambarBintang
{
	public string nama;
	public Image bintangDunia, bintangLevel;
}

[System.Serializable]
public class DataTombol
{
	public string nama;
	public Button tombolDunia;
	public List<Tombol> dataTombolLevel;
}

[System.Serializable]
public class Tombol
{
	public string nama;
	public Button tombolLevel;
}
