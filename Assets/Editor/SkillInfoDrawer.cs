using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CharacterProgressionMatrix;
using TMPro;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(SkillTemplate.SkillInfo))]
public class SkillInfoDrawer : PropertyDrawer
{
    const int DESCRIPTION_WIDTH = 150;
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        // Name
        var nameField = new PropertyField(property.FindPropertyRelative("Name"), "Skill Name");
        container.Add(nameField);
        
        // Erstellen der PropertyFields für Duration und Cooldown
        var durationValueField = new PropertyField(property.FindPropertyRelative("Duration"), "Duration");
        var cooldownField = new PropertyField(property.FindPropertyRelative("Cooldown"), "Cooldown");

        // Erstellen der PropertyFields für die Checkboxes
        var activateOnceCheckbox = new PropertyField(property.FindPropertyRelative("OnlyActivatedOnceOnUnlock"), "Activate Once");
        var alwaysActiveCheckbox = new PropertyField(property.FindPropertyRelative("AlwaysActive"), "Always Active");

        // Hinzufügen der Felder zum Container
        container.Add(activateOnceCheckbox);
        container.Add(alwaysActiveCheckbox);
        container.Add(durationValueField);
        container.Add(cooldownField);
        

        // Initiale Sichtbarkeitssteuerung
        UpdateFieldsVisibility(property, durationValueField, cooldownField);

        // Callback für Änderungen an der "Activate Once"-Checkbox
        activateOnceCheckbox.RegisterCallback<ChangeEvent<bool>>(e =>
        {
            // Wenn "Activate Once" aktiviert wird, deaktiviere "Always Active"
            alwaysActiveCheckbox.SetEnabled(value: !e.newValue);
            
            // Sichtbarkeit der Felder aktualisieren
            UpdateFieldsVisibility(property, durationValueField, cooldownField);
        });

        // Callback für Änderungen an der "Always Active"-Checkbox
        alwaysActiveCheckbox.RegisterCallback<ChangeEvent<bool>>(e =>
        {
            // Wenn "Always Active" aktiviert wird, deaktiviere "Activate Once"
            activateOnceCheckbox.SetEnabled(value: !e.newValue);
            
            // Sichtbarkeit der Felder aktualisieren
            UpdateFieldsVisibility(property, durationValueField, cooldownField);
        });

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
        
        var gap = new VisualElement();
        gap.style.height = 30; // Setzt die Höhe der Lücke
        container.Add(gap);

        // Überschrift hinzufügen
        var heading = new Label("Skill Specifics");
        heading.style.fontSize = 14;  // Schriftgröße
        container.Add(heading);
        
        var gap2 = new VisualElement();
        gap2.style.height = 10; // Setzt die Höhe der Lücke
        container.Add(gap2);
        
        // Skill
        Type[] skillList = typeof(SkillLibrary).GetNestedTypes(BindingFlags.Public);
        List<string> dropdownOptions = new();
        foreach (var skill in skillList)
        {
            dropdownOptions.Add(skill.Name);
        }
        var methodDropdown = new DropdownField("Skill", dropdownOptions, 0);

        SerializedProperty skillProperty = property.FindPropertyRelative("SkillRef");
        
        // set value from scriptable object if Skill class is available
        if (methodDropdown.choices.Contains(skillProperty.stringValue))
        {
            methodDropdown.value = skillProperty.stringValue;
        }
        else
        {
            methodDropdown.index = 0;
            Debug.LogWarning($"The skill {skillProperty.stringValue} can't be found in the skill library. The skill {methodDropdown.value} was selected as placeholder");
        }
        
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
                classInstance = Activator.CreateInstance(classType, null);
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
                
                case ValueType.GameObject:
                    SerializedProperty gameObjectProp = paramProp.FindPropertyRelative("GameObjectValue");
                    ObjectField gameObjectField = new($"{parameterList[i].Name} (GameObject)");

                    // set values to the field
                    if (classInstance != null) gameObjectField.value = (GameObject)parameterList[i].GetValue(classInstance); // take base value from class
                    else gameObjectField.value = gameObjectProp.objectReferenceValue; // take saved value
                    gameObjectProp.objectReferenceValue = gameObjectField.value;

                    gameObjectField.RegisterValueChangedCallback(
                        evt =>
                        {
                            gameObjectProp.objectReferenceValue = evt.newValue;
                            parameterProperty.serializedObject.ApplyModifiedProperties();
                        }
                    );
                    field = gameObjectField;
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
    
    private void UpdateFieldsVisibility(SerializedProperty property, VisualElement durationValueField, VisualElement cooldownField)
    {
        // Zugriff auf die aktuellen Werte der Checkboxen über SerializedProperty
        bool isActivateOnceChecked = property.FindPropertyRelative("OnlyActivatedOnceOnUnlock").boolValue;
        bool isAlwaysActiveChecked = property.FindPropertyRelative("AlwaysActive").boolValue;

        // Zeige die Felder nur, wenn beide Checkboxen deaktiviert sind
        bool showFields = !(isActivateOnceChecked || isAlwaysActiveChecked);

        // Sichtbarkeit der Felder festlegen
        durationValueField.style.display = showFields ? DisplayStyle.Flex : DisplayStyle.None;
        cooldownField.style.display = showFields ? DisplayStyle.Flex : DisplayStyle.None;
    }
}