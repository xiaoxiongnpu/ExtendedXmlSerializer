﻿using ExtendedXmlSerializer.ContentModel.Conversion;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Parsing;
using ExtendedXmlSerializer.ExtensionModel.Expressions;
using ExtendedXmlSerializer.ExtensionModel.Markup;
using FluentAssertions;
using Sprache;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ExtensionModel.Markup
{
	public class MarkupExtensionParserTests
	{
		[Fact]
		public void Type()
		{
			const string text  = "{x:Testing}";
			var          parts = MarkupExtensionParser.Default.Get(text);
			parts.Type.Should()
			     .BeEquivalentTo(new TypeParts("Testing", "x"));
			parts.Arguments.Should()
			     .BeEmpty();
		}

		[Fact]
		public void MarkupExtensionExpression()
		{
			const string text  = "{x:Testing {x:Type sys:string, TypeName=exs:Another}}";
			var          parts = MarkupExtensionParser.Default.Get(text);
			parts.Type.Should()
			     .BeEquivalentTo(new TypeParts("Testing", "x"));
			parts.Properties.Should()
			     .BeEmpty();

			var actual = parts.Arguments.Should()
			                  .ContainSingle()
			                  .Which.Should()
			                  .BeOfType<MarkupExtensionPartsExpression>()
			                  .Subject.Get();
			var expected = new MarkupExtensionParts(
			                                        new TypeParts("Type", "x"), new GeneralExpression("sys:string")
			                                                                    .Yield<IExpression>()
			                                                                    .ToImmutableArray(),
			                                        new KeyValuePair<string, IExpression>("TypeName",
			                                                                              new
				                                                                              GeneralExpression("exs:Another"))
				                                        .Yield()
				                                        .ToImmutableArray());
			actual.Should()
			      .BeEquivalentTo(expected);
			actual.Arguments.Single()
			      .ToString()
			      .Should()
			      .Be("sys:string");
			actual.Properties.Single()
			      .Value.ToString()
			      .Should()
			      .Be("exs:Another");
		}

		[Fact]
		public void Optional()
		{
			Parser<MarkupExtensionParts> sut = MarkupExtensionParser.Default;
			sut.ParseAsOptional("12345")
			   .Should()
			   .BeNull();
			sut.Invoking(x => x.ParseAsOptional("{x:Partial"))
			   .Should()
			   .Throw<InvalidOperationException>();
		}

		[Fact]
		public void VerifySpaced()
		{
			const string text = "{x:Testing }";
			var parts = MarkupExtensionParser.Default.ToParser()
			                                 .Parse(text);
			parts.Type.Should()
			     .BeEquivalentTo(new TypeParts("Testing", "x"));
			parts.Arguments.Should()
			     .BeEmpty();
		}

		[Fact]
		public void VerifyArguments()
		{
			const string text  = "{x:Testing 'one', 12345, ' two ', '{}This is an escaped {} literal.', 3 * 3}";
			var          parts = MarkupExtensionParser.Default.Get(text);
			parts.Type.Should()
			     .BeEquivalentTo(new TypeParts("Testing", "x"));
			parts.Arguments.Select(x => x.ToString())
			     .Should()
			     .HaveCount(5)
			     .And
			     .ContainInOrder("one", "12345", " two ", "This is an escaped {} literal.", "3 * 3");
			parts.Properties.Should()
			     .BeEmpty();
		}

		[Fact]
		public void VerifyProperties()
		{
			const string text  = "{x:Testing MemberName='Value'}";
			var          parts = MarkupExtensionParser.Default.Get(text);
			parts.Type.Should()
			     .BeEquivalentTo(new TypeParts("Testing", "x"));
			parts.Arguments.Select(x => x.ToString())
			     .Should()
			     .BeEmpty();
			parts.Properties.ToDictionary(x => x.Key, x => x.Value.ToString())
			     .Should()
			     .BeEquivalentTo(new Dictionary<string, string> {{"MemberName", "Value"}});
		}

		[Fact]
		public void VerifyArgumentsAndProperties()
		{
			const string text  = "{x:Testing 'one', 12345, ' two ', MemberName='Value', MemberTwo = 3 + 4}";
			var          parts = MarkupExtensionParser.Default.Get(text);
			parts.Type.Should()
			     .BeEquivalentTo(new TypeParts("Testing", "x"));
			parts.Arguments.Select(x => x.ToString())
			     .Should()
			     .HaveCount(3)
			     .And.BeEquivalentTo("one", "12345", " two ");
			parts.Properties.ToDictionary(x => x.Key, x => x.Value.ToString())
			     .Should()
			     .BeEquivalentTo(new Dictionary<string, string>
			     {
				     {"MemberName", "Value"},
				     {"MemberTwo", "3 + 4"}
			     });
		}
	}
}