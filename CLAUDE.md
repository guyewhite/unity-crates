# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview
Unity-based Sokoban-style puzzle game using Unity 6000.2.9f1 with Universal Render Pipeline (URP).

## Common Development Commands

### Unity Development
- **Open in Unity Hub**: Unity version 6000.2.9f1 is required
- **Play Mode**: Use Unity Editor play button or Ctrl/Cmd+P
- **Build**: File → Build Settings → Select platform → Build

### Git Workflow
```bash
git add .
git commit -m "message"
git push
```

## Architecture

### Core Game Systems

**BoardGenerator** (`Assets/Scripts/BoardGenerator.cs`)
- Procedurally generates game boards with configurable dimensions
- Creates walls around board edges and floor tiles for playable area
- Exposes public properties: `width`, `height`, `holeCount`
- Uses prefab instantiation for tile generation

**CameraController** (`Assets/Scripts/CameraController.cs`)
- Automatically centers camera over the game board
- Calculates optimal camera height based on board dimensions
- Positions camera looking straight down (90° rotation)

### Prefab Structure
- `FloorTile.prefab`: Standard walkable tiles
- `WallTile.prefab`: Board boundary tiles
- Both prefabs located in `Assets/Prefabs/`

### Scene Setup
Main scene: `Assets/Scenes/SampleScene.unity`
- BoardGenerator attached to GameObject in scene
- CameraController automatically finds BoardGenerator at runtime

## Package Dependencies
Key packages used:
- **URP (Universal Render Pipeline)**: Graphics rendering
- **Input System**: New Unity input handling
- **AI Navigation**: For potential pathfinding features
- **Visual Scripting**: Node-based scripting support

## Development Notes
- Tile size is configurable via `tileSize` field (default: 1f)
- Board generates at runtime in Start() method
- All generated tiles are parented to BoardGenerator transform for organization