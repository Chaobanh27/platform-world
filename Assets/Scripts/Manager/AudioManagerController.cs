using UnityEngine;

public class AudioManagerController : MonoBehaviour
{
    public static AudioManagerController instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private int bgmIndex;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }


        if (bgm.Length <= 0)
        {
            return;
        }
        InvokeRepeating("PlayMusicIfNeeded", 0, 2);
    }

    public void PlayMusicIfNeeded()
    {
        if (bgm[bgmIndex].isPlaying == false)
        {
            PlayRandomBgm();
        }
    }

    public void PlayRandomBgm()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBgm(bgmIndex);
    }

    public void PlayBgm(int bgmToPlay)
    {
        if(bgm.Length <= 0)
        {
            Debug.LogWarning("you have no music on audio manager");
            return;
        }

        for(int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }

        bgmIndex = bgmToPlay;
        bgm[bgmToPlay].Play();
    }

    public void PlaySfx(int sfxToPlay)
    {
        if(sfxToPlay >= sfx.Length)
        {
            return;
        }

        sfx[sfxToPlay].Play();
    }

    public void StopSfx(int sfxToStop)
    {
        sfx[sfxToStop].Stop();
    }
}
