using System;
using UnityEngine;

public enum ValueType { Int, Float, Bool, String, Vector3, GameObject }

[Serializable]
public struct AnyValue
{
    public ValueType type;

    // Storage forr different types of values
    public int IntValue;
    public float FloatValue;
    public bool BoolValue;
    public string StringValue;
    public Vector3 Vector3Value;
    public GameObject GameObjectValue;


    // Implicit conversion operators to convert TypeValue to different types
    public static implicit operator int(AnyValue value) => value.ConvertValue<int>();
    public static implicit operator float(AnyValue value) => value.ConvertValue<float>();
    public static implicit operator bool(AnyValue value) => value.ConvertValue<bool>();
    public static implicit operator string(AnyValue value) => value.ConvertValue<string>();
    public static implicit operator Vector3(AnyValue value) => value.ConvertValue<Vector3>();
    public static implicit operator GameObject(AnyValue value) => value.ConvertValue<GameObject>();

    public T ConvertValue<T>()
    {
        if (typeof(T) == typeof(object)) return CastToObject<T>();
        return type switch
        {
            ValueType.Int => AsInt<T>(IntValue),
            ValueType.Float => AsFloat<T>(FloatValue),
            ValueType.Bool => AsBool<T>(BoolValue),
            ValueType.String => (T)(object)StringValue,
            ValueType.Vector3 => AsVector3<T>(Vector3Value),
            _ => throw new NotSupportedException($"Not supported value type: {typeof(T)}")
        };
    }

    // Methods to convert primitive types to generic types with type safety and without boxing
    // if the correct type is requested, it returns the value, otherwise it returns the default value of T without producing an exception
    T AsInt<T>(int value) => typeof(T) == typeof(int) && value is T correctType ? correctType : default;
    T AsFloat<T>(float value) => typeof(T) == typeof(float) && value is T correctType ? correctType : default;
    T AsBool<T>(bool value) => typeof(T) == typeof(bool) && value is T correctType ? correctType : default;
    T AsVector3<T>(Vector3 value) => typeof(T) == typeof(Vector3) && value is T correctType ? correctType : default;

    // gets the matching type to a given enum value
    public static Type TypeOf(ValueType valueType)
    {
        return valueType switch
        {
            ValueType.Bool => typeof(bool),
            ValueType.Int => typeof(int),
            ValueType.Float => typeof(float),
            ValueType.String => typeof(string),
            ValueType.Vector3 => typeof(Vector3),
            ValueType.GameObject => typeof(GameObject),
            _ => throw new NotSupportedException($"Unsupported ValueType: {valueType}")
        };
    }

    // gets the matching enum value to a given type
    public static ValueType ValueTypeOf(Type type)
    {
        return type switch
        {
            _ when type == typeof(bool) => ValueType.Bool,
            _ when type == typeof(int) => ValueType.Int,
            _ when type == typeof(float) => ValueType.Float,
            _ when type == typeof(string) => ValueType.String,
            _ when type == typeof(Vector3) => ValueType.Vector3,
            _ when type == typeof(GameObject) => ValueType.GameObject,
            _ => throw new NotSupportedException($"Unsupported type: {type}")
        };
    }

    T CastToObject<T>()
    {
        return type switch
        {
            ValueType.Int => (T)(object)IntValue,
            ValueType.Float => (T)(object)FloatValue,
            ValueType.Bool => (T)(object)BoolValue,
            ValueType.String => (T)(object)StringValue,
            ValueType.Vector3 => (T)(object)Vector3Value,
            ValueType.GameObject => (T)(object)GameObjectValue,
            _ => throw new InvalidCastException($"Cannot convert AnyValue of type {type} to {typeof(T).Name}")
        };
    }
}
