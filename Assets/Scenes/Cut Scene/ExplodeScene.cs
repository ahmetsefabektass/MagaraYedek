
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class ExplodeScene : MonoBehaviour
{
    VideoPlayer videoPlayer;
    float videoDuration;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoDuration = (float)videoPlayer.clip.length;
    }
    void Start()
    {
        GeneralUI.Instance.SetInfoText("Press \"ESC\" to skip", 3f);
        StartCoroutine(PlayVideoAndLoadScene());
    }
    IEnumerator PlayVideoAndLoadScene()
    {
        videoPlayer.Play();
        yield return new WaitForSeconds(videoDuration);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Clock");
    }


}
