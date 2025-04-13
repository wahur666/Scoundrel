using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Scoundrel.scripts.model;

public class Board {
	public static readonly ImmutableList<CardStruct> FullDeck = [
		new() { AssetPath = "res://assets/Cards/card_hearts_02.png", Suit = CardSuit.Heart, Value = 2 },
		new() { AssetPath = "res://assets/Cards/card_hearts_03.png", Suit = CardSuit.Heart, Value = 3 },
		new() { AssetPath = "res://assets/Cards/card_hearts_04.png", Suit = CardSuit.Heart, Value = 4 },
		new() { AssetPath = "res://assets/Cards/card_hearts_05.png", Suit = CardSuit.Heart, Value = 5 },
		new() { AssetPath = "res://assets/Cards/card_hearts_06.png", Suit = CardSuit.Heart, Value = 6 },
		new() { AssetPath = "res://assets/Cards/card_hearts_07.png", Suit = CardSuit.Heart, Value = 7 },
		new() { AssetPath = "res://assets/Cards/card_hearts_08.png", Suit = CardSuit.Heart, Value = 8 },
		new() { AssetPath = "res://assets/Cards/card_hearts_09.png", Suit = CardSuit.Heart, Value = 9 },
		new() { AssetPath = "res://assets/Cards/card_hearts_10.png", Suit = CardSuit.Heart, Value = 10 },
		new() { AssetPath = "res://assets/Cards/card_diamonds_02.png", Suit = CardSuit.Diamond, Value = 2 },
		new() { AssetPath = "res://assets/Cards/card_diamonds_03.png", Suit = CardSuit.Diamond, Value = 3 },
		new() { AssetPath = "res://assets/Cards/card_diamonds_04.png", Suit = CardSuit.Diamond, Value = 4 },
		new() { AssetPath = "res://assets/Cards/card_diamonds_05.png", Suit = CardSuit.Diamond, Value = 5 },
		new() { AssetPath = "res://assets/Cards/card_diamonds_06.png", Suit = CardSuit.Diamond, Value = 6 },
		new() { AssetPath = "res://assets/Cards/card_diamonds_07.png", Suit = CardSuit.Diamond, Value = 7 },
		new() { AssetPath = "res://assets/Cards/card_diamonds_08.png", Suit = CardSuit.Diamond, Value = 8 },
		new() { AssetPath = "res://assets/Cards/card_diamonds_09.png", Suit = CardSuit.Diamond, Value = 9 },
		new() { AssetPath = "res://assets/Cards/card_diamonds_10.png", Suit = CardSuit.Diamond, Value = 10 },
		new() { AssetPath = "res://assets/Cards/card_spades_02.png", Suit = CardSuit.Spade, Value = 2 },
		new() { AssetPath = "res://assets/Cards/card_spades_03.png", Suit = CardSuit.Spade, Value = 3 },
		new() { AssetPath = "res://assets/Cards/card_spades_04.png", Suit = CardSuit.Spade, Value = 4 },
		new() { AssetPath = "res://assets/Cards/card_spades_05.png", Suit = CardSuit.Spade, Value = 5 },
		new() { AssetPath = "res://assets/Cards/card_spades_06.png", Suit = CardSuit.Spade, Value = 6 },
		new() { AssetPath = "res://assets/Cards/card_spades_07.png", Suit = CardSuit.Spade, Value = 7 },
		new() { AssetPath = "res://assets/Cards/card_spades_08.png", Suit = CardSuit.Spade, Value = 8 },
		new() { AssetPath = "res://assets/Cards/card_spades_09.png", Suit = CardSuit.Spade, Value = 9 },
		new() { AssetPath = "res://assets/Cards/card_spades_10.png", Suit = CardSuit.Spade, Value = 10 },
		new() { AssetPath = "res://assets/Cards/card_spades_J.png", Suit = CardSuit.Spade, Value = 11 },
		new() { AssetPath = "res://assets/Cards/card_spades_Q.png", Suit = CardSuit.Spade, Value = 12 },
		new() { AssetPath = "res://assets/Cards/card_spades_Q.png", Suit = CardSuit.Spade, Value = 13 },
		new() { AssetPath = "res://assets/Cards/card_spades_A.png", Suit = CardSuit.Spade, Value = 14 },
		new() { AssetPath = "res://assets/Cards/card_clubs_02.png", Suit = CardSuit.Club, Value = 2 },
		new() { AssetPath = "res://assets/Cards/card_clubs_03.png", Suit = CardSuit.Club, Value = 3 },
		new() { AssetPath = "res://assets/Cards/card_clubs_04.png", Suit = CardSuit.Club, Value = 4 },
		new() { AssetPath = "res://assets/Cards/card_clubs_05.png", Suit = CardSuit.Club, Value = 5 },
		new() { AssetPath = "res://assets/Cards/card_clubs_06.png", Suit = CardSuit.Club, Value = 6 },
		new() { AssetPath = "res://assets/Cards/card_clubs_07.png", Suit = CardSuit.Club, Value = 7 },
		new() { AssetPath = "res://assets/Cards/card_clubs_08.png", Suit = CardSuit.Club, Value = 8 },
		new() { AssetPath = "res://assets/Cards/card_clubs_09.png", Suit = CardSuit.Club, Value = 9 },
		new() { AssetPath = "res://assets/Cards/card_clubs_10.png", Suit = CardSuit.Club, Value = 10 },
		new() { AssetPath = "res://assets/Cards/card_clubs_J.png", Suit = CardSuit.Club, Value = 11 },
		new() { AssetPath = "res://assets/Cards/card_clubs_Q.png", Suit = CardSuit.Club, Value = 12 },
		new() { AssetPath = "res://assets/Cards/card_clubs_Q.png", Suit = CardSuit.Club, Value = 13 },
		new() { AssetPath = "res://assets/Cards/card_clubs_A.png", Suit = CardSuit.Club, Value = 14 }
	];

	public bool RunAvailable { get => !_runnned; }

	public static CardStruct Blank() {
		return new CardStruct { AssetPath = "res://assets/Cards/card_back.png", Suit = CardSuit.None, Value = 0 };
	}
	private List<CardStruct> _currentDeck;
	private List<CardStruct> _discardDeck;
	public CardStruct[] ActiveBoard = [Blank(), Blank(), Blank(), Blank()];

	public CardStruct Weapon = Blank();
	public CardStruct LastSlain = Blank();
	private bool _runnned = false;
	private bool _healed = false;

	public Board() {
		_currentDeck = FullDeck.Shuffle2().ToList();
		_discardDeck = [];
	}

	public void DealCards() {
		while (ActiveBoard.Any(x => x.Suit == CardSuit.None)) {
			if (_currentDeck.Count == 0) {
				return;
			}
			var card = _currentDeck[0];
			_currentDeck.RemoveAt(0);
			var index = ActiveBoard.ToList().FindIndex(x => x.Suit == CardSuit.None);
			ActiveBoard[index] = card;
		}
	}

	public string Serialize() {
		return "[" + string.Join(", ", _currentDeck.Select(card => card.ToString())) + "]";
	}

	public int UseHealCardAtIndex(int index) {
		var card = ActiveBoard[index];
		if (card.Suit != CardSuit.Heart) {
			throw new InvalidOperationException("Cannot use non-heart card to heal");
		}

		ActiveBoard[index] = Blank();
		_discardDeck.Add(card);
		if (_healed) {
			return 0;
		}

		_healed = true;
		return card.Value;
	}
	
	public int UseAttackCardAtIndex(int index) {
		var card = ActiveBoard[index];
		if (card.Suit != CardSuit.Spade && card.Suit != CardSuit.Club) {
			throw new InvalidOperationException("Cannot use non-spade or non-club card to attack");
		}
		ActiveBoard[index] = Blank();
		_discardDeck.Add(card);
		return card.Value;
	}
	
	public int UseTankCardAtIndex(int index) {
		var card = ActiveBoard[index];
		if (card.Suit != CardSuit.Spade && card.Suit != CardSuit.Club) {
			throw new InvalidOperationException("Cannot use non-spade or non-club card to tank");
		}
		ActiveBoard[index] = Blank();
		_discardDeck.Add(card);
		return card.Value;
	}
	
	public void UseEquipCardAtIndex(int index) {
		var card = ActiveBoard[index];
		if (card.Suit != CardSuit.Diamond) {
			throw new InvalidOperationException("Cannot use non-diamond card to equip");
		}
		ActiveBoard[index] = Blank();
		_discardDeck.Add(card);
		LastSlain = Blank();
	}

	public void NewGame() {
		_runnned = false;
		_currentDeck = FullDeck.Shuffle2().ToList();
		_discardDeck = [];
		ActiveBoard = [Blank(), Blank(), Blank(), Blank()];
		Weapon = Blank();
		LastSlain = Blank();
		DealCards();
	}
	
	public int GetRemainingCards() {
		return _currentDeck.Count;
	}
	
	public int GetDiscardCards() {
		return _discardDeck.Count;
	}

	public bool IsAllClubsAndSpadesInDiscardDeck() {
		return _discardDeck.Count(x => x.Suit is CardSuit.Club or CardSuit.Spade) == 26;
	}

	public void Run() {
		_runnned = true;
		_currentDeck.AddRange(ActiveBoard);
		ActiveBoard = [Blank(), Blank(), Blank(), Blank()];
		DealCards();
	}

	public void PrepareNewDeal() {
		_runnned = false;
		_healed = false;
	}
}
