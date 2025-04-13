using Godot;
using Scoundrel.scripts.model;

namespace Scoundrel.scripts.entity;

public partial class Card : TextureRect {
	public CardStruct CardStruct;

	public void SetCard(CardStruct cardStruct) {
		CardStruct = cardStruct;
		Texture = GD.Load<CompressedTexture2D>(cardStruct.AssetPath);
	}
}
