import random

weapons = ["Machine Gun", "Pistol", "Sniper", "SMG", "Shotgun"]
effects = ["Ice Slow", "Hacking Mind Control", "Electric Stun", "Fire DOT", "Concussive Knockback", "Magnetic", "Critical Damage"]
additiveDelays = ["0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9", "1.0"]
modifierCounts = ["1", "2", "3", "4"]
rarities = ["Common", "Uncommon", "Rare", "Exotic", "Legendary"]
modifiers = [
    "Anti Grav",
    "Bouncy",
    "Build Up",
    "Condense",
    "Curve",
    "Expand",
    "Explosive",
    "Featherweight",
    "Frictionless",
    "Heavyweight",
    "Homing",
    "Launch",
    "Orbital",
    "Piercing",
    "Platform",
    "Preserved",
    "Rebound",
    "Spiral",
    "Sticky",
    "Velocity Boost",
    "Volatile"
]

def get_discrete_gene_permutations():
    # Remove taking into account the modifier num, as there will never be an instance where the modifier count is different from the amount of modifiers,
    # therefore, in that instance there is only ever one possibility for that slot based on the amount of modifiers selected
    return len(weapons) * len(effects) * len(additiveDelays) * 1 * len(rarities)

def get_distinct_discrete_gene_permutations():
    # Factor out minor differences in the additive delays and rarities, as the most volatile differences are apparent at the ends of the spectrum,
    # only factor in those possibilites. I.e. the diff between 0.1 and 0.2 seconds between stacking modifiers is minimal, whilst the diff between
    # 0.1 and 1 is distinct. Same for rarities

    # Essentially, only accounts for additive delays {0.1, 0.5, 1.0} and rarities {common, rare, legendary}
    return len(weapons) * len(effects) * 3 * 1 * 3

def get_modifier_permutations():
    permNum = 0

    for count in modifierCounts:
        modCount = int(count)
        permNum += pow(len(modifiers), modCount)
    
    return permNum

def get_total_permutations():
    return get_discrete_gene_permutations() * get_modifier_permutations()

totalConfigNum = get_total_permutations()

distinctConfigNum = get_distinct_discrete_gene_permutations() * get_modifier_permutations()

print("\n")
print("Discrete Gene Permutations: " + format(get_discrete_gene_permutations(), ',d'))
print("Distinct Discrete Gene Permutations: " + format(get_distinct_discrete_gene_permutations(), ',d'))
print("Modifier Gene Permutations: " + format(get_modifier_permutations(), ',d'))
print("\n")
print("Total Permutations: " + format(totalConfigNum, ',d'))
print("Distinct Permutations: " + format(distinctConfigNum, ',d'))
print("\n")