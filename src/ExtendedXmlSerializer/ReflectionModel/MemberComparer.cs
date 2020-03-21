﻿using System.Reflection;

namespace ExtendedXmlSerializer.ReflectionModel
{
	sealed class MemberComparer : IMemberComparer
	{
		public static MemberComparer Default { get; } = new MemberComparer();

		MemberComparer() : this(Defaults.TypeComparer) {}

		readonly ITypeComparer _type;

		public MemberComparer(ITypeComparer type) => _type = type;

		public bool Equals(MemberInfo x, MemberInfo y)
			=> x.Name.Equals(y.Name) && _type.Equals(x.DeclaringType.GetTypeInfo(), y.DeclaringType.GetTypeInfo());

		public int GetHashCode(MemberInfo obj) => 0;
	}
}