﻿// <copyright file="OperatorEnum.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SimpleFlaskManager.ProfileManager
{
    /// <summary>
    /// Different type of operators available to use on the Conditions.
    /// </summary>
    public enum OperatorEnum
    {
        /// <summary>
        /// Equal To Operator.
        /// </summary>
        EQUAL_TO,

        /// <summary>
        /// Not Equal To Operator.
        /// </summary>
        NOT_EQUAL_TO,

        /// <summary>
        /// Greater Than Operator.
        /// </summary>
        BIGGER_THAN,

        /// <summary>
        /// Less Than Operator.
        /// </summary>
        LESS_THAN,

        /// <summary>
        /// List/Dict Contains Operator.
        /// </summary>
        CONTAINS,

        /// <summary>
        /// List/Dict doesn't Contain Operator.
        /// </summary>
        NOT_CONTAINS,
    }
}
