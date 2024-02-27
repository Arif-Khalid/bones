# Cloudy with a Chance of Pizza
You find yourself in a wonderful land where its raining pizzas!  
Catch them as they fall and be the cool friend that brings pizzas for once.  
Watch out though as pizzas aren't the only things falling!
# Features
* Falling pizzas that stack on your tray, you run around to collect the pizzas
* Top of the stack is harder to control as it grows, making it harder to collect pizzas the more you have
* Bombs might spawn in place of pizzas that will destroy your stack and end the game, though without a stack you find they don't hurt you at all!
* Don't worry, press your primary action (usually left mouse button) while a bomb is directly under your tray to kick it away
* Items spawn more frequently and chances of bomb spawning increases over time
* When the game ends, you see all the pizzas (and other items in the future) you collected while playing
# Requirements met
* 3D interactable objects
  * Explicitly interactable objects are the pizzas and the bombs
* Interactive UI with layout groups and scroll views
  * Menu and Player UI uses layout groups for consistency
  * Scroll view on the end screen containing displays for each item collected
* Runtime updating of materials/shaders
  * Bombs material colour pulses indicating explosion imminent
  * Pizza uses dithering shader graph to fade away after landing on the ground
* Physics interactions
  * Almost every gameplay interaction relies on Physics
* Considerable usage of C# scripts
  * A grand total of 23
## Bonus
* Lose condition
  * No win condition but you lose when your stack gets blown up
* Use of URP
  * Project was built using URP mainly to use shader graphs for customisable shaders
* Use of shaders
  * Custom dithering transparency shaders for pizza
* Particle systems
  * Particle system used for bomb fuse and explosion
