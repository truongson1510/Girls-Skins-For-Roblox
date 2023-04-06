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
	[SerializeField] private Transform contentFace;

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

	private void Init()
	{
		/// Spawn item skin for face
		for (int i = 0; i < listFace.Count; i++)
        {
			GameObject item = Instantiate(itemButton, contentFace);
			item.GetComponent<ItemButton>().Setup(listFace[i], i);
		}

		/// Spawn item skin for shirt
		for (int i = 0; i < listShirt.Count; i++)
		{
			GameObject item = Instantiate(itemButton, contentShirt);
			item.GetComponent<ItemButton>().Setup(listShirt[i], i);
		}

		/// Spawn item skin for hair
		for (int i = 0; i < listHair.Count; i++)
		{
			GameObject item = Instantiate(itemButton, contentHair);
			item.GetComponent<ItemButton>().Setup(listHair[i], i);
		}

		/// Spawn item skin for pant
		for (int i = 0; i < listPant.Count; i++)
		{
			GameObject item = Instantiate(itemButton, contentPant);
			item.GetComponent<ItemButton>().Setup(listPant[i], i);
		}

		/// Spawn item skin for glasses
		for (int i = 0; i < listGlasses.Count; i++)
		{
			GameObject item = Instantiate(itemButton, contentGlasses);
			item.GetComponent<ItemButton>().Setup(listGlasses[i], i);
		}

		/// Spawn item skin for hat
		for (int i = 0; i < listHat.Count; i++)
		{
			GameObject item = Instantiate(itemButton, contentHat);
			item.GetComponent<ItemButton>().Setup(listHat[i], i);
		}

		Setup(0);
	}

    #endregion
}
