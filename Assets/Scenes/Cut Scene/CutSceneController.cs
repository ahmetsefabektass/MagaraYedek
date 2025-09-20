using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class CutSceneController : MonoBehaviour
{
    VideoPlayer videoPlayer;
    float videoDuration;
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoDuration = (float)videoPlayer.clip.length;
    }
    void Start()
    {
        StartCoroutine(PlayVideoAndLoadScene());
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Clock");
        }
    }
    IEnumerator PlayVideoAndLoadScene()
    {
        videoPlayer.Play();
        yield return new WaitForSeconds(videoDuration);
        SceneManager.LoadScene("Clock");
    }
}
