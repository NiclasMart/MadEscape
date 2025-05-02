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
            property.FindPropertyRelative("isDirty").boolValue = true; // set data to dirty state
            Debug.Log("Set Dirty");

            skillProperty.stringValue = methodDropdown.value;
            container.Children().Last().Clear();
            container.Add(CreateParameterList(property, skillList[methodDropdown.index]));

            property.serializedObject.ApplyModifiedProperties();

        });
        container.Add(methodDropdown);
        container.Add(CreateParameterList(property, skillList[methodDropdown.index]));

        return container;
    }

    private VisualElement CreateParameterList(SerializedProperty property, Type classType)
    {
        VisualElement paramContainer = new();

        // get selected skill class
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

            // set static value from skill class, if skill was changed
            object classInstance = null;
            if (property.FindPropertyRelative("isDirty").boolValue)
            {
                classInstance = Activator.CreateInstance(classType, new object[] { null, null });
                // value = parameterList[i].GetValue(parameterList[i]);
            }

            // iterate over all parameters and create fields
            switch (paramType)
            {
                case ValueType.Int:
                    SerializedProperty intProp = paramProp.FindPropertyRelative("IntValue");
                    IntegerField intField = new($"{parameterList[i].Name} (Int)");

                    // set values to the field
                    if (classInstance != null) intField.value = (int)parameterList[i].GetValue(classInstance); // take base value from class
                    else intField.value = intProp.intValue; // take saved value
                    intProp.intValue = intField.value;

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

                    // set values to the field
                    if (classInstance != null) floatField.value = (float)parameterList[i].GetValue(classInstance); // take base value from class
                    else floatField.value = floatProp.floatValue; // take saved value
                    floatProp.floatValue = floatField.value;

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

                    // set values to the field
                    if (classInstance != null) stringField.value = (string)parameterList[i].GetValue(classInstance); // take base value from class
                    else stringField.value = stringProp.stringValue; // take saved value
                    stringProp.stringValue = stringField.value;

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

                    // set values to the field
                    if (classInstance != null) boolField.value = (bool)parameterList[i].GetValue(classInstance); // take base value from class
                    else boolField.value = boolProp.boolValue; // take saved value
                    boolProp.boolValue = boolField.value;

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

                    // set values to the field
                    if (classInstance != null) vector3Field.value = (Vector3)parameterList[i].GetValue(classInstance); // take base value from class
                    else vector3Field.value = vector3Prop.vector3Value; // take saved value
                    vector3Prop.vector3Value = vector3Field.value;

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


        property.FindPropertyRelative("isDirty").boolValue = false; // set data as clean
        Debug.Log("Set Clean");
        return paramContainer;

    }
}