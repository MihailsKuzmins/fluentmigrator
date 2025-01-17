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

using FluentMigrator.Runner.Generators.Generic;
using FluentMigrator.Runner.Initialization;

using Microsoft.Extensions.Options;

namespace FluentMigrator.Runner.Generators.SqlServer
{
    public class SqlServer2000Quoter : GenericQuoter
    {
        [Obsolete]
        public SqlServer2000Quoter()
        {
        }

        public SqlServer2000Quoter(IOptions<QuoterOptions> options)
            : base(options)
        {
        }

        public override string OpenQuote { get { return "["; } }

        public override string CloseQuote { get { return "]"; } }

        public override string CloseQuoteEscapeString { get { return "]]"; } }

        public override string OpenQuoteEscapeString { get { return string.Empty; } }

        public override string QuoteSchemaName(string schemaName)
        {
            return string.Empty;
        }

        public override string FormatNationalString(string value)
        {
            return $"N{FormatAnsiString(value)}";
        }

        public override string FormatSystemMethods(SystemMethods value)
        {
            switch (value)
            {
                case SystemMethods.NewGuid:
                    return "NEWID()";
                case SystemMethods.NewSequentialId:
                    return "NEWSEQUENTIALID()";
                case SystemMethods.CurrentDateTime:
                    return "GETDATE()";
                case SystemMethods.CurrentDateTimeOffset:
                case SystemMethods.CurrentUTCDateTime:
                    return "GETUTCDATE()";
                case SystemMethods.CurrentUser:
                    return "CURRENT_USER";
            }

            return base.FormatSystemMethods(value);
        }
    }
}
