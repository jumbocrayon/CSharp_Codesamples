using System;
using TetrisSkeleton;
using NUnit.Framework;

namespace TetrisTests {
	[TestFixture ()]
	public class BoardTests {

		[Test ()]
		public void TestBoardCreation () {
			TetrisBoard board = new TestGameBoard(4,4,new Random(100));
			Console.Out.WriteLine(board.ToString());
			Assert.IsTrue(board.Score == 0);
			Assert.IsTrue(board.ActivePiece.PieceType == TetrominoType.S);
		}

		[Test()]
		public void TestBoardScore(){
			TestGameBoard board = new TestGameBoard(4,4,new Random(100));
			Assert.IsTrue(board.Score == 0);
			board.TestSetGameBoard(new int[8,4]{
				{0,0,0,0},
				{0,0,0,0},
				{0,0,0,0},
				{0,0,0,0},
				{0,0,0,0},
				{0,0,8,8},
				{8,8,8,8},
				{0,8,8,8}
			});
			try {
				int activePieceRow = -100;
				while (board.ActivePiece.BoardPositionRow != activePieceRow){
					activePieceRow = board.ActivePiece.BoardPositionRow;
					board.MoveActivePiece(Direction.DOWN);
				}
			} catch (GameOverException gEx){
				Console.Out.WriteLine("Game over exception: "+gEx.Message);
				Console.Out.WriteLine(board.ToString());
			}
			Assert.IsTrue(board.Score == 4);
		}
	}
}


