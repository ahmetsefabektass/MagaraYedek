
using System.Collections;
using Unity.VisualScripting;
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Clock");
        }
    }
    IEnumerator PlayVideoAndLoadScene()
    {
        videoPlayer.Play();
        yield return new WaitForSeconds(videoDuration);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Clock");
    }


}
