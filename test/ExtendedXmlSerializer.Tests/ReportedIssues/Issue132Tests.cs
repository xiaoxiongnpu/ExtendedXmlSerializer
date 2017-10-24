﻿using ExtendedXmlSerializer.Configuration;

// ReSharper disable All

namespace ExtendedXmlSerializer.Tests.ReportedIssues
{
	public sealed class Issue132Tests
	{
		public void Examples()
		{
			new ConfigurationContainer().EnableDeferredReferences()
			                            .Type<IElement>().EnableReferences(p => p.Id)
										.Type<Section>().EnableReferences(p => p.Id)
										.Type<Building>().EnableReferences(p => p.Id)
			                            .Create();


			new ConfigurationContainer().Type<Section>().Member(p => p.IsSelected).Name("Selected")
			                            .Type<Section>().Member(p => p.IsEmpty).Name("Empty")
			                            .Create();

			var config = new ConfigurationContainer();
			config.EnableDeferredReferences();
			config.Type<Section>().Member(p => p.Id).Name("Identity");
			config.Type<Section>().EnableReferences(p => p.Id);
			var exs = config.Create();

		}

		interface IElement
		{
			string Id { get; }
		}

		sealed class Building : IElement
		{
			public string Id { get; set; }
		}

		sealed class Section : IElement
		{
			public string Id { get; set; }

			public bool IsSelected { get; set; }
			public bool IsEmpty { get; set; }
		}
	}
}