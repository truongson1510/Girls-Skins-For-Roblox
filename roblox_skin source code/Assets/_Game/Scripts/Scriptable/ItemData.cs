
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
	public Sprite itemSprite;
	
	public bool hasMaterial;
	[ShowIf("hasMaterial")]
	public Material itemMaterial;

	public bool hasTexture;
	[ShowIf("hasTexture")] [PreviewField]
	public Texture2D ingameTexture;
	[ShowIf("hasTexture")] [PreviewField]
	public Texture2D robloxTexture;

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
