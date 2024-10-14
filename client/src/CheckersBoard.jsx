import React, { useState, useEffect } from 'react';
import './styles.css';
import api from "./api"

const CheckersBoard = () => {
  const [board, setBoard] = useState([]);
  const [currentTurn, setCurrentTurn] = useState('player1');
  const [selectedPiece, setSelectedPiece] = useState(null);
  const [gameMessage, setGameMessage] = useState('');

  useEffect(() => {
    fetchBoard();
  }, []);
    
  const setGame = (response) => {
      setBoard(response.data.board);
      setCurrentTurn(response.data.currentTurn);
      setGameMessage(response.data.message || '');
  }
    
  const fetchBoard = async () => {
    const response = await api.get();
    // setBoard(response.data.board);
    // setCurrentTurn(response.data.currentTurn);
      // setGameMessage('');
      setGame(response);
  };

  // move piece
    const handleClick = async (row, col) => {
        if (selectedPiece) {
        //if player selects the same piece = diselect
        if (selectedPiece.row === row && selectedPiece.col === col) {
            setSelectedPiece(null);
            return;
          }
             const moveRequest = {
                FromRow: selectedPiece.row,
                FromCol: selectedPiece.col,
                ToRow: row,
                ToCol: col,
            };
            try {
                const response = await api.post('/move', moveRequest)
                // console.log({ response })
                // setBoard(response.data.board);
                // setCurrentTurn(response.data.currentTurn);
                // setGameMessage(response.data.message || '');
                setGame(response);
                setSelectedPiece(null);
                
            } catch (error) {
                alert(error.response.data);
                setSelectedPiece(null);
            }

    } else {
      // select piece
      if (board[row][col] && board[row][col].player === currentTurn) {
        setSelectedPiece({ row, col });
      }
    }
  };

  const resetGame = async () => {
    try {
        const response = await api.post('/reset');  
        // setBoard(response.data.board);  
        // setCurrentTurn(response.data.currentTurn);  
        // setGameMessage(response.data.message || '');  
        setGame(response);
        setSelectedPiece(null);  
    } catch (error) {
        console.error("Error resetting game:", error);
        alert("Failed to reset the game.");
    }
    };
    return (
      <div>
         <div className={`turn-indicator`}>
          Current Turn: <div className={currentTurn === 'player1' ? 'player1-text' : 'player2-text'}>{currentTurn}</div>
          </div>
          <div className="checkers-board">
            {board.map((row, rowIndex) => (
              <div key={rowIndex} className="checkers-row">
                {row.map((checker, colIndex) => {
                  const isSelected = selectedPiece && selectedPiece.row === rowIndex && selectedPiece.col === colIndex;
                  return (
                    <div
                      key={colIndex}
                      className={`checker-square ${((rowIndex + colIndex) % 2 === 0) ? 'white' : 'black'}`}
                      onClick={() => handleClick(rowIndex, colIndex)}
                    >
                      {checker && (
                        <div className={`checker ${checker.player} ${isSelected ? 'selected' : ''}`}></div>
                      )}
                    </div>
                      );

                })}
              </div>
            ))}
      </div>
      <button onClick={resetGame} className="reset-button">
                Reset Game
            </button>
      {gameMessage && <div className="game-message">{gameMessage}</div>}
    </div>
  );
};

export default CheckersBoard;
