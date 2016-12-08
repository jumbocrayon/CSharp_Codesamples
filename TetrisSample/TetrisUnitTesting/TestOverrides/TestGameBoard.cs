using System;
using TetrisSkeleton;
using NUnit.Framework;

namespace TetrisTests {
	public class TestGameBoard : TetrisBoard {
		public TestGameBoard (int boardHeight, int boardWidth, Random rand = null) : base(boardHeight,boardWidth,rand) {}
		/// <summary>
		/// Allows the unit tests to set the gameboard to something new for testing purposes.
		/// </summary>
		/// <param name="newBoard">New board, must be of same dimensions as existing board.</param>
		public void TestSetGameBoard(int[,] newBoard){
			Assert.IsTrue(newBoard.GetLength(0) == GameBoard.GetLength(0));
			Assert.IsTrue(newBoard.GetLength(1) == GameBoard.GetLength(1));
			GameBoard = newBoard;
		}
		/// <summary>
		/// Updates the score value based on the number of rows removed this turn.
		/// </summary>
		/// <param name="numRowsRemoved">Number rows removed.</param>
		protected override void UpdateScore (int numRowsRemoved){
			Console.Out.WriteLine(String.Format("Calling updatescore with {0} rows removed.",numRowsRemoved));
			base.UpdateScore (numRowsRemoved);
		}
	}
}

