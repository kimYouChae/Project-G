using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    #region Enum : 타입 자체에는 사용불가, 불가피하게 제네릭 사용 
    // Enum 의 길이 반환
    public static int EnumCount<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Length;
    }

    // 인덱스로 Enum 요소 반환
    public static TEnum GetElement<TEnum>(int idx) where TEnum : Enum
    {
        // enum 범위를 넘으면
        if (idx < 0 || idx >= EnumCount<Enum>())
        {
            throw new IndexOutOfRangeException($"Index {idx} is out of range for enum {typeof(TEnum).Name}");
        }

        Array values = Enum.GetValues(typeof(TEnum));
        return (TEnum)values.GetValue(idx);
    }

    // String을 같은 이름의 Enum으로 변환
    public static TEnum StringToEnum<TEnum>(string input) where TEnum : Enum
    {
        // return Enum.Parse<TEnum>(input);
        return (TEnum)Enum.Parse(typeof(TEnum), input);
    }

    // Enum을 Array 로 return
    public static Array ToArray<TEnum>() where TEnum : Enum
    {
        Array array = Enum.GetValues(typeof(TEnum));
        return array;
    }

    #endregion
}
