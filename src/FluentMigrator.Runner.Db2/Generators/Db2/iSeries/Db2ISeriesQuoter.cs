#region License
// Copyright (c) 2018, Fluent Migrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;

using FluentMigrator.Runner.Initialization;

using Microsoft.Extensions.Options;

namespace FluentMigrator.Runner.Generators.DB2.iSeries
{
    public class Db2ISeriesQuoter : Db2Quoter
    {
        [Obsolete]
        public Db2ISeriesQuoter()
        {
        }

        public Db2ISeriesQuoter(IOptions<QuoterOptions> options)
            : base(options)
        {
        }

        public override string QuoteConstraintName(string constraintName, string schemaName = null)
        {
            return CreateSchemaPrefixedQuotedIdentifier(
                QuoteSchemaName(schemaName),
                IsQuoted(constraintName) ? constraintName : Quote(constraintName));
        }

        public override string QuoteIndexName(string indexName, string schemaName)
        {
            return CreateSchemaPrefixedQuotedIdentifier(
                QuoteSchemaName(schemaName),
                IsQuoted(indexName) ? indexName : Quote(indexName));
        }
    }
}
