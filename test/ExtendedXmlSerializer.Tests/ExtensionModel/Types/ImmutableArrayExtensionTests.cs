﻿using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.Tests.Support;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ExtensionModel.Types
{
	public class ImmutableArrayExtensionTests
	{
		[Fact]
		public void Verify()
		{
			var expected  = ImmutableArray.Create("Hello", "World!");
			var container = new ConfigurationContainer();
			// container.EnableImmutableArrays();
			var serializer = new SerializationSupport(container);
			var actual =
				serializer.Assert(expected,
				                  @"<?xml version=""1.0"" encoding=""utf-8""?><ImmutableArray xmlns:sys=""https://extendedxmlserializer.github.io/system"" xmlns:exs=""https://extendedxmlserializer.github.io/v2"" exs:arguments=""sys:string"" xmlns=""clr-namespace:System.Collections.Immutable;assembly=System.Collections.Immutable""><sys:string>Hello</sys:string><sys:string>World!</sys:string></ImmutableArray>");
			Assert.True(expected.SequenceEqual(actual));
		}
	}
}