using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.ContentModel.Members;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ExtensionModel.Expressions;
using ExtendedXmlSerializer.ReflectionModel;

namespace ExtendedXmlSerializer.ExtensionModel.Markup
{
	sealed class MarkupExtensions
		: ReferenceCacheBase<IFormatReader, IMarkupExtensionPartsEvaluator>,
		  IMarkupExtensions
	{
		readonly IEvaluator              _evaluator;
		readonly ITypeMembers            _members;
		readonly IMemberAccessors        _accessors;
		readonly IConstructors           _constructors;
		readonly System.IServiceProvider _provider;

		// ReSharper disable once TooManyDependencies
		public MarkupExtensions(IEvaluator evaluator, ITypeMembers members, IMemberAccessors accessors,
		                        IConstructors constructors, System.IServiceProvider provider)
		{
			_evaluator    = evaluator;
			_members      = members;
			_accessors    = accessors;
			_constructors = constructors;
			_provider     = provider;
		}

		protected override IMarkupExtensionPartsEvaluator Create(IFormatReader parameter)
			=> new MarkupExtensionPartsEvaluator(parameter, _evaluator, _members, _accessors, _constructors, _provider,
			                                     parameter);
	}
}