﻿using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.Tests.ReportedIssues.Support;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace ExtendedXmlSerializer.Tests.ReportedIssues
{
	public sealed class Issue201Tests
	{
		[Fact]
		void Verify()
		{
			var subject = new TestClass();
			subject.TestProperty.Add(456);
			var testClass = new ConfigurationContainer().Create()
			                                            .ForTesting()
			                                            .Cycle(subject);
			testClass.Should().BeEquivalentTo(subject);
			testClass.TestProperty.Count.Should()
			         .Be(5);
		}
	}

	public class TestClass
	{
		public List<object> TestProperty { get; set; } = new List<object> {"Hello", null, "World!", 123};
	}
}