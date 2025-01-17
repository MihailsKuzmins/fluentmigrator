#region License
// Copyright (c) 2007-2018, FluentMigrator Project
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

using FluentMigrator.Runner.Generators.MySql;
using FluentMigrator.Runner.Initialization;

using Microsoft.Extensions.Options;

namespace FluentMigrator.Runner.Processors.MySql
{
    [Obsolete]
    public class MySql5ProcessorFactory : MigrationProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        [Obsolete]
        public MySql5ProcessorFactory()
            : this(serviceProvider: null)
        {
        }

        public MySql5ProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [Obsolete]
        public override IMigrationProcessor Create(string connectionString, IAnnouncer announcer, IMigrationProcessorOptions options)
        {
            var factory = new MySqlDbFactory(_serviceProvider);
            var connection = factory.CreateConnection(connectionString);
            var quoterOptions = new OptionsWrapper<QuoterOptions>(new QuoterOptions());
            return new MySqlProcessor(connection, new MySql5Generator(new MySqlQuoter(quoterOptions)), announcer, options, factory);
        }
    }
}
