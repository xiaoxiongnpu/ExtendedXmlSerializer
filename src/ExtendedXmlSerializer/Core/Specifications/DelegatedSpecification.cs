﻿using System;

namespace ExtendedXmlSerializer.Core.Specifications
{
	/// <inheritdoc />
	public class DelegatedSpecification<T> : ISpecification<T>
	{
		readonly Func<T, bool> _delegate;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="delegate"></param>
		public DelegatedSpecification(Func<T, bool> @delegate) => _delegate = @delegate;

		/// <inheritdoc />
		public virtual bool IsSatisfiedBy(T parameter) => _delegate(parameter);
	}
}