#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5 || UNITY_5_3_OR_NEWER
#define UNITY
#endif

using System;
using System.Globalization;
#if UNITY
using UnityEngine;
#endif

namespace LibBSP {
#if UNITY
	using Vector4d = Vector4;
	using Vector3d = Vector3;
#endif

	/// <summary>
	/// Class containing all data necessary to render a Terrain from Star Trek EF2.
	/// </summary>
	[Serializable] public class MAPTerrainEF2 {

		private static IFormatProvider _format = CultureInfo.CreateSpecificCulture("en-US");

		public int side;
		public string texture;
		public double textureShiftS;
		public double textureShiftT;
		public float texRot;
		public double texScaleX;
		public double texScaleY;
		public int flags;
		public double sideLength;
		public Vector3d start;
		public Vector4d IF;
		public Vector4d LF;
		public float[,] heightMap;
		public float[,] alphaMap;

		/// <summary>
		/// Creates a new empty <see cref="MAPTerrainEF2"/> object. Internal data will have to be set manually.
		/// </summary>
		public MAPTerrainEF2() { }

		/// <summary>
		/// Constructs a new <see cref="MAPTerrainEF2"/> object using the supplied string array as data.
		/// </summary>
		/// <param name="lines">Data to parse.</param>
		public MAPTerrainEF2(string[] lines) {

			texture = lines[2];

			switch (lines[0]) {
				case "terrainDef": {
					for (int i = 2; i < lines.Length; ++i) {
						string[] line = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						switch (line[0]) {
							case "TEX(": {
								texture = line[1];
								textureShiftS = float.Parse(line[2], _format);
								textureShiftT = float.Parse(line[3], _format);
								texRot = float.Parse(line[4], _format);
								texScaleX = double.Parse(line[5], _format);
								texScaleY = double.Parse(line[6], _format);
								flags = int.Parse(line[8]);
								break;
							}
							case "TD(": {
								sideLength = int.Parse(line[1], _format);
								start = new Vector3d(float.Parse(line[2], _format), float.Parse(line[3], _format), float.Parse(line[4], _format));
								break;
							}
							case "IF(": {
								IF = new Vector4d(float.Parse(line[1], _format), float.Parse(line[2], _format), float.Parse(line[3], _format), float.Parse(line[4], _format));
								break;
							}
							case "LF(": {
								LF = new Vector4d(float.Parse(line[1], _format), float.Parse(line[2], _format), float.Parse(line[3], _format), float.Parse(line[4], _format));
								break;
							}
							case "V(": {
								++i;
								line = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								if (side == 0) {
									side = line.Length;
								}
								heightMap = new float[side, side];
								for (int j = 0; j < side; ++j) {
									for (int k = 0; k < side; ++k) {
										heightMap[j, k] = float.Parse(line[k], _format);
									}
									++i;
									line = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								}
								break;
							}
							case "A(": {
								++i;
								line = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								if (side == 0) {
									side = line.Length;
								}
								alphaMap = new float[side, side];
								for (int j = 0; j < side; ++j) {
									for (int k = 0; k < side; ++k) {
										alphaMap[j, k] = float.Parse(line[k], _format);
									}
									++i;
									line = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								}
								break;
							}
						}
					}
					break;
				}
				default: {
					throw new ArgumentException(string.Format("Unknown terrain type {0}!", lines[0]));
				}
			}

		}

	}
}
