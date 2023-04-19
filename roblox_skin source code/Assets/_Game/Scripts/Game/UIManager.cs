
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;
using ATSoft.Ads;

public class UIManager : Singleton<UIManager>
{
    #region Inspector Variables

    [Space]

    [SerializeField] private Transform  panelRate;
    [SerializeField] private Transform  panelMenu;
    [SerializeField] private Transform  panelGameUpper;
    [SerializeField] private Transform  panelGameLower;
    [SerializeField] private Transform  panelSaveUpper;
    [SerializeField] private Transform  panelSaveLower;

    [Space]

    [SerializeField] private Button     playButton;
    [SerializeField] private Button     backButton;
    [SerializeField] private Button     saveButton;
    [SerializeField] private Button     menuButton;

    #endregion

    #region Member Variables

    private int     screenHeight;
    private int     itemClickCount;
    private bool    isPanelMenuMoving;

    #endregion

    #region Properties

    #endregion

    #region Unity Methods

    private void Start()
    {
        screenHeight    = Screen.height * 2;
        itemClickCount  = 0;

        playButton.onClick.AddListener(() => 
        { 
            MovePanelMenuOut(true);
            MovePanelGameOut(false);
        });

        backButton.onClick.AddListener(() => 
        { 
            MovePanelMenuOut(false);
            MovePanelGameOut(true);
        });

        saveButton.onClick.AddListener(() => 
        { 
            UnityAction actionComplete = delegate ()
            {
                MovePanelSaveOut(false);
                MovePanelGameOut(true);
            };
            Advertisements.Instance.ShowInterstitial(actionComplete);
        });

        menuButton.onClick.AddListener(() => 
        { 
            MovePanelSaveOut(true);
            MovePanelGameOut(false);
        });
    }

    #endregion

    #region Public Methods

    public void OpenRating()
    {
        itemClickCount++;
        if(itemClickCount % 3 == 0)
        {
            if (PlayerPrefs.GetInt(StringCollection.DATA_RATED) != 1)
            {
                panelRate.gameObject.SetActive(true);
            }
        }
    }

    public void MovePanelMenuOut(bool state)
    {
        if(!isPanelMenuMoving)
        {
            isPanelMenuMoving = true;
            if (state)
            {
                panelMenu.DOLocalMoveX(-screenHeight, 1f).SetEase(Ease.InOutBack)
                    .OnComplete(() => { isPanelMenuMoving = false; });
            }
            else
            {
                panelMenu.DOLocalMoveX(0f, 1f).SetEase(Ease.OutBack)
                    .OnComplete(() => { isPanelMenuMoving = false; }); ;
            }
        }
    }

    public void MovePanelGameUpperOut(bool state)
    {
        if (state)
            panelGameUpper.DOLocalMoveY(500f, 1f).SetEase(Ease.InOutBack);
        else
            panelGameUpper.DOLocalMoveY(0f, 1f).SetEase(Ease.OutBack);
    }

    public void MovePanelGameLowerOut(bool state)
    {
        if (state)
            panelGameLower.DOLocalMoveY(-1000f, 1f).SetEase(Ease.InOutBack);
        else
            panelGameLower.DOLocalMoveY(0f, 1f).SetEase(Ease.OutBack);
    }

    public void MovePanelGameOut(bool state)
    {
        MovePanelGameUpperOut(state);
        MovePanelGameLowerOut(state);
    }

    public void MovePanelSaveUpperOut(bool state)
    {
        if (state)
            panelSaveUpper.DOLocalMoveY(500f, 1f).SetEase(Ease.InOutBack);
        else
            panelSaveUpper.DOLocalMoveY(0f, 1f).SetEase(Ease.OutBack);
    }

    public void MovePanelSaveLowerOut(bool state)
    {
        if (state)
            panelSaveLower.DOLocalMoveY(-1000f, 1f).SetEase(Ease.InOutBack);
        else
            panelSaveLower.DOLocalMoveY(0f, 1f).SetEase(Ease.OutBack);
    }

    public void MovePanelSaveOut(bool state)
    {
        MovePanelSaveUpperOut(state);
        MovePanelSaveLowerOut(state);
    }

    #endregion

    #region Protected Methods

    #endregion

    #region Private Methods

    #endregion
}
