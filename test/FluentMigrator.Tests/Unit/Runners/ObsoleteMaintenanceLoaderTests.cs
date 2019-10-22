// ***********************************************************************
// Assembly         : FluentMigrator.Tests
// Author           : eivin
// Created          : 10-10-2019
//
// Last Modified By : eivin
// Last Modified On : 10-10-2019
// ***********************************************************************
// <copyright file="ObsoleteMaintenanceLoaderTests.cs" company="FluentMigrator Project">
//     Sean Chambers and the FluentMigrator project 2008-2018
// </copyright>
// <summary></summary>
// ***********************************************************************
#region License
//
// Copyright (c) 2007-2018, Sean Chambers <schambers80@gmail.com>
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

using FluentMigrator.Infrastructure;
using FluentMigrator.Infrastructure.Extensions;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Infrastructure;

using Moq;
using NUnit.Framework;

using Shouldly;

namespace FluentMigrator.Tests.Unit.Runners
{
    /// <summary>
    /// Defines test class ObsoleteMaintenanceLoaderTests.
    /// </summary>
    [TestFixture]
    [Obsolete]
    public class ObsoleteMaintenanceLoaderTests
    {
        /// <summary>
        /// The tag1
        /// </summary>
        public const string Tag1 = "MaintenanceTestTag1";
        /// <summary>
        /// The tag2
        /// </summary>
        public const string Tag2 = "MaintenanceTestTag2";
        /// <summary>
        /// The tags
        /// </summary>
        private string[] _tags = {Tag1, Tag2};

        /// <summary>
        /// The migration conventions
        /// </summary>
        private Mock<IMigrationRunnerConventions> _migrationConventions;
        /// <summary>
        /// The maintenance loader
        /// </summary>
        private MaintenanceLoader _maintenanceLoader;
        /// <summary>
        /// The maintenance loader no tags
        /// </summary>
        private MaintenanceLoader _maintenanceLoaderNoTags;

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _migrationConventions = new Mock<IMigrationRunnerConventions>();
            _migrationConventions.Setup(x => x.GetMaintenanceStage).Returns(DefaultMigrationRunnerConventions.Instance.GetMaintenanceStage);
            _migrationConventions.Setup(x => x.TypeHasTags).Returns(DefaultMigrationRunnerConventions.Instance.TypeHasTags);
            _migrationConventions.Setup(x => x.TypeHasMatchingTags).Returns(DefaultMigrationRunnerConventions.Instance.TypeHasMatchingTags);

            _maintenanceLoader = new MaintenanceLoader(new SingleAssembly(GetType().Assembly), _tags, _migrationConventions.Object);
            _maintenanceLoaderNoTags = new MaintenanceLoader(new SingleAssembly(GetType().Assembly), null, _migrationConventions.Object);
        }

        /// <summary>
        /// Defines the test method LoadsMigrationsForCorrectStage.
        /// </summary>
        [Test]
        public void LoadsMigrationsForCorrectStage()
        {
            var migrationInfos = _maintenanceLoader.LoadMaintenance(MigrationStage.BeforeEach);
            _migrationConventions.Verify(x => x.GetMaintenanceStage, Times.AtLeastOnce());
            Assert.IsNotEmpty(migrationInfos);

            foreach (var migrationInfo in migrationInfos)
            {
                migrationInfo.Migration.ShouldNotBeNull();

                // The NoTag maintenance should not be found in the tagged maintenanceLoader because it wants tagged classes
                Assert.IsFalse(migrationInfo.Migration.GetType().Equals(typeof(MaintenanceBeforeEachNoTag)));

                var maintenanceAttribute = migrationInfo.Migration.GetType().GetOneAttribute<MaintenanceAttribute>();
                maintenanceAttribute.ShouldNotBeNull();
                maintenanceAttribute.Stage.ShouldBe(MigrationStage.BeforeEach);
            }
        }

        /// <summary>
        /// Defines the test method LoadsMigrationsFilteredByTag.
        /// </summary>
        [Test]
        public void LoadsMigrationsFilteredByTag()
        {
            var migrationInfos = _maintenanceLoader.LoadMaintenance(MigrationStage.BeforeEach);
            _migrationConventions.Verify(x => x.TypeHasMatchingTags, Times.AtLeastOnce());
            Assert.IsNotEmpty(migrationInfos);

            foreach (var migrationInfo in migrationInfos)
            {
                migrationInfo.Migration.ShouldNotBeNull();

                // The NoTag maintenance should not be found in the tagged maintenanceLoader because it wants tagged classes
                Assert.IsFalse(migrationInfo.Migration.GetType().Equals(typeof(MaintenanceBeforeEachNoTag)));

                DefaultMigrationRunnerConventions.Instance.TypeHasMatchingTags(migrationInfo.Migration.GetType(), _tags)
                    .ShouldBeTrue();
            }
        }

        /// <summary>
        /// Defines the test method MigrationInfoIsAttributedIsFalse.
        /// </summary>
        [Test]
        public void MigrationInfoIsAttributedIsFalse()
        {
            var migrationInfos = _maintenanceLoader.LoadMaintenance(MigrationStage.BeforeEach);
            Assert.IsNotEmpty(migrationInfos);

            foreach (var migrationInfo in migrationInfos)
            {
                migrationInfo.IsAttributed().ShouldBeFalse();
            }
        }

        /// <summary>
        /// Defines the test method SetsTransactionBehaviorToSameAsMaintenanceAttribute.
        /// </summary>
        [Test]
        public void SetsTransactionBehaviorToSameAsMaintenanceAttribute()
        {
            var migrationInfos = _maintenanceLoader.LoadMaintenance(MigrationStage.BeforeEach);
            Assert.IsNotEmpty(migrationInfos);

            foreach (var migrationInfo in migrationInfos)
            {
                migrationInfo.Migration.ShouldNotBeNull();

                var maintenanceAttribute = migrationInfo.Migration.GetType().GetOneAttribute<MaintenanceAttribute>();
                maintenanceAttribute.ShouldNotBeNull();
                migrationInfo.TransactionBehavior.ShouldBe(maintenanceAttribute.TransactionBehavior);
            }
        }

        /// <summary>
        /// Defines the test method LoadsMigrationsNoTag.
        /// </summary>
        [Test]
        public void LoadsMigrationsNoTag()
        {
            var migrationInfos = _maintenanceLoaderNoTags.LoadMaintenance(MigrationStage.BeforeEach);
            _migrationConventions.Verify(x => x.TypeHasMatchingTags, Times.AtLeastOnce());
            Assert.IsNotEmpty(migrationInfos);

            bool foundNoTag = false;
            foreach (var migrationInfo in migrationInfos)
            {
                migrationInfo.Migration.ShouldNotBeNull();

                // Both notag maintenance and tagged maintenance should be found in the notag maintenanceLoader because he doesn't care about tags
                if (migrationInfo.Migration.GetType().Equals(typeof(MaintenanceBeforeEachNoTag)))
                {
                    foundNoTag = true;
                }
                else
                {
                    DefaultMigrationRunnerConventions.Instance.TypeHasMatchingTags(migrationInfo.Migration.GetType(), _tags)
                        .ShouldBeTrue();
                }
            }

            Assert.IsTrue(foundNoTag);
        }
    }
}