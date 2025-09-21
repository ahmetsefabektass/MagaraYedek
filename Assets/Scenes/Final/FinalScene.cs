using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FinalScene : MonoBehaviour
{
    [SerializeField] GameObject finalUI;
    VideoPlayer videoPlayer;
    float videoDuration;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoDuration = (float)videoPlayer.clip.length;
    }
    void Start()
    {
        StartCoroutine(PlayVideoAndLoadScene());
    }
    IEnumerator PlayVideoAndLoadScene()
    {
        videoPlayer.Play();
        yield return new WaitForSeconds(videoDuration);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

}
