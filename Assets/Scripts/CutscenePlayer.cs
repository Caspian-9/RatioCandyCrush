using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutscenePlayer : MonoBehaviour
{

	private PlayableDirector director;
	public GameObject controlPanel;

	// Use this for initialization
	void Awake()
	{
		director = GetComponent<PlayableDirector>();
        director.played += Director_played;
        director.stopped += Director_stopped;
	}

    public void StartTimeline()
    {
        director.Play();
    }

    private void Director_played(PlayableDirector obj)
    {
        controlPanel.SetActive(false);
    }

    private void Director_stopped(PlayableDirector obj)
    {
        controlPanel.SetActive(true);
    }
}

