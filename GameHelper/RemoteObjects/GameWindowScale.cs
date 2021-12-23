﻿// <copyright file="GameWindowScale.cs" company="None">
// Copyright (c) None. All rights reserved.
// </copyright>

namespace GameHelper.RemoteObjects
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Coroutine;
    using CoroutineEvents;
    using GameOffsets.Objects.UiElement;
    using ImGuiNET;

    /// <summary>
    ///     Reads the Game Window Scale values from the game.
    ///     It's good to read from the game because there are 6 instances of them for different
    ///     type of Ui-Elements. Only reads when game window moves/resize.
    /// </summary>
    public class GameWindowScale : RemoteObjectBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GameWindowScale" /> class.
        /// </summary>
        /// <param name="address">address of the remote memory object.</param>
        internal GameWindowScale(IntPtr address)
            : base(address)
        {
            CoroutineHandler.Start(this.OnGameMove(), priority: int.MaxValue);
            CoroutineHandler.Start(this.OnGameForegroundChange(), priority: int.MaxValue);
        }

        /// <summary>
        ///     Gets the current game window scale values.
        /// </summary>
        public float[] Values { get; } = { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };

        /// <summary>
        ///     Gets the Scale Value depending on the index and multiplier.
        /// </summary>
        /// <param name="index">index read from the Ui-Element.</param>
        /// <param name="multiplier">multiplier read from the Ui-Element.</param>
        /// <returns>Returns the Width and Height scale value valid for the specific Ui-Element.</returns>
        public (float WidthScale, float HeightScale) GetScaleValue(int index, float multiplier)
        {
            var widthScale = multiplier;
            var heightScale = multiplier;
            switch (index)
            {
                case 1:
                    widthScale *= this.Values[UiElementBaseFuncs.SCALE_INDEX_1];
                    heightScale *= this.Values[UiElementBaseFuncs.SCALE_INDEX_1 + 1];
                    break;
                case 2:
                    widthScale *= this.Values[UiElementBaseFuncs.SCALE_INDEX_2];
                    heightScale *= this.Values[UiElementBaseFuncs.SCALE_INDEX_2 + 1];
                    break;
                case 3:
                    widthScale *= this.Values[UiElementBaseFuncs.SCALE_INDEX_3];
                    heightScale *= this.Values[UiElementBaseFuncs.SCALE_INDEX_3 + 1];
                    break;
            }
            if (widthScale != 0) { 
                //Debug.Assert(widthScale == 1.35f && heightScale == 1.35f);
            }
            return (widthScale, heightScale);
        }

        /// <summary>
        ///     Converts the <see cref="GameWindowScale" /> class data to ImGui.
        /// </summary>
        internal override void ToImGui()
        {
            base.ToImGui();
            ImGui.Text($"Index 1: width, height {this.GetScaleValue(1, 1)} ratio");
            ImGui.Text($"Index 2: width, height {this.GetScaleValue(2, 1)} ratio");
            ImGui.Text($"Index 3: width, height {this.GetScaleValue(3, 1)} ratio");
        }

        /// <inheritdoc />
        protected override void CleanUpData()
        {
            for (var i = 0; i < this.Values.Length; i++)
            {
                this.Values[i] = 1f;
            }
        }
        IntPtr temp_width {
            get {
                var m = Core.Process.Handle;
                var poe_addr = Core.Process.Information.MainModule.BaseAddress;
                var _0 = m.ReadMemory<IntPtr>(poe_addr + 0x024373B8);
                var _1 = m.ReadMemory<IntPtr>(_0 + 0xE0);
                var res = _1 + 0x180;
                return res;
            }
        }
     
        /// <inheritdoc />
        protected override void UpdateData(bool hasAddressChanged)
        {
            var reader = Core.Process.Handle;
            var data = reader.ReadMemoryArray<float>(temp_width/*this.Address*/, this.Values.Length);
            for (var i = 0; i < data.Length; i++)
            {
                this.Values[i] = data[i];
            }
        }

        private IEnumerator<Wait> OnGameMove()
        {
            while (true)
            {
                yield return new Wait(GameHelperEvents.OnMoved);

                // No need to check for IntPtr.zero
                // because game will only move when it exists. :D
                this.UpdateData(false);
            }
        }

        private IEnumerator<Wait> OnGameForegroundChange()
        {
            while (true)
            {
                yield return new Wait(GameHelperEvents.OnForegroundChanged);

                // No need to check for IntPtr.zero
                // because game will only move when it exists. :D
                this.UpdateData(false);
            }
        }
    }
}