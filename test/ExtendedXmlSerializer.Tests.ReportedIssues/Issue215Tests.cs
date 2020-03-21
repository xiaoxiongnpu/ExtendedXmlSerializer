﻿using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.Tests.ReportedIssues.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ReportedIssues
{
	public sealed class Issue215Tests
	{
		[Fact]
		void Verify()
		{
			var data = @"<?xml version=""1.0"" encoding=""utf-8""?>
						 <Issue215Tests-TestClass xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ReportedIssues;assembly=ExtendedXmlSerializer.Tests.ReportedIssues"">
							<Foo/>
							<Baz>Baz</Baz>
						 </Issue215Tests-TestClass>";
			var subject = new ConfigurationContainer().Create()
			                                          .ForTesting()
			                                          .Deserialize<TestClass>(data);
			subject.Foo.Should()
			       .BeEmpty();
			subject.Baz.Should()
			       .Be("Baz");
		}

		public class TestClass
		{
			public string Foo { get; [UsedImplicitly] set; }
			public string Baz { get; [UsedImplicitly] set; }
		}
	}
}