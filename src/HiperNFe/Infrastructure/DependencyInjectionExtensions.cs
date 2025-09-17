using HiperNFe.Email;
using HiperNFe.Models;
using HiperNFe.Printing;
using HiperNFe.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HiperNFe.Infrastructure;

/// <summary>
/// Extensões para registrar os componentes da biblioteca na DI do .NET.
/// </summary>
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddHiperNFe(this IServiceCollection services, SefazConfiguration configuration, EmailSettings? emailSettings = null, DanfeOptions? danfeOptions = null)
    {
        services.AddSingleton(configuration);
        services.AddSingleton<CertificateManager>();
        services.AddSingleton<XmlGenerator>();
        services.AddSingleton<XmlSigner>();
        services.AddSingleton<SchemaValidator>();
        services.AddSingleton(danfeOptions ?? new DanfeOptions());
        services.AddSingleton(emailSettings ?? new EmailSettings());
        services.AddSingleton<DanfePrinter>();
        services.AddSingleton<EmailSender>();

        services.AddHttpClient<SefazHttpClient>(client =>
        {
            client.Timeout = configuration.RequestTimeout;
        });

        services.AddTransient<AuthorizationService>();
        services.AddTransient<ConsultationService>();
        services.AddTransient<CancellationService>();
        services.AddTransient<InutilizationService>();
        services.AddTransient<LetterCorrectionService>();
        services.AddTransient<ManifestationService>();
        services.AddTransient<StatusService>();

        services.AddTransient<ISefazService>(sp => sp.GetRequiredService<AuthorizationService>());
        services.AddTransient<ISefazService>(sp => sp.GetRequiredService<ConsultationService>());
        services.AddTransient<ISefazService>(sp => sp.GetRequiredService<CancellationService>());
        services.AddTransient<ISefazService>(sp => sp.GetRequiredService<InutilizationService>());
        services.AddTransient<ISefazService>(sp => sp.GetRequiredService<LetterCorrectionService>());
        services.AddTransient<ISefazService>(sp => sp.GetRequiredService<ManifestationService>());
        services.AddTransient<ISefazService>(sp => sp.GetRequiredService<StatusService>());

        services.AddSingleton<BatchProcessor>();

        return services;
    }
}
