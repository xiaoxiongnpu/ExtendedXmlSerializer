using System;
using System.Reflection;
using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Identification;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ContentModel.Collections
{
	sealed class ArrayElement : IElement
	{
		readonly ICollectionItemTypeLocator _locator;
		readonly IWriter<Array>             _identity;

		public ArrayElement(IIdentities identities)
			: this(new Identity<Array>(identities.Get(Support<Array>.Metadata)), CollectionItemTypeLocator.Default) {}

		public ArrayElement(IWriter<Array> identity, ICollectionItemTypeLocator locator)
		{
			_locator  = locator;
			_identity = identity;
		}

		public IWriter Get(TypeInfo parameter)
			=> new ArrayIdentity(_identity, _locator.Get(parameter)).Adapt();
	}
}