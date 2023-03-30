import random

weapons = ["Machine Gun", "Pistol", "Sniper", "SMG", "Shotgun"]
effects = ["Ice Slow", "Hacking Mind Control", "Electric Stun", "Fire DOT", "Concussive Knockback", "Magnetic", "Critical Damage"]
additiveDelays = ["0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9", "1.0"]
modifierCounts = ["1", "2", "3", "4"]
rarities = ["Common", "Uncommon", "Rare", "Exotic", "Legendary"]
modifiers = [
    "Anti Grav",
    "Bouncy",
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
    return len(weapons) * len(effects) * len(additiveDelays) * len(modifierCounts) * len(rarities)

def get_modifier_permutations():
    permNum = 0

    for count in modifierCounts:
        modCount = int(count)
        permNum += pow(len(modifiers), modCount)
    
    return permNum

def get_total_permutations():
    print("Discrete Gene Permutations: " + format(get_discrete_gene_permutations(), ',d'))
    print("Modifier Gene Permutations: " + format(get_modifier_permutations(), ',d'))
    return get_discrete_gene_permutations() * get_modifier_permutations()

def get_example_dna():
    dna = []

    dna.append(random.randint(0,len(weapons))) # Weapon Type
    dna.append(random.randint(0,len(effects))) # Effect Type
    dna.append(random.randint(0, len(additiveDelays))) # Modifier Additive Delay
    dna.append(random.randint(0, len(modifierCounts))) # Number of Modifiers

    for i in range(int(dna[len(dna)-1])):
        dna.append(random.randint(0,len(modifiers))) # Select Modifiers to Apply
    
    return dna

totalConfigNum = get_total_permutations()


# Factor out minor differences in the additive delays and rarities, as the most volatile differences are apparent at the ends of the spectrum,
# only factor in those possibilites. I.e. the diff between 0.1 and 0.2 seconds between stacking modifiers is minimal, whilst the diff between
# 0.1 and 1 is distinct. Same for rarities

print("Total Permutations: " + format(totalConfigNum, ',d'))
print("\n")
print("Example DNA:")
for i in range(10):
    print(get_example_dna())
print("\n")