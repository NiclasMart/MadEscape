using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(CharacterSkill.SkillInfo))]
public class SkillInfoDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        // Name
        var nameField = new PropertyField(property.FindPropertyRelative("Name"), "Skill Name");
        container.Add(nameField);

        // Update loop check
        var updateCheckerField = new PropertyField(property.FindPropertyRelative("OnlyActivatedOnceOnUnlock"), "Skill only acivates once when unlocked");
        container.Add(updateCheckerField);

        // Skill
        Type[] skillList = typeof(CharacterSkillLibrary).GetNestedTypes(BindingFlags.Public);
        List<string> dropdownOptions = new();
        foreach (var skill in skillList)
        {
            dropdownOptions.Add(skill.Name);
        }

        var methodDropdown = new DropdownField("Skill", dropdownOptions, 0);
        SerializedProperty skillProperty = property.FindPropertyRelative("SkillRef");
        methodDropdown.value = skillProperty.stringValue;
        methodDropdown.RegisterValueChangedCallback(evt =>
        {
            skillProperty.stringValue = methodDropdown.value;
            container.Children().Last().Clear();
            container.Add(CreateParameterList(property, methodDropdown.value));

            property.serializedObject.ApplyModifiedProperties();

        });
        container.Add(methodDropdown);

        // parameters




        // Description
        var descriptionField = new TextField("Description", 100, true, false, ' ');
        descriptionField.verticalScrollerVisibility = ScrollerVisibility.AlwaysVisible;
        SerializedProperty descriptionProperty = property.FindPropertyRelative("Description");
        descriptionField.value = descriptionProperty.stringValue;
        descriptionField.RegisterCallback<ChangeEvent<string>>((evt) =>
        {
            descriptionProperty.stringValue = evt.newValue;
            property.serializedObject.ApplyModifiedProperties();
        });
        container.Add(descriptionField);

        return container;
    }

    private VisualElement CreateParameterList(SerializedProperty property, string className)
    {
        VisualElement paramContainer = new();

        Type classType = typeof(CharacterSkillLibrary).GetNestedType(className);
        if (classType == null)
        {
            Debug.LogWarning($"Couldn't find Skill of type {classType.Name}");
            return paramContainer;
        }

        FieldInfo[] parameterList = classType.GetFields();

        SerializedProperty parameterProperty = property.FindPropertyRelative("Parameters");
        parameterProperty.arraySize = parameterList.Length;
        for (int i = 0; i < parameterList.Length; i++)
        {
            Debug.Log(parameterList[i].FieldType);
            SerializedProperty paramProp = parameterProperty.GetArrayElementAtIndex(i); // gets the AnyValue in the array
            SerializedProperty typeProp = paramProp.FindPropertyRelative("type");   // gets the type of AnyValue
            typeProp.enumValueIndex = (int) AnyValue.ValueTypeOf(parameterList[i].FieldType);

            ValueType paramType = (ValueType) typeProp.enumValueIndex;
            VisualElement field;

            switch (paramType)
            {
                case ValueType.Int:
                    SerializedProperty intProp = paramProp.FindPropertyRelative("IntValue");
                    IntegerField intField = new($"Parameter {i + 1} (Int)");
                    intField.value = intProp.intValue;
                    intField.RegisterValueChangedCallback(
                        evt =>
                        {
                            intProp.intValue = evt.newValue;
                            parameterProperty.serializedObject.ApplyModifiedProperties();
                        }
                    );
                    field = intField;
                    break;

                case ValueType.Float:
                    SerializedProperty floatProp = paramProp.FindPropertyRelative("FloatValue");
                    FloatField floatField = new($"Parameter {i + 1} (Float)");
                    floatField.value = floatProp.floatValue;
                    floatField.RegisterValueChangedCallback(
                        evt =>
                        {
                            floatProp.floatValue = evt.newValue;
                            parameterProperty.serializedObject.ApplyModifiedProperties();
                        }
                    );
                    field = floatField;
                    break;

                case ValueType.String:
                    SerializedProperty stringProp = paramProp.FindPropertyRelative("StringValue");
                    TextField stringField = new($"Parameter {i + 1} (String)");
                    stringField.value = stringProp.stringValue;
                    stringField.RegisterValueChangedCallback(
                        evt =>
                        {
                            stringProp.stringValue = evt.newValue;
                            parameterProperty.serializedObject.ApplyModifiedProperties();
                        }
                    );
                    field = stringField;
                    break;

                case ValueType.Bool:
                    SerializedProperty boolProp = paramProp.FindPropertyRelative("BoolValue");
                    Toggle boolField = new($"Parameter {i + 1} (Bool)");
                    boolField.value = boolProp.boolValue;
                    boolField.RegisterValueChangedCallback(
                        evt =>
                        {
                            boolProp.boolValue = evt.newValue;
                            parameterProperty.serializedObject.ApplyModifiedProperties();
                        }
                    );
                    field = boolField;
                    break;

                case ValueType.Vector3:
                    SerializedProperty vector3Prop = paramProp.FindPropertyRelative("Vector3Value");
                    Vector3Field vector3Field = new($"Parameter {i + 1} (Vector3)");
                    vector3Field.value = vector3Prop.vector3Value;
                    vector3Field.RegisterValueChangedCallback(
                        evt =>
                        {
                            vector3Prop.vector3Value = evt.newValue;
                            parameterProperty.serializedObject.ApplyModifiedProperties();
                        }
                    );
                    field = vector3Field;
                    break;

                default:
                    field = new Label($"Parameter {i + 1}: Unsupported Type");
                    break;
            }

            paramContainer.Add(field);
        }



        return paramContainer;

    }
}