import random

numWeapons = 5
numEffects = 6
numAdditiveDelays = 10 # 0.1, 0.2, 0.3 ... 1.0
numModifierCounts = 4 # 1, 2, 3, 4
numModifiers = 20
numRarities = 5

def get_discrete_gene_permutations():
    return numWeapons * numEffects * numAdditiveDelays * numModifierCounts * numRarities

def get_modifier_permutations():
    permNum = 0

    for i in range(numModifierCounts):
        permNum += pow(numModifiers, i)
    
    return permNum

def get_total_permutations():
    return get_discrete_gene_permutations() * get_modifier_permutations()

def get_example_dna():
    dna = []

    dna.append(random.randint(0,4)) # Weapon Type
    dna.append(random.randint(0,5)) # Effect Type
    dna.append(random.randint(1,10)) # Modifier Additive Delay
    dna.append(random.randint(1,4)) # Number of Modifiers

    for i in range(dna[len(dna)-1]):
        dna.append(random.randint(0,16)) # Select Modifiers to Apply
    
    return dna

totalConfigNum = get_total_permutations()


# Factor out minor differences in the additive delays and rarities, as the most volatile differences are apparent at the ends of the spectrum,
# only factor in those possibilites. I.e. the diff between 0.1 and 0.2 seconds between stacking modifiers is minimal, whilst the diff between
# 0.1 and 1 is distinct. Same for rarities

numAdditiveDelays = 2
numRarities = 2
distinctConfigNum = get_total_permutations()

print("\n")
print("Total Permutations: " + format(totalConfigNum, ',d'))
print("Distinct Permutations: " + format(distinctConfigNum, ',d'))
print("Example DNA:")
for i in range(10):
    print(get_example_dna())
print("\n")