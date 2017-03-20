//-----------------------------------------------------------------------
// <copyright file="CamelCaseToLowerDashConverterTest.cs" company="Project Kaleidoscope Authors">
// Copyright (c) Moegirlsaikou Foundation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moegirlpedia.MediaWikiInterop.Primitives.Transform;

namespace PrimitiveLibraryTest.Transform
{
    [TestClass]
    public class CamelCaseToLowerDashConverterTest
    {
        [TestMethod]
        public void TestConvert()
        {
            var converter = new CamelCaseToLowerDashConverter();

            Assert.AreEqual("some-object", converter.SerializeFinalizedFields(TestEnum.SomeObject));
            Assert.AreEqual("general", converter.SerializeFinalizedFields(TestEnum.General));
            Assert.AreEqual("something-test15", converter.SerializeFinalizedFields(TestEnum.SomethingTest15));
        }

        public enum TestEnum
        {
            SomeObject,
            General,
            SomethingTest15
        }
    }
}
