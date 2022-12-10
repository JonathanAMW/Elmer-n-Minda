using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
	[Header("Cek Depan utk Nyerang")]
	[SerializeField] Transform cekDepan;
	[SerializeField] float cekDepanRange;
	[SerializeField] LayerMask targetLayer;

	[Header("Delay")]
	[SerializeField] float atkRate = 2f;
	private float nextAtkRate = 0f;

	enum aiCombatType
	{
		Diam, isNyerangBiasa, isNembak
	}
	[Header("Tipe Serangan")]
	[SerializeField] aiCombatType tipeCombatAI;

	CharStat karakterStat;

	Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();
		karakterStat = GetComponent<CharStat>();
	}

	private void Update()
	{
		if(tipeCombatAI == aiCombatType.isNyerangBiasa)
			CekPlayer();
	}

	void CekPlayer()
	{
		Collider2D collider = Physics2D.OverlapCircle(cekDepan.position, cekDepanRange, targetLayer);
		if (Time.time >= nextAtkRate)
		{
			if (collider != null)
			{
				nextAtkRate = Time.time + 1f / atkRate;
				anim.SetTrigger("Serang");
			}
		}
	}

	public void SerangBiasa()
	{
		Collider2D collider = Physics2D.OverlapCircle(cekDepan.position, cekDepanRange, targetLayer);
		if(collider != null)
			collider.GetComponent<Player>().KenaDamage(karakterStat.serangan);
	}

	private void OnDrawGizmosSelected()
	{
		if (cekDepan != null)
			Gizmos.DrawWireSphere(cekDepan.position, cekDepanRange);
	}
}
