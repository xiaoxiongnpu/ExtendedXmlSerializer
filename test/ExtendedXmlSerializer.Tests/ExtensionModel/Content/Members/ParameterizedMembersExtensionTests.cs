﻿using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.Tests.Support;
using FluentAssertions;
using JetBrains.Annotations;
using System;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ExtensionModel.Content.Members
{
	public class ParameterizedMembersExtensionTests
	{
		[Fact]
		public void Verify()
		{
			var serializer = new SerializationSupport(new ConfigurationContainer().EnableParameterizedContent());
			var expected   = new Subject("Hello World!");
			var actual = serializer.Assert(expected,
			                               @"<?xml version=""1.0"" encoding=""utf-8""?><ParameterizedMembersExtensionTests-Subject xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ExtensionModel.Content.Members;assembly=ExtendedXmlSerializer.Tests""><Message>Hello World!</Message></ParameterizedMembersExtensionTests-Subject>");
			Assert.Equal(expected.Message, actual.Message);
		}

		[Fact]
		public void BasicTuple()
		{
			var serializer = new SerializationSupport(new ConfigurationContainer().EnableParameterizedContent());
			var expected   = new Tuple<string>("Hello World!");
			var actual = serializer.Assert(expected,
			                               @"<?xml version=""1.0"" encoding=""utf-8""?><Tuple xmlns:exs=""https://extendedxmlserializer.github.io/v2"" exs:arguments=""string"" xmlns=""https://extendedxmlserializer.github.io/system""><Item1>Hello World!</Item1></Tuple>");
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void CreatedTuple()
		{
			var serializer = new SerializationSupport(new ConfigurationContainer().EnableParameterizedContent());
			var expected   = Tuple.Create("Hello World!", 6776, TypeCode.Empty);

			var actual = serializer.Assert(expected,
			                               @"<?xml version=""1.0"" encoding=""utf-8""?><Tuple xmlns:exs=""https://extendedxmlserializer.github.io/v2"" exs:arguments=""string,int,TypeCode"" xmlns=""https://extendedxmlserializer.github.io/system""><Item1>Hello World!</Item1><Item2>6776</Item2><Item3>Empty</Item3></Tuple>");
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void ConfiguredTuple()
		{
			var container = new ConfigurationContainer();
			container.EnableParameterizedContent()
			         .Type<Tuple<string>>()
			         .Member(x => x.Item1)
			         .Name("NewName");
			var serializer = new SerializationSupport(container);
			var expected   = new Tuple<string>("Hello World!");
			var actual = serializer.Assert(expected,
			                               @"<?xml version=""1.0"" encoding=""utf-8""?><Tuple xmlns:exs=""https://extendedxmlserializer.github.io/v2"" exs:arguments=""string"" xmlns=""https://extendedxmlserializer.github.io/system""><NewName>Hello World!</NewName></Tuple>");
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void ConfiguredCreatedTuple()
		{
			var container = new ConfigurationContainer();
			container.UseAutoFormatting()
			         .EnableParameterizedContent()
			         .Type<Tuple<string, int, TypeCode>>()
			         .Member(x => x.Item1, m => m.Name("Message"))
			         .Member(x => x.Item2, m => m.Name("Number"))
			         .Member(x => x.Item3, m => m.Name("Codez"));
			var serializer = new SerializationSupport(container);
			var expected   = Tuple.Create("Hello World!", 6776, TypeCode.Empty);
			var actual = serializer.Assert(expected,
			                               @"<?xml version=""1.0"" encoding=""utf-8""?><Tuple xmlns:exs=""https://extendedxmlserializer.github.io/v2"" exs:arguments=""string,int,TypeCode"" Message=""Hello World!"" Number=""6776"" Codez=""Empty"" xmlns=""https://extendedxmlserializer.github.io/system"" />");
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public void MultipleParameters()
		{
			var serializer = new SerializationSupport(new ConfigurationContainer().EnableParameterizedContent());
			var expected   = new SubjectWithMultipleParameters("Hello World!", 6776);
			var actual = serializer.Assert(expected,
			                               @"<?xml version=""1.0"" encoding=""utf-8""?><ParameterizedMembersExtensionTests-SubjectWithMultipleParameters xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ExtensionModel.Content.Members;assembly=ExtendedXmlSerializer.Tests""><Message>Hello World!</Message><Number>6776</Number></ParameterizedMembersExtensionTests-SubjectWithMultipleParameters>");
			Assert.Equal(expected.Message, actual.Message);
			Assert.Equal(expected.Number, actual.Number);
		}

		[Fact]
		public void MultipleConstructors()
		{
			var serializer = new SerializationSupport(new ConfigurationContainer().EnableParameterizedContent());
			var expected   = new SubjectWithMultipleConstructors(6776, new DateTime(1976, 6, 7));
			var actual = serializer.Assert(expected,
			                               @"<?xml version=""1.0"" encoding=""utf-8""?><ParameterizedMembersExtensionTests-SubjectWithMultipleConstructors xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ExtensionModel.Content.Members;assembly=ExtendedXmlSerializer.Tests""><Number>6776</Number><DateTime>1976-06-07T00:00:00</DateTime></ParameterizedMembersExtensionTests-SubjectWithMultipleConstructors>");
			Assert.Equal(expected.Number, actual.Number);
			Assert.Equal(expected.DateTime, actual.DateTime);
			Assert.Equal(SubjectWithMultipleConstructors.Message, actual.Get());
		}

		[Fact]
		public void Invalid()
		{
			var serializer = new SerializationSupport(new ConfigurationContainer().EnableParameterizedContent());
			var expected   = new SubjectWithInvalidConstructor("Hello World!", 6776);
			Assert.Throws<InvalidOperationException>(() => serializer.Serialize(expected));
		}

		class Subject
		{
			public Subject(string message) => Message = message;

			public string Message { get; }
		}

		class SubjectWithMultipleParameters
		{
			public SubjectWithMultipleParameters(string message, int number)
			{
				Message = message;
				Number  = number;
			}

			public string Message { get; }
			public int Number { get; }
		}

		class SubjectWithInvalidConstructor
		{
			[UsedImplicitly] readonly string _message;

			public SubjectWithInvalidConstructor(string message, int number)
			{
				Number   = number;
				_message = message;
			}

			public int Number { [UsedImplicitly] get; }
		}

		class SubjectWithMultipleConstructors : ISource<string>
		{
			public const string Message = "Hello World from Second Candidate Selector!";
			readonly     string _message;

			public SubjectWithMultipleConstructors(int number, DateTime dateTime) : this(Message, number, dateTime) {}

			public SubjectWithMultipleConstructors(string message, int number, DateTime dateTime)
			{
				Number   = number;
				DateTime = dateTime;
				_message = message;
			}

			public int Number { get; }
			public DateTime DateTime { get; }

			public string Get() => _message;
		}
	}
}