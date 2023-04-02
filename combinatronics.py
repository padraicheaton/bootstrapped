import math
import random

numExampleWeapons = 5
numProjectileModifiers = 4

#Range Categories: Long, Medium, Short
weaponTypes = ["Pistol", "Machine Gun", "Shotgun", "SMG", "Sniper"]
effectTypes = ["Critical Damage", "Fire DOT", "Stun", "Knockback", "Lifesteal"]

#ADD IN SPLIT WHEN IMPLEMENTED
projectileModifiers = [
    "Bouncy",
    "Homing",
    "Heavyweight",
    "Anti-Grav",
    "Velocity Boost",
    "Expand",
    "Condense",
    "Spiral",
    "Sticky",
    "Featherweight",
    "Curve",
    "Jetpack",
    "Slippery",
    "Platform",
    "Explosive",
    "Preserved",
    "Volatile",
    "Piercing",
    "Orbital",
    "Rebound"
]

def get_permutations():
    # formula for number of permutations with repetitions is (totalChoiceNum)^numSelected
    numConfigs = pow(len(projectileModifiers), numProjectileModifiers)
    numConfigs *= len(weaponTypes)
    numConfigs *= len(effectTypes)

    return numConfigs

def print_example_weapon():
    print(random.choice(effectTypes) + " " + random.choice(weaponTypes))
    for i in range (numProjectileModifiers - 1):
        print(random.choice(projectileModifiers) + ", then ", end = '')
    
    print(random.choice(projectileModifiers))


print("\n\nNumber of Possible Combinations: " + str(get_permutations()))
print("\tWith Rarity: " + str(get_permutations() * 5) + "\n\n")
print("Example Weapons:\n")

for i in range (numExampleWeapons):
    print_example_weapon()
    print("\n")