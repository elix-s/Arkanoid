using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class LayoutGroupFader : MonoBehaviour
{
    [SerializeField] private float _fadeOutTime = 1.0f;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 1f;
        _canvasGroup.DOFade(0f, _fadeOutTime);
    }
}