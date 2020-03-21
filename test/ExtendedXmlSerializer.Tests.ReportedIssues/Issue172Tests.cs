﻿using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.Tests.ReportedIssues.Support;
using JetBrains.Annotations;
using System.Collections.Generic;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ReportedIssues
{
	public class Issue172Tests
	{
		[Fact]
		public void Verify()
		{
			new ConfigurationContainer()
				.EnableImplicitTyping(typeof(Course), typeof(Student), typeof(List<Student>))
				.Ignore(typeof(List<>).GetProperty(nameof(List<object>.Capacity)))
				.ForTesting()
				.Assert(new Course
				        {
					        Title = "Hello World",
					        Students = new List<Student>
					        {
						        new Student {Name = "Name"}
					        }
				        },
				        @"<?xml version=""1.0"" encoding=""utf-8""?><Issue172Tests-Course><Title>Hello World</Title><Students><Issue172Tests-Student><Name>Name</Name></Issue172Tests-Student></Students></Issue172Tests-Course>"
				       );
		}

		public class Course
		{
			public string Title { [UsedImplicitly] get; set; }
			public List<Student> Students { [UsedImplicitly] get; set; }
		}

		public class Student
		{
			public string Name { [UsedImplicitly] get; set; }
		}
	}
}