using System;
using System.Collections.Generic;

namespace TetrisSkeleton {
	public class TetrisBoard {
		#region board propertiest
		/// <summary>
		/// The width in tetromino units of a board
		/// </summary>
		public int WidthBoard {
			get;
			private set;
		}
		/// <summary>
		/// The height in tetromino units of a board
		/// </summary>
		public int HeightBoard {
			get;
			private set;
		}
		/// <summary>
		/// The representation of the current game board state
		/// </summary>
		protected int[,] GameBoard;
		/// <summary>
		/// Gets the starting row for new pieces
		/// </summary>
		/// <value>The starting row.</value>
		protected virtual int StartingRow {
			get {
				return 0;
			}
		}

		protected virtual int StartingCol {
			get {
				return (int) (WidthBoard - Tetromino.PIECE_SIZE)/2;
			}
		}
		#endregion
		#region properties
		/// <summary>
		/// The next pieces.
		/// </summary>
		protected List<Tetromino> NextPieces = new List<Tetromino>();
		/// <summary>
		/// The game random.
		/// </summary>
		protected Random GameRandom;
		/// <summary>
		/// Gets or sets the active piece.
		/// </summary>
		/// <value>The active piece.</value>
		public Tetromino ActivePiece{
			get;
			protected set;
		}
		/// <summary>
		/// Accessor for the score of the game so far.
		/// </summary>
		/// <value>The score so far.</value>
		public int Score{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets the number of starting pieces.
		/// </summary>
		/// <value>The number of starting pieces visible at the beginning of the game.</value>
		public int StartingPieces {
			get;
			protected set;
		}
		#endregion
		#region init
		/// <summary>
		/// Initializes a new instance of the <see cref="TetrisTest.TetrisBoard"/> class.
		/// </summary>
		/// <param name="rand">The random to be used to generate pieces. If null, new one will be created</param>
		/// <remarks>Allowing the random to be passed in allows for unit testing or competative play where a reliable
		/// random sequence of tetrominos is valuable.</remarks>
		public TetrisBoard (int boardHeight, int boardWidth, Random rand = null) {
			HeightBoard = boardHeight+Tetromino.PIECE_SIZE;
			WidthBoard = boardWidth;
			if (rand == null){
				rand = new Random();
			}
			GameRandom = rand;
			//create PIECE_SIZE extra dummy rows at the top of the board to hold new incoming pieces
			GameBoard = new int[HeightBoard,WidthBoard];
			AddTetrominosToQueue(5);
			PopTetromino();
		}
		/// <summary>
		/// Creates and adds the tetrominos to the "NextPieces" queue.
		/// </summary>
		/// <param name="numPieces">(OPTIONAL) Number of pieces to generate. Defaults to single piece.</param>
		private void AddTetrominosToQueue(int numPieces = 1){
			Array tetrominoTypes = Enum.GetValues(typeof(TetrominoType));
			while (numPieces > 0){
				int pieceId = GameRandom.Next(0,tetrominoTypes.Length);
				NextPieces.Add(new Tetromino((TetrominoType)tetrominoTypes.GetValue(pieceId),StartingRow,StartingCol));
				numPieces--;
			}
		}

		public override string ToString (){
			string img = "";
			for (int rowIdx = 0; rowIdx < HeightBoard; rowIdx++){
				img += "[";
				for (int colIdx = 0; colIdx < WidthBoard; colIdx++){
					img += GameBoard[rowIdx,colIdx];
				}
				img += "] \n";
			}
			return string.Format ("[TetrisBoard: ActivePiece={0}, Score={1}]\n {2}", ActivePiece.PieceType, Score,img);
		}
		#endregion
		#region BoardControl
		/// <summary>
		/// Pops the tetromino into the ActivePiece position from the top of the queue.
		/// </summary>
		private void PopTetromino(){
			ActivePiece = NextPieces[0];
			NextPieces.RemoveAt(0);
		}
		/// <summary>
		/// Moves the active piece if it is legally valid to do so. Pieces can only be moved 
		/// in one unit of one direction at a time, as per the game rules.
		/// </summary>
		/// <param name="d">Direction in which to mobe the piece.</param>
		public void MoveActivePiece(Direction d){
			if (ValidNewPosition(d)){
				ActivePiece.ShiftPiece(d);
			}
			//check whether or not to freeze the tetromino in place
			if (FreezePiece()){
				Console.WriteLine(String.Format("Freezing piece {0}",ActivePiece.ToString()));
				if (CheckBoardPostMove()){//if the game continues
					PopTetromino();
					AddTetrominosToQueue();
				} else {
					throw new GameOverException(Score,ActivePiece);
				}
			}
		}
		#endregion
		#region active piece freezing
		/// <summary>
		/// Freezes the piece if it is in a position to be frozen.
		/// </summary>
		/// <returns><c>true</c>, if piece should freeze, <c>false</c> otherwise.</returns>
		private bool FreezePiece(){
			bool shouldFreeze = ShouldFreeze();
			if (shouldFreeze){
				int trueRowPos; 
				int trueColPos;
				int maxRow = ActivePiece.ImageMask.GetLength(0);
				int maxCol = ActivePiece.ImageMask.GetLength(1);
				for (int maskRow = 0; maskRow < maxRow; maskRow++){
					trueRowPos = ActivePiece.BoardPositionRow+maskRow;
					for (int maskCol = 0; maskCol < maxCol; maskCol++){
						trueColPos = ActivePiece.BoardPositionCol+maskCol;
						if (ActivePiece.ImageMask[maskRow,maskCol] != 0) {
							GameBoard[trueRowPos,trueColPos] = ActivePiece.ImageMask[maskRow,maskCol];
						}
					}
				}
			}
			return shouldFreeze;
		}
		/// <summary>
		/// Determines whether or not the piece should freeze by checking the row against the
		/// bottom of the game board as well as if any piece in the image mask has a filled
		/// board space directly below it.
		/// </summary>
		/// <returns><c>true</c>, if piece should be frozen, <c>false</c> otherwise.</returns>
		private bool ShouldFreeze(){
			int trueRowPos; 
			int trueColPos;
			int maxRow = ActivePiece.ImageMask.GetLength(0);
			int maxCol = ActivePiece.ImageMask.GetLength(1);
			for (int maskRow = maxRow-1; maskRow >= 0; maskRow--){
				trueRowPos = ActivePiece.BoardPositionRow+maskRow;
				for (int maskCol = 0; maskCol < maxCol; maskCol++){
					trueColPos = ActivePiece.BoardPositionCol+maskCol;
					if (ActivePiece.ImageMask[maskRow,maskCol] == 0){
						continue;//if there's nothing at this place in the image mask, ignore
					} 
					//if bottom row or there exists a piece directly below a piece in this one
					if ((trueRowPos >= HeightBoard-1) || (GameBoard[trueRowPos+1,trueColPos] != 0)){
						return true;
					} 
				}
			}
			return false;
		}
		/// <summary>
		/// Validates the new position against the rules of the game (no overlap, on game board).
		/// </summary>
		/// <returns><c>true</c>, if new position was validated, <c>false</c> otherwise.</returns>
		/// <param name="d">Direction the piece is to move</param>
		/// <param name="freezePiece">
		#endregion
		#region board helpers
		private bool ValidNewPosition(Direction d){
			int trueRow;
			int trueCol;
			int maxRow = ActivePiece.ImageMask.GetLength(0);
			int maxCol = ActivePiece.ImageMask.GetLength(1);
			int newMaskRow = ActivePiece.BoardPositionRow + (d==Direction.DOWN ? 1 : 0);
			int newMaskCol = ActivePiece.BoardPositionCol + (d==Direction.RIGHT ? 1 : (d==Direction.LEFT ? -1 : 0));
			for (int maskRow = 0; maskRow < maxRow; maskRow++){
				trueRow = newMaskRow+maskRow;
				for (int maskCol = 0; maskCol < maxCol; maskCol++){
					if (ActivePiece.ImageMask[maskRow,maskCol] == 0){
						continue;//if there's nothing at this place in the image mask, ignore
					}
					trueCol = newMaskCol+maskCol;
					//check that the piece itself is within the board, even if the mask is not
					if ((trueRow > (HeightBoard-1)) || (trueCol >= WidthBoard || trueCol < 0)){
						return false;
					}
					//check that the piece does not occupy the same space as the board
					if (GameBoard[trueRow,trueCol] != 0) {
						return false;
					}
				}
			}
			return true;
		}
		/// <summary>
		/// Checks the board after the move of the active piece. Returns False if the game is over.
		/// </summary>
		/// <returns><c>true</c>, if board post move was checked and board is in valid state, 
		/// <c>false</c> if the game needs to be ended as a result of the move.</returns>
		protected bool CheckBoardPostMove(){
			bool continueGame = true;
			int row = HeightBoard-1;//start at the bottom, work upwards
			int rowsRemoved = 0;
			//loop over rows checking for lines to delete/score
			while (row >= Tetromino.PIECE_SIZE){
				if (CheckRow(row)){
					ShiftBoardDown(row);
					rowsRemoved++;
				} else {
					row--;
				}
			}
			//check that top row is not full
			for (int col = 0; col < WidthBoard; col++){
				continueGame = continueGame && (GameBoard[Tetromino.PIECE_SIZE-1,col] == 0);
			}
			//update the player's score
			UpdateScore(rowsRemoved);
			return continueGame;
		}
		/// <summary>
		/// Checks the row to determine whether it needs to be collapsed and scored.
		/// </summary>
		/// <returns><c>true</c>, if row was checked, <c>false</c> otherwise.</returns>
		/// <param name="rowNumber">Row number.</param>
		protected bool CheckRow(int rowNumber){
			for (int col = 0; col < WidthBoard; col++){
				if (GameBoard[rowNumber,col] == 0){
					return false;
				}
			}
			return true;
		}
		/// <summary>
		/// Shifts the board down, overwriting whats in the row specified.
		/// </summary>
		/// <param name="startRow">The first row to be replaced with the row above it.</param>
		private void ShiftBoardDown(int startRow){
			for (int row = startRow; row >= 0; row--){
				for (int col = 0; col < WidthBoard; col++){
					if (row == 0){
						GameBoard[row,col] = 0;
					} else {
						GameBoard[row,col] = GameBoard[row-1,col];
					}
				}
			}
		}
		#endregion
		#region score
		/// <summary>
		/// Updates the score value based on the number of rows removed this turn.
		/// </summary>
		/// <param name="numRowsRemoved">Number rows removed.</param>
		protected virtual void UpdateScore(int numRowsRemoved){
			if (numRowsRemoved <= 0){
				return;
			}
			Score += (int)Math.Pow(2.0,numRowsRemoved);
		}
		#endregion
	}
}
