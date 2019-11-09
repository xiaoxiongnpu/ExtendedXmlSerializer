ExtendedXmlSerializer
=====================

[![Build status](https://ci.appveyor.com/api/projects/status/ub776yxp0nj535qp?svg=true)](https://ci.appveyor.com/project/ExtendedXmlSerializer/extendedxmlserializer) [![Nuget](https://img.shields.io/nuget/v/ExtendedXmlSerializer.svg)](https://www.nuget.org/packages/ExtendedXmlSerializer/)

<img src="https://extendedxmlserializer.github.io/img/logoBig.png" height="200px">

ExtendedXmlSerializer v2 is proudly developed and maintained with ReSharper Ultimate

[<img src="https://blog.dragonspark.us/images/ReSharper.png" height="200px"></a>](https://www.jetbrains.com/resharper/download/)

Welcome!
========

Welcome to ExtendedXmlSerializer's source repository, our little home on
the webternets for ExtendedXmlSerializer. Please have a look around. If
you have a question about our serializer, please do not hesitate to post
a question in our issues:
<https://github.com/ExtendedXmlSerializer/ExtendedXmlSerializer/issues/new/>

We will mark it as a question/discussion and talk it through with you
using sample code. This process is currently being used to fill in the
gaps with our documentation, which could use a little love.

Additionally, please make use of our documentation tag seen here:

<https://github.com/ExtendedXmlSerializer/ExtendedXmlSerializer/issues?q=is%3Aissue+label%3ADocumentation>

You can see the history of questions asked that we feel could eventually
be added to our documentation. We'll eventually get to it, we swear. 😆

## Mini-FAQ

Some quick questions for those wanting to use ExtendedXMLSerializer or are considering opening a new issue.

**What is ExtendedXmlSerializer designed to do?**

The primary vision and purpose of ExtendedXMLSerializer is to serialize a .NET object graph into an XML document, and then be able to read (deserialize) back into memory using the same documents created with it.

You can consider this serializer a hybrid of the `System.Xml.XmlSerializer` and the powerful `System.Xaml` serializer.

**I can do *xyz* with the classic `System.Xml.XmlSerializer`.  Can I do the same with ExtendedXmlSerializer?**

*Maybe*.™  The primary intended purpose of ExtendedXMLSerializer is to work with .NET object graphs, not to parse XML documents, manipulate an XML document model, or to work with legacy XML functionality.

That stated, we do support a lot of classic functionality, but it isn't a focus.  There is a lot to be desired about how the classic serializer handles .NET object graphs and subsequent activation.  Rectifying these rough spots was the main motivation behind ExtendedXMLSerializer.

If you would like to talk about support of a classic feature that does not appear to be supported, please do a [search through our issues list first](https://github.com/ExtendedXmlSerializer/ExtendedXmlSerializer/issues?utf8=%E2%9C%93&q=is%3Aissue+sort%3Aupdated-desc+), and if it has not been discussed, [open a new issue](https://github.com/ExtendedXmlSerializer/ExtendedXmlSerializer/issues/new) and we'll meet you there. 👍

**I have documents that were serialized with the classic `System.Xml.XmlSerializer`.  Can they be read with ExtendedXMLSerializer?**

Typically, yes.  But some features are not supported -- either because they have not yet been brought to our attention or we cannot due to current designs.  For instance, lists serialized with an element name configured via the `ElementAttributeAttribute` [don't do so well with ExtendedXmlSerializer](https://github.com/ExtendedXmlSerializer/ExtendedXmlSerializer/issues/240).  `XmlTextAttribute` also [gets a little grumpy](https://github.com/ExtendedXmlSerializer/ExtendedXmlSerializer/issues/242).

If you find that you run into an incompatibility, our recommendation is to:

- Serialize the graph using ExtendedXmlSerializer to see what the expected XML should look like.
- Create an XSLT transformation to transform the classically-serialized XML document.
- Run classic-serialized document through XSLT to create the ExtendedXmlSerializer-friendly document.

**It would seem you focused on serializing .NET object graphs and not on preserving classic legacy functionality.**

That's exactly what we did. :)  Along the way, we introduced a highly adaptable extension model to boot.  In fact, most issues submitted to our repository are solved by way of adding a new extension.

**I would like a legacy feature that is not currently supported with ExtendedXMLSerializer.  Can I submit a PR for this feature?**

Yes. :)  We certainly welcome any PRs for our project, having to do with implementing missing legacy functionality and/or otherwise.  Feel free to fire one up and we will meet you there. 👍

Information
===========

Support platforms:

-   .NET 4.5
-   .NET Standard 2.0

Support features:

-   Deserialization xml from standard XMLSerializer (mostly, see:
    <https://github.com/ExtendedXmlSerializer/ExtendedXmlSerializer/issues/240>)
-   Serialization class, struct, generic class, primitive type, generic
    list and dictionary, array, enum
-   Serialization class with property interface
-   Serialization circular reference and reference Id
-   Deserialization of old version of xml
-   Property encryption
-   Custom serializer
-   Support `XmlElementAttribute`, `XmlRootAttribute`, and `XmlTypeAttribute`
    for identity resolution.
-   Additional attribute support: `XmlIgnoreAttribute`,
    `XmlAttributeAttribute`, and `XmlEnumAttribute`.
-   POCO - all configurations (migrations, custom serializer...) are
    outside the clas

Standard XML Serializer in .NET is very limited:

-   Does not support serialization of class with circular reference or
    class with interface property.
-   There is no mechanism for reading the old version of XML.
-   Does not support properties that are defined with interface types.
-   Does not support read-only collection properties (like Xaml does).
-   Does not support parameterized constructors.
-   Does not support private constructors.
-   If you want create custom serializer, your class must inherit from
    `IXmlSerializable`. This takes the "plain" out of [POCO](https://en.wikipedia.org/wiki/Plain_old_CLR_object). 😁

The Basics
==========

Everything in ExtendedXmlSerializer begins with a configuration
container, from which you can use to configure the serializer and
ultimately create it:

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer()
                                                // Configure...
                                                .Create();
```

Using this simple subject class:

``` csharp
    public sealed class Subject
    {
        public string Message { get; set; }
    
        public int Count { get; set; }
    }
```

The results of the default serializer will look like this:

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <Subject xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Introduction;assembly=ExtendedXmlSerializer.Samples">
      <Message>Hello World!</Message>
      <Count>6776</Count>
    </Subject>
```

We can take this a step further by configuring the `Subject`'s `Type` and
`Member` properties, which will effect how its Xml is emitted. Here is an
example of configuring the `Subject`'s element name to emit as
`ModifiedSubject`:


``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().ConfigureType<Subject>()
                                                                    .Name("ModifiedSubject")
                                                                    .Create();
```

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <ModifiedSubject xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Introduction;assembly=ExtendedXmlSerializer.Samples">
      <Message>Hello World!</Message>
      <Count>6776</Count>
    </ModifiedSubject>
```

Diving a bit further, we can also configure the type's member
information. For example, configuring `Subject.Message` to emit as `Text`
instead:

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().ConfigureType<Subject>()
                                                                    .Member(x => x.Message)
                                                                    .Name("Text")
                                                                    .Create();

```

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <Subject xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Introduction;assembly=ExtendedXmlSerializer.Samples">
      <Text>Hello World!</Text>
      <Count>6776</Count>
    </Subject>
```

Xml Settings
============

In case you want to configure the XML write and read settings via
`XmlWriterSettings` and `XmlReaderSettings` respectively, you can do that
via extension methods created for you to do so:

``` csharp
    Subject subject = new Subject{ Count = 6776, Message = "Hello World!" };
    IExtendedXmlSerializer serializer = new ConfigurationContainer().Create();
    string contents = serializer.Serialize(new XmlWriterSettings {Indent = true}, subject);
    // ...
```

And for reading:

``` csharp
    Subject instance = serializer.Deserialize<Subject>(new XmlReaderSettings{IgnoreWhitespace = false}, contents);
    // ...
```

Serialization
=============

Now that your configuration container has been configured and your
serializer has been created, it's time to get to the serialization.

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().Create();
    TestClass obj = new TestClass();
    string xml = serializer.Serialize(obj);
```

Deserialization
===============

``` csharp
    TestClass obj2 = serializer.Deserialize<TestClass>(xml);
```


Classic Serialization/Deserialization
=====================================

Most of the code examples that you see in this documentation make use of
useful extension methods that make serialization and deserialization a
snap with ExtendedXmlSerializer.  However, if you would like to break down 
into the basic, classical pattern of serialization, and deserialization, 
this is supported too, as seen by the following examples.  Here's serialization:

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().Create();
    TestClass              instance   = new TestClass();
    MemoryStream           stream     = new MemoryStream();
    using (XmlWriter writer = XmlWriter.Create(stream))
    {
        serializer.Serialize(writer, instance);
        writer.Flush();
    }

    stream.Seek(0, SeekOrigin.Begin);
    string contents = new StreamReader(stream).ReadToEnd();
```

And here's how to deserialize:

``` csharp
    TestClass deserialized;
    MemoryStream contentStream = new MemoryStream(Encoding.UTF8.GetBytes(contents));
    using (XmlReader reader = XmlReader.Create(contentStream))
    {
        deserialized = (TestClass)serializer.Deserialize(reader);
    }
```

Serialization/Deserialization with Settings Overrides
=====================================================

Additionally, ExtendedXmlSerializer also supports overrides for
serialization and deserialization that allow you to pass in
`XmlWriterSettings` and `XmlReaderSettings` respectively. Here's an example
of this for serialization:

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().Create();
    TestClass              instance   = new TestClass();
    MemoryStream           stream     = new MemoryStream();
    
    string contents = serializer.Serialize(new XmlWriterSettings { /* ... */ }, stream, instance);
```

And for deserialization:

``` csharp
    MemoryStream contentStream = new MemoryStream(Encoding.UTF8.GetBytes(contents));
    TestClass deserialized = serializer.Deserialize<TestClass>(new XmlReaderSettings{ /* ... */ }, contentStream);
```

Fluent API
==========

ExtendedXmlSerializer use fluent API to configuration. Example:

``` csharp
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
```

Serialization of dictionary
===========================

You can serialize generic dictionary, that can store any type.

``` csharp
    public class TestClass
    {
        public Dictionary<int, string> Dictionary { get; set; }
    }
```

``` csharp
    TestClass obj = new TestClass
    {
        Dictionary = new Dictionary<int, string>
        {
            {1, "First"},
            {2, "Second"},
            {3, "Other"},
        }
    };
```

Output XML will look like:

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <TestClass xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Dictianary;assembly=ExtendedXmlSerializer.Samples">
      <Dictionary>
        <Item xmlns="https://extendedxmlserializer.github.io/system">
          <Key>1</Key>
          <Value>First</Value>
        </Item>
        <Item xmlns="https://extendedxmlserializer.github.io/system">
          <Key>2</Key>
          <Value>Second</Value>
        </Item>
        <Item xmlns="https://extendedxmlserializer.github.io/system">
          <Key>3</Key>
          <Value>Other</Value>
        </Item>
      </Dictionary>
    </TestClass>
```

If you use `UseOptimizedNamespaces` function xml will look like:

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <TestClass xmlns:sys="https://extendedxmlserializer.github.io/system" xmlns:exs="https://extendedxmlserializer.github.io/v2" xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Dictianary;assembly=ExtendedXmlSerializer.Samples">
      <Dictionary>
        <sys:Item>
          <Key>1</Key>
          <Value>First</Value>
        </sys:Item>
        <sys:Item>
          <Key>2</Key>
          <Value>Second</Value>
        </sys:Item>
        <sys:Item>
          <Key>3</Key>
          <Value>Other</Value>
        </sys:Item>
      </Dictionary>
    </TestClass>
```

Custom serialization
====================

If your class has to be serialized in a non-standard way:

``` csharp
    public class TestClass
    {
        public TestClass(string paramStr, int paramInt)
        {
            PropStr = paramStr;
            PropInt = paramInt;
        }
    
        public string PropStr { get; private set; }
        public int PropInt { get; private set; }
    }
```

You must create custom serializer:

``` csharp
    public class TestClassSerializer : IExtendedXmlCustomSerializer<TestClass>
    {
        public TestClass Deserialize(XElement element)
        {
            XElement xElement = element.Member("String");
            XElement xElement1 = element.Member("Int");
            if (xElement != null && xElement1 != null)
            {
                string strValue = xElement.Value;
    
                int intValue = Convert.ToInt32(xElement1.Value);
                return new TestClass(strValue, intValue);
            }
            throw new InvalidOperationException("Invalid xml for class TestClassWithSerializer");
        }
    
        public void Serializer(XmlWriter writer, TestClass obj)
        {
            writer.WriteElementString("String", obj.PropStr);
            writer.WriteElementString("Int", obj.PropInt.ToString(CultureInfo.InvariantCulture));
        }
    }
```

Then, you have to add custom serializer to configuration of TestClass:

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().Type<TestClass>()
                                                                    .CustomSerializer(new TestClassSerializer())
                                                                    .Create();
```

Deserialize old version of xml
==============================

In standard XMLSerializer you can't deserialize XML in case you change
model. In ExtendedXMLSerializer you can create migrator for each class
separately. E.g.: If you have big class, that uses small class and this
small class will be changed you can create migrator only for this small
class. You don't have to modify whole big XML. Now I will show you a
simple example.

If you had a class:

``` csharp
    public class TestClass
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
```

and generated XML look like:

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <TestClass xmlns="clr-namespace:ExtendedXmlSerialization.Samples.MigrationMap;assembly=ExtendedXmlSerializer.Samples">
      <Id>1</Id>
      <Type>Type</Type>
    </TestClass>
```

Then you renamed property:

``` csharp
    public class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
```

and generated XML look like:

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <TestClass xmlns:exs="https://extendedxmlserializer.github.io/v2" exs:version="1" xmlns="clr-namespace:ExtendedXmlSerialization.Samples.MigrationMap;assembly=ExtendedXmlSerializer.Samples">
      <Id>1</Id>
      <Name>Type</Name>
    </TestClass>
```

Then, you added new property and you wanted to calculate a new value
during deserialization.

``` csharp
    public class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
```

and new XML should look like:

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <TestClass xmlns:exs="https://extendedxmlserializer.github.io/v2" exs:version="2" xmlns="clr-namespace:ExtendedXmlSerializer.Samples.MigrationMap;assembly=ExtendedXmlSerializer.Samples">
      <Id>1</Id>
      <Name>Type</Name>
      <Value>Calculated</Value>
    </TestClass>
```

You can migrate (read) old version of XML using migrations:

``` csharp
    public class TestClassMigrations : IEnumerable<Action<XElement>>
    {
        public static void MigrationV0(XElement node)
        {
            XElement typeElement = node.Member("Type");
            // Add new node
            node.Add(new XElement("Name", typeElement.Value));
            // Remove old node
            typeElement.Remove();
        }
    
        public static void MigrationV1(XElement node)
        {
            // Add new node
            node.Add(new XElement("Value", "Calculated"));
        }
    
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
        public IEnumerator<Action<XElement>> GetEnumerator()
        {
            yield return MigrationV0;
            yield return MigrationV1;
        }
    }
```

Then, you must register your TestClassMigrations class in configuration

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().ConfigureType<TestClass>()
                                                                    .AddMigration(new TestClassMigrations())
                                                                    .Create();
```

Extensibility
=============

With type and member configuration out of the way, we can turn our
attention to what really makes ExtendedXmlSeralizer tick: extensibility.
As its name suggests, ExtendedXmlSeralizer offers a very flexible (but
albeit new) extension model from which you can build your own
extensions. Pretty much all if not all features you encounter with
ExtendedXmlSeralizer are through extensions. There are quite a few in
our latest version here that showcase this extensibility. The remainder
of this document will showcase the top features of ExtendedXmlSerializer
that are accomplished through its extension system.

Object reference and circular reference
=======================================

If you have a class:

``` csharp
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    
        public Person Boss { get; set; }
    }
    
    public class Company
    {
        public List<Person> Employees { get; set; }
    }
```

then you create object with circular reference, like this:

``` csharp
    Person boss = new Person {Id = 1, Name = "John"};
    boss.Boss = boss; //himself boss
    Person worker = new Person {Id = 2, Name = "Oliver"};
    worker.Boss = boss;
    Company obj = new Company
    {
        Employees = new List<Person>
        {
            worker,
            boss
        }
    };
```

You must configure Person class as reference object:

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().ConfigureType<Person>()
                                                                    .EnableReferences(p => p.Id)
                                                                    .Create();
```

Output XML will look like this:

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <Company xmlns="clr-namespace:ExtendedXmlSerializer.Samples.ObjectReference;assembly=ExtendedXmlSerializer.Samples">
      <Employees>
        <Capacity>4</Capacity>
        <Person Id="2">
          <Name>Oliver</Name>
          <Boss Id="1">
            <Name>John</Name>
            <Boss xmlns:exs="https://extendedxmlserializer.github.io/v2" exs:entity="1" />
          </Boss>
        </Person>
        <Person xmlns:exs="https://extendedxmlserializer.github.io/v2" exs:entity="1" />
      </Employees>
    </Company>
```

Property Encryption
===================

If you have a class with a property that needs to be encrypted:

``` csharp
    public class Person
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
```

You must implement interface `IEncryption`. For example, it will show the
Base64 encoding, but in the real world better to use something safer,
eg. RSA.:

``` csharp
    public class CustomEncryption : IEncryption
    {
        public string Parse(string data)
            => Encoding.UTF8.GetString(Convert.FromBase64String(data));
    
        public string Format(string instance)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(instance));
    }
```


Then, you have to specify which properties are to be encrypted and
register your `IEncryption` implementation.

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().UseEncryptionAlgorithm(new CustomEncryption())
                                                                    .ConfigureType<Person>()
                                                                    .Member(p => p.Password)
                                                                    .Encrypt()
                                                                    .Create();
```

Custom Conversion
=================

ExtendedXmlSerializer does a pretty decent job (if we do say so
ourselves) of composing and decomposing objects, but if you happen to
have a type that you want serialized in a certain way, and this type can
be destructured into a string, then you can register a custom converter
for it.

Using the following:

``` csharp
    public sealed class CustomStructConverter : IConverter<CustomStruct>
    {
        public static CustomStructConverter Default { get; } = new CustomStructConverter();
        CustomStructConverter() {}
    
        public bool IsSatisfiedBy(TypeInfo parameter) => typeof(CustomStruct).GetTypeInfo()
                                                                             .IsAssignableFrom(parameter);
    
        public CustomStruct Parse(string data) =>
            int.TryParse(data, out int number) ? new CustomStruct(number) : CustomStruct.Default;
    
        public string Format(CustomStruct instance) => instance.Number.ToString();
    }
    
    public struct CustomStruct
    {
        public static CustomStruct Default { get; } = new CustomStruct(6776);
    
        public CustomStruct(int number)
        {
            Number = number;
        }
        public int Number { get; }
    }
```

Register the converter:

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().Register(CustomStructConverter.Default).Create();
    CustomStruct subject = new CustomStruct(123);
    string contents = serializer.Serialize(subject);
    // ...
```

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <CustomStruct xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Extensibility;assembly=ExtendedXmlSerializer.Samples">123</CustomStruct>
```


Optimized Namespaces
====================

By default Xml namespaces are emitted on an "as needed" basis:

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <List xmlns:exs="https://extendedxmlserializer.github.io/v2" exs:arguments="Object" xmlns="https://extendedxmlserializer.github.io/system">
      <Capacity>4</Capacity>
      <Subject xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Extensibility;assembly=ExtendedXmlSerializer.Samples">
        <Message>First</Message>
      </Subject>
      <Subject xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Extensibility;assembly=ExtendedXmlSerializer.Samples">
        <Message>Second</Message>
      </Subject>
      <Subject xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Extensibility;assembly=ExtendedXmlSerializer.Samples">
        <Message>Third</Message>
      </Subject>
    </List>
```

But with one call to the `UseOptimizedNamespaces` call, namespaces get
placed at the root of the document, thereby reducing document footprint:


``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().UseOptimizedNamespaces()
                                                                    .Create();
    List<object> subject = new List<object>
                    {
                        new Subject {Message = "First"},
                        new Subject {Message = "Second"},
                        new Subject {Message = "Third"}
                    };
    string contents = serializer.Serialize(subject);
    // ...
```


``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <List xmlns:ns1="clr-namespace:ExtendedXmlSerializer.Samples.Extensibility;assembly=ExtendedXmlSerializer.Samples" xmlns:exs="https://extendedxmlserializer.github.io/v2" exs:arguments="Object" xmlns="https://extendedxmlserializer.github.io/system">
      <Capacity>4</Capacity>
      <ns1:Subject>
        <Message>First</Message>
      </ns1:Subject>
      <ns1:Subject>
        <Message>Second</Message>
      </ns1:Subject>
      <ns1:Subject>
        <Message>Third</Message>
      </ns1:Subject>
    </List>
```

Implicit Namespaces/Typing
==========================

If you don't like namespaces at all, you can register types so that they
do not emit namespaces when they are rendered into a document:

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().EnableImplicitTyping(typeof(Subject))
                                                                    .Create();
    Subject subject = new Subject{ Message = "Hello World!  No namespaces, yay!" };
    string contents = serializer.Serialize(subject);
    // ...
```

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <Subject>
      <Message>Hello World!  No namespaces, yay!</Message>
    </Subject>
```


Auto-Formatting (Attributes)
============================

The default behavior for emitting data in an Xml document is to use
elements, which can be a little chatty and verbose:

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().UseOptimizedNamespaces()
                                                                    .Create();
    List<object> subject = new List<object>
                    {
                        new Subject {Message = "First"},
                        new Subject {Message = "Second"},
                        new Subject {Message = "Third"}
                    };
    string contents = serializer.Serialize(subject);
```

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <SubjectWithThreeProperties xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Extensibility;assembly=ExtendedXmlSerializer.Samples">
      <Number>123</Number>
      <Message>Hello World!</Message>
      <Time>2018-05-26T11:52:19.4981212-04:00</Time>
    </SubjectWithThreeProperties>
```

Making use of the `UseAutoFormatting` call will enable all types that have
a registered IConverter (convert to string and back) to emit as
attributes:

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <SubjectWithThreeProperties Number="123" Message="Hello World!" Time="2018-05-26T11:52:19.4981212-04:00" xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Extensibility;assembly=ExtendedXmlSerializer.Samples" />
```

Verbatim Content (CDATA)
========================

If you have an element with a member that can hold lots of data, or data
that has illegal characters, you configure it to be a verbatim field and
it will emit a CDATA section around it:

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().Type<Subject>()
                                                                    .Member(x => x.Message)
                                                                    .Verbatim()
                                                                    .Create();
    Subject subject = new Subject {Message = @"<{""Ilegal characters and such""}>"};
    string contents = serializer.Serialize(subject);
    // ...
```

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <Subject xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Extensibility;assembly=ExtendedXmlSerializer.Samples">
      <Message><![CDATA[<{"Ilegal characters and such"}>]]></Message>
    </Subject>
```

You can also denote these fields with an attribute and get the same
functionality:

``` csharp
    public sealed class VerbatimSubject
    {
        [Verbatim]
        public string Message { get; set; }
    }
```

Private Constructors
====================

One of the limitations of the classic XmlSerializer is that it does not
support private constructors, but ExtendedXmlSerializer does via its
EnableAllConstructors call:

``` csharp
    public sealed class SubjectByFactory
    {
        public static SubjectByFactory Create(string message) => new SubjectByFactory(message);
    
        SubjectByFactory() : this(null) {} // Used by serializer.
    
        SubjectByFactory(string message) => Message = message;
    
        public string Message { get; set; }
    }
```

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().EnableAllConstructors()
                                                                    .Create();
    SubjectByFactory subject = SubjectByFactory.Create("Hello World!");
    string contents = serializer.Serialize(subject);
    // ...
```

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <SubjectByFactory xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Extensibility;assembly=ExtendedXmlSerializer.Samples">
      <Message>Hello World!</Message>
    </SubjectByFactory>
```


Immutable Classes and Content
=================================

Taking this concept bit further leads to a favorite feature of ours in
ExtendedXmlSerlializer. The classic serializer only supports
parameterless public constructors. With ExtendedXmlSerializer, you can
use the EnableParameterizedContent call to enable parameterized
parameters in the constructor that by convention have the same name as
the property for which they are meant to assign:

``` csharp
    public sealed class ImmutableSubject
    {
        public ImmutableSubject(string message, int number, DateTime time)
        {
            Message = message;
            Number = number;
            Time = time;
        }
    
        public string Message { get; }
        public int Number { get; }
        public DateTime Time { get; }
    }
```

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().EnableParameterizedContent()
                                                                    .Create();
    ImmutableSubject subject = new ImmutableSubject("Hello World!", 123, DateTime.Now);
    string contents = serializer.Serialize(subject);
```

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <ImmutableSubject xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Extensibility;assembly=ExtendedXmlSerializer.Samples">
      <Message>Hello World!</Message>
      <Number>123</Number>
      <Time>2018-05-26T11:52:19.7551187-04:00</Time>
    </ImmutableSubject>
```

Tuples
======

By enabling parameterized content, it opens up a lot of possibilities,
like being able to serialize Tuples. Of course, serializable Tuples were
introduced recently with the latest version of C#. Here, however, you
can couple this with our member-naming funtionality and provide better
naming for your tuple properties:

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer().EnableParameterizedContent()
                                                                    .Type<Tuple<string>>()
                                                                    .Member(x => x.Item1)
                                                                    .Name("Message")
                                                                    .Create();
    Tuple<string> subject = Tuple.Create("Hello World!");
    string contents = serializer.Serialize(subject);
    // ...
```

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <Tuple xmlns:exs="https://extendedxmlserializer.github.io/v2" exs:arguments="string" xmlns="https://extendedxmlserializer.github.io/system">
      <Message>Hello World!</Message>
    </Tuple>
```

Experimental Xaml-ness: Attached Properties
===========================================

We went ahead and got a little cute with v2 of ExtendedXmlSerializer,
adding support for Attached Properties on objects in your serialized
object graph. But instead of constraining it to objects that inherit
from `DependencyObject`, *every* object can benefit from it. Check it out:

``` csharp
    sealed class NameProperty : ReferenceProperty<Subject, string>
    {
        public const string DefaultMessage = "The Name Has Not Been Set";
    
        public static NameProperty Default { get; } = new NameProperty();
        NameProperty() : base(() => Default, x => DefaultMessage) {}
    }
    
    sealed class NumberProperty : StructureProperty<Subject, int>
    {
        public const int DefaultValue = 123;
    
        public static NumberProperty Default { get; } = new NumberProperty();
        NumberProperty() : base(() => Default, x => DefaultValue) {}
    }
```

``` csharp
    IExtendedXmlSerializer serializer = new ConfigurationContainer()
                                        .EnableAttachedProperties(NameProperty.Default, NumberProperty.Default)
                                        .Create();
    Subject subject = new Subject {Message = "Hello World!"};
    subject.Set(NameProperty.Default, "Hello World from Attached Properties!");
    subject.Set(NumberProperty.Default, 123);
    
    string contents = serializer.Serialize(subject);
    // ...
```

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <Subject xmlns="clr-namespace:ExtendedXmlSerializer.Samples.Extensibility;assembly=ExtendedXmlSerializer.Samples">
      <Message>Hello World!</Message>
      <NameProperty.Default>Hello World from Attached Properties!</NameProperty.Default>
      <NumberProperty.Default>123</NumberProperty.Default>
    </Subject>
```

(Please note that this feature is experimental, but please try it out
and let us know what you think!)

Experimental Xaml-ness: Markup Extensions
=========================================

Saving the most novel feature for last, we have experimental support for one of
Xaml's greatest features, Markup Extensions:

``` csharp
    sealed class Extension : IMarkupExtension
    {
        const string Message = "Hello World from Markup Extension! Your message is: ", None = "N/A";
    
        readonly string _message;
    
        public Extension() : this(None) {}
    
        public Extension(string message)
        {
            _message = message;
        }
    
        public object ProvideValue(IServiceProvider serviceProvider) => string.Concat(Message, _message);
    }
```

``` csharp
    string contents =
        @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Subject xmlns=""clr-namespace:ExtendedXmlSerializer.Samples.Extensibility;assembly=ExtendedXmlSerializer.Samples""
            Message=""{Extension 'PRETTY COOL HUH!!!'}"" />";
    IExtendedXmlSerializer serializer = new ConfigurationContainer().EnableMarkupExtensions()
                                                                    .Create();
    Subject subject = serializer.Deserialize<Subject>(contents);
    Console.WriteLine(subject.Message); // "Hello World from Markup Extension! Your message is: PRETTY COOL HUH!!!"
```

(Please note that this feature is experimental, but please try it out
and let us know what you think!)

How to Upgrade from v1.x to v2
==============================

Finally, if you have documents from v1, you will need to upgrade them to
v2 to work. This involves reading the document in an instance of v1
serializer, and then writing it in an instance of v2 serializer. We have
provided the ExtendedXmlSerializer.Legacy nuget package to assist in
this goal.

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <ArrayOfSubject>
        <Subject type="ExtendedXmlSerializer.Samples.Introduction.Subject">
            <Message>First</Message>
            <Count>0</Count>
        </Subject>
        <Subject type="ExtendedXmlSerializer.Samples.Introduction.Subject">
            <Message>Second</Message>
            <Count>0</Count>
        </Subject>
        <Subject type="ExtendedXmlSerializer.Samples.Introduction.Subject">
            <Message>Third</Message>
            <Count>0</Count>
        </Subject>
    </ArrayOfSubject>
```

``` csharp
    ExtendedXmlSerialization.ExtendedXmlSerializer legacySerializer = new ExtendedXmlSerialization.ExtendedXmlSerializer();
    string content = File.ReadAllText(@"bin\Upgrade.Example.v1.xml"); // Path to your legacy xml file.
    List<Subject> subject = legacySerializer.Deserialize<List<Subject>>(content);
    
    // Upgrade:
    IExtendedXmlSerializer serializer = new ConfigurationContainer().Create();
    string contents = serializer.Serialize(new XmlWriterSettings {Indent = true}, subject);
    File.WriteAllText(@"bin\Upgrade.Example.v2.xml", contents);
    // ...
```

``` xml
    <?xml version="1.0" encoding="utf-8"?>
    <List xmlns:ns1="clr-namespace:ExtendedXmlSerializer.Samples.Introduction;assembly=ExtendedXmlSerializer.Samples" xmlns:exs="https://extendedxmlserializer.github.io/v2" exs:arguments="ns1:Subject" xmlns="https://extendedxmlserializer.github.io/system">
      <Capacity>4</Capacity>
      <ns1:Subject>
        <Message>First</Message>
        <Count>0</Count>
      </ns1:Subject>
      <ns1:Subject>
        <Message>Second</Message>
        <Count>0</Count>
      </ns1:Subject>
      <ns1:Subject>
        <Message>Third</Message>
        <Count>0</Count>
      </ns1:Subject>
    </List>
```

Authors
=======

- [Wojciech Nagórski](https://github.com/WojciechNagorski) - v1.x Author.
- [Mike-EEE](https://github.com/Mike-EEE) - v2.x Author.

