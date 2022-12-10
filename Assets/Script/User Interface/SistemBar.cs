using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SistemBar : MonoBehaviour
{
	[Header("Referensi stat yang dituju")]
	[SerializeField] CharStat karStatPemain, karStatEnemy;
	[SerializeField] bool isPemain;

	[Header("Konfigurasi Darah")]
	[SerializeField] Slider sliderDarah;
	[SerializeField] TextMeshProUGUI indikatorDarahTeks;

	[Header("Konfigurasi Armor")]
	[SerializeField] Slider sliderArmor;
	[SerializeField] TextMeshProUGUI indikatorArmorTeks;

	private void Start()
	{
		if (isPemain)
		{
			karStatPemain = GameObject.Find("StatPemain").GetComponent<CharStat>();
			karStatPemain.darah = karStatPemain.darahPenuh;
			karStatPemain.zirah = karStatPemain.zirahPenuh;
		}
		else
		{
			karStatEnemy.darah = karStatEnemy.darahPenuh;
			karStatEnemy.zirah = karStatEnemy.zirahPenuh;
		}
	}

	private void Update()
	{
		if (isPemain)
		{
			sliderDarah.maxValue = karStatPemain.darahPenuh;
			sliderArmor.maxValue = karStatPemain.zirahPenuh;

			sliderDarah.value = karStatPemain.darah;
			sliderArmor.value = karStatPemain.zirah;

			indikatorDarahTeks.text = karStatPemain.darah + " / " + karStatPemain.darahPenuh;
			indikatorArmorTeks.text = karStatPemain.zirah + " / " + karStatPemain.zirahPenuh;
		}
		else
		{
			sliderDarah.maxValue = karStatEnemy.darahPenuh;
			sliderArmor.maxValue = karStatEnemy.zirahPenuh;

			sliderDarah.value = karStatEnemy.darah;
			sliderArmor.value = karStatEnemy.zirah;

			indikatorDarahTeks.text = karStatEnemy.darah + " / " + karStatEnemy.darahPenuh;
			indikatorArmorTeks.text = karStatEnemy.zirah + " / " + karStatEnemy.zirahPenuh;
		}
	}
}
