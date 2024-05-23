using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using NUnit.Framework.Constraints;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Class responsible for managing the graphview component of
    /// the Node based Dialogue System
    /// </summary>
    public class DialogueControllerGraphView : GraphView
    {

        /// <summary>
        /// Constructor of this class
        /// </summary>
        public DialogueControllerGraphView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale,
               ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            Color backgroundColor = new Color(0.15f, 0.15f, 0.15f); // Dark gray background color
            Color backgroundLines = new Color(0.1f, 0.1f, 0.1f); // Dark gray background color

            // Create the grid texture
            style.backgroundImage = new StyleBackground(CreateGridTexture(100, 100, 1f, backgroundLines, backgroundColor));

            style.unityBackgroundScaleMode = ScaleMode.ScaleAndCrop;

        }

        /// <summary>
        /// Override that adds more menu options to the contextual menu 
        /// of the items
        /// </summary>
        /// <param name="evt"></param>
        public override void BuildContextualMenu
            (ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

        }

        /// <summary>
        /// Create a grid pattern texture with the given dimensions and colors.
        /// The line thickness is controlled by the lineSize parameter.
        /// </summary>
        /// <param name="width">Width of the grid.</param>
        /// <param name="height">Height of the grid.</param>
        /// <param name="lineSize">Line thickness (in pixels) between grid lines.</param>
        /// <param name="lineColor">Color of the grid lines.</param>
        /// <param name="bgColor">Background color of the grid.</param>
        /// <returns>Texture2D representing the grid pattern.</returns>
        private Texture2D CreateGridTexture(int width, int height, float lineSize, Color lineColor, Color bgColor)
        {
         
            int cellWidth = Mathf.RoundToInt((float)1920 / width);
            int cellHeight = Mathf.RoundToInt((float)1920 / height);

            Texture2D gridTexture = new Texture2D((width - 1) * cellWidth, (height - 1) * cellHeight);
            Color[] pixels = new Color[gridTexture.width * gridTexture.height];

            for (int y = 0; y < gridTexture.height; y++)
            {
                for (int x = 0; x < gridTexture.width; x++)
                {
                    bool isLineX = (x + 1) % cellWidth == 0;
                    bool isLineY = (y + 1) % cellHeight == 0;

                    if (lineSize > 0f)
                    {
                        // If lineSize is greater than 0, draw the grid lines with specified lineSize
                        if (isLineX || isLineY)
                            pixels[y * gridTexture.width + x] = lineColor;
                        else
                            pixels[y * gridTexture.width + x] = bgColor;
                    }
                    else
                    {
                        // If lineSize is 0, fill the entire cell with lineColor
                        pixels[y * gridTexture.width + x] = lineColor;
                    }
                }
            }

            gridTexture.SetPixels(pixels);
            gridTexture.Apply();

            return gridTexture;
        }

    }
   
 
}

