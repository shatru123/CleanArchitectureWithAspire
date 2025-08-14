using Aspire.Hosting.ApplicationModel;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresDatabaseResource> database = builder
    .AddPostgres("database")
    .WithImage("postgres:17")
    .WithBindMount("../../.containers/db", "/var/lib/postgresql/data")
    .AddDatabase("clean-architecture");

// To-do: Commented out for now, as not exporting the traces.

// OpenTelemetry Collector
// IResourceBuilder<ContainerResource> otelCollector = builder
//    .AddContainer("otel-collector", "otel/opentelemetry-collector", "latest")
//    .WithBindMount(
//        Path.GetFullPath("../otel-config"), // host config file
//        "/etc/otelcol/config",                         // container file path
//        isReadOnly: true
//    )
//    .WithEndpoint(port: 4317, targetPort: 4317, name: "grpc") // OTLP gRPC
//    .WithEndpoint(port: 4318, targetPort: 4318, name: "http") // OTLP HTTP
//    .WithEnvironment("OTEL_LOG_LEVEL", "debug"); // optional

builder.AddProject<Projects.Web_Api>("web-api")
    .WithEnvironment("ConnectionStrings__Database", database)
    .WithReference(database)
    // .WithEnvironment("OTEL_EXPORTER_OTLP_ENDPOINT", "http://otel-collector:4317") // gRPC
    //.WithEnvironment("OTEL_EXPORTER_OTLP_ENDPOINT", "http://otel-collector:4318") // HTTP
    //.WithEnvironment("OTEL_EXPORTER_OTLP_PROTOCOL", "http/protobuf")
    .WaitFor(database);
    //.WaitFor(otelCollector);

builder.Build().Run();
