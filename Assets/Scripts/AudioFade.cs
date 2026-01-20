using UnityEngine;
using System.Collections;
using static GameManager;

public class AudioFade : MonoBehaviour
{
	private AudioSource[] audioSources;

	void Start()
	{
		audioSources = GetComponents<AudioSource>();
	}

	// フェードアウトを開始するメソッド
	public void StartFadeOut(float fadeDuration)
	{
	}
}