﻿using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.Tests.ReportedIssues.Shared;
using ExtendedXmlSerializer.Tests.ReportedIssues.Support;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ReportedIssues
{
	public sealed class Issue241Tests
	{
		[Fact]
		void Verify()
		{
			var instance = new VerbasResc()
			{
				DmDev = new List<DmDev>()
				{
					new DmDev()
				}
			};

			new ConfigurationContainer().Create()
			                            .ForTesting()
			                            .Cycle(instance)
			                            .Should().BeEquivalentTo(instance);
		}
	}
}