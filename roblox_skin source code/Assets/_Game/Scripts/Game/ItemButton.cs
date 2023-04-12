
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
	#region Inspector Variables

	[SerializeField] private Image iconImage;
	[SerializeField] private Button button;

	#endregion

	#region Member Variables

	private ItemData	data;
	private int			index;

    #endregion

    #region Properties

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
		transform.localScale = Vector3.zero;
		transform.DOScale(1, 0.25f).SetEase(Ease.OutBack);
	}

    #endregion

    #region Public Methods

    public void Setup(ItemData data, int index)
    {
		this.data	= data;
		this.index	= index;

		iconImage.sprite = data.itemSprite;
		button.onClick.AddListener(OnClick);
	}

	public void OnClick()
	{
		GirlSkin.Instance.ChangeSkin(data.itemType, index, data);
	}

	#endregion

	#region Protected Methods

	#endregion

	#region Private Methods

	#endregion
}
