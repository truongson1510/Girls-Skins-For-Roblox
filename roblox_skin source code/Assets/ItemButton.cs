
using UnityEngine;
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
        // scale up
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

	#endregion

	#region Protected Methods

	#endregion

	#region Private Methods

	private void OnClick()
    {
		GirlSkin.Instance.ChangeSkin(data.itemType, index, data.itemMaterial);
	}

	#endregion
}
