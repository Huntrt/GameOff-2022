# Rule:
	Stash = Contain data about inventory and crafting of an structure.
	Structure = The object that gonna be build by the player in game.
		Function:
			Filler = Do nothing.
			Tower = Structure that provide active defense against enemy.
			Dynamo = Structure that increase max material energy.
		Occupation:
			Tower = An structure that allow platform to place on it.
			Platform = An structure that allow to place tower inside.
			Fill = An structure that block the whole plot it on.
	Tower:
		Tower_CasterX = Handle the way tower create an assigned [strike] or deal damage themself.
		Tower_StrikeX = Object create by [caster] that has unquie way to function.
	Capacity = Term refer to both current energy and max energy.

# Inherit
	Entity:
		Structure:
			Dynamo.
			Tower.
		Enemy .

# Color code:
	Health = #17FF59
	Energy = #ECFF17
	Damage = #FF3333
	Speed = #336CFF
	Range = #FF33ED
	Consumption = #FF7133
	Aim = #9B33FF