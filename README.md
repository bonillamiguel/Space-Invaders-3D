# Space Invaders 3D

Miguel Bonilla, CSCI 381: Video Game Design and Development, Fall 2026, Assignment 1

## Running the game

1. Open the project in Unity 6000.6.0f1.
2. Open `Assets/Scenes/SampleScene.unity`.
3. Press Play, then click **Start**.

**Controls:** Left and Right arrow keys to move, Space to fire.

Destroy every alien to win. You lose if you run out of lives or if the aliens get down to the ship.

## Required changes

| Requirement | What I did | Where |
|---|---|---|
| Aliens speed up as others are destroyed | Fleet speed scales from its starting speed up to `maxSpeed` based on the share of aliens destroyed. | `AlienFleet.FixedUpdate` |
| Detect when the game can no longer be won | Darts only travel upward from the ship, so the game ends when the lowest alien gets down to about the ship's height. | `AlienFleet.FixedUpdate`, `Spaceship.GameOver` |
| Keep the spaceship on screen | The ship's position is clamped to the camera's visible width, recalculated each frame. | `Spaceship.Update` |
| Fix at least three code issues | Fixed the issues listed below. | See below |

## Code issues fixed

1. **Redundant `GetComponent` call (`Alien.OnCollisionEnter`).** `fleet` is already an `AlienFleet`, so `fleet.GetComponent<AlienFleet>().aliens` is now `fleet.aliens`.
2. **Hard-coded screen bounds (`AlienFleet`).** The fleet turned around at fixed values of `-16` and `16`, which only suit certain aspect ratios. The bounds now come from the camera's size and aspect ratio.
3. **Fire rate tied to the physics timestep (`Alien`, `AlienFleet`).** Each alien used a fixed chance of firing per physics tick (`bulletSpawnRate`), so changing the timestep would change the fire rate. The rate is now set in shots per second and converted using `Time.fixedDeltaTime`.
4. **Non-idiomatic fleet movement (`AlienFleet.FixedUpdate`).** The sign of `speed` also stored the fleet's direction, which required `position.y += -Mathf.Abs(speed)` when moving down. Direction now has its own variable.

## Additional features

| Feature | What I did | Where |
|---|---|---|
| Starting and ending screens | A start screen appears when the game opens. Winning or losing shows "You Win!" or "Game Over" with Replay and Quit buttons. | `GameMenu`, `Spaceship.Win`, `Spaceship.GameOver` |
| Lives | The player has 3 lives, shown as full and empty hearts. | `Spaceship.LoseLife`, `Spaceship.UpdateHearts` |
| Better graphics | Replaced the aliens with a slime model and the spaceship with a new ship model. | `Assets/Alien.prefab`, `Assets/Spaceship.prefab` |
| Collision animation | The ship blinks after being hit, and it can't lose another life while blinking. | `Spaceship.DamageFlash` |
| New background | Added a space background image. | `SampleScene`, `Assets/Materials/back.png` |

I also made the aliens fire more often as their numbers drop (`AlienFleet.FixedUpdate`).

## Credits and AI use

Asset sources, tools used and licenses are listed in [CREDITS.md](CREDITS.md).
