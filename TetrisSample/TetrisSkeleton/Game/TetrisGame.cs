using System;

namespace TetrisSkeleton
{
	class MainClass {
		/// <summary>
		/// This is the class I was using to test each piece of board and tetromino functionality
		/// during development as I go. There is unit test coverage for some tetromino things and 
		/// ideally there would be more board coverage.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main (string[] args) {
			Console.WriteLine("----------- Creating board ---------------");
			TetrisBoard b = new TetrisBoard(20,10,new Random(42));
			Console.WriteLine(b.ToString());
			try {
				int activePieceRow = -100;
				while (b.ActivePiece.BoardPositionRow != activePieceRow){
					activePieceRow = b.ActivePiece.BoardPositionRow;
					b.MoveActivePiece(Direction.DOWN);
					b.ActivePiece.RotatePiece();
				}
			} catch (GameOverException gEx){
				Console.WriteLine("GAME OVER!");
				Console.WriteLine(gEx.Message);
				Console.WriteLine(b.ToString());
			}
		}
	}
}

