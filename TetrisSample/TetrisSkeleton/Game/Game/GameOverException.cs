using System;

namespace TetrisSkeleton {
	public class GameOverException : Exception {
		/// <summary>
		/// Gets the score when the game was ended
		/// </summary>
		/// <value>The score.</value>
		public int FinalScore {
			get;
			private set;
		}
		/// <summary>
		/// Gets the last piece that made the player lose
		/// </summary>
		/// <value>The last piece.</value>
		public Tetromino LastPiece {
			get;
			private set;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TetrisTest.GameOverException"/> class.
		/// </summary>
		/// <param name="score">Player's final score.</param>
		/// <param name="t">The tetromino that lost the game.</param>
		public GameOverException (int score, Tetromino t) : base (){
			LastPiece = t;
			FinalScore = score;
		}

		public override string Message {
			get {
				return String.Format("Game Over from piece {1}! Score: {0}",FinalScore,LastPiece.PieceType);
			}
		}
	}
}

