using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Santa.Core.Attributes;

namespace Santa.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(AddressableKeySelectionAttribute))]
    public class AddressableKeySelectionDrawer : PropertyDrawer
    {
        private string[] _options;
        private bool _initialized;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            if (!_initialized)
            {
                InitializeOptions();
            }

            if (_options == null || _options.Length == 0)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            // Find current index
            string currentVal = property.stringValue;
            int index = Array.IndexOf(_options, currentVal);

            // Draw Popup
            int newIndex = EditorGUI.Popup(position, label.text, index, _options);

            // Update value
            if (newIndex >= 0 && newIndex < _options.Length)
            {
                property.stringValue = _options[newIndex];
            }

        }

        private void InitializeOptions()
        {
            var attr = attribute as AddressableKeySelectionAttribute;
            if (attr?.ConstantsType == null) return;

            var fields = attr.ConstantsType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string));

            var values = new List<string>();
            foreach (var field in fields)
            {
                string val = field.GetValue(null) as string;
                if (val != null) values.Add(val);
            }
            
            _options = values.ToArray();
            _initialized = true;
        }
    }
}
