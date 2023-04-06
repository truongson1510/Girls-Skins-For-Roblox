
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Inspector Variables

    [SerializeField] private Transform  panelMenu;
    [SerializeField] private Transform  panelSave;

    [Space]

    [SerializeField] private Button     playButton;
    [SerializeField] private Button     backButton;
    [SerializeField] private Button     saveButton;
    [SerializeField] private Button     menuButton;

    #endregion

    #region Member Variables

    private int screenWidth;
    private int screenHeight;

    private bool isPanelMenuMoving;

    #endregion

    #region Properties

    #endregion

    #region Unity Methods

    private void Start()
    {
        screenWidth     = Screen.width;
        screenHeight    = Screen.height;

        playButton.onClick.AddListener(() => { MovePanelMenuOut(true); });
        backButton.onClick.AddListener(() => { MovePanelMenuOut(false); });

        saveButton.onClick.AddListener(() => { MovePanelSaveOut(true); });
        menuButton.onClick.AddListener(() => { MovePanelSaveOut(false); });
    }

    #endregion

    #region Public Methods

    public void MovePanelMenuOut(bool state)
    {
        if(!isPanelMenuMoving)
        {
            isPanelMenuMoving = true;
            if (state)
            {
                panelMenu.DOLocalMoveX(-screenWidth, 1f).SetEase(Ease.InOutBack)
                    .OnComplete(() => { isPanelMenuMoving = false; });
            }
            else
            {
                panelMenu.DOLocalMoveX(0f, 1f).SetEase(Ease.OutBack)
                    .OnComplete(() => { isPanelMenuMoving = false; }); ;
            }
        }
    }

    public void MovePanelSaveOut(bool state)
    {
        if (!isPanelMenuMoving)
        {
            isPanelMenuMoving = true;
            if (state)
            {
                panelMenu.DOLocalMoveX(screenWidth, 1f).SetEase(Ease.InOutBack)
                    .OnComplete(() => { isPanelMenuMoving = false; });
            }
            else
            {
                panelMenu.DOLocalMoveX(0f, 1f).SetEase(Ease.OutBack)
                    .OnComplete(() => { isPanelMenuMoving = false; }); ;
            }
        }
    }

    #endregion

    #region Protected Methods

    #endregion

    #region Private Methods

    #endregion
}
