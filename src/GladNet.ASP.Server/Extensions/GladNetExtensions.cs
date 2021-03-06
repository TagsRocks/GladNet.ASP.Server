﻿using GladNet.ASP.Formatters;
using GladNet.Message;
using GladNet.Payload;
using GladNet.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//I know we shouldn't hijack Microsoft namespaces but it's so much easier for consumers
//to access these extensions this way
namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Extensions for Fluent MvcBuilder interfaces.
	/// </summary>
	public static class GladNetExtensions
	{
		/// <summary>
		/// Adds the <see cref="GladNetInputFormatter"/> and <see cref="GladNetOutputFormatter"/> to the known formatters.
		/// Also registers the gladnet media header to map to these formatters.
		/// </summary>
		/// <param name="builder">Builder to chain off.</param>
		/// <param name="serializerStrat">Serialization strategy</param>
		/// <param name="deserializerStrat">Deserialization strategy.</param>
		/// <param name="registry">Serialization registry.</param>
		/// <returns>The fluent <see cref="IServiceCollection"/> instance.</returns>
		public static IServiceCollection AddGladNet(this IServiceCollection builder, ISerializerStrategy serializerStrat, IDeserializerStrategy deserializerStrat, ISerializerRegistry registry)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			if (serializerStrat == null) throw new ArgumentNullException(nameof(serializerStrat));
			if (deserializerStrat == null) throw new ArgumentNullException(nameof(deserializerStrat));
			if (registry == null) throw new ArgumentNullException(nameof(registry));

			//need to register these types
			registry.Register(typeof(NetworkMessage));
			registry.Register(typeof(PacketPayload));
			registry.Register(typeof(RequestMessage));
			registry.Register(typeof(ResponseMessage));

			return builder.AddMvcCore()
				.AddGladNetFormatters(serializerStrat, deserializerStrat).Services;
		}
	}
}
