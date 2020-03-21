﻿using System;
using System.Collections.Generic;

namespace ExtendedXmlSerializer.Samples.FluentApi
{
	using Configuration;
	using CustomSerializator;
	using Encrypt;
	using System.Linq;

	public class FluentApiSamples
    {
	    public static void Run()
	    {
		    Program.PrintHeader("Serialization reference object");

// FluentAPI
		    IExtendedXmlSerializer serializer = new ConfigurationContainer()
				.UseEncryptionAlgorithm(new CustomEncryption())
			    .Type<Person>() // Configuration of Person class
					.Member(p => p.Password) // First member
						.Name("P")
						.Encrypt()
					.Member(p => p.Name) // Second member
						.Name("T")
				.Type<TestClass>() // Configuration of another class
					.CustomSerializer(new TestClassSerializer())
			    .Create();
// EndFluentAPI

			Run(serializer);
	    }

	    static void Run(IExtendedXmlSerializer serializer)
	    {
		    List<Person> list = new List<Person>
			               {
				               new Person {Name = "John", Password = "Ab238ds2"},
				               new Person {Name = "Oliver", Password = "df89nmXhdf"}
			               };

		    string xml = serializer.Serialize(list);
		    Console.WriteLine(xml);

		    List<Person> obj2 = serializer.Deserialize<List<Person>>(xml);
		    Console.WriteLine("Employees count = " + obj2.Count + " - passwords " +
		    string.Join(", ", obj2.Select(p => p.Password)));
	    }
    }
}

