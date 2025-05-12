using System;
using System.Collections.Generic;
using Combat;
using Core;
using UnityEngine;

namespace CharacterProgressionMatrix
{
    public class CharacterMatrix : MonoBehaviour
    {
        [SerializeField] CharacterMatrixTemplate _matrixDataReference;

        private readonly List<List<ISocket<ISocketable>>> _matrix = new();
        public event Action OnUpdateCharacterMatrix;

        private bool _isActive = true;

        public void Initialize()
        {
            Debug.Assert(_matrixDataReference != null, "The character has no assigned Skillset!");

            for (int r = 0; r < _matrixDataReference.Rows.Count; r++)
            {
                _matrix.Add(new List<ISocket<ISocketable>>());
                for (int c = 0; c < _matrixDataReference.Rows[r].Slots.Count; c++)
                {
                    CharacterMatrixTemplate.SocketInfo socketInfo = _matrixDataReference.GetSocketInfo(r, c);

                    //create new sockets according to the predefined socket types
                    switch (socketInfo.SocketType)
                    {
                        case SocketType.Weapon:
                            Socket<Weapon> weaponSocket = new Socket<Weapon>(r, c);
                            _matrix[r].Add(weaponSocket);
                            break;
                        case SocketType.Mod:
                        case SocketType.Mod | SocketType.Skill:
                            Skill skill = Skill.CreateSkillFromTemplate(socketInfo.SocketSkillTemplate.info, gameObject);
                            Socket<Mod> modSocket = new Socket<Mod>(r, c, skill);
                            _matrix[r].Add(modSocket);
                            break;
                        default:
                            Debug.LogError($"Mod Type at position ({r},{c}) is not supported");
                            break;
                    }
                }
            }
            ServiceProvider.Get<GameManager>().GetPlayer().OnDeath += () => _isActive = false;
        }

        void Update()
        {
            if (!_isActive) return;
            OnUpdateCharacterMatrix?.Invoke(); // invoke all active skills
        }

        public void UnlockSkill(int rowIndex, int columnIndex)
        {
            ISocket<ISocketable> socket = GetSocket(rowIndex, columnIndex);
            OnUpdateCharacterMatrix += socket?.Skill?.Unlock(); // registers skill to update loop
        }

        /// <summary>
        /// Sockets the given weapon into the socket at the given position.
        /// </summary>
        /// <returns>The Weapon that was socketed before and null if the socket can't socket a weapon or if no previous weapon was set.</returns>
        public Weapon SetWeapon(Weapon weapon, int rowIndex, int columnIndex)
        {
            ISocket<ISocketable> socket = GetSocket(rowIndex, columnIndex);
            Socket<Weapon> weaponSocket = socket as Socket<Weapon>;

            Weapon oldWeapon = weaponSocket?.SocketItem(weapon);
            return oldWeapon;
        }

        /// <summary>
        /// Sockets the given mod into the socket at the given position.
        /// </summary>
        /// <returns>The Mod that was socketed before and null if the socket can't socket a mod or if no previous Mod was set.</returns>
        public Mod SetMod(Mod mod, int rowIndex, int columnIndex)
        {
            ISocket<ISocketable> socket = GetSocket(rowIndex, columnIndex);
            Socket<Mod> modSocket = socket as Socket<Mod>;

            Mod oldMod = modSocket?.SocketItem(mod);
            return oldMod;
        }

        private ISocket<ISocketable> GetSocket(int rowIndex, int columnIndex)
        {
            ISocket<ISocketable> socket = _matrix[rowIndex][columnIndex];
            Debug.Assert(socket != null, $"ASSERT: The used socket at index {rowIndex} : {columnIndex} is not assigned.");
            return socket;
        }
    }
}










