using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class LyricsSequencer : MonoBehaviour
{
    [System.Serializable]
    public class LyricEntry
    {
        public TextMeshProUGUI tmp;
        public float appearTime = 0f;
        public float visibleDuration = 2f;
        public bool isExplosion = false; // à cocher uniquement pour le dernier (EXPLOSION)
    }
    public AudioSource meguminAudioSource;
    public GameObject IntroManager;
    public LyricEntry[] lyrics;
    public bool skipMegumin = true;

    private void Start()
    {
        //IntroManager.gameObject.SetActive(false);

        foreach (var entry in lyrics)
        {
            if (entry.tmp != null)
                entry.tmp.gameObject.SetActive(false);
        }

        if (skipMegumin)
        {
            IntroManager.SetActive(true);
            IntroManager.GetComponent<IntroManager>().PlayIntro();
            return;
        }

        if (meguminAudioSource != null)
            meguminAudioSource.Play();

        StartCoroutine(PlayLyrics());
    }


    IEnumerator PlayLyrics()
    {
        for (int i = 0; i < lyrics.Length; i++)
        {
            LyricEntry entry = lyrics[i];
            float waitTime = (i == 0) ? entry.appearTime : entry.appearTime - lyrics[i - 1].appearTime;
            yield return new WaitForSeconds(waitTime);

            if (entry.tmp == null) continue;

            if (entry.isExplosion)
            {
                yield return PlayExplosion(entry.tmp);
            }
            else
            {
                yield return PlayLine(entry.tmp, entry.visibleDuration);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SkipIntro();
        }
    }

    IEnumerator PlayLine(TextMeshProUGUI tmp, float duration)
    {
        tmp.gameObject.SetActive(true);
        tmp.alpha = 0f;
        tmp.DOFade(1f, 0.5f);

        yield return new WaitForSeconds(duration);

        tmp.DOFade(0f, 0.5f).OnComplete(() => tmp.gameObject.SetActive(false));
    }

    IEnumerator PlayExplosion(TextMeshProUGUI tmp)
    {
        tmp.gameObject.SetActive(true);
        tmp.alpha = 0f;
        tmp.rectTransform.localScale = Vector3.one * 0.8f;

        tmp.DOFade(1f, 0.3f);
        tmp.rectTransform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        tmp.rectTransform.DOShakeScale(1f, 0.1f, 10, 90, false);

        yield return new WaitForSeconds(1.5f);

        Sequence exitSeq = DOTween.Sequence();
        exitSeq.Append(tmp.DOFade(0f, 0.5f));
        exitSeq.Join(tmp.rectTransform.DOScale(1.2f, 0.5f));
        exitSeq.OnComplete(() => tmp.gameObject.SetActive(false));

        yield return exitSeq.WaitForCompletion();

        // Tempo d'1s
        yield return new WaitForSeconds(1f);

        IntroManager.SetActive(true);
        StartCoroutine(DelayedPlayIntro());

        IEnumerator DelayedPlayIntro()
        {
            yield return null; // une frame de délai
            IntroManager.GetComponent<IntroManager>().PlayIntro();
        }

    }

    private void SkipIntro()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
