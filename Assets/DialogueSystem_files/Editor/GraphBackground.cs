using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

// Implementation from UnityEditor.Graphs.GraphGUI
public static class GraphBackground
{
	private static readonly Color kGridMinorColorDark = new Color(0f, 0f, 0f, 0.18f);
	private static readonly Color kGridMajorColorDark = new Color(0f, 0f, 0f, 0.28f);
	private static readonly Color kGridMinorColorLight = new Color(0f, 0f, 0f, 0.1f);
	private static readonly Color kGridMajorColorLight = new Color(0f, 0f, 0f, 0.15f);

	private static Color gridMinorColor
	{
		get
		{
			if (EditorGUIUtility.isProSkin)
				return kGridMinorColorDark;
			else
				return kGridMinorColorLight;
		}
	}

	private static Color gridMajorColor
	{
		get
		{
			if (EditorGUIUtility.isProSkin)
				return kGridMajorColorDark;
			else
				return kGridMajorColorLight;
		}
	}

	public static void DrawGraphBackground(Rect position, Rect graphExtents, float zoom, Vector2 offset)
	{
		if (Event.current.type == EventType.Repaint)
		{
			UnityEditor.Graphs.Styles.graphBackground.Draw(position, false, false, false, false);
		}
		DrawGrid(graphExtents, zoom, offset);
	}
	
	private static void DrawGrid(Rect graphExtents, float zoom, Vector2 offset)
	{
		if (Event.current.type == EventType.Repaint)
		{

			float minGrid = 15 * (float)zoom;
			float bigGrid = minGrid * 10;

            HandleUtility.ApplyWireMaterial();
			GL.PushMatrix();
			GL.Begin(1);

            // Calculate the blend factor based on the zoom value
            float blendFactor = Mathf.Clamp01((zoom - 0.3f) / 1); // Adjust 0.3f and 0.7f as needed for the desired range
            
			Color transparentGridMinorColor = gridMinorColor;
            transparentGridMinorColor.a *= blendFactor;

            DrawGridLines(graphExtents, minGrid, transparentGridMinorColor, offset);
			

            // Blend the grid major and minor colors
            Color blendedGridColor = Color.Lerp(gridMajorColor, gridMinorColor, blendFactor);

            DrawGridLines(graphExtents, bigGrid, blendedGridColor, offset);
			GL.End();
			GL.PopMatrix();
		}
	}
	
	private static void DrawGridLines(Rect graphExtents, float gridSize, Color gridColor, Vector2 offset)
	{
		float offX = offset.x % gridSize;
        float offY = offset.y % gridSize;
        Vector2 rOffset = new Vector2(-gridSize + offX, -gridSize + offY);

		GL.Color(gridColor);		
        for (float x = (graphExtents.xMin - graphExtents.xMin % gridSize) + rOffset.x; x < graphExtents.xMax; x += gridSize)
		{
			DrawLine(new Vector2(x, graphExtents.yMin), new Vector2(x, graphExtents.yMax));
		}

		GL.Color(gridColor);
		for (float y = (graphExtents.yMin - graphExtents.yMin % gridSize) + rOffset.y; y < graphExtents.yMax; y += gridSize)
		{
			DrawLine(new Vector2(graphExtents.xMin, y), new Vector2(graphExtents.xMax, y));
		}
	}

	private static void DrawLine(Vector2 p1, Vector2 p2)
	{
		GL.Vertex(p1);
		GL.Vertex(p2);
	}

	// Implementation from UnityEditor.HandleUtility
	static class HandleUtility
	{
		static Material s_HandleWireMaterial;
		static Material s_HandleWireMaterial2D;

		internal static void ApplyWireMaterial(CompareFunction zTest = CompareFunction.Always)
		{
			Material handleWireMaterial = HandleUtility.handleWireMaterial;
			handleWireMaterial.SetInt("_HandleZTest", (int)zTest);
			handleWireMaterial.SetPass(0);
		}
		
		private static Material handleWireMaterial
		{
			get
			{
				InitHandleMaterials();
				return (!Camera.current) ? s_HandleWireMaterial2D : s_HandleWireMaterial;
			}
		}

		private static void InitHandleMaterials()
		{
			if (!s_HandleWireMaterial)
			{
				s_HandleWireMaterial = (Material)EditorGUIUtility.LoadRequired("SceneView/HandleLines.mat");
				s_HandleWireMaterial2D = (Material)EditorGUIUtility.LoadRequired("SceneView/2DHandleLines.mat");
			}
		}
	}
}
