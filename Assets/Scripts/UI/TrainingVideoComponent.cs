using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class TrainingVideoComponent : MonoBehaviour
{
    [SerializeField]
    private List<string> videoUrls;
    [SerializeField]
    private string hostSiteUrl = "http://www.bezierwars.altervista.org/";
    private VideoPlayer player;
    [SerializeField]
    private Image loadingImage;
    private int index = 0;

    private void Awake()
    {
        player = GetComponent<VideoPlayer>();
        player.isLooping = true;
    }

    private void OnEnable()
    {
        index = 0;
        ScrollVideo();
    }

    private void OnDisable()
    {
        player.Stop();
    }

    public void ScrollVideo()
    {
        if (videoUrls.Count == 0) return;
        if (index >= videoUrls.Count)
            index = 0;
        player.url = hostSiteUrl + "?video=" + videoUrls[index];
        player.Prepare();
        loadingImage.enabled = true;
        player.prepareCompleted += (player) => {
            player.Play();
            loadingImage.enabled = false;
        };
        index++;
    }
}
