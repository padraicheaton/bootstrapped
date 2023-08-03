# Allele Voting for Preference Based Genetic Evolution of Game Components

## Basic Idea
Traditional genetic algorithms can be altered to suit any generation goal, given the proper fitness function and phenotypic expression. However, for generating content to be used by the player, a much more granular approach must be formed. This is as population sizes are much smaller, combined with fewer generations.

This method will essentially look at the historical component usage data for the player and select the alleles for each gene based on their amount of times used, the more times it is used the more likely it will be picked. This is different to traditional genetic algorithms as it assesses genes/alleles in isolation according to their fitness, rather than assessing entire organisms according to their fitness.