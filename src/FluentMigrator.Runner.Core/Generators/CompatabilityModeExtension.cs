// ***********************************************************************
// Assembly         : FluentMigrator.Runner.Core
// Author           : eivin
// Created          : 10-10-2019
//
// Last Modified By : eivin
// Last Modified On : 10-10-2019
// ***********************************************************************
// <copyright file="CompatabilityModeExtension.cs" company="FluentMigrator Project">
//     Sean Chambers and the FluentMigrator project 2008-2018
// </copyright>
// <summary></summary>
// ***********************************************************************
#region License
//
// Copyright (c) 2018, Fluent Migrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;

using FluentMigrator.Exceptions;

namespace FluentMigrator.Runner.Generators
{
    /// <summary>
    /// Class CompatabilityModeExtension.
    /// </summary>
    [Obsolete]
    public static class CompatabilityModeExtension
    {
        /// <summary>
        /// Handles the compatabilty.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="message">The message.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="DatabaseOperationNotSupportedException"></exception>
        [Obsolete]
        public static string HandleCompatabilty(this CompatabilityMode mode, string message)
        {
            if (CompatabilityMode.STRICT == mode)
            {
                throw new DatabaseOperationNotSupportedException(message);
            }

            return string.Empty;
        }
    }
}