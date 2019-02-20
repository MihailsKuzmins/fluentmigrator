#region License
//
// Copyright (c) 2019, Fluent Migrator Project
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
namespace FluentMigrator.Runner.Constraints
{
    /// <summary>
    /// Specifies minimum schema version against which this migration will be run.
    /// </summary>
    public class CurrentVersionMigrationConstraintAttribute : MigrationConstraintAttribute
    {
        /// <param name="minimumVersionToRunAgainst">The schema must equal or greater to this value for this migration to be run.</param>
        public CurrentVersionMigrationConstraintAttribute(long minimumVersionToRunAgainst)
            : base(ctx => ctx.VersionInfo.Latest() >= minimumVersionToRunAgainst) {
        }

    }
}
