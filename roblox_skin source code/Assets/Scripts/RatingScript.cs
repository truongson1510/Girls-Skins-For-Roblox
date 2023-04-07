
using UnityEngine;

public class RatingScript : MonoBehaviour
{
    public static RatingScript Instance;

    [SerializeField] private GameObject ratingCanvas;
    [SerializeField] private ButtonsController buttonsController;
    [SerializeField] private RatingUIController ratingUIController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            /// DontDestroyOnLoad(this);
        }
    }

    public void OpenPopup()
    {
        ratingCanvas.SetActive(true);
    }

    public void ClosePopup()
    {
        buttonsController.OnClose();
    }
}
