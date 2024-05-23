using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddToMenu
{


    [MenuItem("GameObject/UI/DialogueSystem/Dialogue Display", false, 2070)]
    static public void AddDisplay()
    {
        GameObject go;
        //Ir aos Resources buscar o DialogueSystem
        go = Resources.Load("Prefabs/DDisplay") as GameObject;
        PlaceUIElementRoot(go);
    }

    private static void PlaceUIElementRoot(GameObject element)
    {
        GameObject parent = GameObject.Find("Canvas");

        //Check if there's a Canvas
        if (parent == null)
        {
            var root = ObjectFactory.CreateGameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            parent = canvas.gameObject;
        }

        //Create GameObject
        GameObject display = GameObject.Instantiate(element, new Vector2(0, 0), Quaternion.identity, parent.transform);
        display.name = "DDisplay";

        //Center Dialogue 
        SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), display.GetComponent<RectTransform>());

        CreateEventSystem();
        
        Selection.activeGameObject = element;
    }



    private static void CreateEventSystem()
    {
        StageHandle stage = StageUtility.GetCurrentStageHandle();
        var esys = stage.FindComponentOfType<EventSystem>();
        if (esys == null)
        {
            var eventSystem = ObjectFactory.CreateGameObject("EventSystem");         
            StageUtility.PlaceGameObjectInCurrentStage(eventSystem);
            esys = ObjectFactory.AddComponent<EventSystem>(eventSystem);
            ObjectFactory.AddComponent<StandaloneInputModule>(eventSystem);

            Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
        }

    }

    private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
    {
        SceneView sceneView = SceneView.lastActiveSceneView;

        // Couldn't find a SceneView. Don't set position.
        if (sceneView == null || sceneView.camera == null)
            return;

        // Create world space Plane from canvas position.
        Vector2 localPlanePosition;
        Camera camera = sceneView.camera;
        Vector3 position = Vector3.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
        {
            // Adjust for canvas pivot
            localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
            localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

            localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
            localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

            // Adjust for anchoring
            position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
            position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

            Vector3 minLocalPosition;
            minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
            minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

            Vector3 maxLocalPosition;
            maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
            maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

            position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
            position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
        }

        itemTransform.anchoredPosition = position;
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.localScale = Vector3.one;
    }

}
