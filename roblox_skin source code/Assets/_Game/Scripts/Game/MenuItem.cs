using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MenuItem : MonoBehaviour
{
	#region Inspector Variables

	[Space]

	[SerializeField] private List<ButtonNavigation> listButton;
	[SerializeField] private List<GameObject>		listItemHolder;

	[Space]

	[TableList]
	[SerializeField] private List<ItemData>			listFace;
	[SerializeField] private Transform				contentFace;
	[TableList]
	[SerializeField] private List<ItemData>			listGlasses;
	[SerializeField] private Transform				contentGlasses;
	[TableList]
	[SerializeField] private List<ItemData>			listHair;
	[SerializeField] private Transform				contentHair;
	[TableList]
	[SerializeField] private List<ItemData>			listHat;
	[SerializeField] private Transform				contentHat;
	[TableList]
	[SerializeField] private List<ItemData>			listPant;
	[SerializeField] private Transform				contentPant;
	[TableList]
	[SerializeField] private List<ItemData>			listShirt;
	[SerializeField] private Transform				contentShirt;

	[Space]

	[SerializeField] private GameObject itemButton;

	#endregion

	#region Member Variables

	#endregion

	#region Properties

	public int SelectedButtonIndex { get; set; }

	#endregion

	#region Unity Methods

	private void Start()
	{
		listPant.AddRange(AssetBundleManager.Instance.pantsBundle.dataItems);
		listShirt.AddRange(AssetBundleManager.Instance.shirtsBundle.dataItems);
		Init();
	}

	#endregion

	#region Public Methods

	#endregion

	#region Protected Methods

	#endregion

	#region Private Methods

	/// <summary>
	/// 
	/// </summary>
	/// <param name="selectedButtonIndex"></param>
	private void Setup(int selectedButtonIndex)
	{
		for (int i = 0; i < listButton.Count; i++)
		{
			listButton[i].SetSelected(i == selectedButtonIndex);
			listItemHolder[i].SetActive(i == selectedButtonIndex);

			listButton[i].Index = i;
			listButton[i].OnListItemClicked = OnButtonClick;
		}

		SelectedButtonIndex = selectedButtonIndex;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="index"></param>
	/// <param name="data"></param>
	private void OnButtonClick(int index, object data)
	{
		if (index != SelectedButtonIndex)
		{
			// Set the current selected button to un-selected and select the new one
			listButton[SelectedButtonIndex].SetSelected(false);
			listItemHolder[SelectedButtonIndex].SetActive(false);

			listButton[index].SetSelected(true);
			listItemHolder[index].SetActive(true);

			SelectedButtonIndex = index;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	private void Init()
	{
		// Calcualte and spawn items to its menu
        for (int i = 0; i < listItemHolder.Count; i++)
        {
			ItemType currentType	= (ItemType)(i);
			List<ItemData> data		= new List<ItemData>();
			Transform content		= transform;

			// Calcualte menu
			switch (currentType)
            {
                case ItemType.Face:
					data.AddRange(listFace);
					content = contentFace;
					break;

                case ItemType.Shirt:
					data.AddRange(listShirt);
					content = contentShirt;
					break;

                case ItemType.Hair:
					data.AddRange(listHair);
					content = contentHair;
					break;

                case ItemType.Pant:
					data.AddRange(listPant);
					content = contentPant;
					break;

                case ItemType.Glasses:
					data.AddRange(listGlasses);
					content = contentGlasses;
					break;

                case ItemType.Hat:
					data.AddRange(listHat);
					content = contentHat;
					break;
            }

			// Spawn items
			for (int j = 0; j < data.Count; j++)
			{
				GameObject item = Instantiate(itemButton, content);
				item.GetComponent<ItemButton>().Setup(data[j], j);

				// Pre-select first item of pant and shirt
				if(j == 0)
                {
					if (currentType == ItemType.Shirt || currentType == ItemType.Pant)
					{
						item.GetComponent<ItemButton>().OnClick();
					}
				}
            }
		}

		// Choose the first menu
		Setup(0);
	}

    #endregion
}
