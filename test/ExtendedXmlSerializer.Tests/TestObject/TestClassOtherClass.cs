﻿using JetBrains.Annotations;
using System.Collections.Generic;

namespace ExtendedXmlSerializer.Tests.TestObject
{
	public sealed class TestClassOtherClass
	{
		public static TestClassOtherClass Create()
		{
			var result = new TestClassOtherClass
			{
				Primitive1   = TestClassPrimitiveTypes.Create(),
				Primitive2   = TestClassPrimitiveTypes.Create(),
				ListProperty = new List<TestClassItem>(),
				Other = new TestClassOther
				{
					Test   = new TestClassItem {Id = 2, Name = "Other Name"},
					Double = 7.3453145324
				}
			};
			for (var i = 0; i < 20; i++)
			{
				result.ListProperty.Add(new TestClassItem {Id = i, Name = $"Name 00{i.ToString()}"});
			}

			return result;
		}

		public TestClassOther Other { [UsedImplicitly] get; set; }
		public TestClassPrimitiveTypes Primitive1 { [UsedImplicitly] get; set; }
		public TestClassPrimitiveTypes Primitive2 { [UsedImplicitly] get; set; }

		// ReSharper disable once CollectionNeverQueried.Global
		public List<TestClassItem> ListProperty { get; set; }
	}

	public sealed class TestClassItem
	{
		public int Id { [UsedImplicitly] get; set; }
		public string Name { [UsedImplicitly] get; set; }
	}

	public sealed class TestClassOther
	{
		public TestClassItem Test { [UsedImplicitly] get; set; }

		public double Double { [UsedImplicitly] get; set; }
	}
}