# Rule:
	Stash = Contain data about inventory and crafting of an structure.
	Structure = The object that gonna be build by the player in game.
		Function:
			Tower = Structure that provide defense against enemy.
			Dynamo = Structure that increase max material energy.
		Occupation:
			Tower = An Structure that allow platform to place on it.
			Platform = An structure that allow to place tower inside.
			Fill = An structure that block the whole plot it on.
	Tower:
		x_Caster = Handle the way tower create an assigned attack such as pattern.
		x_Attack = The object create by caster each has indidepent stats, mostly handle damaging enemy.
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