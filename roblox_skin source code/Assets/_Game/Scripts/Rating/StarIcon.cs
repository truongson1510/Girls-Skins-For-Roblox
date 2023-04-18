
using UnityEngine;
using UnityEngine.UI;

public class StarIcon : MonoBehaviour
{
    [SerializeField] Sprite starOn;
    [SerializeField] Sprite starOff;

    [Range(1, 5)]
    [SerializeField] int starPosition;

    private Image image;
    private Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(NotifyRatingUIController);
    }

    private void NotifyRatingUIController()
    {
        Observer.Instance.Notify(StringCollection.EVENT_STAR_CLICK, starPosition);
    }

    public void TurnOnStar(bool state)
    {
        if (state)
            image.sprite = starOn;
        else
            image.sprite = starOff;
    }
}
