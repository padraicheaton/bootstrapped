# Overall Design
The weapon generation algorithm is based on a traditional genetic algorithm, altered slightly to fit the gameplay context.

# Design Choices
## Novelty Chance Stacks until Chosen
The weapon generator can either generate weapons randomly or based on the genetic algorithm. The chance for the generator to generate a random weapon is the 'Novelty Chance'. Every time the generator does NOT pick this option, the chance is increased by a fixed amount. Upon this option being selected, it is reset back to its default option. This has been done to ensure  that there are novel weapons being generated regularly, and removing the possibility for a low novelty chance resulting in no novel weapons being generated during the playthrough.

- [[Epsilon Greedy Path Optimisation]]

## Metrics are Stored Normalised
All metrics for each weapon genotype are stored as normalised values for use in the Fitness Function. This is calculated for each metric using this formula:
$$f(x) = x / y$$
$x$ = This objects metric, $y$ = The max value of this metric from all objects in pool

This has been done so that all metrics are stored with a normalised weight despite themselves being of variable ranges. This allows specified weights being able to be placed upon each metric to value them differently regardless of gross range.

# Stored Metrics
To begin, a weapon enters the population pool upon being equipped/picked up by the player, making that the first metric tracked.

For a given genotype:
- The amount of times the clip has been emptied
- The amount of kills that weapon has done
- The amount of times the weapon has been reloaded
- If this weapon is the chosen starting weapon or not