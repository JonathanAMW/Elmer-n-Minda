using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutscenePortal : MonoBehaviour
{
    [SerializeField] private int scene; //scene mana yang mau di load

	private IEnumerator LoadSceneAsync(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
		while(!asyncLoad.isDone)
		{
			yield return null;
		}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") == true && gameObject.tag == "PortalTeleport") //untuk dialog npc unskipable
        {
            StartCoroutine(LoadSceneAsync(scene));
        }
    }
}


