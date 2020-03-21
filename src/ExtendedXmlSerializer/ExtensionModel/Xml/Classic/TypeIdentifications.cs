using ExtendedXmlSerializer.ContentModel.Identification;
using ExtendedXmlSerializer.Core;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExtendedXmlSerializer.ExtensionModel.Xml.Classic
{
	sealed class TypeIdentifications : ITypeIdentifications
	{
		readonly Func<IEnumerable<TypeInfo>, IEnumerable<KeyValuePair<TypeInfo, IIdentity>>> _identities;

		[UsedImplicitly]
		public TypeIdentifications(ITypeIdentityRegistrations identities) : this(identities.Get) {}

		public TypeIdentifications(
			Func<IEnumerable<TypeInfo>, IEnumerable<KeyValuePair<TypeInfo, IIdentity>>> identities)
			=> _identities = identities;

		public ITypeIdentification Get(IEnumerable<TypeInfo> parameter)
			=> new TypeIdentification(_identities(parameter).ToDictionary());
	}
}