﻿using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.Tests.ReportedIssues.Support;
using FluentAssertions;
using Xunit;

// ReSharper disable All

namespace ExtendedXmlSerializer.Tests.ReportedIssues
{
	public sealed class Issue146Tests
	{
		[Fact]
		public void Verify()
		{
			var serializer = new ConfigurationContainer().EnableParameterizedContent()
			                                             .Type<Plane>()
			                                             .Member(x => x.Rows)
			                                             .Ignore()
			                                             .Member(x => x.Length)
			                                             .Attribute()
			                                             .Create()
			                                             .ForTesting();
			var subject = new Plane(4.33);
			serializer.Cycle(subject)
			          .Should().BeEquivalentTo(subject);
			serializer.Serialize(subject)
			          .Should()
			          .Be(@"<?xml version=""1.0"" encoding=""utf-8""?><Issue146Tests-Plane Length=""4.33"" xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ReportedIssues;assembly=ExtendedXmlSerializer.Tests.ReportedIssues"" />");
		}

		[Fact]
		public void Configuration()
		{
			var master = new Master(1);
			var serializer = new ConfigurationContainer().EnableParameterizedContent()
			                                             .Type<Master>()
			                                             //.Member(x => x.sub).Include()
			                                             .Member(x => x.A)
			                                             .Ignore()
			                                             .Type<SubItem>()
			                                             .Member(x => x.X)
			                                             .Attribute()
			                                             .Create()
			                                             .ForTesting();
			serializer.Assert(master,
			                  @"<?xml version=""1.0"" encoding=""utf-8""?><Issue146Tests-Master xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ReportedIssues;assembly=ExtendedXmlSerializer.Tests.ReportedIssues""><Sub X=""1"" /></Issue146Tests-Master>");
		}

		public struct SubItem
		{
			public readonly double X;

			public SubItem(double x)
			{
				this.X = x;
			}
		}

		public struct Master
		{
			readonly double _a;
			public SubItem Sub { get; }
			public double A => _a;

			public Master(double a)
				: this(new SubItem(a))
			{
				_a = a;
			}

			public Master(SubItem sub)
			{
				Sub = sub;
				_a  = sub.X;
			}
		}

		public sealed class Plane
		{
			public double Length { get; }

			public int Rows { get; set; }

			public Plane(int rows, double Length)
			{
				Rows        = rows;
				this.Length = Length;
			}

			public Plane(double Length) => this.Length = Length;
		}
	}
}