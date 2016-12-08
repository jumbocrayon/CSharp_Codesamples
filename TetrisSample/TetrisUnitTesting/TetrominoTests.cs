using NUnit.Framework;
using System;
using TetrisSkeleton;

namespace TetrisTests {
	[TestFixture ()]
	public class TetrominoTests {
		[Test ()]
		public void TestRotation () {
			int[,] zMask = new int[4,4]{
				{0,0,0,0},
				{0,0,0,0},
				{7,7,0,0},
				{0,7,7,0}
			};
			Tetromino z = new Tetromino(TetrominoType.Z,0,0);
			Assert.AreEqual(zMask,z.ImageMask);
			z.RotatePiece();
			z.RotatePiece();
			z.RotatePiece();
			z.RotatePiece();
			Assert.AreEqual(z.ImageMask,zMask);
		}

		[Test ()]
		public void TestClockwiseRotation () {
			int[,] zMask = new int[4,4]{
				{0,7,0,0},
				{7,7,0,0},
				{7,0,0,0},
				{0,0,0,0}
			};
			Tetromino z = new Tetromino(TetrominoType.Z,0,0);
			z.RotatePiece();
			Assert.AreEqual(z.ImageMask,zMask);
		}
	}
}

