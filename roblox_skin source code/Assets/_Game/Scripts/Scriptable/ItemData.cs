
using UnityEngine;
using Sirenix.OdinInspector;

public enum ItemType
{
	Face,
	Shirt,
	Hair,
	Pant,
	Glasses,
	Hat
}

[CreateAssetMenu(menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
	#region Inspector Variables

	public ItemType itemType;
	[PreviewField]
	public Sprite	itemSprite;
	public Material itemMaterial;

	#endregion

	#region Member Variables

	#endregion

	#region Properties

	#endregion

	#region Unity Methods

	#endregion

	#region Public Methods

	#endregion

	#region Protected Methods

	#endregion

	#region Private Methods

	#endregion
}
