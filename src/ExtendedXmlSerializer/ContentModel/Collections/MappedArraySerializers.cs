using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.ContentModel.Properties;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.ReflectionModel;
using JetBrains.Annotations;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ExtendedXmlSerializer.ContentModel.Collections
{
	sealed class MappedArraySerializers : IContents
	{
		readonly IContents _contents;

		public MappedArraySerializers(IContents contents) => _contents = contents;

		public ISerializer Get(TypeInfo parameter)
		{
			var serializer = _contents.Get(parameter);
			var result = new Serializer(new Reader(serializer, parameter),
			                            new Writer(serializer, Dimensions.Defaults.Get(parameter)).Adapt());
			return result;
		}

		sealed class Writer : IWriter<Array>
		{
			readonly IWriter                          _writer;
			readonly IProperty<ImmutableArray<int>>   _property;
			readonly Func<Array, ImmutableArray<int>> _dimensions;

			public Writer(IWriter writer, Func<Array, ImmutableArray<int>> dimensions)
				: this(writer, MapProperty.Default, dimensions) {}

			public Writer(IWriter writer, IProperty<ImmutableArray<int>> property,
			              Func<Array, ImmutableArray<int>> dimensions)
			{
				_writer     = writer;
				_property   = property;
				_dimensions = dimensions;
			}

			public void Write(IFormatWriter writer, Array instance)
			{
				_property.Write(writer, _dimensions(instance));
				_writer.Write(writer, instance);
			}
		}

		sealed class SizeOf<T> : DelegatedSource<int>
		{
			[UsedImplicitly]
			public static SizeOf<T> Default { get; } = new SizeOf<T>();

			SizeOf() : base(Unsafe.SizeOf<T>) {}
		}

		sealed class Sizes : StructureCache<TypeInfo, int>
		{
			static IGeneric<ISource<int>> Sources { get; } = new Generic<ISource<int>>(typeof(SizeOf<>));

			public static Sizes Default { get; } = new Sizes();

			Sizes() : base(info => Sources.Get(info)
			                              .Invoke()
			                              .Get()) {}
		}

		sealed class Reader : IReader
		{
			readonly IReader                        _reader;
			readonly IProperty<ImmutableArray<int>> _map;
			readonly TypeInfo                       _root;
			readonly int                            _size;

			public Reader(IReader reader, TypeInfo type)
				: this(reader, MapProperty.Default, RootType.Default.Get(type)) {}

			public Reader(IReader reader, IProperty<ImmutableArray<int>> map, TypeInfo root)
				: this(reader, map, root, Sizes.Default.Get(root)) {}

			// ReSharper disable once TooManyDependencies
			public Reader(IReader reader, IProperty<ImmutableArray<int>> map, TypeInfo root, int size)
			{
				_reader = reader;
				_map    = map;
				_root   = root;
				_size   = size;
			}

			public object Get(IFormatReader parameter)
			{
				var dimensions = _map.Get(parameter);
				var source = _reader.Get(parameter)
				                    .AsValid<Array>();
				var result = Array.CreateInstance(_root, dimensions.ToArray());
				Buffer.BlockCopy(source, 0, result, 0, source.Length * _size);
				return result;
			}
		}
	}
}