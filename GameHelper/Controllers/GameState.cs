﻿// <copyright file="GameState.cs" company="None">
// Copyright (c) None. All rights reserved.
// </copyright>

namespace GameHelper.Controllers
{
    using System;
    using System.Collections.Generic;
    using Coroutine;
    using GameOffsets.Controllers;
    using GameOffsets.Native;

    /// <summary>
    /// Reads and stores the global state of the game.
    /// </summary>
    public class GameState : ControllerBase
    {
        /// <summary>
        /// Gets a dictionary containing all the Game States addresses.
        /// </summary>
        public Dictionary<string, IntPtr> States
        {
            get;
            private set;
        }

        = new Dictionary<string, IntPtr>();

        /// <inheritdoc/>
        protected override void OnAddressUpdated(IntPtr newAddress)
        {
            if (newAddress != IntPtr.Zero)
            {
                this.UpdateAllStates();
            }

            CoroutineHandler.RaiseEvent(this.OnControllerReady);
        }

        /// <summary>
        /// This function Updates the states addresses.
        /// </summary>
        private void UpdateAllStates()
        {
            var reader = Core.Process.Handle;
            var staticAddressData = reader.ReadMemory<GameStateStaticObject>(this.Address);
            var gameStateData = reader.ReadMemory<GameStateObject>(staticAddressData.GameState);
            var states = reader.ReadStdMap<StdWString, IntPtr>(gameStateData.States);
            foreach (var state in states)
            {
                string name = reader.ReadStdWString(state.Key);
                if (this.States.ContainsKey(name))
                {
                    this.States[name] = state.Value;
                }
                else
                {
                    this.States.Add(name, state.Value);
                }
            }
        }
    }
}