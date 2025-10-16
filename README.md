# Ninjutsu Gameplay Guide

This document summarizes how to play with the bundled ninjutsu system while running the project inside the Unity editor.

## Bundled Unity Scripts
All gameplay code referenced below already lives under `Assets/Scripts/` so you can drag the scripts straight into Unity without writing new files:

- `PlayerController.cs` – Handles movement, health, and damage for the player character.
- `NinjutsuManager.cs` – Contains every ninjutsu element, combo rule, and runtime effect implementation.
- `HeartUIController.cs` – Manages the heart display and ninjutsu slot UI.

## Core Controls
- **Move**: `A` / `D` or the left and right arrow keys. Tune top speed and acceleration from the `PlayerController` component.
- **Jump**: Press `Space`. Adjust jump strength with the `Jump Force` field.
- **Reload Scrolls**: Press the up arrow to refill empty ninjutsu slots. Elements are distributed evenly across fire, water, and wind.
- **Cast Ninjutsu**: Press the left arrow. Abilities spawn from the optional `Projectile Spawn Point`; otherwise they originate at the player.

## Slot Logic and Ability Table
`NinjutsuManager` inspects the filled slots from the top and resolves the strongest valid option.

1. **Combo check** – If the first two slots match one of the pairs below, that combo fires and consumes both slots.
   | Pair | Ability | Effect |
   | --- | --- | --- |
   | Fire + Fire | FireFireBarrier | Emits repeated area pulses. Tune radius (`Fire Fire Radius`) and hit count (`Fire Fire Hits`). |
   | Water + Wind | WaterWindTornado | Spawns a forward-moving tornado. Adjust width (`Tornado Width`) and duration (`Tornado Duration`). |
   | Wind + Fire | WindFireFireball | Launches a fireball. Control its speed with `Fireball Speed`. |
2. **Single element** – If no combo matches, only the first slot is consumed according to this table.
   | Element | Ability | Effect |
   | --- | --- | --- |
   | Fire | FireExplosion | Triggers an instant explosion using `Fire Single Radius`. |
   | Water | WaterArrows | Fires three homing water arrows. Configure with `Water Arrow Speed` and `Water Arrow Spread`. |
   | Wind | WindShield | Summons a rotating shield whose radius comes from `Wind Shield Radius`. |

Each activation decrements the internal balance counters so the next reload favors underrepresented elements.

## UI and Supporting Scripts
- **Health UI** (`HeartUIController`) keeps the heart icons in sync with the `Health` component living inside `PlayerController`.
- **Slot UI** (`NinjutsuSlotUI`) visualizes current elements and fades empty slots.
- **Damage Routing**: Runtime abilities call `IDamageable.TakeDamage`, which is implemented by `Health`. All combat scripts now live in `NinjutsuManager.cs`, while player logic (including `Health` and `IDamageable`) sits in `PlayerController.cs`, and UI scripts share `HeartUIController.cs`.

## Testing Tips
Enter Play Mode in the Unity editor and verify each control. When tuning values, keep the inspector open so you can iterate quickly and confirm balance changes immediately.
