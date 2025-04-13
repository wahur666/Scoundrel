using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Scoundrel.scripts.entity;
using Scoundrel.scripts.model;

namespace Scoundrel.scripts;

public partial class Level : Control {
	private GridContainer _cardContainer;
	private Card _weaponCard;
	private Card _weaponMonster;
	
	private Button _runButton;
	private Button _dealButton;
	private Button _newGameButton;

	private Label _remainingDeckLabel;
	private Label _currentHealthLabel;
	private Label _versionLabel;
	private Label _gameStatusLabel;
	private Label _discardLabel;
	private List<ActiveCard> _activeCards;

	public Board Board;
	public int CurrentHealth = 20;

	public override void _Ready() {
		Board = new Board();
		_cardContainer = GetNode<GridContainer>("GridContainer");
		_activeCards = _cardContainer.GetChildren().Select(x => (ActiveCard)x).ToList();
		for (var i = 0; i < _activeCards.Count; i++) {
			_activeCards[i].Init(i, Board, this);
		}

		_weaponCard = GetNode<Card>("Weapon/WeaponCard");
		_weaponMonster = GetNode<Card>("Weapon/WeaponMonster");
		_runButton = GetNode<Button>("DeckPile/VBoxContainer/RunButton");
		_dealButton = GetNode<Button>("DeckPile/VBoxContainer/DealButton");
		_remainingDeckLabel = GetNode<Label>("DeckPile/VBoxContainer2/RemainingDeckLabel");
		_currentHealthLabel = GetNode<Label>("Heart/HeartLabel");
		_newGameButton = GetNode<Button>("NewGameButton");
		_versionLabel = GetNode<Label>("VersionLabel");
		_gameStatusLabel = GetNode<Label>("GameStatusLabel");
		_discardLabel = GetNode<Label>("DiscardPile/VBoxContainer/DiscardLabel");
		SetupEventHandlers();
		NewGame();
	}

	private void NewGame() {
		Board.NewGame();
		_gameStatusLabel.Visible = false;
		_weaponCard.SetCard(Board.Blank());
		_weaponMonster.SetCard(Board.Blank());
		UpdateUi();
	}

	private void SetupEventHandlers() {
		_runButton.Pressed += RunButtonOnPressed;
		_dealButton.Pressed += DealButtonOnPressed;
		_newGameButton.Pressed += NewGameButtonOnPressed;
	}
	private void RemoveEventHandlers() {
		_runButton.Pressed -= RunButtonOnPressed;
		_dealButton.Pressed -= DealButtonOnPressed;
		_newGameButton.Pressed -= NewGameButtonOnPressed;
	}

	private void NewGameButtonOnPressed() {
		CurrentHealth = 20;
		NewGame();
	}

	private void DealButtonOnPressed() {
		Board.DealCards();
		UpdateUi();
	}

	private void RunButtonOnPressed() {
		Board.Run();
		UpdateUi();
	}


	private void UpdateUi() {
		for (int i = 0; i < _activeCards.Count; i++) {
			_activeCards[i].SetCard(Board.ActiveBoard[i]);
		}

		var activeCards = Board.ActiveBoard.Count(x => x.Suit != CardSuit.None);
		_runButton.Disabled = !Board.RunAvailable || activeCards != 4;
		_dealButton.Disabled =  activeCards != 1;
		if (activeCards == 1) {
			PrepareNewDeal();
		}
		_currentHealthLabel.Text = CurrentHealth.ToString();
		_remainingDeckLabel.Text = Board.GetRemainingCards().ToString();
		
		_weaponCard.SetCard(Board.Weapon);
		_weaponMonster.SetCard(Board.LastSlain);
		_discardLabel.Text = Board.GetDiscardCards().ToString();
	}

	private void PrepareNewDeal() {
		Board.PrepareNewDeal();
		_activeCards.ForEach(x => x.SetButtonsVisible(false, false, false, false));
	}

	public void TankDamage(int damage) {
		CurrentHealth = Math.Max(CurrentHealth - damage, 0);
		if (CurrentHealth == 0) {
			EndGame(false);
			return;
		}

		if (Board.IsAllClubsAndSpadesInDiscardDeck()) {
			EndGame(true);
			return;
		}
		
		UpdateUi();
	}

	public void TakeDamage(int damage) {
		var newDamage = Math.Max(damage - _weaponCard.CardStruct.Value, 0);
		TankDamage(newDamage);
	}

	private void EndGame(bool result) {
		_gameStatusLabel.Visible = true;
		_gameStatusLabel.Text = result ? "You Win!" : "You Lose!";
		if (!result) {
			_currentHealthLabel.Text = "0";
		}
		_activeCards.ForEach(x => x.SetButtonsVisible(false, false, false, false));
		_runButton.Disabled = true;
		_dealButton.Disabled = true;
	}

	public void Heal(int amount) {
		CurrentHealth = Math.Min(CurrentHealth + amount, 20);
		UpdateUi();
	}

	public void Equip() {
		UpdateUi();
	}
}
