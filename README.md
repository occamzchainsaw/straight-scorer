# Straight Scorer

Straight Scorer is a dedicated scoring application for **14.1 Continuous Pool (Straight Pool)**. Built using **.NET MAUI**, it provides a streamlined interface to track match progress, manage rack transitions, and maintain a detailed history of breaks and match results. Curently supports Android and Windows.

There are plenty of scoring apps for Straight Pool around, but when my friend and I tried to find one, it turned out many were overly complicated or had some simple features missing/not working. I decided I'll have a go at making a scoring app myself and this is the result. Practically no fluff, easy to use and has a few 'quality of life' features. While I plan to put Straight Scorer up on the Google Play Store, I want to, first of all, share the code, because I believe open-source software is important and if I'm able to contribute to the community, at least a little bit, I should.

## Features

* **14.1 Core Scoring**: Track points for each player with support for legal pocketed shots, and "head start" handicaps.
* **Game State Management**: Automatically tracks balls remaining on the table (starting at 15), the current rack number, and the current break (or inning) of the player at the table.
* **Action Tracking**: Specific commands to handle different break endings:
    * **Miss**: Transitions play to the next player.
    * **Safe**: Records a safety shot.
    * **Foul**: Penalizes the player by subtracting points and tracks total fouls.
* **Comprehensive Undo/Redo**: A robust command-based system allows users to revert any action.
* **Break History**: Logs every scoring run, including the player's name, points scored, and the action that ended the break.
* **Match History**: Saves completed match summaries, including final scores, average breaks, highest breaks, and total fouls.
* **Dynamic UI**: 
    * **Head-to-Head View**: Optimized layout for two-player matches.
    * **List View**: Layout for more than two players.
    * **Adaptive Controls**: Custom controls for numeric and text entries, and active player indicators.
    * **Light and Dark Modes**: Adjustable in the settings.
* **In-App Rules**: Includes a full copy of the 14.1 Continuous Pool rules for quick reference during play.

## User Guide

### Game Setup

<img width="356" height="800" alt="game_setup_page" src="https://github.com/user-attachments/assets/408506ce-fd87-456c-b78b-bc3985e22d40" />

Set a target score, and the players. You can add up to 10 players, becuase why not? Select which player will play the break off shot and, if you wish, give them a head start. Swipe a player from the left to remove them. You can even remove all but one player if you want to practise solo and track your session.

### Match Screen

<img width="356" height="800" alt="match_screen" src="https://github.com/user-attachments/assets/5d751f51-c9f9-4add-9033-f9eb5101d4b3" />
<img width="356" height="800" alt="match_screen_2" src="https://github.com/user-attachments/assets/e0fc3ff9-86d0-49ad-8df7-b30057e90d20" />
<img width="356" height="800" alt="match_screen_end" src="https://github.com/user-attachments/assets/6b4e6950-e0a1-4c9b-ad60-a77799f5a404" />

The match screen shows the current score, current break, balls left on table, and rack number, as well as the game's history in the form of a 'timeline'. Use the action buttons to reflect what goes on at the table. Consecutive fouls are counted for each player and will be displayed as red dots next to their names. A regular two-player game will have the head-to-head display, while a game with more players will display them in a list.

When the match ends, you can save the results, and view them later in your Match History.

### Match History

<img width="356" height="800" alt="match_history" src="https://github.com/user-attachments/assets/a022ad3f-5643-4cbf-a409-bf5306718b46" />

Saved matches will appear here. The displayed statistics are basic, but useful - highest break, average break and total number of fouls commited by each player. An entry can be deleted by swiping from the left, and the whole history can be cleared by pressing the 'Clear History' button. Your match history is stored on device.

### Rules Page

<img width="356" height="800" alt="rules_page" src="https://github.com/user-attachments/assets/307b7674-2bb9-4fe2-9828-d4a9a2c49ef5" />
<img width="356" height="800" alt="rules_page_2" src="https://github.com/user-attachments/assets/a3c2fd3a-0611-42e1-afd7-febff40091e9" />

The official WPA rules are included right in the application, so if you ever need to check them (even during a game), they are right there. This is a fully functional markdown document with a table of contents and working links.

### App Settings

<img width="356" height="800" alt="settings_page" src="https://github.com/user-attachments/assets/99f92661-fa39-461a-83bf-ed1ea4cb7783" />

Currently, the settings are very simple. You can select the app's theme and there is one game setting. I included this setting, because this seems to be fairly niche rule and it is very punishing. If you want to learn more, check sections 12 and 12.1 of the rules :)

## Privacy

The app collects no user data, and requires no extra permissions.

## Repository Structure

The repository is divided into two main projects to separate game logic from the UI framework:

### StraightScorer.Core
This project contains the platform-agnostic game logic and data models.
* **Models**: Definitions for `Player`, `Break`, and `MatchResult`.
* **Services**: 
    * `GameState`: The central engine managing the current match, scoring logic, and player turns.
    * **Commands**: Implementation of the Command pattern for `AddPoints`, `Foul`, `Miss`, and `Safe` to support undo/redo.
* **Interfaces**: Abstractions for settings, match history, and undo/redo services.

### StraightScorer.Maui
This is the .NET MAUI implementation providing the cross-platform UI.
* **ViewModels**: Driven by the `CommunityToolkit.Mvvm`, these classes (e.g., `GameViewModel`) bridge the UI to the Core logic.
* **Pages**: XAML definitions for the Game, Match History, Settings, and Setup screens.
* **Views/Controls**: Reusable UI components like `BreakHistoryItem` and specialized score views.
* **Services**: Platform-specific implementations such as `SqliteMatchHistoryService` for local data storage.
* **Resources**: Contains application styles, icons, and the raw Markdown file for pool rules.

## Getting Started for Developers

### Prerequisites
* **.NET 8.0 SDK** or later.
* **Visual Studio 2022** (with .NET MAUI workload installed) or **VS Code** with the .NET MAUI extension.

### Development Tips
* **Messaging**: The app uses `WeakReferenceMessenger` from the Community Toolkit to communicate state changes (like game progress) across the application without tight coupling.
* **Dependency Injection**: Services and ViewModels are registered in the application's startup logic, facilitating easy testing and maintenance.
* **Platform Specifics**: Platform-specific configurations (Android Manifests, iOS Info.plist, etc.) are located in the `Platforms` directory.

### Dependencies

* [CommunityToolkit.MVVM](https://github.com/CommunityToolkit/dotnet)
* [CommunityToolkit.Maui](https://github.com/CommunityToolkit/Maui)
* [markdig](https://github.com/xoofx/markdig)
* [Mopups](https://github.com/LuckyDucko/Mopups)
* [sqlite-net-pcl](https://github.com/praeclarum/sqlite-net)

Thank you to all the kind souls for these open-source projects, which allow developers to make better software.

# License and Contributions

This is, by no means, a professional endevour. Merely a hobbyist project. If you'd like to contribute, feel free to create a pull request. If you find bugs or have ideas for improvements but can't/don't want to implement them yourself - start a thread and label it accordingly. That said, I make no guarantees I will get it done.

This project is under the MIT License, so if you would like to fork it and grow it into something much bigger - have at it!
