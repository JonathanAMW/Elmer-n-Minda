using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelinePlayer : MonoBehaviour
{
    private PlayableDirector director;
    public GameObject controlPanel;
    public GameObject kamera;
    public GameObject mainCamera;
    public bool isPlayingTM = false;

    void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.played += Director_Played;
        director.stopped += Director_Stopped;
        director.stopped += OnPlayableDirectorStopped;
    }

    private void Director_Played(PlayableDirector obj) //fungsi menghilangkan ui ketika timeline bermain
    {
        controlPanel.SetActive(false);
    }

    private void Director_Stopped(PlayableDirector obj) //fungsi mengembalikan ui ketika timeline berakhir
    {
        controlPanel.SetActive(true);
    }

    public void StartTimeline()
    {
        director.Play();
        isPlayingTM = true;
        kamera.SetActive(true);
        mainCamera.SetActive(false);

    }

    public void StopTimeline()
    {
        director.Stop();
        isPlayingTM = false;
        kamera.SetActive(false);
        mainCamera.SetActive(true);
    }

    void OnPlayableDirectorStopped(PlayableDirector obj) //setelah timeline berhenti
    {
        Debug.Log("Timeline selesai");
        isPlayingTM = false;
        kamera.SetActive(false);
        mainCamera.SetActive(true);
    }

    void Start()
    {
        director.Stop();
        isPlayingTM = false;
        kamera.SetActive(false);
        mainCamera.SetActive(true);
    }


   
}
