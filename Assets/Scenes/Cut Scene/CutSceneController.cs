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
        GeneralUI.Instance.SetInfoText("Press ESC to skip cutscene", 5f);
        StartCoroutine(PlayVideoAndLoadScene());
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("ClockTutorial");
        }
    }
    IEnumerator PlayVideoAndLoadScene()
    {
        videoPlayer.Play();
        yield return new WaitForSeconds(videoDuration);
        SceneManager.LoadScene("ClockTutorial");
    }
}
