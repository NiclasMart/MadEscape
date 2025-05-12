using System;
using UnityEngine;

namespace CharacterProgressionMatrix
{
    [Flags]
    public enum SocketType
    {
        None = 0,
        Weapon = 1,
        Mod = 2,
        Skill = 4
    }

    public interface ISocketable { }

    public interface ISocket<out T> where T : ISocketable
    {
        T SocketedItem { get; }
        Skill Skill { get; }
    }

    public class Socket<T> : ISocket<T> where T : ISocketable
    {
        int _rowIndex, _columnIndex;
        public T SocketedItem { get; private set; }
        public Skill Skill { get; private set; }

        public Socket(int rowIndex, int columnIndex, Skill skill = null)
        {
            _rowIndex = rowIndex;
            _columnIndex = columnIndex;
            Skill = skill;
            Debug.Log($"Created Socket at position {rowIndex} : {columnIndex}");
        }

        public T SocketItem(T newItem)
        {
            T oldSocketedItem = SocketedItem;
            SocketedItem = newItem;
            return oldSocketedItem;
        }
    }
}