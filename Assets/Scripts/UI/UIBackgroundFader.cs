using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class UIBackgroundFader : MonoBehaviour
{
    public static UIBackgroundFader Instance;
    
    private Image bgImage;

    [Header("淡入淡出設定")]
    public float fadeDuration = 0.3f;
    public float targetAlpha = 0.5f;

    void Awake()
    {
        Instance = this;
        
        bgImage = GetComponent<Image>();
        bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 0f);
    }
    
    public void FadeIn()
    {
        bgImage.DOFade(targetAlpha, fadeDuration);
    }
    
    public void FadeOut()
    {
        bgImage.DOFade(0f, fadeDuration);
    }
}