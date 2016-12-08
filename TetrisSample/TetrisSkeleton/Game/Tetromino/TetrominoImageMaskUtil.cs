using System;
using System.Collections.Generic;

namespace TetrisSkeleton {
	public static class TetrominoImageMaskUtil {

		private static Dictionary<TetrominoType, int[,]> ImageMaskInit = new Dictionary<TetrominoType, int[,]>(){
			{
				TetrominoType.SQUARE, new int[4,4]{
					{0,0,0,0},
					{0,0,0,0},
					{0,1,1,0},
					{0,1,1,0}
				}
			},
			{
				TetrominoType.LINE, new int[4,4]{
					{0,2,0,0},
					{0,2,0,0},
					{0,2,0,0},
					{0,2,0,0}
				}
			},
			{
				TetrominoType.T, new int[4,4]{
					{0,0,0,0},
					{0,0,0,0},
					{3,3,3,0},
					{0,3,0,0}
				}
			},
			{
				TetrominoType.L, new int[4,4]{
					{0,0,0,0},
					{0,4,0,0},
					{0,4,0,0},
					{0,4,4,0}
				}
			},
			{
				TetrominoType.L_INVERTED, new int[4,4]{
					{0,0,0,0},
					{0,0,5,0},
					{0,0,5,0},
					{0,5,5,0}
				}
			},
			{
				TetrominoType.S, new int[4,4]{
					{0,0,0,0},
					{0,0,0,0},
					{0,6,6,0},
					{6,6,0,0}
				}
			},
			{
				TetrominoType.Z, new int[4,4]{
					{0,0,0,0},
					{0,0,0,0},
					{7,7,0,0},
					{0,7,7,0}
				}
			}
		};

		public static int[,] GetImageMask(TetrominoType typeId){
			if (!ImageMaskInit.ContainsKey(typeId)){
				throw new ArgumentException("TetrominoType not found: "+typeId);
			}
			return ImageMaskInit[typeId];
		}
	}
}

