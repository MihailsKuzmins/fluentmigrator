using System;
using FluentMigrator.Runner.Generators.Oracle;
using FluentMigrator.Runner.Initialization;

using Microsoft.Extensions.Options;

using NUnit.Framework;

using Shouldly;

namespace FluentMigrator.Tests.Unit.Generators.OracleWithQuotedIdentifier
{
    [TestFixture]
    public class OracleDataTests : BaseDataTests
    {
        private static OracleGenerator CreateFixture(QuoterOptions options = null) =>
            new OracleGenerator(new OracleQuoterQuotedIdentifier(new OptionsWrapper<QuoterOptions>(options)));

        [Test]
        public override void CanDeleteDataForAllRowsWithCustomSchema()
        {
            var expression = GeneratorTestHelper.GetDeleteDataAllRowsExpression();
            expression.SchemaName = "TestSchema";

            var result = CreateFixture().Generate(expression);
            result.ShouldBe("DELETE FROM \"TestSchema\".\"TestTable1\" WHERE 1 = 1");
        }

        [Test]
        public override void CanDeleteDataForAllRowsWithDefaultSchema()
        {
            var expression = GeneratorTestHelper.GetDeleteDataAllRowsExpression();

            var result = CreateFixture().Generate(expression);
            result.ShouldBe("DELETE FROM \"TestTable1\" WHERE 1 = 1");
        }

        [Test]
        public override void CanDeleteDataForMultipleRowsWithCustomSchema()
        {
            var expression = GeneratorTestHelper.GetDeleteDataMultipleRowsExpression();
            expression.SchemaName = "TestSchema";

            var result = CreateFixture().Generate(expression);
            result.ShouldBe(
                "DELETE FROM \"TestSchema\".\"TestTable1\" WHERE \"Name\" = 'Just''in' AND \"Website\" IS NULL" + Environment.NewLine +
                ";" + Environment.NewLine +
                "DELETE FROM \"TestSchema\".\"TestTable1\" WHERE \"Website\" = 'github.com'");
        }

        [Test]
        public override void CanDeleteDataForMultipleRowsWithDefaultSchema()
        {
            var expression = GeneratorTestHelper.GetDeleteDataMultipleRowsExpression();

            var result = CreateFixture().Generate(expression);
            result.ShouldBe(
                "DELETE FROM \"TestTable1\" WHERE \"Name\" = 'Just''in' AND \"Website\" IS NULL" + Environment.NewLine +
                ";" + Environment.NewLine +
                "DELETE FROM \"TestTable1\" WHERE \"Website\" = 'github.com'");
        }

        [Test]
        public override void CanDeleteDataWithCustomSchema()
        {
            var expression = GeneratorTestHelper.GetDeleteDataExpression();
            expression.SchemaName = "TestSchema";

            var result = CreateFixture().Generate(expression);
            result.ShouldBe("DELETE FROM \"TestSchema\".\"TestTable1\" WHERE \"Name\" = 'Just''in' AND \"Website\" IS NULL");
        }

        [Test]
        public override void CanDeleteDataWithDefaultSchema()
        {
            var expression = GeneratorTestHelper.GetDeleteDataExpression();

            var result = CreateFixture().Generate(expression);
            result.ShouldBe("DELETE FROM \"TestTable1\" WHERE \"Name\" = 'Just''in' AND \"Website\" IS NULL");
        }

        [Test]
        public override void CanDeleteDataWithDbNullCriteria()
        {
            var expression = GeneratorTestHelper.GetDeleteDataExpressionWithDbNullValue();
            var result = CreateFixture().Generate(expression);
            result.ShouldBe("DELETE FROM \"TestTable1\" WHERE \"Name\" = 'Just''in' AND \"Website\" IS NULL");
        }

        [Test]
        public override void CanInsertDataWithCustomSchema()
        {
            var expression = GeneratorTestHelper.GetInsertDataExpression();
            expression.SchemaName = "TestSchema";

            var expected = "INSERT ALL INTO \"TestSchema\".\"TestTable1\" (\"Id\", \"Name\", \"Website\") VALUES (1, 'Just''in', 'codethinked.com')";
            expected += " INTO \"TestSchema\".\"TestTable1\" (\"Id\", \"Name\", \"Website\") VALUES (2, 'Na\\te', 'kohari.org')";
            expected += " SELECT 1 FROM DUAL";

            var result = CreateFixture().Generate(expression);
            result.ShouldBe(expected);
        }

        [Test]
        public override void CanInsertDataWithDefaultSchema()
        {
            var expression = GeneratorTestHelper.GetInsertDataExpression();

            var expected = "INSERT ALL INTO \"TestTable1\" (\"Id\", \"Name\", \"Website\") VALUES (1, 'Just''in', 'codethinked.com')";
            expected += " INTO \"TestTable1\" (\"Id\", \"Name\", \"Website\") VALUES (2, 'Na\\te', 'kohari.org')";
            expected += " SELECT 1 FROM DUAL";

            var result = CreateFixture().Generate(expression);
            result.ShouldBe(expected);
        }

        [Test]
        public override void CanInsertGuidDataWithCustomSchema()
        {
            //Oracle can not insert GUID data using string representation
            var expression = GeneratorTestHelper.GetInsertGUIDExpression(new Guid("7E487B79-626C-4E7D-811C-BC30AB31C564"));
            expression.SchemaName = "TestSchema";

            var result = CreateFixture().Generate(expression);
            result.ShouldBe("INSERT ALL INTO \"TestSchema\".\"TestTable1\" (\"guid\") VALUES ('797B487E6C627D4E811CBC30AB31C564') SELECT 1 FROM DUAL");
        }

        [Test]
        public override void CanInsertGuidDataWithDefaultSchema()
        {
            //Oracle can not insert GUID data using string representation
            var expression = GeneratorTestHelper.GetInsertGUIDExpression(new Guid("7E487B79-626C-4E7D-811C-BC30AB31C564"));

            var result = CreateFixture().Generate(expression);
            result.ShouldBe("INSERT ALL INTO \"TestTable1\" (\"guid\") VALUES ('797B487E6C627D4E811CBC30AB31C564') SELECT 1 FROM DUAL");
        }

        [Test]
        public override void CanInsertEnumAsString()
        {
            var expression = GeneratorTestHelper.GetInsertEnumExpression();

            var result = CreateFixture().Generate(expression);
            result.ShouldBe("INSERT ALL INTO \"TestTable1\" (\"enum\") VALUES ('Boo') SELECT 1 FROM DUAL");
        }

        [Test]
        public override void CanInsertEnumAsUnderlyingType()
        {
            var options = new QuoterOptions
            {
                EnumAsUnderlyingType = true
            };

            var expression = GeneratorTestHelper.GetInsertEnumExpression();

            var result = CreateFixture(options).Generate(expression);
            result.ShouldBe("INSERT ALL INTO \"TestTable1\" (\"enum\") VALUES (2) SELECT 1 FROM DUAL");
        }

        [Test]
        public override void CanUpdateDataForAllDataWithCustomSchema()
        {
            var expression = GeneratorTestHelper.GetUpdateDataExpressionWithAllRows();
            expression.SchemaName = "TestSchema";

            var result = CreateFixture().Generate(expression);
            result.ShouldBe("UPDATE \"TestSchema\".\"TestTable1\" SET \"Name\" = 'Just''in', \"Age\" = 25 WHERE 1 = 1");
        }

        [Test]
        public override void CanUpdateDataForAllDataWithDefaultSchema()
        {
            var expression = GeneratorTestHelper.GetUpdateDataExpressionWithAllRows();

            var result = CreateFixture().Generate(expression);
            result.ShouldBe("UPDATE \"TestTable1\" SET \"Name\" = 'Just''in', \"Age\" = 25 WHERE 1 = 1");
        }

        [Test]
        public override void CanUpdateDataWithCustomSchema()
        {
            var expression = GeneratorTestHelper.GetUpdateDataExpression();
            expression.SchemaName = "TestSchema";

            var result = CreateFixture().Generate(expression);
            result.ShouldBe("UPDATE \"TestSchema\".\"TestTable1\" SET \"Name\" = 'Just''in', \"Age\" = 25 WHERE \"Id\" = 9 AND \"Homepage\" IS NULL");
        }

        [Test]
        public override void CanUpdateDataWithDefaultSchema()
        {
            var expression = GeneratorTestHelper.GetUpdateDataExpression();

            var result = CreateFixture().Generate(expression);
            result.ShouldBe("UPDATE \"TestTable1\" SET \"Name\" = 'Just''in', \"Age\" = 25 WHERE \"Id\" = 9 AND \"Homepage\" IS NULL");
        }

        [Test]
        public override void CanUpdateDataWithDbNullCriteria()
        {
            var expression = GeneratorTestHelper.GetUpdateDataExpressionWithDbNullValue();

            var result = CreateFixture().Generate(expression);
            result.ShouldBe("UPDATE \"TestTable1\" SET \"Name\" = 'Just''in', \"Age\" = 25 WHERE \"Id\" = 9 AND \"Homepage\" IS NULL");
        }
    }
}
