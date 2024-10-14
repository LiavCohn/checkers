# Checkers Game

This project is a classic implementation of the **Checkers (Draughts)** game, built as a full-stack application. The frontend is developed using **React**, while the backend is powered by **ASP.NET Core**.

## Features

- **Two-player Mode**: Play against another player locally, taking turns to move your pieces across the board.
- **Checker Movement Rules**: The game enforces standard checkers rules, including:
  - Diagonal moves for pieces.
  - Capturing opponent pieces by jumping over them.
  - End-of-game detection when one player runs out of pieces or has no valid moves left.
- **Piece Highlighting**: Selected pieces are highlighted to show the active player’s turn.
- **Game Reset**: A reset button allows players to restart the game at any time.

## Technology Stack

### Frontend:

- **React**: For the user interface, allowing dynamic updates of the board and player moves.
- **CSS**: Custom styles for the board, checkers pieces, and game interface.

### Backend:

- **ASP.NET Core**: RESTful API for managing the game state, player turns, and move validations.
- **C#**: Logic for checkers rules, move validation, and game status tracking.

### Deployment:

- **API**: Deployed using ASP.NET Core to handle move requests and game state.
- **Frontend**: Served with the React app, which communicates with the backend API.

## API Endpoints

- **GET /game**: Fetches the current state of the game board and whose turn it is.
- **POST /game/move**: Processes a move made by a player and updates the board.
- **POST /game/reset**: Resets the game to its initial state, starting with player 1’s turn.
