using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(MatrixInt))]
public class MatrixIntPropertyDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        SerializedProperty matrix = property.FindPropertyRelative("m");
        SerializedProperty matrixSize = property.FindPropertyRelative("size");

        Foldout foldout = new Foldout();
        foldout.text = property.displayName;

        VisualElement matrixContainer = new VisualElement();
        matrixContainer.Add(BuildMatrix(matrix, matrixSize.intValue, true));
        foldout.Add(matrixContainer);

        SliderInt sizeSlider = new SliderInt("Size", 2, 9, SliderDirection.Horizontal, 1);
        sizeSlider.value = matrixSize.intValue;
        sizeSlider.showInputField = true;

        sizeSlider.RegisterCallback<ChangeEvent<int>>((ev) => {
            SetDimensions(property, ev.newValue);
            matrixContainer.Clear();
            matrixContainer.Add(BuildMatrix(matrix, matrixSize.intValue, false));
        });

        foldout.Add(sizeSlider);

        return foldout;
    }

    protected virtual VisualElement MakeLabel(int index)
    {
        Label rowNumber = new Label($"{index}");

        return rowNumber;
    }

    private void SetDimensions(SerializedProperty property, int dim)
    {
        int size = dim * dim;

        SerializedProperty matrix = property.FindPropertyRelative("m");
        SerializedProperty matrixSize = property.FindPropertyRelative("size");

        matrix.arraySize = size;
        matrixSize.intValue = dim;

        property.serializedObject.ApplyModifiedProperties();
    }

    private VisualElement BuildMatrix(SerializedProperty matrix, int dim, bool first)
    {
        VisualElement root = new VisualElement();

        // Create header
        VisualElement header = new VisualElement();
        header.style.flexDirection = FlexDirection.Row;
        header.style.justifyContent = Justify.FlexEnd;

        VisualElement empty = new VisualElement();
        empty.style.width = 20;
        header.Add(empty);

        // Add labels with column number
        for (int i = 0; i < dim; i++)
        {
            VisualElement colLabel = MakeLabel(i);
            colLabel.style.width = 50;
            header.Add(colLabel);
        }

        root.Add(header);

        for (int i = 0; i < dim; i++)
        {
            // Create row
            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.justifyContent = Justify.FlexEnd;

            // Add label with row number
            VisualElement rowLabel = MakeLabel(i);
            rowLabel.style.width = 20;
            row.Add(rowLabel);

            // Create columns
            for (int j = 0; j < dim; j++)
            {
                int index = i * dim + j;

                SerializedProperty cell = matrix.GetArrayElementAtIndex(index);

                PropertyField cellElement = new PropertyField(cell);
                cellElement.label = "";
                cellElement.style.width = 50;

                // This function will not explicitly bind the property fields
                // the first time it is called because `CreatePropertyGUI()` will
                // call `Bind()` for us on the entire UI tree
                if (!first)
                {
                    cellElement.BindProperty(cell);
                }

                row.Add(cellElement);
            }

            root.Add(row);
        }

        return root;
    }
}
