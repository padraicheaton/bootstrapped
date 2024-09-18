# Project Bootstrapped

_Created as part of fulfilling a Masters of Computing Science (Research) at the University of Technology Sydney_

## Overview

This game prototype's intended use case is as the primary component of experiments investigating the validity and application of an experimental algorithm (see more in below section). This project investigates the ability of games to automatically customise content towards a player's perceived preferences, using in-game heuristics to remain unintrusive to the player experience. As this game is intended as a tool to test an algorithm, design focus has been directed towards clinical and scientific experiment conditions - with the gameplay experience centering around this.

## Processes & Approaches

Agile/SCRUM, Co-Designed with stakeholders (Supervisor, Target Market, Ethics Committee)
In order to create this prototype, a modified Agile approach was used. Creating this prototype as an individual saw fortnightly check-ins with a Supervisory panel on the progress of development; these check-ins acted as the retrospective meetings that each informed the following user stories in the self-determined planning meeting. Throughout this process, the prototype was exposed to those in the target market to co-design many features and content created for the game. Finally, the UTS Ethics Committee was the most important stakeholder as the experiment can only be conducted pending their approval; this informed many design decisions in order to comply with their requirements.

## Experimental Algorithm

Named Theorised Best Parent Optimisation (TBPO), the experimental algorithm featured in this prototype is a modified version of a genetic algorithm. In this prototype, the generation targets are weapons for the player to use which themselves are composed of many interchangeable components. Using heurisitcs derived from player interaction metrics, the genetic algorithm constructs generations of weapons for the player when they interact with a loot box. In creating these generations, the most 'fit' (based on the heuristics) members of the previous population are used as blueprints for the following weapons. To speed up the process of finding optimal solutions, upon generation one parent is artificially constructed from the most favoured components in each category - matching the theoretically ideal parent with the historically favoured weapons to create a new generation of more personalised weapons. This algorithm is also supported by novelty and mutation chances to facilitate the exploration of new playstyles, allow for random chance for the algorithm to stumble upon ideal solutions, and to remove the 'pigeon-holing' effect.

## Tools Used

- Unity Game Engine
- C#
- Git (GitHub)
- Game Experience Questionnaire (GEQ)
