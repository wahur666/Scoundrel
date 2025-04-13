using Godot;

namespace Scoundrel.scripts.model;

public struct CardStruct(CardSuit suit, int value, string assetPath)
{
    public CardSuit Suit = suit;
    public int Value = value;
    public string AssetPath = assetPath;
    private CompressedTexture2D _texture = null;
    
    public CompressedTexture2D Texture {
	    get => _texture ??= GD.Load<CompressedTexture2D>(AssetPath);
	    set => _texture = value;
    }	
}
