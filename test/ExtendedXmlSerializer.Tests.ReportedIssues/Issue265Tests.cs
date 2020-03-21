﻿using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.Tests.ReportedIssues.Support;
using FluentAssertions;
using JetBrains.Annotations;
using System.Xml.Serialization;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ReportedIssues
{
	public sealed class Issue265Tests
	{
		[Fact]
		void Verify()
		{
			const string message = "Hello World!";
			new ConfigurationContainer().Create()
			                            .ForTesting()
			                            .Cycle(new Subject(message))
			                            .Message()
			                            .Should()
			                            .Be(message);
		}

		[Fact]
		void VerifyInheritance()
		{
			const string message = "Hello World!";
			new ConfigurationContainer().Create()
			                            .ForTesting()
			                            .Cycle(new Inherited(message))
			                            .Message()
			                            .Should()
			                            .Be(message);
		}

		class Base
		{
			[XmlElement] protected string _message;

			public Base() : this("Default Value") {}

			public Base(string test) => _message = test;

			public string Message() => _message;
		}

		class Inherited : Base
		{
			[UsedImplicitly]
			public Inherited() {}

			public Inherited(string test) : base(test) {}
		}

		sealed class Subject
		{
			[XmlElement] string _message;

			[UsedImplicitly]
			public Subject() : this("Default Value") {}

			public Subject(string test) => _message = test;

			public string Message() => _message;
		}
	}
}