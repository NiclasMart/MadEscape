using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

// [CustomPropertyDrawer(typeof(MatrixSocketInfo))]
// public class MatrixSocketDrawerUI : PropertyDrawer
// {
//     public override VisualElement CreatePropertyGUI(SerializedProperty property)
//     {
//         VisualElement container = new();

//         var objectField = new ObjectField(property.displayName)
//         {
//             objectType = typeof(MatrixSocketInfo)
//         };

//         var valueLabel = new Label();


//         container.Add(objectField);
//         container.Add(valueLabel);

//         objectField.RegisterValueChangedCallback(
//             evt => {
//                 var variable = evt.newValue as MatrixSocketInfo;
//                 if (variable != null)
//                 {
//                     valueLabel.text = "Hier ist ein Text.";
//                 }
//             }
//         );

//         return container;
//     }

//     public void BuildUI(VisualElement container, SerializedProperty property)
//     {

//     }
// }