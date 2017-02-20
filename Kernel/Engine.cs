﻿using System;

using TrulyQuantumChess.Kernel.Errors;
using TrulyQuantumChess.Kernel.Moves;
using TrulyQuantumChess.Kernel.Chess;
using TrulyQuantumChess.Kernel.Quantize;

namespace TrulyQuantumChess.Kernel.Engine {
    public class QuantumChessEngine {
        public QuantumChessEngine() {
            Chessboard_ = QuantumChessboard.StartingQuantumChessboard();
            ActivePlayer_ = Player.White;
        }

        private readonly QuantumChessboard Chessboard_;
        private Player ActivePlayer_;

        public QuantumChessboard Chessboard {
            get { return Chessboard_; }
        }

        public Player ActivePlayer {
            get { return ActivePlayer_; }
        }

        public GameState GameState {
            get { return Chessboard_.GameState; }
        }

        public void Submit(QuantumChessMove move) {
            if (move.ActorPlayer != ActivePlayer_)
                QuantumChessEngineException.Throw("Waiting for another player's move");

            if (move is CapitulateMove) {
                Chessboard_.RegisterVictory(PlayerUtils.InvertPlayer(ActivePlayer_));
            } else if (move is OrdinaryMove) {
                var omove = move as OrdinaryMove;
                if (Chessboard_.CheckOrdinaryMoveApplicable(omove)) {
                    Chessboard_.ApplyOrdinaryMove(omove);
                    ActivePlayer_ = PlayerUtils.InvertPlayer(ActivePlayer_);
                } else {
                    QuantumChessEngineException.Throw("Move is unapplicable on all harmonics");
                }
            } else if (move is QuantumMove) {
                var qmove = move as QuantumMove;
                if (Chessboard_.CheckQuantumMoveApplicable(qmove)) {
                    Chessboard_.ApplyQuantumMove(qmove);
                    ActivePlayer_ = PlayerUtils.InvertPlayer(ActivePlayer_);
                } else {
                    QuantumChessEngineException.Throw("Quantum move is unapplicable on all harmonics");
                }
            } else {
                AssertionException.Assert(false, $"Unsupported move type: {move.GetType().Name}");
            }
        }
    }
}