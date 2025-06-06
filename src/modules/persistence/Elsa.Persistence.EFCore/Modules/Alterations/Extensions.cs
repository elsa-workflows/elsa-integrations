﻿using Elsa.Alterations.Features;
using JetBrains.Annotations;

namespace Elsa.Persistence.EFCore.Modules.Alterations;

/// <summary>
/// Provides extensions to the <see cref="AlterationsFeature"/> feature.
/// </summary>
[PublicAPI]
public static class Extensions
{
    /// <summary>
    /// Configures the <see cref="AlterationsFeature"/> to use EF Core persistence providers.
    /// </summary>
    public static AlterationsFeature UseEntityFrameworkCore(this AlterationsFeature feature, Action<EFCoreAlterationsPersistenceFeature>? configure = null)
    {
        feature.Module.Configure(configure);
        return feature;
    }
}