using UnityEngine;
using TMPro;
using DG.Tweening;

public class IntroManager : MonoBehaviour
{
    public TextMeshProUGUI moveText;
    public TextMeshProUGUI orText;
    public TextMeshProUGUI dieText;
    //public ParticleSystem impactParticles;
    public AudioSource audioSource;
    public AudioClip boomClip;

    private void Start()
    {
        // Désactive au début
        //moveText.gameObject.SetActive(false);
        //orText.gameObject.SetActive(false);
        //dieText.gameObject.SetActive(false);
    }

    public void PlayIntro()
    {
        Debug.Log("PlayIntro lancé");

        var t1 = ApparitionWord(moveText);
        var t2 = ApparitionWord(orText);
        var t3 = ApparitionWord(dieText);

        Sequence introSeq = DOTween.Sequence();
        introSeq.Append(t1)
                .Join(t2)
                .Join(t3)
                .AppendInterval(3f)
                .AppendCallback(() => {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                });
    }



    public Tween ApparitionWord(TextMeshProUGUI word)
    {
        if (word == null)
        {
            Debug.LogError("Le TMP est null !");
            return DOVirtual.DelayedCall(0.01f, () => { });
        }

        word.gameObject.SetActive(true);
        word.alpha = 0f;
        word.rectTransform.localScale = Vector3.one * 0.8f;
        
        audioSource.volume = 0.05f;
        audioSource.PlayOneShot(boomClip);

        Sequence seq = DOTween.Sequence();
        seq.Append(word.DOFade(1f, 0.3f));
        seq.Join(word.rectTransform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
        seq.Join(word.rectTransform.DOShakeScale(1f, 0.1f, 10, 90, false));

        return seq;
    }

}
