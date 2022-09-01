using FastEndpoints;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;
using Uno.Discord.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables()
    .AddCommandLine(args);

builder.Host.UseSerilog((ctx, configuration) =>
{
    const string DefaultFormat =
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
    var consoleLogFormat = ctx.Configuration["Logging:Console:Format"] ?? DefaultFormat;
    var fileLogFormat = ctx.Configuration["Logging:File:Format"] ?? DefaultFormat;
    var logPath = ctx.Configuration["Logging:File:Path"] ?? "./Logs";
    configuration
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.Console(formatter: new ExpressionTemplate(consoleLogFormat, theme: new TemplateTheme(
            TemplateTheme.Literate, new Dictionary<TemplateThemeStyle, string>
            {
                [TemplateThemeStyle.LevelDebug] = "\u001b[38;5;212m",
                [TemplateThemeStyle.LevelInformation] = "\u001b[38;5;141m",
                [TemplateThemeStyle.LevelError] = "\u001b[38;5;196m",
                [TemplateThemeStyle.LevelFatal] = "\u001b[38;5;88m",

                [TemplateThemeStyle.String] = "\u001b[38;5;159m"
            })))
        .WriteTo.Map
        (
            _ => $"{DateOnly.FromDateTime(DateTimeOffset.UtcNow.DateTime):yyyy-MM-dd}",
            (v, cf) =>
            {
                cf.File
                (
                    new ExpressionTemplate(fileLogFormat),
                    Path.Combine(logPath, $"{v}.log"),
                    fileSizeLimitBytes: 33_554_432, // 32mb
                    flushToDiskInterval: TimeSpan.FromMinutes(2.5),
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 50
                );
            },
            sinkMapCountLimit: 1
        );
});
builder.Host.AddDiscordServices(builder.Configuration["Discord:Token"] ?? throw new InvalidOperationException("No token was found in the config"));
builder.Services.AddFastEndpoints();
builder.Services.AddAuthentication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseFastEndpoints();
app.Run();