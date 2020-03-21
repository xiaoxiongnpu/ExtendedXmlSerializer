﻿using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.Tests.ReportedIssues.Support;
using System;
using System.Runtime.Serialization;
using Xunit;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace ExtendedXmlSerializer.Tests.ReportedIssues
{
	public sealed class Issue161Tests
	{
		[Fact]
		void Verify()
		{
			var serializer = new ConfigurationContainer().Register(SerializationSurrogateProvider.Default)
			                                             .Create()
			                                             .ForTesting();
			serializer.Assert(new Subject {Message = "Surrogates in the hizzy, dawg."},
			                  @"<?xml version=""1.0"" encoding=""utf-8""?><Issue161Tests-Subject xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ReportedIssues;assembly=ExtendedXmlSerializer.Tests.ReportedIssues""><Message>Hello world from Surrogate: Surrogates in the hizzy, dawg.</Message></Issue161Tests-Subject>");
		}

		sealed class Subject
		{
			public string Message { get; set; }
		}

		sealed class Surrogate
		{
			public string Message { get; set; }
		}

		sealed class SerializationSurrogateProvider : ISerializationSurrogateProvider
		{
			public static SerializationSurrogateProvider Default { get; } = new SerializationSurrogateProvider();

			SerializationSurrogateProvider() {}

			public object GetDeserializedObject(object obj, Type targetType)
			{
				var message = obj.AsValid<Surrogate>()
				                 .Message;
				var result = new Subject {Message = message};
				return result;
			}

			public object GetObjectToSerialize(object obj, Type targetType)
			{
				var message = obj.AsValid<Subject>()
				                 .Message;
				var result = new Surrogate {Message = $"Hello world from Surrogate: {message}"};
				return result;
			}

			public Type GetSurrogateType(Type type) => type == typeof(Subject) ? typeof(Surrogate) : null;
		}
	}
}