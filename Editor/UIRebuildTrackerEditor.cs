#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIRebuildTracker))]
public class UIRebuildTrackerEditor : Editor
{
	private static readonly Vector3[] WorldCorners = new Vector3[4];
	private static GUIStyle _badgeStyle;

	[InitializeOnLoadMethod]
	private static void InitEditorEvents()
	{
		SceneView.duringSceneGui += OnSceneGUIGlobal;

		// Unity 6.6 에서 instanceID(int) 계열이 EntityId 계열로 바뀌었다.
		// 옛 hierarchyWindowItemOnGUI 는 obsolete 를 넘어 컴파일 에러다.
		EditorApplication.hierarchyWindowItemByEntityIdOnGUI += OnHierarchyGUI;

		// 매 프레임 하이어라키 창을 다시 그려 잔상이 남지 않게 한다.
		EditorApplication.update += OnEditorUpdate;
	}

	private static void OnEditorUpdate()
	{
		if (UIRebuildTracker.Instance != null && UIRebuildTracker.Instance.ActiveData.Count > 0)
		{
			EditorApplication.RepaintHierarchyWindow();
		}
	}

	private static void OnSceneGUIGlobal(SceneView sceneView)
	{
		var tracker = UIRebuildTracker.Instance;
		if (tracker == null || tracker.ActiveData.Count == 0) return;

		foreach (var (rect, info) in tracker.ActiveData)
		{
			if (rect == null || info.Intensity <= 0.01f) continue;

			rect.GetWorldCorners(WorldCorners);

			Color outlineColor = info.Type == UIRebuildTracker.RebuildType.Layout ? Color.red : Color.yellow;
			outlineColor.a = info.Intensity;

			Handles.DrawSolidRectangleWithOutline(WorldCorners, new Color(0, 0, 0, 0), outlineColor);
		}

		sceneView.Repaint();
	}

	private static void OnHierarchyGUI(EntityId entityId, Rect selectionRect)
	{
		var tracker = UIRebuildTracker.Instance;
		if (tracker == null || tracker.ActiveData.Count == 0) return;

		var go = EditorUtility.EntityIdToObject(entityId) as GameObject;
		if (go == null) return;

		if (go.TryGetComponent<RectTransform>(out var rect) && tracker.ActiveData.TryGetValue(rect, out var info))
		{
			// 강도가 0 이면 그리지 않고 빠진다 — 잔상 방지.
			if (info.Intensity <= 0.01f) return;

			if (_badgeStyle == null)
			{
				_badgeStyle = new GUIStyle(EditorStyles.miniButton)
				{
					fontSize = 10,
					fontStyle = FontStyle.Bold,
					alignment = TextAnchor.MiddleCenter
				};
			}

			Rect badgeRect = new Rect(selectionRect.xMax - 75, selectionRect.y, 70, selectionRect.height);

			Color badgeColor = info.Type == UIRebuildTracker.RebuildType.Layout
				? new Color(0.9f, 0.1f, 0.2f, info.Intensity)
				: new Color(0.9f, 0.75f, 0.0f, info.Intensity);

			Color prevColor = GUI.color;
			GUI.color = badgeColor;
			GUI.Box(badgeRect, info.Type == UIRebuildTracker.RebuildType.Layout ? "LAYOUT" : "GRAPHIC", _badgeStyle);
			GUI.color = prevColor;
		}
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		var tracker = (UIRebuildTracker)target;
		EditorGUILayout.Space(10);
		EditorGUILayout.LabelField("Tracking UI Count", tracker.ActiveData.Count.ToString(), EditorStyles.boldLabel);
	}
}
#endif