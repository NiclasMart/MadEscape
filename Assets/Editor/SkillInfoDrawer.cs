using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(CharacterSkill.SkillInfo))]
public class SkillInfoDrawer : PropertyDrawer
{
    const int DESCRIPTION_WIDTH = 150;
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();
        container.style.flexDirection = FlexDirection.Column;

        // Name
        var nameField = new PropertyField(property.FindPropertyRelative("Name"), "Skill Name");
        container.Add(nameField);

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

        // Update loop check
        var updateCheckerField = new PropertyField(property.FindPropertyRelative("OnlyActivatedOnceOnUnlock"), "ActivatedOnce");
        updateCheckerField.tooltip = "Should be checked, if the skill is only activated once one unlock like for stat increasements";
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
        container.Add(CreateParameterList(property, methodDropdown.value));

        return container;
    }

    private VisualElement CreateParameterList(SerializedProperty property, string className)
    {
        VisualElement paramContainer = new();

        // get selected skill class
        Type classType = typeof(CharacterSkillLibrary).GetNestedType(className);
        if (classType == null)
        {
            Debug.LogWarning($"Couldn't find Skill of type {classType.Name}");
            return paramContainer;
        }

        FieldInfo[] parameterList = classType.GetFields(); // gets public fields in class
        SerializedProperty parameterProperty = property.FindPropertyRelative("Parameters"); // gets parameters reference from scriptable object
        parameterProperty.arraySize = parameterList.Length;


        for (int i = 0; i < parameterList.Length; i++)
        {
            SerializedProperty paramProp = parameterProperty.GetArrayElementAtIndex(i); // gets the AnyValue with index i from the array
            SerializedProperty typeProp = paramProp.FindPropertyRelative("type");   // gets the type of that AnyValue
            typeProp.enumValueIndex = (int)AnyValue.ValueTypeOf(parameterList[i].FieldType); // sets the type to the corresponding class field type

            ValueType paramType = (ValueType)typeProp.enumValueIndex;
            VisualElement field;

            switch (paramType)
            {
                case ValueType.Int:
                    SerializedProperty intProp = paramProp.FindPropertyRelative("IntValue");
                    IntegerField intField = new($"{parameterList[i].Name} (Int)");
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
                    FloatField floatField = new($"{parameterList[i].Name} (Float)");
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
                    TextField stringField = new($"{parameterList[i].Name} (String)");
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
                    Toggle boolField = new($"{parameterList[i].Name} (Bool)");
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
                    Vector3Field vector3Field = new($"{parameterList[i].Name} (Vector3)");
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
                    field = new Label($"{parameterList[i].Name}: Unsupported Type");
                    break;
            }

            paramContainer.Add(field);
        }



        return paramContainer;

    }
}