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
using System.Linq;

using FluentMigrator.Runner.Generators.Generic;
using FluentMigrator.Runner.Initialization;

using Microsoft.Extensions.Options;

namespace FluentMigrator.Runner.Generators.Redshift
{
    public class RedshiftQuoter : GenericQuoter
    {
        [Obsolete]
        public RedshiftQuoter()
        {
        }

        public RedshiftQuoter(IOptions<QuoterOptions> options)
            : base(options)
        {
        }

        public override string FormatBool(bool value) { return value ? "true" : "false"; }

        public override string QuoteSchemaName(string schemaName)
        {
            if (string.IsNullOrEmpty(schemaName))
                schemaName = "public";
            return base.QuoteSchemaName(schemaName);
        }

        protected override string FormatByteArray(byte[] array)
        {
            var arrayAsHex = array.Select(b => b.ToString("X2")).ToArray();
            return @"E'\\x" + string.Concat(arrayAsHex) + "'";
        }

        public string UnQuoteSchemaName(string quoted)
        {
            if (string.IsNullOrEmpty(quoted))
                return "public";

            return UnQuote(quoted);
        }

        public override string FormatSystemMethods(SystemMethods value)
        {
            switch (value)
            {
                case SystemMethods.CurrentDateTimeOffset:
                case SystemMethods.CurrentDateTime:
                    return "SYSDATE";
                case SystemMethods.CurrentUTCDateTime:
                    return "(SYSDATE at time zone 'UTC')";
                case SystemMethods.CurrentUser:
                    return "current_user";
            }

            return base.FormatSystemMethods(value);
        }
    }
}
