
using DG.Tweening;
using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    [SerializeField] private GameObject canvasController;

    private void OnEnable()
    {
        transform.localScale = Vector3.one * 0.1f;
        transform.DOScale(1, 0.65f).SetEase(Ease.OutBounce);
    }

    public void OnClose()
    {
        transform.DOScale(0.1f, 0.3f).SetEase(Ease.InSine).
            OnComplete(() => { canvasController.SetActive(false); });
    }
}
