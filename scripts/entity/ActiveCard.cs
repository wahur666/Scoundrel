using System;
using Godot;
using Scoundrel.scripts.model;

namespace Scoundrel.scripts.entity;

public partial class ActiveCard : VBoxContainer {
	private Button _healButton;
	private Button _attackButton;
	private Button _tankButton;
	private Button _equipButton;
	private Card _card;
	private int _index;
	private Board _board;
	private Level _level;

	public override void _Ready() {
		_healButton = GetNode<Button>("Heal");
		_attackButton = GetNode<Button>("Attack");
		_tankButton = GetNode<Button>("Tank");
		_equipButton = GetNode<Button>("Equip");
		_card = GetNode<Card>("Card");
		_healButton.Pressed += HealButtonOnPressed;
		_attackButton.Pressed += AttackButtonOnPressed;
		_tankButton.Pressed += TankButtonOnPressed;
		_equipButton.Pressed += EquipButtonOnPressed;
	}

	public override void _ExitTree() {
		_healButton.Pressed -= HealButtonOnPressed;
		_attackButton.Pressed -= AttackButtonOnPressed;
		_tankButton.Pressed -= TankButtonOnPressed;
		_equipButton.Pressed -= EquipButtonOnPressed;
	}


	public void Init(int index, Board board, Level level) {
		_index = index;
		_board = board;
		_level = level;
	}

	public void SetCard(CardStruct cardStruct) {
		_card.SetCard(cardStruct);
		switch (_card.CardStruct.Suit) {
			case CardSuit.None:
				SetButtonsVisible(false, false, false, false);
				break;
			case CardSuit.Heart:
				SetButtonsVisible(false, true, false, false);
				break;
			case CardSuit.Diamond:
				SetButtonsVisible(false, false, false, true);
				break;
			case CardSuit.Spade:
			case CardSuit.Club:
				var canAttack = false;
				if (_board.Weapon.Suit != CardSuit.None) {
					if (_board.LastSlain.Suit == CardSuit.None || _board.LastSlain.Value > _card.CardStruct.Value) {
						canAttack = true;
					}
				}

				SetButtonsVisible(canAttack, false, true, false);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	public void SetButtonsVisible(bool attack, bool heal, bool tank, bool equip) {
		_healButton.Visible = heal;
		_attackButton.Visible = attack;
		_tankButton.Visible = tank;
		_equipButton.Visible = equip;
	}

	private void EquipButtonOnPressed() {
		var card = _board.ActiveBoard[_index];
		_board.Weapon = card;
		_board.UseEquipCardAtIndex(_index);
		_level.Equip();
	}

	private void TankButtonOnPressed() {
		var damage = _board.UseTankCardAtIndex(_index);
		_level.TankDamage(damage);
	}

	private void AttackButtonOnPressed() {
		var card = _board.ActiveBoard[_index];
		_board.LastSlain = card;
		var damage = _board.UseAttackCardAtIndex(_index);
		_level.TakeDamage(damage);
	}

	private void HealButtonOnPressed() {
		var heal = _board.UseHealCardAtIndex(_index);
		_level.Heal(heal);
	}
}
