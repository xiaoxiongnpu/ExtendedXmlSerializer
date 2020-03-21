﻿using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.Tests.ReportedIssues.Support;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ReportedIssues
{
	public sealed class Issue245Tests
	{
		[Fact]
		void Verify()
		{
			new ConfigurationContainer().Type<TestData>()
			                            .Member(x => x.IdValue)
			                            .EmitWhenInstance(data => data.IdValueSpecified)
			                            .Create()
			                            .ForTesting()
			                            .Assert(new TestData {Name = "Null ID"},
			                                    @"<?xml version=""1.0"" encoding=""utf-8""?><Issue245Tests-TestData xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ReportedIssues;assembly=ExtendedXmlSerializer.Tests.ReportedIssues""><Name>Null ID</Name></Issue245Tests-TestData>");
		}

		[System.SerializableAttribute]
		public class TestData
		{
			[System.Xml.Serialization.XmlElementAttribute("Name")]
			public string Name { get; set; }

			[System.Xml.Serialization.XmlElementAttribute("Id", DataType = "int")]
			public int IdValue { get; set; }

			[System.Xml.Serialization.XmlIgnoreAttribute]
			public bool IdValueSpecified { get; set; }

			[System.Xml.Serialization.XmlIgnoreAttribute]
			public System.Nullable<int> Id
			{
				get
				{
					if (IdValueSpecified)
					{
						return IdValue;
					}
					else
					{
						return null;
					}
				}
				set
				{
					IdValue          = value.GetValueOrDefault();
					IdValueSpecified = value.HasValue;
				}
			}
		}
	}
}