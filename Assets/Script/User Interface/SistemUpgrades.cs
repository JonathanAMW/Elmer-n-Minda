using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SistemUpgrades : MonoBehaviour
{
	[Header("Harga Item")]
	[SerializeField] int hargaDarah;
	[SerializeField] int hargaZirah, hargaSerangan;

	[Header("Harga Bintang Item")]
	[SerializeField] int bintangDarah;
	[SerializeField] int bintangZirah, bintangSerangan;

	[Header("Total Pembelian")]
	[SerializeField] int totalBeliDarah;
	[SerializeField] int totalBeliZirah, totalBeliSerangan;

	//test ananta
	[SerializeField] int maxBeliDarah, maxBeliZirah, maxBeliSerangan;

	[Header("Indikator Pembelian")]
	[SerializeField] GameObject[] indikatorDarah;
	[SerializeField] GameObject[] indikatorZirah, indikatorSerangan;

	string namaBeli;
	[SerializeField] int temp;
	[Header("Referensi")]
	[SerializeField] TextMeshProUGUI hargaBintangTeks;
	[SerializeField] TextMeshProUGUI hargaKoinTeks, koinTeks, bintangTeks;

	Inventaris inventaris;
	CharStat karakterStat;

	private void Start()
	{
		inventaris = GameObject.Find("Inventaris").GetComponent<Inventaris>();
		karakterStat = GameObject.Find("StatPemain").GetComponent<CharStat>();
		TampilInventaris();
	}

	public void Tampil(string nama)
	{
		namaBeli = nama;
		if(nama == "Darah")
		{
			hargaKoinTeks.text = hargaDarah.ToString();
			hargaBintangTeks.text = bintangDarah.ToString();
		}
		else if (nama == "Zirah")
		{
			hargaKoinTeks.text = hargaZirah.ToString();
			hargaBintangTeks.text = bintangZirah.ToString();
		}
		else if (nama == "Serangan")
		{
			hargaKoinTeks.text = hargaSerangan.ToString();
			hargaBintangTeks.text = bintangSerangan.ToString();
		}
	}

	/* source ariq
	public void Upgrade()
	{
		if (inventaris.koin <= 0 && inventaris.bintang <= 0) return;
		if(namaBeli == "Darah")
		{
			Pembelian(ref hargaDarah, ref bintangDarah, ref hargaKoinTeks, ref hargaBintangTeks, ref totalBeliDarah, ref indikatorDarah);
			karakterStat.darahMaks += totalBeliDarah;
		}
		else if(namaBeli == "Zirah")
		{
			Pembelian(ref hargaZirah, ref bintangZirah, ref hargaKoinTeks, ref hargaBintangTeks, ref totalBeliZirah, ref indikatorZirah);
			karakterStat.zirahMaks += totalBeliZirah;
		}
		else if(namaBeli == "Serangan")
		{
			Pembelian(ref hargaSerangan, ref bintangSerangan, ref hargaKoinTeks, ref hargaBintangTeks, ref totalBeliSerangan, ref indikatorSerangan);
			karakterStat.serangan += totalBeliSerangan;
		}
	}
	
	public void Pembelian(ref int harga, ref int bintang, ref TextMeshProUGUI hargaTeks, ref TextMeshProUGUI bintangTeks, ref int indikator ,ref GameObject[] gmbrIndikator)
	{
		if (harga > 4000)
			harga = 4000;

		if (bintang > 4)
			bintang = 4;

		inventaris.koin -= harga;

		harga += 1000;
		bintang += 1;
		hargaTeks.text = harga.ToString();
		bintangTeks.text = bintang.ToString();

		if (indikator > 4) return;
		gmbrIndikator[indikator].SetActive(true);
		indikator++;
		TampilInventaris();
	}
	*/


	//source ananta
	public void Upgrade()
	{
		if(inventaris.koin <= 0 || inventaris.bintang <= 0) return;
		
		if(namaBeli == "Darah" && hargaDarah <= inventaris.koin && (totalBeliDarah<maxBeliDarah || karakterStat.darahPenuh < karakterStat.darahMaks) && (inventaris.bintang >= bintangDarah))
		{
			Pembelian(ref hargaDarah, ref bintangDarah, ref hargaKoinTeks, ref hargaBintangTeks, ref totalBeliDarah, ref indikatorDarah, namaBeli);
			karakterStat.darahPenuh += 1;
		}
		else if(namaBeli == "Zirah" && hargaZirah <= inventaris.koin && (totalBeliZirah<maxBeliZirah || karakterStat.zirahPenuh < karakterStat.zirahMaks) && (inventaris.bintang >= bintangZirah))
		{
			Pembelian(ref hargaZirah, ref bintangZirah, ref hargaKoinTeks, ref hargaBintangTeks, ref totalBeliZirah, ref indikatorZirah, namaBeli);
			karakterStat.zirahPenuh += 1;
		}
		else if(namaBeli == "Serangan" && hargaSerangan <= inventaris.koin && (totalBeliSerangan<maxBeliSerangan || karakterStat.serangan < karakterStat.seranganMaks) && (inventaris.bintang >= bintangSerangan))
		{
			Pembelian(ref hargaSerangan, ref bintangSerangan, ref hargaKoinTeks, ref hargaBintangTeks, ref totalBeliSerangan, ref indikatorSerangan, namaBeli);
			karakterStat.serangan += 1;
		}
		else
		{
			return;
		}
	}

	public void Pembelian(ref int harga, ref int bintang, ref TextMeshProUGUI hargaTeks, ref TextMeshProUGUI bintangTeks, ref int indikator ,ref GameObject[] gmbrIndikator, string namaBeli)
	{
		if (harga > inventaris.koin || inventaris.koin <= 0 || inventaris.bintang <= 0) return;
		int hargaKoinNaik=0, hargaBintangNaik=0;
		switch (namaBeli)
		{
			case "Darah":
				hargaKoinNaik = 150;
				if(indikator == 4) hargaBintangNaik = 1;
				else hargaBintangNaik = 2;
				break;

			case "Zirah":
				hargaKoinNaik = 150;
				hargaBintangNaik = 2;
				break;

			case "Serangan":
				hargaKoinNaik = 100;
				if(indikator == 4) hargaBintangNaik = 1;
				else hargaBintangNaik = 2;
				break;

			default:
				Debug.LogWarning("nama beli tidak ada");
				break;
		}

		inventaris.koin -= harga;

		harga += hargaKoinNaik;
		bintang += hargaBintangNaik;
		hargaTeks.text = harga.ToString();
		bintangTeks.text = bintang.ToString();

		gmbrIndikator[indikator].SetActive(true);
		indikator++;
		TampilInventaris();
	}

	void TampilInventaris()
	{
		koinTeks.text = " x " + inventaris.koin.ToString();
		bintangTeks.text = " x " + inventaris.bintang.ToString();
	}
}
