using ExtendedXmlSerializer.ContentModel.Collections;
using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.ContentModel.Identification;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.ReflectionModel;
using System.Text;
using System.Xml;

namespace ExtendedXmlSerializer.ExtensionModel.Xml
{
	/// <summary>
	/// A default extension that is used to configure all necessary components for xml-specific serialization and
	/// deserialization.
	/// </summary>
	public sealed class XmlSerializationExtension : ISerializerExtension
	{
		readonly XmlNameTable      _names;
		readonly XmlReaderSettings _reader;
		readonly XmlWriterSettings _writer;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public XmlSerializationExtension()
			: this(Defaults.ReaderSettings, Defaults.WriterSettings, new NameTable()) {}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="writer"></param>
		/// <param name="names"></param>
		public XmlSerializationExtension(XmlReaderSettings reader, XmlWriterSettings writer, XmlNameTable names)
		{
			_reader = reader;
			_writer = writer;
			_names  = names;
		}

		/// <inheritdoc />
		public IServiceRepository Get(IServiceRepository parameter)
			=> parameter.RegisterInstance(Encoding.UTF8)
			            .RegisterInstance(_names)
			            .RegisterInstance(_reader.Clone())
			            .RegisterInstance(_writer.Clone())
			            .RegisterInstance<IIdentifierFormatter>(IdentifierFormatter.Default)
			            .RegisterInstance<IPrefixes>(Prefixes.Default)
			            .RegisterInstance<IReaderFormatter>(ReaderFormatter.Default)
			            .RegisterInstance<IFormattedContentSpecification>(FormattedContentSpecification.Default)
			            .RegisterInstance<IListContentsSpecification>(
			                                                          new ListContentsSpecification(
			                                                                                        IsTypeSpecification
				                                                                                        <
					                                                                                        IListInnerContent
				                                                                                        >
				                                                                                        .Default
				                                                                                        .And(ElementSpecification
					                                                                                             .Default)))
			            .Register<IInnerContentActivation, XmlInnerContentActivation>()
			            .Register<IFormatReaderContexts, FormatReaderContexts>()
			            .Register<IFormatWriters, FormatWriters>()
			            .Register<IXmlReaderFactory, XmlReaderFactory>()
			            .Register<IFormatReaders, FormatReaders>()
			            .Register<IExtendedXmlSerializer, ExtendedXmlSerializer>();

		void ICommand<IServices>.Execute(IServices parameter) {}
	}
}