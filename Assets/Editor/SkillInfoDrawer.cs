using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.UIElements;
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
        var updateCheckerField = new PropertyField(property.FindPropertyRelative("NeedsUpdateLoop"), "Skill needs Update loop");
        container.Add(updateCheckerField);

        // Skill
        MethodInfo[] skillList = typeof(CharacterSkillLibrary).GetMethods(BindingFlags.Public | BindingFlags.Static);
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
            property.serializedObject.ApplyModifiedProperties();
        });
        container.Add(methodDropdown);



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
}