using System;

namespace TetrisSkeleton {
	public class Tetromino {
		#region constants
		/// <summary>
		/// The number of tetromino units found in a piece, therefore the max possible
		/// height or length of a single piece.
		/// </summary>
		public const int PIECE_SIZE = 4;
		#endregion
		#region properties
		/// <summary>
		/// Gets the type of the piece.
		/// </summary>
		/// <value>The type of the piece.</value>
		public TetrominoType PieceType {
			get;
			private set;
		}
		/// <summary>
		/// Gets the image mask (2-d array, 4x4)
		/// </summary>
		/// <value>The image mask.</value>
		public int[,] ImageMask {
			get;
			private set;
		}
		/// <summary>
		/// The board position row.
		/// </summary>
		public int BoardPositionRow {
			get;
			private set;
		}
		/// <summary>
		/// The board position row.
		/// </summary>
		public int BoardPositionCol {
			get;
			private set;
		}
		#endregion
		#region tetromino descriptors
		/// <summary>
		/// Initializes a new instance of the <see cref="TetrisTest.Tetromino"/> class.
		/// </summary>
		/// <param name="typeId">Type identifier.</param>
		public Tetromino (TetrominoType typeId, int posRow, int posCol) {
			BoardPositionRow = posRow;
			BoardPositionCol = posCol;
			PieceType = typeId;
			ImageMask = TetrominoImageMaskUtil.GetImageMask(typeId);
		}
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString (){
			string img = "";
			for (int rowIdx = 0; rowIdx < PIECE_SIZE; rowIdx++){
				img += "[";
				for (int colIdx = 0; colIdx < PIECE_SIZE; colIdx++){
					img += ImageMask[rowIdx,colIdx];
				}
				img += "] \n";
			}
			return string.Format ("[Tetromino: PieceType={0}] ImageMask: \n{1}", PieceType, img);
		}
		#endregion
		#region control
		/// <summary>
		/// Rotates the pieces image mask internally clockwise.
		/// </summary>
		public void RotatePiece(){
			int[,] newMask = new int[PIECE_SIZE,PIECE_SIZE];
			int verticalOffset = PIECE_SIZE-1;
			int horizontalOffset;
			for (int rowIdx = 0; rowIdx < PIECE_SIZE; rowIdx++){//0 -> n
				horizontalOffset = 0;
				for (int colIdx = 0; colIdx < PIECE_SIZE; colIdx++){//0 -> n
					newMask[horizontalOffset,verticalOffset] = ImageMask[rowIdx,colIdx];
					horizontalOffset++;
				}
				verticalOffset--;
			}
			ImageMask = newMask;
		}
		/// <summary>
		/// Shifts the piece by one unit in the specified direction.
		/// </summary>
		/// <param name="d">Direction in which to shift the position of the piece</param>
		public void ShiftPiece(Direction d){
			switch (d){
			case Direction.DOWN:
				BoardPositionRow += 1;
				break;
			case Direction.LEFT: 
				BoardPositionCol -= 1;
				break;
			case Direction.RIGHT:
				BoardPositionCol += 1;
				break;
			}
		}
		#endregion
	}
}

