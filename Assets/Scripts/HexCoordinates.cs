using UnityEngine;

/// <summary>
/// A struct to represent coordinates in a Hex system
/// </summary>
[System.Serializable]
public struct HexCoordinates {

	public int X { get; private set; }

	public int Z { get; private set; }

	public int Y {
		get {
			return -X - Z;
		}
	}

	public HexCoordinates (int x, int z) {
		X = x;
		Z = z;
	}

	// static method overloaded constructor to create a set of coordinates using regular offset coordinates
	public static HexCoordinates FromOffsetCoordinates (int x, int z) {
		return new HexCoordinates(x - z / 2, z);
	}

	//static method overloaded constructor to construct a hex coordinate from the position given
	public static HexCoordinates FromPosition (Vector3 position) {
		float x = position.x / (HexMetrics.innerRadius * 2f);
		float y = -x;

		// every two rows, shift one unit to the left
		float offset = position.z / (HexMetrics.outerRadius * 3f);
		x -= offset;
		y -= offset;

		// round the itnergers to get the coordinates
		int iX = Mathf.RoundToInt(x);
		int iY = Mathf.RoundToInt(y);
		int iZ = Mathf.RoundToInt(-x -y);

		//  when coordinates don't add to zero
		if (iX + iY + iZ != 0) {
			//Debug.LogWarning("rounding error!");

			// happens at edges of between hexes, so assume the coordinate that got rounded the most is incorrect
			float dX = Mathf.Abs(x - iX);
			float dY = Mathf.Abs(y - iY);
			float dZ = Mathf.Abs(-x -y - iZ);

			if (dX > dY && dX > dZ) {
				iX = -iY - iZ;
			}
			else if (dZ > dY) {
				iZ = -iX - iY;
			}
		}

		return new HexCoordinates(iX, iZ);
	}

	// override toStrings so it doesn't just reutrn the struct's type name
	public override string ToString () {
		return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
	}

	public string ToStringOnSeparateLines () {
		return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
	}
}