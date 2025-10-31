# unity-crates

![Game Screenshot](Screenshot.png)

## Overview
A box puzzle game demonstrating fundamental Unity Engine concepts. This is meditative puzzle game where you play as a glowing orb, pushing crates into mystical holes to complete each level. Set above a serene forest with gently swaying trees, the game combines strategic thinking with a calming atmosphere.

## Gameplay
Guide your ethereal orb through procedurally generated puzzles, pushing crates to fill every hole on the board. Each move requires careful planning - crates can only be pushed, not pulled, making every decision count.

### Controls
- **Arrow Keys / WASD** - Move the orb one tile at a time
- **Spacebar** - Reset all crates and player to starting positions
- **Push Crates** - Move into a crate to push it forward

### Objective
Fill all holes with crates to complete each puzzle. When successful, you'll hear a harmonious meditation bowl sound celebrating your achievement.

## Features

### Core Mechanics
- **Tile-Based Movement** - Precise grid-based navigation system
- **Physics-Free Pushing** - Deterministic crate movement without physics complications
- **Smart Collision Detection** - Prevents invalid moves and crate stacking
- **Position Reset System** - Instantly retry puzzles with spacebar

### Visual Experience
- **Glowing Orb Player** - Dual-sphere design with pulsating emission
- **Ethereal Particles** - Orange particles orbiting the player
- **Dynamic Forest** - Procedurally placed trees below the game board
- **Wind Simulation** - Trees sway naturally with customizable wind strength
- **Translucent Holes** - Visual depth through material transparency

### Audio Design
- **Ambient Forest Sounds** - Continuous nature ambience at adjustable volume
- **Stone Slab Effects** - Satisfying sound when pushing crates
- **Meditation Bowl Tones** - Peaceful feedback when crates enter holes
- **Victory Celebration** - Extended bowl resonance upon puzzle completion

### Procedural Generation
- **Dynamic Board Creation** - 6x6 grid with randomized elements
- **Smart Hole Placement** - Ensures solvable puzzle configurations
- **Crate Distribution** - Balanced placement for engaging challenges
- **Forest Generation** - Random tree placement with varied scales

## Technical Implementation

### Architecture
- **Unity 6000.2.9f1** - Built with Unity's latest stable release
- **Universal Render Pipeline (URP)** - Modern rendering for optimal performance
- **Component-Based Design** - Modular scripts for easy maintenance
- **Event System** - Static events for win condition handling

### Key Scripts
- **BoardGenerator.cs** - Procedural level generation with prefab instantiation
- **PlayerController.cs** - Input handling and movement validation
- **HoleDetection.cs** - Position-based detection and win condition
- **Reset.cs** - Universal position memory for retry functionality
- **GroundAndTreeGenerator.cs** - Environment creation with audio management
- **TreeSwaySimulator.cs** - Custom wind animation for standard prefabs
- **SnapToGrid.cs** - Ensures objects align to tile centers

## Project Structure
```
Assets/
├── Prefabs/          # Game objects (Player, Crate, Tiles, Trees)
├── Materials/        # Visual materials and textures
├── Scripts/          # C# game logic
├── Audio/            # Sound effects and ambience
│   ├── forest-surroundings/
│   ├── meditation-bowl/
│   └── stone-slab/
└── Scenes/           # Game scene files
```

## Setup Instructions
1. Clone the repository
2. Open project in Unity 6000.2.9f1 or later
3. Ensure URP is properly configured
4. Open SampleScene from Assets/Scenes
5. Press Play to start the game

## Customization Options

### In Unity Inspector
- **Board Size** - Adjust grid dimensions in BoardGenerator
- **Hole Count** - Control puzzle difficulty
- **Tree Density** - Modify forest thickness
- **Wind Strength** - Change tree sway intensity
- **Audio Volumes** - Fine-tune all sound levels

### Visual Settings
- **Camera Height** - Modify viewing angle
- **Material Properties** - Adjust transparency and colors
- **Particle Effects** - Customize orb's ethereal particles
- **Emission Intensity** - Control orb glow strength

## Credits
- Developed as a Unity learning project
- Audio assets from royalty-free sources
- Built with Unity's Universal Render Pipeline