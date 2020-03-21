using ExtendedXmlSerializer.ContentModel;
using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Conversion;
using ExtendedXmlSerializer.ContentModel.Reflection;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Specifications;
using JetBrains.Annotations;
using System.Reflection;
// ReSharper disable TooManyDependencies

namespace ExtendedXmlSerializer.ExtensionModel.References
{
	sealed class ReferenceContents : IContents
	{
		readonly static IsReferenceSpecification Specification = IsReferenceSpecification.Default;

		readonly ISpecification<TypeInfo> _specification;
		readonly ISpecification<TypeInfo> _convertible;
		readonly IReferenceEncounters     _identifiers;
		readonly IReferenceMaps           _maps;
		readonly IEntities                _entities;
		readonly IContents                _option;
		readonly IClassification          _classification;

		[UsedImplicitly]
		public ReferenceContents(IReferenceEncounters identifiers, IReferenceMaps maps, IEntities entities,
		                         IContents option, IClassification classification, IConverters converters)
			: this(Specification, converters.IfAssigned(), identifiers, maps, entities, option, classification) {}

		public ReferenceContents(ISpecification<TypeInfo> specification, ISpecification<TypeInfo> convertible,
		                         IReferenceEncounters identifiers, IReferenceMaps maps, IEntities entities,
		                         IContents option, IClassification classification)

		{
			_identifiers    = identifiers;
			_maps           = maps;
			_entities       = entities;
			_option         = option;
			_classification = classification;
			_specification  = specification;
			_convertible    = convertible;
		}

		public ISerializer Get(TypeInfo parameter)
		{
			var serializer = _option.Get(parameter);
			var result = serializer as RuntimeSerializer ??
			             (_specification.IsSatisfiedBy(parameter) ? Serializer(parameter, serializer) : serializer);
			return result;
		}

		ReferenceSerializer Serializer(TypeInfo parameter, ISerializer serializer)
		{
			var reader = _convertible.IsSatisfiedBy(parameter)
				             ? new ReferenceActivation(serializer.Accept, _entities).Get(parameter)
				             : serializer;
			var referenceReader = new ReferenceReader(reader, _maps, _entities, parameter, _classification);
			var result          = new ReferenceSerializer(_identifiers, referenceReader, serializer);
			return result;
		}
	}
}